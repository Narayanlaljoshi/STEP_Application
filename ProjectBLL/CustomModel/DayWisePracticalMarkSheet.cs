using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class DayWisePracticalMarkSheet
    {
        public string MSPIN { get; set; }
        public string SessionID { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> ProgramTestCalenderId { get; set; }
        public Nullable<int> PracticalDefaultMarks { get; set; }
        public Nullable<int> PracticalMaxMarks { get; set; }
        public Nullable<int> PracticalMinMarks { get; set; }
        public string ActionA { get; set; }
        public string ActionB { get; set; }
        public string ActionC { get; set; }
        public string ActionD { get; set; }
        public string ActionE { get; set; }
        public string ActionF { get; set; }
        public int Marks_A { get; set; }
        public int Marks_B { get; set; }
        public int Marks_C { get; set; }
        public int Marks_D { get; set; }
        public int Marks_E { get; set; }
        public int Marks_F { get; set; }
        public Nullable<int> Total { get; set; }
        public string Question { get; set; }
    }

    public class PracticalMarksAndExcel {
        public List<DayWisePracticalMarkSheet> DayWisePracticalMarkSheet { get; set; }
        public string ExcelUrl { get; set; }
        public int MarksEarned { get; set; }
    }
}
