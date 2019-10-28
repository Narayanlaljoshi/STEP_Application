using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class VendorList
    {
        public int Id { get; set; }
        public string FAC_Code { get; set; }
        public string VendorName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerMobile { get; set; }
        public string ManagerID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
    public class VendorTrainerList
    {
        public int Id { get; set; }
        public int Vendor_Id { get; set; }
        public string TrainerCode { get; set; }
        public string TrainerName { get; set; }
        public string TrainerMobile { get; set; }
        public string TrainerEmail { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string VendorName { get; set; }
    }
}
