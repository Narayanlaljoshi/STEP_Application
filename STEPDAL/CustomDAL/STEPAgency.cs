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
    public class STEPAgency
    {
        public static List<ActiveSessionIdListForVendorTrainer> GetActiveSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.GetActiveSessionIDListForSTEP_Agency_Trainer_V2(Agency_Id, FacultyCode).ToList();
                List<ActiveSessionIdListForVendorTrainer> objList = new List<ActiveSessionIdListForVendorTrainer>();

                if (data.Count != 0)
                {
                    var ProgramList = data.Select(x => new { x.ProgramName ,x.StartDate,x.EndDate}).Distinct().ToList();
                    //var ProgramList_V2 = data.GroupBy(d => new { d.ProgramName, d.StartDate, d.EndDate }).Select(m => new { m.Key.ProgramName, m.Key.StartDate, m.Key.EndDate });
                    foreach (var ss in ProgramList)
                    {
                        var RegionVenues = data.Where(x => x.ProgramName == ss.ProgramName &&x.StartDate==ss.StartDate && x.EndDate==ss.EndDate).ToList();
                        //PRG_SD_ED Details = data.Where(x => x.ProgramName == ss).FirstOrDefault();
                        var Details = data.Where(x => x.ProgramName == ss.ProgramName && x.StartDate == ss.StartDate && x.EndDate == ss.EndDate).FirstOrDefault();
                        objList.Add(new ActiveSessionIdListForVendorTrainer
                        {

                            EndDate = Details.EndDate,
                            StartDate = Details.StartDate,
                            ProgramName = Details.ProgramName,
                            ProgramType_Id=Details.ProgramType_Id,
                            RegionVenues = RegionVenues.Select(x => new RegionVenue
                            {
                                Region = x.Region,
                                Venue = x.Venue,
                                SessionID = x.SessionID,
                            }).ToList()
                        });
                    }
                }


                return objList;
            }
        }

        public static List<ActiveSessionIdListForVendorTrainer> GetUpcomingSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.GetUpComingSessionIDListForSTEP_Agency_Trainer(Agency_Id, FacultyCode).ToList();

                List<ActiveSessionIdListForVendorTrainer> objList = new List<ActiveSessionIdListForVendorTrainer>();
                if (data.Count != 0)
                {
                    //var ProgramList = data.Select(x => x.ProgramName).Distinct().ToList();

                    var ProgramList = data.Select(x => new { x.ProgramName, x.StartDate, x.EndDate }).Distinct().ToList();

                    foreach (var ss in ProgramList)
                    {
                        var RegionVenues = data.Where(x => x.ProgramName == ss.ProgramName&&x.StartDate==ss.StartDate&&x.EndDate==ss.EndDate).ToList();
                        var Details = data.Where(x => x.ProgramName == ss.ProgramName && x.StartDate == ss.StartDate && x.EndDate == ss.EndDate).FirstOrDefault();
                        objList.Add(new ActiveSessionIdListForVendorTrainer
                        {

                            EndDate = Details.EndDate,
                            StartDate = Details.StartDate,
                            ProgramName = Details.ProgramName,
                            RegionVenues = RegionVenues.Select(x => new RegionVenue
                            {
                                Region = x.Region,
                                Venue = x.Venue,
                                SessionID = x.SessionID,
                            }).ToList()
                        });
                    }
                }
                return objList;
            }
        }

        public static List<PendingSessionIdListForVendorTrainer> GetPendingSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.GetPendingSessionIDListForSTEP_Agency_Trainer_V2(Agency_Id, FacultyCode).ToList();

                List<PendingSessionIdListForVendorTrainer> objList = new List<PendingSessionIdListForVendorTrainer>();
                if (data.Count != 0)
                {
                    //var ProgramList = data.Select(x => x.ProgramName).Distinct().ToList();
                    var ProgramList = data.Select(x => new { x.ProgramName, x.StartDate, x.EndDate }).Distinct().ToList();

                    foreach (var ss in ProgramList)
                    {
                        var RegionVenues = data.Where(x => x.ProgramName == ss.ProgramName && x.StartDate==ss.StartDate&&x.EndDate==ss.EndDate).ToList();
                        var Details = data.Where(x => x.ProgramName == ss.ProgramName && x.StartDate == ss.StartDate && x.EndDate == ss.EndDate).FirstOrDefault();
                        List<MRK_ATNDC_Check> MRK_ATNDC_Check = new List<MRK_ATNDC_Check>();
                        foreach (var item in RegionVenues) {
                            MRK_ATNDC_Check.Add(CheckIf_ATNDC_MRKS_Submitted(item.SessionID));
                        }
                        bool IsAttendanceSubmitted = MRK_ATNDC_Check.Where(x=>x.IsAttendanceSubmitted==false).FirstOrDefault()!=null? false:true;
                        bool IsMarksSubmitted = MRK_ATNDC_Check.Where(x => x.IsMarksSubmitted == false).FirstOrDefault() != null ? false : true; ;
                        objList.Add(new PendingSessionIdListForVendorTrainer
                        {
                            //SessionID = Details.SessionID,
                            EndDate = Details.EndDate,
                            StartDate = Details.StartDate,
                            ProgramName = Details.ProgramName,
                            IsAttendanceSubmitted = IsAttendanceSubmitted,
                            IsMarksSubmitted = IsMarksSubmitted,
                            IsClosable = IsAttendanceSubmitted ? (IsMarksSubmitted ? true : false) : false,
                            ProgramType_Id=Details.ProgramType_Id,
                            RegionVenues = RegionVenues.Select(x => new RegionVenue
                            {
                                Region = x.Region,
                                Venue = x.Venue,
                                SessionID = x.SessionID
                            }).ToList()
                        });
                    }
                }
                return objList;
            }
        }

        private static MRK_ATNDC_Check CheckIf_ATNDC_MRKS_Submitted(string SessionID)
        {
            
            using (var context = new CEIDBEntities()) {
                var ATDNC_DTL = context.sp_GetVendorReport_Attendance(SessionID).ToList();
                var MRKS_DTL = context.sp_GetVendorReport_Marks(SessionID).ToList();
                //var MarksDayCount = context.sp_GetDaySequence_STEPAgency(SessionID,"M");
                //var ATNDCDayCount = context.sp_GetDaySequence_STEPAgency(SessionID, "A");
                MRK_ATNDC_Check Obj = new MRK_ATNDC_Check();
                var MSPINList_AT = ATDNC_DTL.Select(x => x.MSPIN).Distinct();
                var MSPINList_Mr = MRKS_DTL.Count!=0? MRKS_DTL.Select(x => x.MSPIN).Distinct():null;
                bool IsMarksSubmitted = true;
                bool IsATNDCSubmitted = true;
                if (MSPINList_Mr.Count() == MSPINList_AT.Count())
                {
                    int Duration = MRKS_DTL[0].Duration.Value;
                    foreach (var MSPIN in MSPINList_Mr)
                    {
                        for (int i = 1; i <= Duration; i++)
                        {
                            var CHECK_ATNDC = ATDNC_DTL.Where(x => x.MSPIN == MSPIN && x.DayCount == i).FirstOrDefault();
                            var CHECK_MRKS = MRKS_DTL.Where(x => x.MSPIN == MSPIN && x.DayCount == i).FirstOrDefault();
                            if (CHECK_ATNDC!=null && CHECK_MRKS!=null)
                            {
                                if (CHECK_ATNDC.P_A == "A" && CHECK_MRKS.Marks == null) { continue; }
                                if (CHECK_ATNDC.P_A != "A" && CHECK_MRKS.Marks != null) { continue; }
                                if (CHECK_ATNDC.P_A != "A" && CHECK_MRKS.Marks == null) { IsMarksSubmitted = false; }
                                if (CHECK_ATNDC.P_A == "A" && CHECK_MRKS.Marks != null) { IsATNDCSubmitted=false; }
                            }
                        }
                    }
                    Obj.IsAttendanceSubmitted = IsATNDCSubmitted;
                    Obj.IsMarksSubmitted = IsMarksSubmitted;
                }
                return Obj;
            }
        }

        public static List<ActiveSessionIdListForVendorTrainer> GetClosedSessionIDListForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.GetClosedSessionIDListForSTEP_Agency_Trainer(Agency_Id, FacultyCode).ToList();

                List<ActiveSessionIdListForVendorTrainer> objList = new List<ActiveSessionIdListForVendorTrainer>();
                if (data.Count != 0)
                {
                    //var ProgramList = data.Select(x => x.ProgramName).Distinct().ToList();
                    var ProgramList = data.Select(x => new { x.ProgramName, x.StartDate, x.EndDate }).Distinct().ToList();

                    foreach (var ss in ProgramList)
                    {
                        var RegionVenues = data.Where(x => x.ProgramName == ss.ProgramName&&x.StartDate==ss.StartDate&&x.EndDate==ss.EndDate).ToList();
                        var Details = data.Where(x => x.ProgramName == ss.ProgramName && x.StartDate == ss.StartDate && x.EndDate == ss.EndDate).FirstOrDefault();
                        objList.Add(new ActiveSessionIdListForVendorTrainer
                        {
                            //SessionID = Details.SessionID,
                            EndDate = Details.EndDate,
                            StartDate = Details.StartDate,
                            ProgramName = Details.ProgramName,
                            RegionVenues = RegionVenues.Select(x => new RegionVenue
                            {
                                Region = x.Region,
                                Venue = x.Venue,
                                SessionID = x.SessionID
                            }).ToList()
                        });
                    }
                }
                return objList;
            }
        }

        public static CountersForStepAgencyTrainer GetCountersForSTEP_Agency_Trainer_Mobile(int Agency_Id, string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.GetCountersForSTEP_Agency_Trainer(Agency_Id, FacultyCode).FirstOrDefault();

                CountersForStepAgencyTrainer Obj = null;
                if (data != null)
                {
                    Obj = new CountersForStepAgencyTrainer
                    {
                        Active = data.Active,
                        Closed = data.Closed,
                        Pending = data.Pending,
                        UpComing = data.UpComing
                    };
                }

                return Obj;
            }
        }

        public static List<CandidatesList_StepAgency_Marks> GetCandidatesListFor_Marks(ActiveSessionIdListForVendorTrainer Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                Context.Database.CommandTimeout = 1200;
                List<CandidatesList_StepAgency_Marks> List1 = new List<CandidatesList_StepAgency_Marks>();
                foreach (var item in Obj.RegionVenues)
                {
                    var ReqData = Context.sp_GetCandidatesListForSSTC(item.SessionID, Obj.Day,item.Region,item.Venue).ToList();

                    if (ReqData.Count != 0)
                    {
                        List1.AddRange(ReqData.Select(x => new CandidatesList_StepAgency_Marks
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

        public static string UpDateMarksForStepAgency(List<CandidatesList_StepAgency_Marks> Obj)
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

        public static List<CandidateList_StepAgency_Attendance> GetCandidateListFor_Attendance(ActiveSessionIdListForVendorTrainer Obj)
        {
            using (var context = new CEIDBEntities())
            {
                List<CandidateList_StepAgency_Attendance> objList = new List<CandidateList_StepAgency_Attendance>();
                foreach (var item in Obj.RegionVenues)
                {
                    var data = context.SP_GetCandidateListBySessionId_SSTC(item.SessionID, Obj.Day,item.Region,item.Venue).ToList();

                    if (data.Count != 0)
                    {
                        //foreach (var s in data)
                        //{
                        objList.AddRange(data.Select(s => new CandidateList_StepAgency_Attendance
                        {
                            EndDate = s.EndDate,
                            MSPIN = s.MSPIN,
                            Name = s.Name,
                            StartDate = s.StartDate,
                            IsChecked = s.IsPresent.Value == true ? true : false,
                            IsPresent = s.IsPresent,
                            AgencyCode = s.AgencyCode,
                            Day = s.Day,
                            Date = s.Date,
                            SessionID = s.SessionID,
                            DealerName = s.DealerName,
                            Dealer_LocationCode = s.Dealer_LocationCode
                        }).ToList());
                        //}

                    }
                }

                return objList;
            }
        }

        public static string UpdateAttendanceForStepAgency(List<CandidateList_StepAgency_Attendance> Obj)
        {
            string Msg = string.Empty;
            if (Obj.Count != 0)
                Obj = Obj.Where(x => x.IsChecked == true).ToList();

            using (var Context = new CEIDBEntities())
            {
                foreach (var row in Obj)
                {
                    if (row.IsChecked != null)
                    {
                        int Status = Context.sp_Update_InsertIntoTblAttendance_SSTC_V2(row.MSPIN, row.Date, row.SessionID, row.Day, row.CreatedBy);
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

        public static string CloseCourse_StepAgency(PendingSessionIdListForVendorTrainer Object)
        {
            string UserEmail = string.Empty;
            string Status = string.Empty;
            List<string> ClosedSession = new List<string>();
            int i = 0;
            using (var Context = new CEIDBEntities())
            {
                UserEmail = Context.TblUsers.Where(x => x.User_Id == Object.CreatedBy).Select(x => x.Email).FirstOrDefault();
                try
                {
                    foreach (var Obj in Object.RegionVenues)
                    {
                        var Check = Context.TblSSTC_CourseClosure.Where(x => x.SessionID == Obj.SessionID && x.IsActive == true).FirstOrDefault();
                        if (Check == null)
                        {
                            TblSSTC_CourseClosure CC = new TblSSTC_CourseClosure
                            {
                                IsActive = true,
                                ClosureDate = DateTime.Now,
                                CreatedBy = Object.CreatedBy,
                                CreationDate = DateTime.Now,
                                SessionID = Obj.SessionID
                            };
                            Context.Entry(CC).State = System.Data.Entity.EntityState.Added;
                            Context.SaveChanges();
                            ClosedSession.Add(Obj.SessionID);
                            i++;
                        }
                        else
                        {
                            Status += Obj.SessionID + " Already Closed. <br />";
                        }
                    }
                    if (Status.Equals(string.Empty))
                    {
                        SendEmailOnCourseClosure(ClosedSession, UserEmail);
                        return "Success : Course closed.";
                    }
                    else
                    {
                        SendEmailOnCourseClosure(ClosedSession, UserEmail);
                        return Status;
                    }
                }
                catch (Exception Ex)
                {
                    return "Error: " + Ex.Message;
                }
            }
        }

        private static void SendEmailOnCourseClosure(List<string> List,string UserEmail)
        {
            
            string Body = string.Empty;
            int Sno = 1;
            Body = "<div>Dear All, <br><br>Greetings for the day!!<br><br>Following courses are closed as on  " + DateTime.Now.ToString("dd-MMM-yy") + ": <br> <br>";

            foreach (string session in List)
            {
                Body += Sno + ". " + session + "<br>";
            Sno++;
            }
            Body += "<br>Thanks, <br>STEP Application<br><br>* This is an auto generated mail, Please do not reply";

            //string ADminEmail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
            MailAddressCollection ToMail = new MailAddressCollection();
            MailAddressCollection CCMail = new MailAddressCollection();
            ToMail.Add(UserEmail);
            //CCMail.Add("ankur.verma@phoenixtech.consulting");
            CCMail.Add("narayan.joshi@phoenixtech.consulting");

            string status=Email.sendEmail("", ToMail, CCMail, "Course Closure | " + DateTime.Now.ToString("dd-MMM-yy"), Body);
        }

        public static List<DaySequenceSSTC> GetDaySequence(ActiveSessionIdListForVendorTrainer Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                List<DaySequenceSSTC> List = null;

                var ReqData = Context.sp_GetDaySequence_SSTC(Obj.RegionVenues[0].SessionID).ToList();

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

        public static List<DaySequenceSSTC> GetDaySequence_V2(ActiveSessionIdListForVendorTrainer Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                List<DaySequenceSSTC> List = null;


                var ReqData = Context.sp_GetDaySequence_STEPAgency(Obj.RegionVenues[0].SessionID,Obj.Type).ToList();

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
    }
}
