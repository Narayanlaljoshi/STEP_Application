using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class ReportInputBLL
    {
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<int> Faculty_Id { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int? ProgramType_Id { get; set; }
    }
    public class FilterBLL
    {
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> Set_Id { get; set; }
        public Nullable<int> LanguageMaster_Id { get; set; }

    }
    public class ReportFilter_Vendor
    {
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<int> Trainer_Id { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int? ProgramType_Id { get; set; }
        public string ManagerID { get; set; }
    }
}
