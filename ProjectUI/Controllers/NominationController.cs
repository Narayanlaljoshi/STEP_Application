using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using OfficeOpenXml;
using STEPDAL.CustomDAL;
using ProjectBLL.CustomModel;
using Newtonsoft.Json;

namespace Project.Controllers
{
    public class NominationController : ApiController
    {
        
        [HttpPost]
        public async Task<HttpResponseMessage> UploadUserMatrix()
         {
            string location = "";
            string fileName = "";
            string ResponseMessage = "";
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

                    if (i == 0)
                    {
                        int count = 0, TotalCount = 0;
                        var sheet = excel.Workbook.Worksheets[1];
                        if (sheet == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Blank Sheet Not Allowed");
                        }

                        foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        tbl.Columns.Add("ProgramId", typeof(Int32));
                        tbl.Columns.Add("Agency_Id",typeof(Int32));
                        tbl.Columns.Add("Faculty_Id", typeof(Int32));
                        DataColumnCollection columns = tbl.Columns;
                        if (tbl.Columns.Count == 22)
                        {
                            if (!columns.Contains("Co_id") || !columns.Contains("region_cd") || !columns.Contains("AGENCY_CD") ||!columns.Contains("FAC_CD") ||!columns.Contains("PRG_ID") ||!columns.Contains("CALNDR_ID") || !columns.Contains("prg_venue") || !columns.Contains("RO_FRM_DATE") ||!columns.Contains("RO_TO_DATE") ||!columns.Contains("PROG_DURATION") || !columns.Contains("DEALER_CD") || !columns.Contains("DEALER_NAME") || !columns.Contains("city") || !columns.Contains("LOC_DESC") || !columns.Contains("MSPIN" )||!columns.Contains("PARTICIPANTS_NAME") ||!columns.Contains("EMP_DOB") ||!columns.Contains("Mobile") ||!columns.Contains("ProgramId") ||!columns.Contains("Agency_Id") ||!columns.Contains("Faculty_Id") )
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "Excel Sheet Not In Format (Column Names are Misspelled).");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Excel Sheet Not In Format (Columns are Missing or Misspelled)");
                        }


                        // Column Data of excel file
                        var startRow = hasHeader ? 2 : 1;

                        for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                        {
                            if (count == sheet.Dimension.End.Row)
                            {
                                //Data = NominationDAL.FilterDataTable(tbl, ref countIfProccessed,Obj);

                                //NominationDAL.BulkInsertDataTable(Data.Datatbl,Obj);

                                //Automation.BulkInsert_RawData(tbl);
                                Automation.FilterNominationData(tbl);
                                ResponseMessage=NominationDAL.FilterDownloadedNomination_v2();

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

                            //Data = NominationDAL.FilterDataTable(tbl, ref countIfProccessed, Obj);
                            //NominationDAL.BulkInsertDataTable(Data.Datatbl, Obj);
                            //Automation.BulkInsert_RawData(tbl);
                            Automation.FilterNominationData(tbl);
                            ResponseMessage=NominationDAL.FilterDownloadedNomination_v2();
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

            if (ResponseMessage.Length < 1)
                ResponseMessage = "Success: File uploaded successfully";
            return Request.CreateResponse(HttpStatusCode.Accepted, ResponseMessage);
        }

        [HttpGet]
        public List<NominationsList> GetNominationsList()
        {
            return NominationDAL.GetNominationsList();
        }
        [HttpPost]
        public string UpdateNominationsList(NominationsList Obj)
        {
            return NominationDAL.UpdateNominationsList(Obj);
        }

        [HttpGet]
        public void FilterDownloadedNomination()
        {
            NominationDAL.FilterDownloadedNomination();
        }
        [HttpGet]
        public void batchJob()
        {
            NominationDAL.batchJob();
        }
    }
}