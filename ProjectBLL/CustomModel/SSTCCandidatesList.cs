using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class SSTCCandidatesList
    {
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
        public Nullable<int> Marks { get; set; }
        public string SessionId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<bool> IsSubmitted { get; set; }
        public string Dealer_LocationCode { get; set; }
        public string DealerName { get; set; }
    }
    public class CandidatesList_StepAgency_Marks
    {
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
        public Nullable<int> Marks { get; set; }
        public string SessionId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<bool> IsSubmitted { get; set; }
        public string Dealer_LocationCode { get; set; }
        public string DealerName { get; set; }
    }
}
