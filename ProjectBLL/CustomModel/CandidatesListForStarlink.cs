using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class CandidatesListForStarlink
    {
        public string AgencyCode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public string ProgramCode { get; set; }
        public Nullable<int> Duration { get; set; }
        public string SessionID { get; set; }
        public string IsRegistered { get; set; }
        public string Picture { get; set; }
        public string DocumentID { get; set; }
    }
}
