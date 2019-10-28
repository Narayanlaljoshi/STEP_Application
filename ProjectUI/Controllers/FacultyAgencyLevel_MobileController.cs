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
    public class FacultyAgencyLevel_MobileController : ApiController
    {
        [HttpGet]
        public List<SessionIDListBLL> GetSessionIdList(int Agency_Id, string FacultyCode)
        {
            return FacultyAgencyLevel_Mobile.GetSessionIdList (Agency_Id, FacultyCode);
        }
        [HttpGet]
        public string GetFacultyName(string UserName)
        {
            return FacultyAgencyLevel_Mobile.GetFacultyName(UserName);
        }
        [HttpPost]
        public List<StudentsList> GetSessionWiseStudentsList(List<SessionIDListBLL> Obj)
        {
            return FacultyAgencyLevel_Mobile.GetSessionWiseStudentsList(Obj);
        }
        [HttpPost]
        public string ResetStudentLogin(StudentsList Obj)
        {
            return FacultyAgencyLevel_Mobile.ResetStudentLogin(Obj);
        }
        [HttpPost]
        public List<DayWiseReportBLL> GetMarksReportForFaculty(List<SessionIDListBLL> Object)
        {
            return FacultyAgencyLevel_Mobile.GetMarksReportForFaculty(Object);
        }
        [HttpGet]
        public List<StudentPostTestScoresBLL> GetStudentPostTestScores(string MSpin)
        {
            return FacultyAgencyLevel_Mobile.GetStudentPostTestScores(MSpin);
        }
        [HttpPost]
        public List<MarksReportBLL> GetMarksReport(List<SessionIDListBLL> Obj)
        {
            return FacultyAgencyLevel_Mobile.GetMarksReport(Obj);
        }
        [HttpPost]
        public string SaveTestInitiationDetailsWithAttendance(AttendanceBLL Obj)
        {
            return FacultyAgencyLevel_Mobile.SaveTestInitiationDetailsWithAttendance(Obj);
        }
    }
}
