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
    public class TestResetController : ApiController
    {
        [HttpGet]
        public List<CurrentSessionIdsForReset> GetCurrentSessionIDsForReset()
        {
            return TestReset.GetCurrentSessionIDsForReset();
        }
        [HttpPost]
        public string ResetForWholeSession(CurrentSessionIdsForReset Obj)
        {
            return TestReset.ResetForWholeSession(Obj);
        }
    }
}
