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
    public class RtcMasterController : ApiController
    {
        [HttpGet]
        public List<RtcMasterBLL> GetAgencyList()
        {
            return RtcMaster.GetAgencyList();
        }
        [HttpPost]
        public string AddUpdateAgency([FromBody] RtcMasterBLL obj)
        {
            return RtcMaster.AddUpdateAgency(obj);
        }

        [HttpPost]
        public string SubmitFacultyList(List<FacultyDetailsBLL> info)
        {
            return RtcMaster.SubmitFacultyList(info);
        }
        [HttpGet]
        public List<FacultyDetailsBLL> GetFacultyDetailsList(int Agency_Id)
        {
            return RtcMaster.GetFacultyDetailsList(Agency_Id);
        }
        [HttpPost]
        public string DeleteFaculty(FacultyDetailsBLL info)
        {
            return RtcMaster.DeleteFaculty(info);
        }
        [HttpGet]
        public List<SessionIDListForAgency> GetSessionIDListForAgency(int Agency_Id)
        {
            return RtcMaster.GetSessionIDListForAgency(Agency_Id);
        }

        [HttpGet]
        public List<CandidateList> GetCandidateList(string SessionId)
        {
            return RtcMaster.GetCandidateList(SessionId);
        }
        [HttpPost]
        public string UpdateAttendance(List<CandidateList> Obj)
        {
            return RtcMaster.UpdateAttendance(Obj);
        }
        [HttpGet]
        public List<SessionIDListForAgency> GetSessionIDListForAgency_Mobile(int Agency_Id)
        {
            return RtcMaster.GetSessionIDListForAgency_Mobile(Agency_Id);
        }

        [HttpGet]
        public List<CandidateList> GetCandidateList_Mobile(string SessionId)
        {
            return RtcMaster.GetCandidateList_Mobile(SessionId);
        }
        [HttpPost]
        public string UpdateAttendance_Mobile(List<CandidateList> Obj)
        {
            return RtcMaster.UpdateAttendance_Mobile(Obj);
        }
    }
}