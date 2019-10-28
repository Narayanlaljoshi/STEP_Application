using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class EligibleCandidatesForEvaluation
    {
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public string SessionID { get; set; }
        public bool IsChecked { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<bool> IsEligible { get; set; }
        public Nullable<bool> IsResumable { get; set; }
        public int UserId { get; set; }
    }
}
