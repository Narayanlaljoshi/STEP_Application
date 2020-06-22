using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class StatusList
    {
        public int Status_Id { get; set; }
        public string StatusName { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
