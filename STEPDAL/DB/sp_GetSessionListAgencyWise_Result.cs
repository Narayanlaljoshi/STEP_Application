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
    
    public partial class sp_GetSessionListAgencyWise_Result
    {
        public string SessionID { get; set; }
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public Nullable<int> Faculty_Id { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> Duration { get; set; }
    }
}
