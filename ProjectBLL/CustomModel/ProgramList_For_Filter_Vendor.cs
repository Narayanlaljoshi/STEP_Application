using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class ProgramList_For_Filter_Vendor
    {
        public string ProgramCode { get; set; }
        public Nullable<int> ProgramID { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ProgramName  { get; set; }
        public int? ProgramType_Id { get; set; }
    }
    public class SessionIdListForFilter
    {
        public string SessionID { get; set; }
    }
}
