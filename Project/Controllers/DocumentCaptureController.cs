using Newtonsoft.Json;
using OfficeOpenXml;
using ProjectBLL.CustomModel;
using ProjectDAL.CustomDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace Project.Controllers
{
    public class DocumentCaptureController : ApiController
    {

        [HttpPost]
        public async Task<HttpResponseMessage> UploadDocument()
        {
            string location = "";
            string fileName = "";
            string ResponseMessage = "Error";
            NominationValidationBLL Data = new NominationValidationBLL();
            // Check if the request contains multipart/form-data.
            string strUniqueId = System.Guid.NewGuid().ToString();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelPath"]);

            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            var model = result.FormData["data"];
            //NominationDAL.Truncate();
            UserDetailsBLL Obj = JsonConvert.DeserializeObject<UserDetailsBLL>(model);


            int LastEntryCount = NominationDAL.getLastEntryCount();

            DataTable tbl = new DataTable();
            DataColumn col = new DataColumn();
            col.ColumnName = "Nomination_Id";

            tbl.Columns.Add(col);

            int countIfProccessed = 0;
            try
            {
                for (int i = 0; i < result.FileData.Count; i++)
                {
                    fileName = result.FileData[i].Headers.ContentDisposition.FileName;
                    string fileserial = result.FileData[i].Headers.ContentDisposition.Name.Trim('"');
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);

                        Match regex = Regex.Match(root, @"(.+) \((\d+)\)\.\w+");

                        if (regex.Success)
                        {
                            fileName = regex.Groups[1].Value;

                        }
                    }

                    //Storing file to temporary location in project

                    try
                    {
                        string fileType = Path.GetExtension(fileName);
                        string filename = DateTime.Now.Ticks.ToString(); //Path.GetFileNameWithoutExtension(fileName);
                        string ExlName = filename + fileType;
                        string str;
                        str = filename.Substring(filename.Length - 1, 1);
                        string ImageWithPath = string.Format("{0}{1}", filename + str, fileType);
                        location = Path.Combine(root, ImageWithPath);
                        File.Move(result.FileData[i].LocalFileName, Path.Combine(root, ImageWithPath));
                    }

                    catch (Exception e)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "2");
                    }

                    FileInfo f = new FileInfo(location);
                    Stream s = f.OpenRead();

                    bool hasHeader = true;
                    //open xlsx file            
                    var excel = new ExcelPackage(s);
                    var workbook = excel.Workbook;
                    //  var model = result.FormData["data"];

                    //       var pro = JsonConvert.DeserializeObject<ASMInfo>(model);


                    if (i == 0)
                    {
                        int count = 0, TotalCount = 0;
                        var sheet = excel.Workbook.Worksheets[1];
                        if (sheet == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "2");
                        }

                        foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        tbl.Columns.Add("ProgramId", typeof(Int32));
                        tbl.Columns.Add("Agency_Id", typeof(Int32));
                        tbl.Columns.Add("Faculty_Id", typeof(Int32));
                        if (tbl.Columns.Count == 16)
                        {
                            if (tbl.Columns[1].ColumnName != "Co_id" ||
                                tbl.Columns[2].ColumnName != "Agency Code" ||
                                tbl.Columns[3].ColumnName != "Faculty Code" ||
                                tbl.Columns[4].ColumnName != "Program Code" ||
                                tbl.Columns[5].ColumnName != "Session ID" ||
                                tbl.Columns[6].ColumnName != "Start Date" ||
                                tbl.Columns[7].ColumnName != "End Date" ||
                                tbl.Columns[8].ColumnName != "Duration(As per Program Master)" ||
                                tbl.Columns[9].ColumnName != "MSPIN" ||
                                tbl.Columns[10].ColumnName != "Name" ||
                                tbl.Columns[11].ColumnName != "Date of Birth" ||
                                tbl.Columns[12].ColumnName != "Mobile No" ||
                                tbl.Columns[13].ColumnName != "ProgramId" ||
                                tbl.Columns[14].ColumnName != "Agency_Id" ||
                                tbl.Columns[15].ColumnName != "Faculty_Id")
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "3");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "2");
                        }


                        // Column Data of excel file
                        var startRow = hasHeader ? 2 : 1;

                        for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                        {
                            if (count == sheet.Dimension.End.Row)
                            {
                                Data = NominationDAL.FilterDataTable(tbl, ref countIfProccessed, Obj);

                                NominationDAL.BulkInsertDataTable(Data.Datatbl, Obj);
                                count = 0;
                                tbl.Rows.Clear();
                                // tbl.Columns.Clear();   //warning: All Columns delete
                                // tbl.Dispose();
                            }
                            var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                            DataRow row = tbl.Rows.Add();
                            LastEntryCount++;
                            foreach (var cell in wsRow)
                            {
                                row[0] = LastEntryCount;//0 index is for ServiceDebtor_Id. 
                                row[cell.Start.Column - 0] = cell.Text;
                                // incrementing ServiceDebtor_Id for every new row
                            }
                            count++;
                            TotalCount++;
                        }
                        if (tbl.Rows.Count > 0 && tbl.Rows.Count < sheet.Dimension.End.Row)
                        {

                            Data = NominationDAL.FilterDataTable(tbl, ref countIfProccessed, Obj);
                            NominationDAL.BulkInsertDataTable(Data.Datatbl, Obj);
                        }
                        tbl.Rows.Clear();
                        tbl.Columns.Clear();   //warning: All Columns delete
                        tbl.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, e.Message);

            }

            //string msg = "";
            //if (countIfProccessed == 0)
            //{
            //    msg = "1";
            //}
            //else
            //{
            //    msg = "4";
            //}
            if (Data.Response.Length < 1)
                Data.Response = "Success: File uploaded successfully";
            return Request.CreateResponse(HttpStatusCode.Accepted, Data.Response);

        }

        [HttpPost]
        public string SaveDocument(DocumentsBLL Obj)
        {            
            return DocumentCapture.SaveDocuments(Obj);
        }
    }
}
