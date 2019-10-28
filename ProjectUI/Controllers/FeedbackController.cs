using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STEPDAL.CustomDAL;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using Newtonsoft.Json;
using System.Data;

namespace ProjectUI.Controllers
{
    public class FeedbackController : ApiController
    {

        [HttpPost]
        public string CaptureFeedback(List<FeedbackQuestionsSet> Obj)
        {
            return FeedbackDAL.CaptureFeedback(Obj);
        }
        [HttpGet]
        public List<FeedbackQuestionsSet> GetFeedbackQuestion(int ProgramId, string SessionID, string MSPIN)
        {
            return FeedbackDAL.GetFeedbackQuestion(ProgramId,SessionID,MSPIN);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UploadExcel()
        {
            try
            {
                var root = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelPath"]);
                var provider = new MultipartFormDataStreamProvider(root);
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                FeedbackModelData ObjD = new FeedbackModelData();
                string location = "";
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                var reqMesg = Request.GetRequestContext();
                if (Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                string illegal = provider.FileData[0].Headers.ContentDisposition.FileName;
                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

                foreach (char c in invalid)
                {
                    illegal = illegal.Replace(c.ToString(), "");
                }

                FileInfo finfo = new FileInfo(illegal);
                string fileType = Path.GetExtension(finfo.Name);

                string filename = DateTime.Now.Ticks.ToString();

                string ExlName = filename + fileType;
                string FileWithPath = string.Format("{0}{1}", filename, fileType);
                location = Path.Combine(root, FileWithPath);
                System.IO.File.Move(provider.FileData.First().LocalFileName, Path.Combine(root, FileWithPath));

                var package = new ExcelPackage(new FileInfo(location));
                var model = result.FormData["data"];
                ObjD = JsonConvert.DeserializeObject<FeedbackModelData>(model);

                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                DataTable dt = new DataTable();

                dt = GetDataTableFromExcel(location, true);
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();
                var response = FeedbackDAL.InsertQuestions(dt, ObjD);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }




        private DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = System.IO.File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets[1];
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    if (firstRowCell.Text != "")
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        try
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                return tbl;
            }
        }

    }
}
