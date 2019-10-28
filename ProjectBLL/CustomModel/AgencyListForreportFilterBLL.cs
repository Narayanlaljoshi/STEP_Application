using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class AgencyListForreportFilterBLL
    {
        public int Agency_Id { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }

    }
}
