using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class SessionIDListBLL
    {
        public Nullable<int> CurrentTestEvaluationType_Id { get; set; }
        public string ProgramCode { get; set; }
        public string AgencyCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> Duration { get; set; }
        public Nullable<int> day { get; set; }
        public int TestAssignable { get; set; }
        public Nullable<int> TotalStudents { get; set; }
        public bool IsChecked { get; set; }
        public int CreatedBy { get; set; }
        public string ProgramName { get; set; }
        public string TestCode { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> Faculty_Id { get; set; }
        public string FacultyCode { get; set; }
        public Nullable<bool> SameDayTestInitiation { get; set; }
        public Nullable<System.DateTime> TestIniateDate { get; set; }
        public Nullable<int> NextTestEvaluationType_Id { get; set; }
        public string NextTestEvaluationType { get; set; }
        public Nullable<int> ProgramType_Id { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
    }

    public class AttendanceBLL
    {
        public List<SessionIDListBLL> SessionIDList { get; set; }
        public List<StudentsList> StudentsList { get; set; }
    }
    public class SessionIDListForAgency
    {
        public string SessionID { get; set; }
        public bool IsChecked { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
    }
    public class SessionIdListForVendorTrainer {
        public string SessionID { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        //public List<RegionVenue> RegionVenues { get; set; }
        public string ProgramName { get; set; }
        public string Venue { get; set; }
        public string Region { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    }
    public class ActiveSessionIdListForVendorTrainer
    {
        public Nullable<int> Day { get; set; }
        public string Type { get; set; }
        public Nullable<DateTime> Date  { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public List<RegionVenue> RegionVenues { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> ProgramType_Id { get; set; }
    }
    public class RegionVenue
    {
        public string Region  { get; set; }
        public string Venue { get; set; }
        public string SessionID { get; set; }
    }
    public class PendingSessionIdListForVendorTrainer
    {
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string ProgramName { get; set; }
        public List<RegionVenue> RegionVenues { get; set; }
        public bool IsMarksSubmitted  { get; set; }
        public bool IsAttendanceSubmitted { get; set; }
        public bool IsClosable { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ProgramType_Id { get; set; }
    }

    public class PRG_SD_ED {
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string ProgramName { get; set; }
    }
    public class MRK_ATNDC_Check
    {
        public bool IsMarksSubmitted { get; set; }
        public bool IsAttendanceSubmitted { get; set; }
    }
}
