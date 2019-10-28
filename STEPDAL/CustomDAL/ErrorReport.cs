using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEPDAL.CustomDAL
{
    public class ErrorReport
    {
        public static List<UploadErrors> GetUploadError()
        {
            List<UploadErrors> List = null;
            using (var Context = new CEIDBEntities())
            {
                var ReqData = Context.sp_GetUploadErrors().ToList();
                if (ReqData.Count != 0)
                {
                    List = ReqData.Select(x => new UploadErrors {
                        ColumnName=x.ColumnName,
                        Date=x.Date,
                        IsActive=x.IsActive,
                        Value=x.Value,
                        IsResolved=false
                    }).ToList();
                }
                return List;
            }
        }

        public static string UpdateErrorRecord(List<UploadErrors> Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                foreach (var Item in Obj)
                {
                    if (Item.IsResolved == true)
                    {
                        var Status = Context.sp_UpdadeUploadErrors(Item.ColumnName, Item.Value, Item.Date, false);
                    }
                }

                return "Success: Updateted Successfully!";
            }
        }
    }
}
