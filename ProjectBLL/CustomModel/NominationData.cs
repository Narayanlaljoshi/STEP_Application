using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class NominationData
    {
        public string Co_id { get; set; }
        public string AgencyCode { get; set; }
        public string FacultyCode { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string MobileNo { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string Location { get; set; }
    }
}
