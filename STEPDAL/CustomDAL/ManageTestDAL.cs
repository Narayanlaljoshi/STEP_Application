using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEPDAL.DB;

namespace STEPDAL.CustomDAL
{
    public class ManageTestDAL
    {
        public static List<SessionIdForResetTestBLL> GetSessionIdForTestReset()
        {
            List<SessionIdForResetTestBLL> SessionIdForResetTest = null;
            using (var Context = new CEIDBEntities())
            {
                var ReqData = Context.sp_GetSessionIdForResetTest().ToList();
                if (ReqData != null)
                {
                    SessionIdForResetTest = ReqData.Select(x => new SessionIdForResetTestBLL
                    {
                        Day = x.Day,
                        Duration = x.Duration,
                        SameDayTestInitiation = x.SameDayTestInitiation,
                        SessionID = x.SessionID,
                        TestDetail_Id = x.TestDetail_Id,

                    }).ToList();

                }
                return SessionIdForResetTest;
            }
        }
        public static string UpdateSessionIdForTestReset(SessionIdForResetTestBLL Obj)
        {            
            using (var Context = new CEIDBEntities())
            {
                int TestDetail_Id = Obj.TestDetail_Id;
                var ReqData = Context.TblTestDtls.Where(x => x.TestDetail_Id == TestDetail_Id).FirstOrDefault();
                if (ReqData != null)
                {
                    ReqData.SameDayTestInitiation = true;

                    Context.Entry(ReqData).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();
                }
                return "Success: Updated successfully";
            }
        }

    }
}
