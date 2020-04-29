using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class StudentPostTestScoresBLL
    {
        public Nullable<int> ProgramId { get; set; }
        public string MSPIN { get; set; }
        public string TypeOfTest { get; set; }
        public string SessionID { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> PostTest_MarksObtained { get; set; }
        public Nullable<int> PostTestMaxMarks { get; set; }
        public Nullable<int> Set_Id { get; set; }
    }
}
