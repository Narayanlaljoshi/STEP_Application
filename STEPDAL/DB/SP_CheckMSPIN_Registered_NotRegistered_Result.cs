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
    
    public partial class SP_CheckMSPIN_Registered_NotRegistered_Result
    {
        public string Name { get; set; }
        public string MSPIN { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public byte[] Thumb_1 { get; set; }
        public byte[] Thumb_2 { get; set; }
        public byte[] Candidate_Image { get; set; }
        public byte[] Document_Image { get; set; }
        public string IsRegistered { get; set; }
    }
}
