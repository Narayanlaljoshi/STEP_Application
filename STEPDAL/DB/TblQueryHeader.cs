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
    
    public partial class TblQueryHeader
    {
        public int Id { get; set; }
        public string QuerySubject { get; set; }
        public string QueryBody { get; set; }
        public Nullable<int> CurrentStatus_Id { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public Nullable<int> IsClosed { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<int> ModifedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}