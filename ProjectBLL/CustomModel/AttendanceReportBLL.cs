using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class AttendanceReportBLL
    {
        public string Co_id { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string AgencyCode { get; set; }
        public string FacultyCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public Nullable<int> Day { get; set; }
        public string P_A { get; set; }
    }
    public class AttendanceReport_Vendor
    {
        public string Co_id { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        //public Nullable<int> ProgramId { get; set; }
        //public string AgencyCode { get; set; }
        public string TrainerCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        //public string MobileNo { get; set; }
        public Nullable<int> Day { get; set; }
        public string P_A { get; set; }
    }
}
