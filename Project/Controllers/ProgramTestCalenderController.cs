using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ProjectDAL.DBContext;
using ProjectBLL.CustomModel;
using ProjectDAL.CustomDAL;

using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class ProgramTestCalenderController : ApiController
    {


        [HttpGet]
        public List<ProgramMaster> GetProgramList()
        {
            return ProgramTestCalenderDAL.GetProgramList();
        }


        [HttpGet]
        public List<ProgramTestCalenderDetail_Model> GetProgramTestCalenderList(int ProgramId)
        {
            return ProgramTestCalenderDAL.GetProgramTestCalenderList(ProgramId);
        }



        [HttpPost]
        public string AddProgramTestCalender([FromBody] List<ProgramTestCalenderBLL> obj)
        {
            return ProgramTestCalenderDAL.AddProgramTestCalender(obj);
        }

        [HttpPost]
        public string UpdateProgramTestCalender(ProgramTestCalenderBLL info)
        {
            return ProgramTestCalenderDAL.UpdateProgramTestCalender(info);
        }


        [HttpPost]
        public async Task<HttpResponseMessage> UploadExcel()
        {
            //_commonDAL _userDAL = new _commonDAL();

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Stream stream = await Request.Content.ReadAsStreamAsync();

            bool hasHeader = true;
            //open xlsx file            
            var excel = new ExcelPackage(stream);
            var workbook = excel.Workbook;
            try
            {
                var sheet = excel.Workbook.Worksheets["ProgramTestCalender"];



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
                string msg = ProgramTestCalenderDAL.UploadList(tbl);
                return Request.CreateResponse(HttpStatusCode.Accepted, msg);


            }
            catch (Exception)//open xls file
            {
                //if its a .xls file it will throw an Exception  
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }




        }


    }
}
