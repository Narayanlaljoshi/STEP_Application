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
    public class ReportsController : ApiController
    {
        [HttpPost]
        public List<AttendanceReportBLL> GetAttendanceReport(ReportInputBLL Obj)
        {
            return ReportsDAL.GetAttendanceReport(Obj);
        }
        [HttpPost]
        public ReportFilterBLL GetReportFilter() {
            return ReportsDAL.GetReportFilter();
        }
        [HttpPost]
        public List<MarksReportBLL> GetMarksReport(List<SessionIDListBLL> Obj)
        {
            return ReportsDAL.GetMarksReport(Obj);
        }
        [HttpPost]
        public List<DayWiseReportBLL> GetMarksReportForAdmin(SessionIDListBLL Obj)
        {
            return ReportsDAL.GetMarksReportForAdmin(Obj);
        }
        [HttpPost]
        public List<MarksReportBLL> GetMarksReportAsperDMS(SessionIDListBLL Obj)
        {
            return ReportsDAL.GetMarksReportAsperDMS(Obj);
        }
        [HttpPost]
        public List<DayWiseReportBLL> GetMarksReportForFaculty(List<SessionIDListBLL> Object)
        {
            return ReportsDAL.GetMarksReportForFaculty(Object);
        }
        [HttpPost]
        public PracticalMarksAndExcel GetMarksReport_Practical(EligibleCandidatesForEvaluation Obj)
        {
            return ReportsDAL.GetMarksReport(Obj);
        }
    }
}