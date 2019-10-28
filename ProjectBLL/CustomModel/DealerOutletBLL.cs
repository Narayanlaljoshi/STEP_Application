using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public  class DealerOutletBLL
    {
        public int DealerOutlet_Id { get; set; }
        public Nullable<int> DealerGroup_Id { get; set; }
        public Nullable<int> OutletType_Id { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public Nullable<int> City_Id { get; set; }
        public Nullable<int> Channel_Id { get; set; }
        public string OutletCode { get; set; }
        public string DealerCode { get; set; }
        public string OutletName { get; set; }
        public string OutletAddress { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
    public partial class GetCity_Model
    {
        public int City_Id { get; set; }
        public string CityName { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
    }

    public partial class Sp_GetDealerOutletDetail
    {
        public int DealerOutlet_Id { get; set; }
        public string DealerCode { get; set; }
        public string OutletCode { get; set; }
        public string OutletName { get; set; }
        public string OutletAddress { get; set; }
        public string ChannelName { get; set; }
        public Nullable<int> Channel_Id { get; set; }
        public Nullable<int> City_Id { get; set; }
        public string CityName { get; set; }
        public Nullable<int> DealerGroup_Id { get; set; }
        public string DealerGroupName { get; set; }
        public string DealerGroupCode { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
    }

}
