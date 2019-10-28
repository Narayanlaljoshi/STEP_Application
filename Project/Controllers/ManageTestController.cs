using ProjectBLL.CustomModel;
using ProjectDAL.CustomDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.Controllers
{
    public class ManageTestController : ApiController
    {
        public List<SessionIdForResetTestBLL> GetSessionIdForTestReset()
        {
            return ManageTestDAL.GetSessionIdForTestReset();
        }
        public string UpdateSessionIdForTestReset(SessionIdForResetTestBLL Obj)
        {
            return ManageTestDAL.UpdateSessionIdForTestReset(Obj);
        }
    }
}
