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
    
    public partial class sp_GetSessionIDList_Result
    {
        public Nullable<int> CurrentTestEvaluationType_Id { get; set; }
        public Nullable<int> NextTestEvaluationType_Id { get; set; }
        public string NextTestEvaluationType { get; set; }
        public Nullable<int> TotalStudents { get; set; }
        public Nullable<int> ProgramType_Id { get; set; }
        public string ProgramCode { get; set; }
        public string AgencyCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> Duration { get; set; }
        public int day { get; set; }
        public string TestCode { get; set; }
        public int TestAssignable { get; set; }
        public string ProgramName { get; set; }
        public Nullable<bool> SameDayTestInitiation { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
    }
}