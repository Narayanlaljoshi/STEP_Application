using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class AttendancePunchDtl
    {
        public string MSPIN { get; set; }
        public string AgencyCode { get; set; }
        public System.DateTime DateTime { get; set; }
        public string MachineCode { get; set; }
        public Nullable<int> MachineId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
