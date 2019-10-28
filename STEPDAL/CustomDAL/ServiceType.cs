using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBLL.CustomModel;
using STEPDAL.DB;

namespace STEPDAL.CustomDAL
{
    public class ServiceType
    {
        public static List<ServiceType_Position> GetServiceTypes()
        {
            List<ServiceType_Position> List = null;
            using (var Context = new CEIDBEntities())
            {
                var ReqData = Context.sp_GetServiceTypes().ToList();
                if (ReqData.Count != 0) {
                    List = ReqData.Select(x => new ServiceType_Position {
                        Id=x.Id,
                        Position=x.Position,
                        ServiceType=x.ServiceType
                    }).ToList();
                }
                return List;
            }
        }
    }
}
