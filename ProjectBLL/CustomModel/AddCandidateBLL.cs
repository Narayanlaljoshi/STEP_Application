using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class AddCandidateBLL
    {       
        
        public string SessionID { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string MobileNo { get; set; }
        public string Location { get; set; }
        public string OrgType { get; set; }
        public string Organization { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string Region { get; set; }
        public string Venue { get; set; }
        public string Msg { get; set; }
    }
}
