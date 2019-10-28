using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class CountersForStepAgencyTrainer
    {
        public Nullable<int> UpComing { get; set; }
        public Nullable<int> Active { get; set; }
        public Nullable<int> Pending { get; set; }
        public Nullable<int> Closed { get; set; }
    }
}
