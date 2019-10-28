using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STEPDAL.CustomDAL;

namespace Project.Controllers
{
    public class AttendancePunchController : ApiController
    {
        [HttpPost]
        public bool SavePunchInDetails(List<AttendancePunchDtl> Obj)
        {
            return AttendancePunch.SavePunchInDetailsList(Obj);
        }
        [HttpPost]
        public bool SavePunchInDetailsSingle(AttendancePunchDtl Obj)
        {
            return AttendancePunch.SavePunchInDetails(Obj);
        }

        public List<CandidatesListForStarlink> GetCandidatesList()
        {
            return AttendancePunch.GetCandidatesList();
        }
        [HttpPost]
        public bool PunchInByRTM(AttendancePunchDtl Obj)
        {
            return AttendancePunch.SavePunchInDetails(Obj);
        }
    }
}
