using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDAL.CustomDAL
{
    public class SSTC
    {
        public static List<SSTCCandidatesList> GetCandidatesListForSSTC(string SessionId,int Day)
        {
            using (var Context= new CEIDBEntities())
            {
                List<SSTCCandidatesList> List = null;
                   var ReqData = Context.sp_GetCandidatesListForSSTC(SessionId,Day).ToList();

                if (ReqData.Count != 0)
                {
                    List = ReqData.Select(x => new SSTCCandidatesList
                    {
                        Day=x.Day,
                        AgencyCode=x.AgencyCode,
                        EndDate=x.EndDate,
                        Marks=x.Marks,
                        MSPIN=x.MSPIN,
                        StartDate=x.StartDate,
                        ProgramId=x.ProgramId,
                        SessionId=x.SessionId,
                        IsSubmitted=x.IsSubmitted,
                        
                    }).ToList();
                }
                return List;
            }

        }
        public static string UpDateMarksForSSTC(List<SSTCCandidatesList> Obj)
        {
            string Msg = string.Empty;
            using (var Context = new CEIDBEntities())
            {
                foreach (var row in Obj)
                {
                    if (row.Marks!=null) {
                        int Status = Context.sp_Inset_Update_TblSSTCMarks(row.MSPIN, row.SessionId, row.Day, row.Marks, null, row.ProgramId, row.CreatedBy, DateTime.Now);
                        if (Status < 1)
                        {
                            Msg = Msg + "Error";
                        }
                    }
                }
                if (Msg.Contains("Error"))
                {
                    return "Error, Please try Again with the correct data";
                }
                else {
                    return "Success: Score sheet updated successfully!";
                }
            }

        }
        public static string Update_InsertIntoTblAttendance_SSTC(AttendanceUpdateForSSTC Obj)
        {
            string Msg = string.Empty;
            if (Obj.CandidateList.Count != 0)
                Obj.CandidateList = Obj.CandidateList.Where(x => x.IsChecked == true).ToList();

            using (var Context = new CEIDBEntities())
            {
                foreach (var row in Obj.CandidateList)
                {
                    if (row.IsChecked != null)
                    {
                        int Status = Context.sp_Update_InsertIntoTblAttendance_SSTC(row.MSPIN, Obj.Date, Obj.SessionID, Obj.Day);
                        if (Status < 1)
                        {
                            Msg = Msg + "Error";
                        }
                    }
                }
                if (Msg.Contains("Error"))
                {
                    return "Error, Please try Again with the correct data";
                }
                else
                {
                    return "Success: Score sheet updated successfully!";
                }
            }

        }

        public static List<DaySequenceSSTC> GetDaySequenceSSTC(string SessionId)
        {
            using (var Context = new CEIDBEntities())
            {
                List<DaySequenceSSTC> List = null;
                var ReqData = Context.sp_GetDaySequence_SSTC(SessionId).ToList();

                if (ReqData.Count != 0)
                {
                    List = ReqData.Select(x => new DaySequenceSSTC
                    {
                        Day = x.DayCount,
                        ProgramId = x.ProgramId,
                        SessionId = x.SessionId,
                        Date=x.Date,
                        Weekday=x.Weekday,
                        DayDate=x.DayDate
                    }).ToList();
                }
                return List;
            }

        }
        public static string CloseSSTCCourse(SessionIDListForAgency Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                try {
                    var Check = Context.TblSSTC_CourseClosure.Where(x => x.SessionID == Obj.SessionID && x.IsActive == true).FirstOrDefault();
                    if (Check==null) {
                        TblSSTC_CourseClosure CC = new TblSSTC_CourseClosure {
                            IsActive=true,
                            ClosureDate=DateTime.Now,
                            CreatedBy=Obj.CreatedBy,
                            CreationDate=DateTime.Now,
                            SessionID=Obj.SessionID
                        };
                        Context.Entry(CC).State = System.Data.Entity.EntityState.Added;
                        Context.SaveChanges();
                        return "Success: Course Closed Successfully!";
                    }
                    else
                    { return "Success: Course Closed Already!"; }
                }
                catch (Exception Ex)
                {
                    return "Error: "+Ex.Message;
                }
            }
        }
    }
}
