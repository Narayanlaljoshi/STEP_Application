using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class UserDetailsBLL
    {
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<int> Zone_Id { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public Nullable<int> City_Id { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public Nullable<int> DealerGroup_Id { get; set; }
        public Nullable<int> DealerOutlet_Id { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string ZoneName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string DealerGroupName { get; set; }
        public string OutletName { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public bool AlreadyLoggedIn { get; set; }
        public string LogKey { get; set; }
        public string Status { get; set; }
        public string ErrorMsg   { get; set; }
    }
    public class UserDetailsBLL_SSTC
    {
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<int> Zone_Id { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public Nullable<int> City_Id { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public Nullable<int> DealerGroup_Id { get; set; }
        public Nullable<int> DealerOutlet_Id { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string ZoneName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string DealerGroupName { get; set; }
        public string OutletName { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public bool AlreadyLoggedIn { get; set; }
        public string LogKey { get; set; }
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
        public string Token { get; set; }
        public string TrainerName { get; set; }
        public string VendorName { get; set; }
    }
    public class UserDetailForBioMetric
    {
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Agency_Id { get; set; }
    }
}
