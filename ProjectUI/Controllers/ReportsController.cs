﻿using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;
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
        /*Attendance report as per DMS*/
        [HttpPost]
        public List<AttendanceReportBLL> GetAttendanceReport(ReportInputBLL Obj)
        {
            //return ReportsDAL.GetAttendanceReport(Obj);
            return ReportsDAL.GetAttendanceReport_V2(Obj);
        }
        [HttpPost]
        public ReportFilterBLL GetReportFilter()
        {
            return ReportsDAL.GetReportFilter();
        }
        [HttpPost]
        public IList<FacultyList> GetFacultyListByFilters(ReportInputBLL Obj)
        {
            return ReportsDAL.GetFacultyListByFilters(Obj);
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
        public List<ProgramListForReportInput> GetProgramList_From_To_Date(ReportInputBLL Obj)
        {
            return ReportsDAL.GetProgramList_From_To_Date(Obj);
        }
        [HttpPost]
        public List<SessionIdListForFilter> GetSessionList_ProgramWise(ReportInputBLL Obj)
        {
            return ReportsDAL.GetSessionList_ProgramWise(Obj);
        }
        /*Marks report as per DMS*/
        [HttpPost]
        public List<MarksReportBLL> GetMarksReportAsperDMS(SessionIDListBLL Obj)
        {
            return ReportsDAL.GetMarksReportAsperDMS_V2(Obj);
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
        [HttpGet]
        public Boolean GenerateMarksReportAsperDMS_Phase_2()
        {
            return ReportsDAL.GenerateMarksReportAsperDMS_Phase_2();
        }
        /*Attendance report as per Vendor*/
        [HttpPost]
        public List<DayWiseReportVendor> GetVendorAttendanceReport(ReportFilter_Vendor Obj)
        {
            return ReportsDAL.GetVendorAttendanceReport(Obj);
        }
        /*Attendance report as per Vendor*/
        [HttpPost]
        public List<DayWiseReportVendor> GetVendorMarksReport(ReportFilter_Vendor Obj)
        {
            return ReportsDAL.GetVendorMarksReport(Obj);
        }
        /*Get Report Filters*/
        [HttpPost]
        public List<ProgramList_For_Filter_Vendor> GetProgramList(ReportFilter_Vendor Obj)
        {
            return ReportsDAL.GetProgramList(Obj);
        }

        [HttpPost]
        public List<SessionIdListForFilter> GetSessionList(ReportFilter_Vendor Obj)
        {
            return ReportsDAL.GetSessionList(Obj);
        }
        [HttpPost]
        public IList<ActiveTrainerForVendor> GetTrainerForFilter(ReportFilter_Vendor Obj)
        {
            return ReportsDAL.GetTrainerForFilter(Obj);
        }
    }
}