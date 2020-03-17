using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class DaySequenceSSTC
    {
        public string SessionId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Weekday { get; set; }
        public string DayDate { get; set; }
        public Nullable<int> Max_Marks { get; set; }
    }
}
