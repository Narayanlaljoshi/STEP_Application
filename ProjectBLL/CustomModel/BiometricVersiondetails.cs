using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class BiometricVersiondetails
    {
        public string Machine_Name { get; set; }
        public string Current_Version { get; set; }
        public System.DateTime LastUpdatedOn { get; set; }
        public string LatestVersion { get; set; }
        public string Link { get; set; }
        public Nullable<bool> IsCurrentVersionLatest { get; set; }
    }
    public class BiometricVersiondetails_V2
    {
        public string Machine_Name { get; set; }
        public string Current_Version { get; set; }
        public string Current_CRC { get; set; }
        public int Mapped_Version_Id { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}
