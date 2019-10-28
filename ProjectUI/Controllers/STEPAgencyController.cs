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
    public class STEPAgencyController : ApiController
    {
        [HttpGet]
        public List<ActiveSessionIdListForVendorTrainer> GetActiveSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            return STEPAgency.GetActiveSessionIDListForSTEP_Agency_Trainer_Mobile(Agency_Id, FacultyCode);
        }
        [HttpGet]
        public List<ActiveSessionIdListForVendorTrainer> GetUpcomingSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            return STEPAgency.GetUpcomingSessionIDListForSTEP_Agency_Trainer_Mobile(Agency_Id, FacultyCode);
        }
        [HttpGet]
        public List<PendingSessionIdListForVendorTrainer> GetPendingSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            return STEPAgency.GetPendingSessionIDListForSTEP_Agency_Trainer_Mobile(Agency_Id, FacultyCode);
        }//

        [HttpGet]
        public List<ActiveSessionIdListForVendorTrainer> GetClosedSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            return STEPAgency.GetClosedSessionIDListForSTEP_Agency_Trainer_Mobile(Agency_Id, FacultyCode);
        }
        [HttpGet]
        public CountersForStepAgencyTrainer GetCountersForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            return STEPAgency.GetCountersForSTEP_Agency_Trainer_Mobile(Agency_Id, FacultyCode);
        }
        /*Get Candidate List for marks screen*/
        [HttpPost]
        public List<CandidatesList_StepAgency_Marks> GetCandidatesListFor_Marks(ActiveSessionIdListForVendorTrainer Obj)//(string SessionID, int Day)
        {
            return STEPAgency.GetCandidatesListFor_Marks(Obj);
        }
        /*Update marks*/
        [HttpPost]
        public string UpDateMarksForStepAgency(List<CandidatesList_StepAgency_Marks> Obj)
        {
            return STEPAgency.UpDateMarksForStepAgency(Obj);
        }
        /*Get Candidate List For attendance screen*/
        [HttpPost]
        public List<CandidateList_StepAgency_Attendance> GetCandidateListFor_Attendance(ActiveSessionIdListForVendorTrainer Obj)//(string SessionID, int Day)
        {
            return STEPAgency.GetCandidateListFor_Attendance(Obj);
        }
        /*Update Attendance*/
        [HttpPost]
        public string UpdateAttendanceForStepAgency([FromBody]List<CandidateList_StepAgency_Attendance> Obj)
        {
            //if (Obj == null)
            //    return "Object received is null";
            //else if (Obj.Count==0)
            //    return "Object received is with zero rows";
            //else
            //    return "Object received is with "+ Obj.Count;

            return STEPAgency.UpdateAttendanceForStepAgency(Obj);
        }

        /*Course Closure*/
        [HttpPost]
        public string CloseCourse_StepAgency([FromBody]PendingSessionIdListForVendorTrainer Obj)
        {
            return STEPAgency.CloseCourse_StepAgency(Obj);
        }

        /*Get Day Sequence*/
        [HttpPost]
        public List<DaySequenceSSTC> GetDaySequence([FromBody]ActiveSessionIdListForVendorTrainer Obj)
        {
            return STEPAgency.GetDaySequence(Obj);
        }
        /*Get Day Sequence*/
        [HttpPost]
        public List<DaySequenceSSTC> GetDaySequence_V2([FromBody]ActiveSessionIdListForVendorTrainer Obj)
        {
            return STEPAgency.GetDaySequence_V2(Obj);
        }
    }
}
