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
    using System.Collections.Generic;
    
    public partial class TblTestHdr
    {
        public int Test_Id { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> TrainingCenter_Id { get; set; }
        public string FacultyCode { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<bool> TestInitiated { get; set; }
        public string Duration { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
