using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class MarksReportBLL
    {
        public string Co_id { get; set; }
        public string FacultyCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<int> PreTest_MarksObtained { get; set; }
        public Nullable<int> PostTest_MarksObtained { get; set; }
        public int IsPresentInPostTest { get; set; }
        public int IsPresentInPreTest { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> PreTestMaxMarks { get; set; }
        public Nullable<int> PostTestMaxMarks { get; set; }
        public Nullable<int> PreTotalMarks { get; set; }
        public Nullable<int> PostTotalMarks { get; set; }
        public Nullable<double> PreTestPercentage { get; set; }
        public Nullable<double> PostTestPercentage { get; set; }
        //public string Co_id { get; set; }
        //public Nullable<int> Agency_Id { get; set; }
        //public Nullable<int> ProgramId { get; set; }
        //public string AgencyCode { get; set; }
        //public string FacultyCode { get; set; }
        //public string ProgramCode { get; set; }
        //public string SessionID { get; set; }
        //public Nullable<System.DateTime> StartDate { get; set; }
        //public Nullable<System.DateTime> EndDate { get; set; }
        //public Nullable<int> Duration { get; set; }
        //public string MSPIN { get; set; }
        //public string Name { get; set; }
        //public string MobileNo { get; set; }
        //public Nullable<int> Day { get; set; }
        //public string P_A { get; set; }
        //public Nullable<int> TotalRightQuestion { get; set; }
        //public Nullable<int> TotalRight_Marks { get; set; }
        //public Nullable<int> Total_Marks { get; set; }
        //public Nullable<int> Marks_Question { get; set; }
        //public string TypeOfTest { get; set; }
        //public Nullable<bool> IsAnswerCorrect { get; set; }
    }
    public class MarksReport_Vendor
    {
        public string Co_id { get; set; }
        public string FacultyCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<int> PreTest_MarksObtained { get; set; }
        public Nullable<int> PostTest_MarksObtained { get; set; }
        public int IsPresentInPostTest { get; set; }
        public int IsPresentInPreTest { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> PreTestMaxMarks { get; set; }
        public Nullable<int> PostTestMaxMarks { get; set; }
        public Nullable<int> PreTotalMarks { get; set; }
        public Nullable<int> PostTotalMarks { get; set; }
        public Nullable<double> PreTestPercentage { get; set; }
        public Nullable<double> PostTestPercentage { get; set; }
    }
    public class DayWiseScoreBLL
    {
        public Nullable<int> DayCount { get; set; }
        public Nullable<int> Total_Marks { get; set; }
        public Nullable<int> IsPresent { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> TotalQuestion { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> studentmrks { get; set; }
      
    }
    public class DayWiseReportBLL
    {
        public string MSPIN { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public string SessionID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string ProgramCode { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public string FacultyName { get; set; }
        public List<DayWiseScoreBLL> DayWiseScore { get; set; }
    }
    public class DayWiseReportVendor
    {
        public string MSPIN { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public string SessionID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string ProgramCode { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public string TrainerCode { get; set; }
        public List<DayWiseScore_Vendor> DayWiseScore { get; set; }
    }
    public class DayWiseScore_Vendor
    {
        public Nullable<int> DayCount { get; set; }
        public Nullable<int> Total_Marks { get; set; }
        public string IsPresent { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> TotalQuestion { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> studentmrks { get; set; }
    }
    //public class DayWiseReportBLL
    //{
    //    public StudentDetailBLL StudentDetail { get; set; }

    //    public List<DayWiseScoreBLL> DayWiseScore { get; set; }
    //}
}
