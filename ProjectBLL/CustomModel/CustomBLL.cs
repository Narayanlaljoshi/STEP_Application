using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{

    public class ZoneModel
    {
        public int Zone_Id { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

    }


    public class zoNE_region_Model
    {
        public int Zone_Id { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public int Region_Id { get; set; }
    }

    public class RegionModel
    {
        public int Region_Id { get; set; }
        public Nullable<int> Zone_Id { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

    }


    public class Region_City
    {
        public int City_Id { get; set; }
        public string CityName { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
    }


    public class CityModel
    {
        public int City_Id { get; set; }
        public Nullable<int> Zone_Id { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }


    }

    public class ChannlModel
    { 

        public int Channel_Id { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

    }

    public class DealerGroupModel
    {

        public int DealerGroup_Id { get; set; }
        public Nullable<int> City_Id { get; set; }
        public string DealerGroupName { get; set; }
        public string DealerGroupCode { get; set; }
        public string DealerGroupAddress { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

    }

}
