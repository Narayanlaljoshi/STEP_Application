using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class StudentsList
    {
        public int Nomination_Id { get; set; }
        public string Co_id { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<int> Faculty_Id { get; set; }
        public string FacultyCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string MobileNo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> IsChecked{ get; set; }
        public Nullable<int> Day { get; set; }
        public int LogDay { get; set; }
        public Nullable<System.DateTime> LoginDateTime { get; set; }
        public Nullable<int> Status_Id { get; set; }
    }
    public class CandidateList 
    {
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<bool> IsPresent { get; set; }
        public string AgencyCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> LastDay { get; set; }
        public Nullable<System.DateTime> LastDate { get; set; }
    }
    public class CandidateList_SSTC
    {
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<bool> IsPresent { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Day { get; set; }
        public string SessionID { get; set; }
        public string Dealer_LocationCode { get; set; }
        public string DealerName { get; set; }
    }
    public class CandidateList_StepAgency_Attendance
    {
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<bool> IsPresent { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Day { get; set; }
        public string SessionID { get; set; }
        public string Dealer_LocationCode { get; set; }
        public string DealerName { get; set; }
        public int CreatedBy { get; set; }
    }
    public class AttendanceUpdateForStepAgency 
    {
        public List<CandidateList_StepAgency_Attendance> CandidateList{ get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Day { get; set; }
        //public string SessionID { get; set; }
        public int CreatedBy { get; set; }
    }
    public class AttendanceUpdateForSSTC
    {
        public List<CandidateList_StepAgency_Attendance> CandidateList { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Day { get; set; }
        //public string SessionID { get; set; }
        public int CreatedBy { get; set; }
    }
}
