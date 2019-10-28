using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class UploadErrors
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsResolved { get; set; }
    }
}
