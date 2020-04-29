using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class MultiNominationList
    {
        public string AgencyCode { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
    }
    public class MultiNominationDetails
    {
       
        public int Id { get; set; }
        public string Co_id { get; set; }
        public string Region { get; set; }
        public string Venue { get; set; }
        public string Dealer_LocationCode { get; set; }
        public string DealerName { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string AgencyCode { get; set; }
        public string FacultyCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string MobileNo { get; set; }
        public Nullable<bool> IsAccepted { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
