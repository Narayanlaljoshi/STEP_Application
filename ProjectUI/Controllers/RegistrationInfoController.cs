using ProjectBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STEPDAL.CustomDAL;
using ProjectBLL.CustomModel;

namespace ProjectUI.Controllers
{
    public class RegistrationInfoController : ApiController
    {
        [HttpGet]
        public RegistrationInfoBLL GetData(string mspin)
        {
            return RegistrationInfoDal.GetData(mspin);
        }

        [HttpPost]
        public string Reset(RegistrationInfoBLL Obj)
        {
            return RegistrationInfoDal.Reset(Obj);
        }
    }

}
