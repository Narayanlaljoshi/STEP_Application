using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBLL.CustomModel;
using STEPDAL.DB;

namespace STEPDAL.CustomDAL
{
    public class TestReset
    {
        public static List<CurrentSessionIdsForReset> GetCurrentSessionIDsForReset()
        {
            List<CurrentSessionIdsForReset> List = null;
            using (var Context = new CEIDBEntities())
            {
                var Getdata = Context.sp_GetCurrentSessionIdsForReset().ToList();
                if (Getdata.Count != 0)
                {
                    List = Getdata.Select(x => new CurrentSessionIdsForReset
                    {
                        Day = x.Day,
                        ProgramCode = x.ProgramCode,
                        SessionID = x.SEssionID,
                        TestDuration = x.TestDuration,
                        ValidDuration = x.ValidDuration
                    }).ToList();
                }
                return List;
            }
        }

        public static string ResetForWholeSession(CurrentSessionIdsForReset Obj)
        {
            int Status = 0;
            using (var Context = new CEIDBEntities())
            {
                if (Obj != null)
                {
                    Status = Context.sp_ResetTestForSessionId(Obj.SessionID,Obj.TestDuration,Obj.Day);
                }
                return Status >= 1 ? "Success: Test reset done, kindly ask trainer to re-initiate the test":"Error: There was some error, Kindly contact admin";
            }
        }
    }
}
