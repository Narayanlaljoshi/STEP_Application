using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STEPDAL.CustomDAL;

namespace ProjectUI.Controllers
{
    public class SSTCController : ApiController
    {
        [HttpGet]
        public List<SSTCCandidatesList> GetCandidatesListForSSTC(List<SessionIDListForAgency> List)
        {
            return SSTC.GetCandidatesListForSSTC(List);
        }
        [HttpPost]
        public string UpDateMarksForSSTC(List<SSTCCandidatesList> Obj)
        {
            return SSTC.UpDateMarksForSSTC(Obj);
        }

        [HttpGet]
        public List<DaySequenceSSTC> GetDaySequenceSSTC(string SessionId)
        {
            return SSTC.GetDaySequenceSSTC(SessionId);
        }
        [HttpPost]
        public string CloseSSTCCourse(List<SessionIDListForAgency> Obj)
        {
            return SSTC.CloseSSTCCourse(Obj);
        }
        //[HttpPost]
        //public string Update_InsertIntoTblAttendance_SSTC(AttendanceUpdateForSSTC Obj)
        //{
        //    return SSTC.Update_InsertIntoTblAttendance_SSTC(Obj);
        //}
    }
}
