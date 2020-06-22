using Newtonsoft.Json;
using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ProjectUI.Controllers
{
    public class QueriesController : ApiController
    {
        [HttpGet]
        public List<QueriesBLL> GetAllQueries(int User_Id)
        {
            return QueriesDAL.GetAllQueries(User_Id);
        }
        [HttpPost]
        public string UpdateQuery(CloseQueryBLL obj)
        {
            return QueriesDAL.UpdateQuery(obj);
        }
        [HttpGet]
        public List<SummaryReport> GetSummaryReport(int User_Id)
        {
            return QueriesDAL.GetSummaryReport(User_Id);
        }
        //public string SaveQuery([FromBody]SaveQueryBLL obj)
        //{
        //    return QueriesDAL.SaveQuery(obj);
        //}
        [HttpGet]
        public List<StatusList> GetStatusList()
        {
            return QueriesDAL.GetStatusList();
        }
        [HttpGet]
        public EmployeeDetails GetEmployeeDetails(int Query_Id)
        {
            return QueriesDAL.GetEmployeeDetails(Query_Id);
        }
        [HttpGet]
        public IList<StatusdetailInfo> GetCurrentStatus(int Query_Id)
        {
            return QueriesDAL.GetCurrentStatus(Query_Id);
        }
        [HttpGet]
        public IList<FutureStatus> GetFutureStatus(int Query_Id)
        {
            return QueriesDAL.GetFutureStatus(Query_Id);
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SaveQuery()
        {
            string location = "";
            string fileName = "";
            string content = "";
            string extension = "";
            long repsonse = 0;

           
            string strPath;
            //   self  //      
            //// Check if the request contains multipart/form-data.
            //if (Request.Content.IsMimeMultipartContent())
            //{
            //       throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}

            QueryAttachementBLL obj = new QueryAttachementBLL();

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

            var root = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["QueryAttachmentPath"]);

            var provider = new MultipartFormDataStreamProvider(root);
            var result = await streamContent.ReadAsMultipartAsync(provider);

            var model = result.FormData["data"];

            var QueryObj = JsonConvert.DeserializeObject<SaveQueryBLL>(model);
            int Query_Id = 0;

            repsonse = QueriesDAL.SaveQuery(QueryObj, ref Query_Id);

            //get the files
            //if (result.FileData.Count < 1)
            //{
            //    return Request.CreateResponse(HttpStatusCode.Accepted, repsonse == 1 ? " Success : Query Saved Successfully" : "Error : Request can not be created please contact admin.");
            //}

            if (repsonse != 0)
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
                        //                      extension = Path.GetExtension(fileupload, fileName);                               // self //

                        Match regex = Regex.Match(root, @"(.+) \((\d+)\)\.\w+");

                        if (regex.Success)
                        {
                            fileName = regex.Groups[1].Value;
                        }
                    }

                    try
                    {
                        //string ImageName = DateTime.Now.Ticks.ToString();
                        // string fileType = Path.GetExtension(fileName);
                        string fileType = Path.GetExtension(fileName);// ".jpeg";
                        if (QueryObj.AttachmentType.IndexOf("video") != -1)
                        {
                            fileType = ".mp4";
                        }
                        string Extension = Path.GetFileNameWithoutExtension(fileName); //+ Guid.NewGuid().ToString();
                        string filename = DateTime.Now.Ticks.ToString();
                        //                 string extension = Path.GetExtension(fileName);
                        string str;
                        str = filename.Substring(filename.Length - 1, 1);
                        //number++;
                        strPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["ServerIP"], string.Format("{0}/{1}{2}", "QueryAttachment", filename + str + i, fileType));//extension));
                        location = root;
                        string AttachmentModifiedName = string.Format("{0}{1}", filename + str + i, fileType);
                        //strPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["ServerPath"], ImageName);//extension));
                        File.Move(result.FileData[i].LocalFileName, Path.Combine(location, AttachmentModifiedName));

                        obj.FileName = filename;
                        obj.FileExtension = fileType;
                        obj.AttachmentType = fileType.Equals(".mp4") ? "V" : "I";
                        obj.Path = strPath;
                        obj.Query_Id = Query_Id;
                        obj.User_Id = QueryObj.User_Id;

                        QueriesDAL.UploadQueryAttachement(obj);
                    }
                    catch (Exception e)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Error : something went wrong, please try again later.");
                    }
                }

                QueriesDAL.QueryEmail(Query_Id);
            }

            return Request.CreateResponse(HttpStatusCode.OK, repsonse == 1 ? " Success : Query Saved Successfully" : "Error : Request can not be created please contact admin.");

        }
    }
}
