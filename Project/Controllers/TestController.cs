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
    public class TestController : ApiController
    {
        [HttpPost]
        public string SaveInitiatedTestDetails(FaciltyProgramdetails Obj)
        {
            return TestDAL.SaveInitiatedTestDetails(Obj);
        }
        [HttpPost]
        public string SaveTestInitiationDetails(List<SessionIDListBLL> Obj)
        {
            return TestDAL.SaveTestInitiationDetails(Obj);
        }
        [HttpGet]
        public StudenttestDetailsBLL GetStudenttestDetails(string MSPin)
        {
            return TestDAL.GetStudenttestDetails(MSPin);
        }
        [HttpGet]
        public List<StudenttestDetailsBLL> GetStudenttestDetails_Practical(string MSPin)
        {
            return TestDAL.GetStudenttestDetails_Practical(MSPin);
        }
        [HttpGet]
        public List<StudentTestQuestionsBLL> GetStudentTestQuestions(int ProgramTestCalenderId)
        {
            return TestDAL.GetStudentTestQuestions(ProgramTestCalenderId);
        }
        [HttpPost]
        public string SaveTestResponseComplete(StudentTestResponse Obj)
        {
            return TestDAL.SaveTestResponse(Obj);
        }
        [HttpPost]
        public string SaveTestResponse(StudentTestResponse2 Obj)
        {
            return TestDAL.SaveTestResponse2(Obj);
            //return "Error: Response Not Saved!";
        }

        [HttpGet]
        public List<StudenttestDetailsBLL> Get_Check(string MSpin, int day)
        {
            return TestDAL.Get_Check(MSpin, day);
        }
        [HttpGet]
        public List<StudentPostTestScoresBLL> GetStudentPostTestScores(string MSpin)
        {
            return TestDAL.GetStudentPostTestScores(MSpin);
        }
        [HttpPost]
        public StudentTestResponse GetStudentQuestionFormatedList(QuestionVariable data)
        {
            return TestDAL.GetStudentQuestionFormatedList(data);
        }
        [HttpPost]
        public StudentTestResponse_Practical GetStudentQuestionFormatedList_Practical(QuestionVariable data)
        {
            return TestDAL.GetStudentQuestionFormatedList_Practical(data);
        }
        [HttpPost]
        public string SaveTestInitiationDetailsWithAttendance(AttendanceBLL Obj)
        {
            return TestDAL.SaveTestInitiationDetailsWithAttendance(Obj);
        }
        [HttpPost]
        public string SaveTestResponse_Practical(StudentLanguageQuestion_Practical Obj)
        {
            return TestDAL.SaveTestResponse_Practical(Obj);
        }
        /*For Mobile App*/
        [HttpPost]
        public string SaveTestResponse_Mobile(StudentTestResponse Obj)
        {
            return TestDAL.SaveTestResponse_Mobile(Obj);
        }
        [HttpGet]
        public StudenttestDetailsBLL GetStudenttestDetails_Mobile(string MSPin)
        {
            return TestDAL.GetStudenttestDetails_Mobile(MSPin);
        }
        [HttpPost]
        public List<StudentLanguageQuestion> GetStudentQuestionFormatedList_Mobile(QuestionVariable data)
        {
            return TestDAL.GetStudentQuestionFormatedList_Mobile(data);
        }
        [HttpPost]
        public string InitiateTestForEvaluation_Theory(List<EligibleCandidatesForEvaluation> Obj)
        {
            return TestDAL.InitiateTestForEvaluation_Theory(Obj);
        }
        [HttpPost]
        public string InitiateTestForEvaluation_Practical(EligibleCandidatesForEvaluation Obj)
        {
            return TestDAL.InitiateTestForEvaluation_Practical(Obj);
        }
        [HttpPost]
        public string CheckIfAnyTestIsGoingOn(EligibleCandidatesForEvaluation Obj)
        {
            return TestDAL.CheckIfAnyTestIsGoingOn(Obj);
        }
        [HttpPost]
        public string List_CheckIfAnyTestIsGoingOn(List<EligibleCandidatesForEvaluation> Obj)
        {
            return TestDAL.CheckIfAnyTestIsGoingOn(Obj);
        }
    }
}