using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using STEPDAL.DB;
using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Text.RegularExpressions;

namespace Project.Controllers
{
    public class QuestionBankController : ApiController
    {
        //[HttpGet]
        //public List<ProgramTestCalenderDetail_Model> GetProgramTestCalenderList()
        //{
        //    return ProgramTestCalenderDAL.GetProgramTestCalenderList();
        //}

        [HttpGet]
        public List<ProgramTest_QuestionDetail> GetQuestionBankList(int id,int Set_Id)
        {
            return QuestionBankDAL.GetQuestionBankList(id,Set_Id);
        }
        [HttpGet]
        public List<Practical_QuestionDetail> GetPracticalQuestionBankList(int id,int Set_Id)
        {
            return QuestionBankDAL.GetPracticalQuestionBankList(id, Set_Id);
        }
        [HttpPost]
        public List<Practical_Question_OtherLanguage> GetPracticalQuestionTemplate(FilterBLL Obj)
        {
            return QuestionBankDAL.GetPracticalQuestionTemplate(Obj);
        }
        [HttpPost]
        public List<Practical_QuestionDetail> GetPracticalQuestionBankList(FilterBLL Obj)
        {
            return QuestionBankDAL.GetPracticalQuestionBankList(Obj);
        }
        [HttpPost]
        public List<ProgramTest_QuestionDetail> GetQuestionFormatedList(QuestionVariable data)
        {
            return QuestionBankDAL.GetQuestionFormatedList(data);
        }

        [HttpGet]
        public List<GetLanguage_Model> GetLanguage(int id)
        {
            return QuestionBankDAL.GetLanguage(id);
        }

        [HttpGet]
        public List<GetLanguage_Model> GetLanguageWithoutEnglish(int id)
        {
            return QuestionBankDAL.GetLanguageWithoutEnglish(id);
        }

        [HttpPost]
        public string DeleteQuestion(List<int> Ids)
        {
            return QuestionBankDAL.DeleteQuestion(Ids);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UploadExcel()
        {
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
            var Obj = JsonConvert.DeserializeObject<ProgramTestModel>(model);

            Stream stream = await Request.Content.ReadAsStreamAsync();

            bool hasHeader = true;
            //open xlsx file
            var excel = new ExcelPackage(stream);
            var workbook = excel.Workbook;
            try
            {
                var sheet = excel.Workbook.Worksheets[1];

                DataTable tbl = new DataTable();
                foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                var PTCDtl = QuestionBankDAL.GetPTCDetail(Obj.ProgramTestCalenderId);
                if (PTCDtl.QuestionPaperType == "QB")
                {
                    if (tbl.Rows.Count < PTCDtl.TotalNoQuestion)
                        return Request.CreateResponse(HttpStatusCode.Accepted, "Number of Questions should be greater then or equal to : " + PTCDtl.TotalNoQuestion);
                }
                else
                {
                    List<SetSequence> setSeqs = QuestionBankDAL.GetUploadedSetSequences(PTCDtl).ToList();
                    if (setSeqs.Count != 0)
                    {
                        var Check = setSeqs.Where(x => x.Set_Id == Obj.Set_Id).FirstOrDefault();
                        if (Check!=null)
                            return Request.CreateResponse(HttpStatusCode.Accepted, "Set-" + Obj.Set_Id.ToString() + " is already uploaded");
                    }
                    if (tbl.Rows.Count != PTCDtl.TotalNoQuestion)
                        return Request.CreateResponse(HttpStatusCode.Accepted, "Number of Questions should be equal to - " + PTCDtl.TotalNoQuestion);
                }
                string msg = QuestionBankDAL.UploadList(tbl, Obj);
                return Request.CreateResponse(HttpStatusCode.Accepted, msg);
            }
            catch (Exception ex)//open xls file
            {
                //if its a .xls file it will throw an Exception
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed,ex.ToString());
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UploadExcel_Practical()
        {
            //_commonDAL _userDAL = new _commonDAL();

            // Check if the request contains multipart/form-data.
            // if (!Request.Content.IsMimeMultipartContent())
            //{
            //   throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}
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
            var Obj = JsonConvert.DeserializeObject<ProgramTestModel>(model);

            Stream stream = await Request.Content.ReadAsStreamAsync();

            bool hasHeader = true;
            //open xlsx file            
            var excel = new ExcelPackage(stream);
            var workbook = excel.Workbook;
            try
            {
                var sheet = excel.Workbook.Worksheets[1];

                DataTable tbl = new DataTable();
                foreach (var firstRowCell in sheet.Cells[2, 1, 1, sheet.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                var startRow = hasHeader ? 3 : 1;
                DataColumnCollection Columns = tbl.Columns;
                if (!Columns.Contains("Question") && !Columns.Contains("Action A") && !Columns.Contains("Action B") && !Columns.Contains("Action C") && !Columns.Contains("Action D") && !Columns.Contains("Action E") && !Columns.Contains("A") && !Columns.Contains("B") && !Columns.Contains("C") && !Columns.Contains("D") && !Columns.Contains("E"))
                {
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Error: Excel Sheet not in format.");
                }
                for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                string msg = QuestionBankDAL.UploadList_Practical(tbl, Obj);
                return Request.CreateResponse(HttpStatusCode.Accepted, msg);
            }
            catch (Exception ex)//open xls file
            {
                //if its a .xls file it will throw an Exception  
                return Request.CreateResponse(HttpStatusCode.Accepted,ex.Message.ToString());
            }
        }

        [HttpGet]
        public List<SetSequenceBll> GetSetSequenceList()
        {
            return QuestionBankDAL.GetSetSequenceList();
        }

        //--------------image upload---------------------//

        public async Task<HttpResponseMessage> UploadData()
        {
            string location = "";
            string fileName = "";
            string rtn = "";

            try
            {
                //   self  //      
                //// Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                Stream reqStream = await Request.Content.ReadAsStreamAsync();
                MemoryStream tempStream = new MemoryStream();
                reqStream.CopyTo(tempStream);



                tempStream.Seek(0, SeekOrigin.End);
                StreamWriter writer = new StreamWriter(tempStream);
                writer.WriteLine();
                writer.Flush();
                tempStream.Position = 0;

                StreamContent streamContent = new StreamContent(tempStream);
                foreach (var header in Request.Content.Headers)
                {
                    streamContent.Headers.Add(header.Key, header.Value);
                }

                var root = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UploadImage"]);



                var provider = new MultipartFormDataStreamProvider(root);
                var result = await streamContent.ReadAsMultipartAsync(provider);
                string strPath = "";

                var model = result.FormData["data"];

               var Obj = JsonConvert.DeserializeObject<QestionDetail_Model>(model);

                if (!Directory.Exists(root))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(root);
                    //  dirInfo.CreateSubdirectory(dirInfo);

                }


                //get the files
                if (result.FileData.Count < 1)
                {
                    var response = QuestionBankDAL.SaveQuestion(Obj);

                    rtn = response;

                  //  return Request.CreateResponse(HttpStatusCode.Accepted, "1");
                }

                else
                {
                    for (int i = 0; i < result.FileData.Count; i++)
                    {
                        fileName = result.FileData[i].Headers.ContentDisposition.FileName;
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                            //                      extension = Path.GetExtension(fileupload, fileName);                               // self //
                            Match regex = Regex.Match(root, @"(.+) \((\d+)\)\.\w+");

                            if (regex.Success)
                            {
                                fileName = regex.Groups[1].Value;

                            }
                        }

                        try
                        {
                            // string fileType = Path.GetExtension(fileName);
                            string fileType = ".jpeg";
                            //  string filename = Path.GetFileNameWithoutExtension(fileName) + Guid.NewGuid().ToString();
                            string filename = DateTime.Now.Ticks.ToString();
                            //string extension = Path.GetExtension(fileName);
                            string ImageName = filename + fileType;
                            string str;
                            str = filename.Substring(filename.Length - 1, 1);
                            //number++;
                            // strPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["ServerPath"], string.Format("/{0}{1}", filename + str, fileType));//extension));
                            strPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["ImageLink"], filename + str + fileType);

                            location = root + "\\";
                            string ImageWithPath = string.Format("{0}{1}", filename + str, fileType);
                            File.Move(result.FileData[i].LocalFileName, Path.Combine(location, ImageWithPath));
                            Obj.Image = strPath;  //Path.Combine(location, ImageWithPath);

                            var response = QuestionBankDAL.SaveQuestion(Obj);

                            rtn = response;

                        }


                        catch (Exception dbEx)
                        {
                            Exception raise = dbEx;
                            //foreach (var validationErrors in dbEx.EntityValidationErrors)
                            //{
                            //    foreach (var validationError in validationErrors.ValidationErrors)
                            //    {
                            //        string message = string.Format("{0}:{1}",
                            //            validationErrors.Entry.Entity.ToString(),
                            //            validationError.ErrorMessage);
                            //        // raise a new exception nesting  
                            //        // the current instance as InnerException  
                            //        raise = new InvalidOperationException(message, raise);
                            //    }
                            //}
                            throw raise;
                        }

                    }

                }

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.OK, e.Message.ToString());

            }
        }

        //------------------detail--------------//

            //-----------------------UPLOAD EXCEL Against language---------//

        [HttpPost]
        public async Task<HttpResponseMessage> UploadLanguage()
        {
           
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
            var Obj = JsonConvert.DeserializeObject<QestionLanguageModel>(model);


            Stream stream = await Request.Content.ReadAsStreamAsync();

            bool hasHeader = true;
            //open xlsx file            
            var excel = new ExcelPackage(stream);
            var workbook = excel.Workbook;
            try
            {
                var sheet = excel.Workbook.Worksheets[1];



                DataTable tbl = new DataTable();
                foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                string msg = QuestionBankDAL.UploadLanguageList(tbl, Obj);
                return Request.CreateResponse(HttpStatusCode.Accepted, msg);


            }
            catch (Exception ex)//open xls file
            {
                //if its a .xls file it will throw an Exception  
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }




        }

        [HttpPost]
        public async Task<HttpResponseMessage> BulkUpload_PracticalQuestions()
        {
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
            var Obj = JsonConvert.DeserializeObject<QestionLanguagePracticalModel>(model);


            Stream stream = await Request.Content.ReadAsStreamAsync();

            bool hasHeader = true;
            //open xlsx file            
            var excel = new ExcelPackage(stream);
            var workbook = excel.Workbook;
            try
            {
                var sheet = excel.Workbook.Worksheets[1];



                DataTable tbl = new DataTable();
                foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                string msg = QuestionBankDAL.UploadLanguageWiseQuestion_Practical(tbl, Obj);
                return Request.CreateResponse(HttpStatusCode.Accepted, msg);


            }
            catch (Exception ex)//open xls file
            {
                //if its a .xls file it will throw an Exception  
                return Request.CreateResponse(HttpStatusCode.Accepted,ex.Message.ToString());
            }




        }
        [HttpPost]
        public async Task<HttpResponseMessage> UploadLanguageWiseQuestion_Practical()
        {
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
            var Obj = JsonConvert.DeserializeObject<ProgramTestModel>(model);


            Stream stream = await Request.Content.ReadAsStreamAsync();

            bool hasHeader = true;
            //open xlsx file            
            var excel = new ExcelPackage(stream);
            var workbook = excel.Workbook;
            try
            {
                var sheet = excel.Workbook.Worksheets[1];
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                string msg = QuestionBankDAL.UploadList(tbl, Obj);
                return Request.CreateResponse(HttpStatusCode.Accepted, msg);


            }
            catch (Exception ex)//open xls file
            {
                //if its a .xls file it will throw an Exception  
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }




        }

        //--------------image upload---------------------//

        public async Task<HttpResponseMessage> UpdateImage_Practical()
        {
            string location = "";
            string fileName = "";
            string rtn = "";
            try
            {                
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                Stream reqStream = await Request.Content.ReadAsStreamAsync();
                MemoryStream tempStream = new MemoryStream();
                reqStream.CopyTo(tempStream);



                tempStream.Seek(0, SeekOrigin.End);
                StreamWriter writer = new StreamWriter(tempStream);
                writer.WriteLine();
                writer.Flush();
                tempStream.Position = 0;

                StreamContent streamContent = new StreamContent(tempStream);
                foreach (var header in Request.Content.Headers)
                {
                    streamContent.Headers.Add(header.Key, header.Value);
                }

                var root = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UploadImage"]);



                var provider = new MultipartFormDataStreamProvider(root);
                var result = await streamContent.ReadAsMultipartAsync(provider);
                string strPath = "";

                var model = result.FormData["data"];

                var Obj = JsonConvert.DeserializeObject<Practical_QuestionDetail>(model);

                if (!Directory.Exists(root))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(root);
                    //  dirInfo.CreateSubdirectory(dirInfo);
                }


                //get the files
                if (result.FileData.Count < 1)
                {
                    var response = QuestionBankDAL.UpdateImage_Parctical(Obj);
                    rtn = response;
                    //  return Request.CreateResponse(HttpStatusCode.Accepted, "1");
                }

                else
                {
                    for (int i = 0; i < result.FileData.Count; i++)
                    {
                        //fileName = result.FileData[i].Headers.Content.FileName;
                        fileName = result.FileData[i].Headers.ContentDisposition.Name.Trim('"');
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                            //                      extension = Path.GetExtension(fileupload, fileName);                               // self //
                            Match regex = Regex.Match(root, @"(.+) \((\d+)\)\.\w+");

                            if (regex.Success)
                            {
                                fileName = regex.Groups[1].Value;

                            }
                        }

                        try
                        {   
                            string fileType = ".jpeg";
                           
                            string filename = DateTime.Now.Ticks.ToString();
                            
                            string ImageName = filename + fileType;
                            string str;
                            str = filename.Substring(filename.Length - 1, 1);
                            
                            strPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["ImageLink"], filename + str + fileType);

                            location = root + "\\";
                            string ImageWithPath = string.Format("{0}{1}", filename + str, fileType);
                            File.Move(result.FileData[i].LocalFileName, Path.Combine(location, ImageWithPath));
                            if (fileName=="QuestionImage") {
                                Obj.Question_Image = strPath;
                            }
                            else if (fileName == "ActionAImage")
                            {
                                Obj.ActionA_Image = strPath;
                            }
                            else if (fileName == "ActionBImage")
                            {
                                Obj.ActionB_Image = strPath;
                            }
                            else if (fileName == "ActionCImage")
                            {
                                Obj.ActionC_Image = strPath;
                            }
                            else if (fileName == "ActionDImage")
                            {
                                Obj.ActionD_Image = strPath;
                            }
                            else if (fileName == "ActionEImage")
                            {
                                Obj.ActionE_Image = strPath;
                            }
                        }


                        catch (Exception dbEx)//System.Data.Entity.Validation.DbEntityValidationException dbEx
                        {
                            Exception raise = dbEx;
                            //foreach (var validationErrors in dbEx.EntityValidationErrors)
                            //{
                            //    foreach (var validationError in validationErrors.ValidationErrors)
                            //    {
                            //        string message = string.Format("{0}:{1}",
                            //            validationErrors.Entry.Entity.ToString(),
                            //            validationError.ErrorMessage);
                            //        // raise a new exception nesting  
                            //        // the current instance as InnerException  
                            //        raise = new InvalidOperationException(message, raise);
                            //    }
                            //}
                            throw raise;
                        }

                    }

                    var response = QuestionBankDAL.UpdateImage_Parctical(Obj);

                    rtn = response;

                }

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.OK, e.Message.ToString());

            }
        }

    }
}
