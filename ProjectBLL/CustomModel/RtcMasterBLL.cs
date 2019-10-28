using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class RtcMasterBLL
    {
        public int Agency_Id { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string RTMName { get; set; }
        public string RTMCode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> AgenyType { get; set; }
    }
}
