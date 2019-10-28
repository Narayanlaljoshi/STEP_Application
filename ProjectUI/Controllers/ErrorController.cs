using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectUI.Controllers
{
    public class ErrorController : ApiController
    {
        public List<UploadErrors> GetUploadError()
        {
            return ErrorReport.GetUploadError();
        }
        public string UpdateErrorRecord(List<UploadErrors> Obj)
        {
            return ErrorReport.UpdateErrorRecord(Obj);
        }
    }
}
