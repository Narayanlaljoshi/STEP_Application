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
    
    public partial class sp_GetCandidatesListForSSTC_Result
    {
        public string MSPIN { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string SessionId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> Marks { get; set; }
        public Nullable<bool> IsSubmitted { get; set; }
        public string Name { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public string Dealer_LocationCode { get; set; }
        public string DealerName { get; set; }
    }
}
