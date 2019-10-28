using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.Controllers
{
    public class FacultyAgencyLevelController : ApiController
    {
        [HttpGet]
        public FaciltyProgram GetFaciltyProgramdetails(string FacultyCode)
        {
            return FacultyAgencyLevel.GetFaciltyProgramdetails(FacultyCode);
        }
        [HttpGet]
        public List<SessionIDListBLL> GetSessionIdList(int Agency_Id,string FacultyCode)
        {
            return FacultyAgencyLevel.GetSessionIdList(Agency_Id,FacultyCode);
        }
        
        [HttpGet]
        public List<ProgramMasterBLL> GetProgramsList_Evaluation()
        {
            return FacultyAgencyLevel.GetProgramsList_Evaluation();
        }
        //[HttpGet]
        //public List<ProgramMasterBLL> GetProgramsList_Evaluation_Mobile()
        //{
        //    return FacultyAgencyLevel.GetProgramsList_Evaluation_Mobile();
        //}
        [HttpPost]
        public List<StudentsList> GetSessionWiseStudentsList(List<SessionIDListBLL> Obj)
        {
            return FacultyAgencyLevel.GetSessionWiseStudentsList(Obj);
        }
        [HttpPost]
        public string AddCandidate(List<AddCandidateBLL> Obj)
        {
            return FacultyAgencyLevel.AddCandidate(Obj);
        }
        [HttpPost]
        public string AddCandidate_Mobile(AddCandidateBLL Obj)
        {
            return FacultyAgencyLevel.AddCandidate_Mobile(Obj);
        }
        [HttpGet]
        public string GetFacultyName(string UserName)
        {
            return FacultyAgencyLevel.GetFacultyName(UserName);
        }
        [HttpPost]
        public string ResetStudentLogin(StudentsList Obj)
        {
            return FacultyAgencyLevel.ResetStudentLogin(Obj);
        }
        [HttpPost]
        public List<TestCodes> GetTestCodes(ProgramMasterBLL Obj)
        {
            return FacultyAgencyLevel.GetTestCodes(Obj);
        }
        [HttpPost]
        public List<EligibleCandidatesForEvaluation> GetEligibleCandidatesForEvaluation(TestCodes Obj)
        {
            return FacultyAgencyLevel.GetEligibleCandidatesForEvaluation(Obj);
        }
    }
}