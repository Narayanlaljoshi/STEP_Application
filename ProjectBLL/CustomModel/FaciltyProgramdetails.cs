using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class FaciltyProgramdetails
    {
        public int ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public bool IsActive { get; set; }
        public string AgencyCode { get; set; }
        public string FacultyCode { get; set; }
        //public string SessionID { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> DayCount { get; set; }
        public int Agency_Id { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<System.DateTime> LastTestDate { get; set; }
        public Nullable<System.DateTime> TestInitiatedDate { get; set; }
        public Nullable<System.TimeSpan> TestInitiatedTime { get; set; }
    }
    public class StudentList
    {
        public int Nomination_Id { get; set; }
        public string Co_id { get; set; }
        public string AgencyCode { get; set; }
        public string FacultyCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public DateTime StartDate { get; set; }
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
        public int PreTest_MarksObtained { get; set; }
        public int PostTest_MarksObtained { get; set; }
        public int IsPresentInPostTest { get; set; }
        public int IsPresentInPreTest { get; set; }

        public Nullable<int> PreTestMaxMarks { get; set; }
        public Nullable<int> PostTestMaxMarks { get; set; }





    }

    public class FaciltyProgram
    {
        public List<StudentList> StudentList { get; set; }
        public FaciltyProgramdetails FaciltyProgramdetails { get; set; }
    }
}
