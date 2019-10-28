using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class TestCodes
    {
        public Nullable<int> EvaluationTypeId { get; set; }
        public string EvaluationType { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> Day { get; set; }
        public string TestCode { get; set; }
    }
}
