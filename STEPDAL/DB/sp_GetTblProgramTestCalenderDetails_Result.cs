//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STEPDAL.DB
{
    using System;
    
    public partial class sp_GetTblProgramTestCalenderDetails_Result
    {
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string TestCode { get; set; }
        public Nullable<int> DayCount { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> EvaluationTypeId { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public Nullable<int> Marks_Question { get; set; }
        public Nullable<int> Total_Marks { get; set; }
        public Nullable<int> Q_Bank { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<int> ValidDuration { get; set; }
        public Nullable<bool> TestInitiated { get; set; }
        public Nullable<System.DateTime> TestInitiatedDate { get; set; }
        public Nullable<System.TimeSpan> TestInitiatedTime { get; set; }
        public Nullable<int> PracticalDefaultMarks { get; set; }
        public Nullable<int> PracticalMaxMarks { get; set; }
        public Nullable<int> PracticalMinMarks { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
