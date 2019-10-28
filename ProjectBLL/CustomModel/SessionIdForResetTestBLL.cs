using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class SessionIdForResetTestBLL
    {
        public int TestDetail_Id { get; set; }
        public string SessionID { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> Duration { get; set; }
        public Nullable<bool> SameDayTestInitiation { get; set; }
    }
    public partial class CurrentSessionIdsForReset
    {
        public string SessionID { get; set; }
        public string ProgramCode { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<int> ValidDuration { get; set; }
        public Nullable<int> Day { get; set; }
    }
}
