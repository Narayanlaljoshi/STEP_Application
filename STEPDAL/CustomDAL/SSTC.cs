using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace STEPDAL.CustomDAL
{
    public class SSTC
    {
        public static List<SSTCCandidatesList> GetCandidatesListForSSTC(List<SessionIDListForAgency> List)
        {
            using (var Context = new CEIDBEntities())
            {
                Context.Database.CommandTimeout = 1200;
                List<SSTCCandidatesList> List1 = new List<SSTCCandidatesList>();
                foreach (var row in List) {
                    var ReqData = Context.sp_GetCandidatesListForSSTC(row.SessionID , row.Day).ToList();

                    if (ReqData.Count != 0)
                    {
                        List1.AddRange(ReqData.Select(x => new SSTCCandidatesList
                        {
                            Day = x.Day,
                            AgencyCode = x.AgencyCode,
                            EndDate = x.EndDate,
                            Marks = x.Marks,
                            Name = x.Name,
                            MSPIN = x.MSPIN,
                            StartDate = x.StartDate,
                            ProgramId = x.ProgramId,
                            SessionId = x.SessionId,
                            IsSubmitted = x.IsSubmitted,
                            ProgramTestCalenderId = x.ProgramTestCalenderId,
                            DealerName = x.DealerName,
                            //CreatedBy=x.DealerName,
                            Dealer_LocationCode = x.Dealer_LocationCode
                        }).ToList());
                    }
                }
                
                return List1;
            }

        }
        public static string UpDateMarksForSSTC(List<SSTCCandidatesList> Obj)
        {
            string Msg = string.Empty;
            using (var Context = new CEIDBEntities())
            {
                foreach (var row in Obj)
                {
                    if (row.Marks != null)
                    {
                        int Status = Context.sp_Inset_Update_TblSSTCMarks(row.MSPIN, row.SessionId, row.Day, row.Marks, row.ProgramTestCalenderId, row.ProgramId, row.CreatedBy, DateTime.Now);
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
                        int Status = Context.sp_Update_InsertIntoTblAttendance_SSTC_V2(row.MSPIN, Obj.Date, row.SessionID, Obj.Day, Obj.CreatedBy);
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
                    return "Success: Attendance Updated Successfully!";
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
                        Date = x.Date,
                        Weekday = x.Weekday,
                        DayDate = x.DayDate
                    }).ToList();
                }
                return List;
            }

        }
        public static string CloseSSTCCourse(List<SessionIDListForAgency> List)
        {
            string Status = string.Empty;
            string[] ClosedSession=new string[List.Count];
            int i = 0;
            using (var Context = new CEIDBEntities())
            {
                try
                {
                    foreach (var Obj in List)
                    {
                        var Check = Context.TblSSTC_CourseClosure.Where(x => x.SessionID == Obj.SessionID && x.IsActive == true).FirstOrDefault();
                        if (Check == null)
                        {
                            TblSSTC_CourseClosure CC = new TblSSTC_CourseClosure
                            {
                                IsActive = true,
                                ClosureDate = DateTime.Now,
                                CreatedBy = Obj.CreatedBy,
                                CreationDate = DateTime.Now,
                                SessionID = Obj.SessionID
                            };
                            Context.Entry(CC).State = System.Data.Entity.EntityState.Added;
                            Context.SaveChanges();
                            ClosedSession[i] = Obj.SessionID;
                            i++;
                        }
                        else
                        { Status += Obj.SessionID + " Already Closed. <br />"; }
                    }
                    if (Status.Equals(string.Empty))
                    {
                        SendEmailOnCourseClosure(ClosedSession);
                        return "Success : Course closed.";
                    }
                    else {
                        SendEmailOnCourseClosure(ClosedSession);
                        return Status;
                    }
                }
                catch (Exception Ex)
                {
                    return "Error: " + Ex.Message;
                }
            }
        }

        private static void SendEmailOnCourseClosure (string [] List)
        {
            string Body = string.Empty;
            int Sno = 1;
            Body= "<div>Dear All, <br><br>Greetings for the day!!<br><br>Following courses are closed as on  "+DateTime.Now.ToString("dd-MMM-yy")+ ": <br> <br>";

            foreach (string session in List) {
                Body += Sno + ". " + session + "<br>";
                Sno++;
            }
            Body += "<br>Thanks, <br>STEP Application<br><br>* This is an auto generated mail, Please do not reply";

            MailAddressCollection ToMail = new MailAddressCollection();
            MailAddressCollection CCMail = new MailAddressCollection();
            ToMail.Add("nalin.kulshrestha@maruti.co.in");
            CCMail.Add("ankur.verma@phoenixtech.consulting");
            CCMail.Add("narayan.joshi@phoenixtech.consulting");

            Email.sendEmail("", ToMail,CCMail,"Course Closure | "+ DateTime.Now.ToString("dd-MMM-yy"),Body);
        }
    }
}
