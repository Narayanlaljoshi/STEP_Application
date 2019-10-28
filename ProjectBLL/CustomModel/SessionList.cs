using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class SessionList
    {
        public string SessionID { get; set; }
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public int? Faculty_Id { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> Duration { get; set; }
        public int User_Id { get; set; }
        public int FAC_User_Id { get; set; }
    }
    public partial class SessionListForStepAgencyManager 
    {
        public string SessionID { get; set; }
        public string FacultyCode { get; set; }
        public string TrainerName { get; set; }
        public Nullable<int> Id { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> Duration { get; set; }
        public string TrainerCode { get; set; }
        public string Venue { get; set; }
        public string Region { get; set; }
        public int User_Id { get; set; }
    }
    public class ActiveTrainerForVendor
    {
        public int Id { get; set; }
        public int Vendor_Id { get; set; }
        public string TrainerCode { get; set; }
        public string TrainerName { get; set; }
        public string TrainerMobile { get; set; }
        public string TrainerEmail { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
