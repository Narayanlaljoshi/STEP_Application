using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STEPDAL.CustomDAL;

namespace ProjectUI.Controllers
{
    public class BiometricReportsController : ApiController
    {
        [HttpGet]
        public void ExportExcel_RegistrationData()
        {
            BiometricReports.ExportExcel_RegistrationData();
        }
    }
}
