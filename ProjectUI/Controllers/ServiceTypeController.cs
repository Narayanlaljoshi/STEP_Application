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
    public class ServiceTypeController : ApiController
    {
        [HttpGet]
        public List<ServiceType_Position> GetServiceTypes()
        {
            return ServiceType.GetServiceTypes();
        }
    }
}
