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
    public class RtcMaster
    {
        public static List<RtcMasterBLL> GetAgencyList()
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.sp_GetAgencyList().ToList();

                List<RtcMasterBLL> objList = null;
                if (data.Count != 0)
                {
                    objList = data.Select(s => new RtcMasterBLL
                    {
                        AgencyCode = s.AgencyCode,
                        Agency_Id = s.Agency_Id,
                        AgencyName = s.AgencyName,
                        RTMName = s.RTMName,
                        Email = s.Email,
                        Mobile = s.Mobile,
                        RTMCode = s.RTMCode,
                        AgenyType = s.AgenyType

                    }).ToList();
                }
                return objList;
            }
        }

        public static string AddUpdateAgency(RtcMasterBLL obj)
        {
            string RTMCode = obj.RTMCode.Trim();

            using (var context = new CEIDBEntities())
            {
                //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                //{
                //    // do something with EF here

                var prop = context.TblRTCMasters.Where(s => s.Agency_Id.Equals(obj.Agency_Id) && s.IsActive == true).FirstOrDefault();

                if (prop == null)
                {

                    TblRTCMaster _Region = new TblRTCMaster
                    {
                        AgencyCode = obj.AgencyCode,
                        AgencyName = obj.AgencyName,
                        RTMName = obj.RTMName,
                        IsActive = true,
                        AgenyType = obj.AgenyType,
                        Email = obj.Email,
                        Mobile = obj.Mobile,
                        RTMCode = RTMCode,
                        CreatedBy = obj.CreatedBy,
                        CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                        ModifiedBy = null,
                        ModifiedDate = null
                    };
                    context.Entry(_Region).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    //Creating Username password for RTM login
                    string Password = obj.RTMCode + "@123";
                    //string Username = obj.AgencyCode;

                    TblUser Tu = new TblUser
                    {
                        Email = obj.Email,
                        Agency_Id = _Region.Agency_Id,
                        Password = Password,
                        Role_Id = obj.AgenyType == 1 ? 2 : 5,
                        IsActive = true,
                        UserName = obj.RTMCode,
                        CreationDate = DateTime.Now,
                        CreatedBy = obj.CreatedBy
                    };
                    context.Entry(Tu).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();

                    //scope.Complete();

                    return "Success: Agency Added successfully";
                }
                else if (prop.RTMCode != RTMCode && prop.Agency_Id == obj.Agency_Id) {
                    string Password = obj.RTMCode + "@123";
                    //Disable user first
                    var UserLoginDetails = context.TblUsers.Where(x => x.UserName == prop.RTMCode && x.IsActive == true).FirstOrDefault();
                    if (UserLoginDetails != null)
                    {
                        UserLoginDetails.Password = Password;
                        UserLoginDetails.UserName = obj.RTMCode;
                        UserLoginDetails.IsActive = false;
                        UserLoginDetails.Agency_Id = prop.Agency_Id;
                        UserLoginDetails.ModifiedDate = DateTime.Now;
                        UserLoginDetails.ModifiedBy = obj.ModifiedBy;
                        context.Entry(UserLoginDetails).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    else {
                        TblUser Tu = new TblUser
                        {
                            Email = obj.Email,
                            Agency_Id = prop.Agency_Id,
                            Password = Password,
                            Role_Id = 2,
                            IsActive = true,
                            UserName = obj.RTMCode,
                            CreationDate = DateTime.Now,
                            CreatedBy = obj.CreatedBy
                        };
                        context.Entry(Tu).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();
                    }
                    //Update the RTM in RTC Master
                    prop.AgencyCode = obj.AgencyCode;
                    prop.AgencyName = obj.AgencyName;
                    prop.RTMName = obj.RTMName;
                    prop.IsActive = obj.IsActive;
                    prop.Email = obj.Email;
                    prop.Mobile = obj.Mobile;
                    prop.ModifiedBy = obj.ModifiedBy;
                    prop.ModifiedDate = null;
                    prop.RTMCode = RTMCode;
                    context.Entry(prop).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    //again Create the new user id for this new RTM
                    //string Password = obj.RTMCode + "@123";

                    //TblUser Tu = new TblUser
                    //{
                    //    Email = obj.Email,
                    //    Agency_Id = prop.Agency_Id,
                    //    Password = Password,
                    //    Role_Id = 2,
                    //    IsActive = true,
                    //    UserName = obj.RTMCode,
                    //    CreationDate = DateTime.Now,
                    //    CreatedBy = obj.CreatedBy
                    //};
                    //context.Entry(Tu).State = System.Data.Entity.EntityState.Added;
                    //context.SaveChanges();

                    //scope.Complete();

                    return "Success: Agency Added successfully";
                }
                else
                {
                    prop.AgencyCode = obj.AgencyCode;
                    prop.AgencyName = obj.AgencyName;
                    prop.RTMName = obj.RTMName;
                    prop.IsActive = obj.IsActive;
                    prop.Email = obj.Email;
                    prop.Mobile = obj.Mobile;
                    //prop.CreatedBy = obj.CreatedBy;
                    //prop.CreationDate = DateTime.Now;//LocalTimeRegion.GetLocalDate(),
                    prop.ModifiedBy = obj.ModifiedBy;
                    prop.ModifiedDate = null;
                    prop.RTMCode = RTMCode;
                    context.Entry(prop).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    if (obj.IsActive == false)
                    {
                        var LoginDetail = context.TblUsers.Where(x => x.UserName == obj.AgencyCode).FirstOrDefault();
                        LoginDetail.IsActive = false;

                        context.Entry(LoginDetail).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                    //scope.Complete();

                    return "Success: Agency Updated Successfully!";
                }

                //}
            }
        }

        public static List<FacultyDetailsBLL> GetFacultyDetailsList(int Agency_Id)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.sp_GetAgencyWiseFaculty(Agency_Id).ToList();

                List<FacultyDetailsBLL> objList = null;
                objList = data.Select(s => new FacultyDetailsBLL
                {
                    FacultyCode = s.FacultyCode,
                    FacultyName = s.FacultyName,
                    Faculty_Id = s.Faculty_Id,
                    Mobile = s.Mobile,
                    IsActive = s.IsActive,
                    CreatedBy = s.CreatedBy,
                    CreationDate = s.CreationDate,
                    Agency_Id = s.Agency_Id,
                    Email = s.Email,

                }).ToList();

                return objList;
            }
        }

        public static List<FacultyDetailsBLL> GetFacultyDetailsList_External(int Agency_Id, string UserName)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.sp_GetAgencyWiseFaculty_External(Agency_Id, UserName).ToList();

                List<FacultyDetailsBLL> objList = null;
                objList = data.Select(s => new FacultyDetailsBLL
                {
                    FacultyCode = s.SubFaculty_Code,
                    FacultyName = s.SubFacultyName,
                    Faculty_Id = s.SubFaculty_Id,
                    Mobile = s.Mobile,
                    IsActive = s.IsActive,
                    CreatedBy = s.CreatedBy,
                    CreationDate = s.CreationDate,
                    Agency_Id = s.Agency_Id,
                    Email = s.Email,

                }).ToList();

                return objList;
            }
        }
        public static string SubmitFacultyList_External(List<FacultyDetailsBLL> info)
        {

            using (var context = new CEIDBEntities())
            {
                foreach (var obj in info)
                {
                    var prop = context.TblSubFacultyMasters.Where(s => s.Agency_Id == obj.Agency_Id && s.SubFaculty_Code == obj.FacultyCode && s.IsActive == true).FirstOrDefault();

                    if (prop == null)
                    {
                        TblSubFacultyMaster _Region = new TblSubFacultyMaster
                        {
                            Agency_Id = obj.Agency_Id,
                            SubFaculty_Code = obj.FacultyCode,
                            SubFacultyName = obj.FacultyName,
                            ParentFaculty_Id = obj.ParentFaculty_Id,
                            ParentFaculty_Code = obj.ParentFaculty_Code,
                            Email = obj.Email,
                            Mobile = obj.Mobile,
                            IsActive = true,
                            CreatedBy = obj.CreatedBy,
                            CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                            ModifiedBy = null,
                            ModifiedDate = null
                        };

                        context.Entry(_Region).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();


                        TblUser _user = new TblUser
                        {
                            UserName = _Region.SubFaculty_Code,
                            Password = _Region.SubFaculty_Code + "@123",
                            Email = _Region.Email,
                            Role_Id = 6,
                            Agency_Id = obj.Agency_Id,
                            IsActive = true,
                            CreatedBy = obj.CreatedBy,
                            CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                            ModifiedBy = null,
                            ModifiedDate = null
                        };
                        context.Entry(_user).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();
                    }
                    else
                    {
                        var userDtl = context.TblUsers.Where(x => x.UserName == obj.FacultyCode && x.IsActive == true).FirstOrDefault();
                        if (userDtl != null)
                        {
                            userDtl.Role_Id = obj.Role_Id == 5 ? 6 : 3;
                            context.Entry(userDtl).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        prop.Agency_Id = obj.Agency_Id;
                        prop.SubFaculty_Code = obj.FacultyCode;
                        prop.SubFacultyName = obj.FacultyName;
                        prop.Email = obj.Email;
                        prop.IsActive = obj.IsActive;
                        //prop.CreatedBy = 1;
                        prop.Mobile = obj.Mobile;
                        //prop.CreationDate = DateTime.Now;//LocalTimeRegion.GetLocalDate(),
                        prop.ModifiedBy = obj.ModifiedBy;
                        prop.ModifiedDate = DateTime.Now;

                        context.Entry(prop).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }
                return "Success: Faculties Updated successfully";
            }

        }
        public static string SubmitFacultyList(List<FacultyDetailsBLL> info)
        {

            using (var context = new CEIDBEntities())
            {
                int? AgenCyType_Id = 1;
                if (info.Count != 0) {
                    int Agency_id = info[0].Agency_Id;
                    AgenCyType_Id = context.TblRTCMasters.Where(x => x.Agency_Id == Agency_id).Select(x => x.AgenyType).FirstOrDefault();
                }
                foreach (var obj in info)
                {

                    var prop = context.TblFaculties.Where(s => s.Agency_Id == obj.Agency_Id && s.FacultyCode == obj.FacultyCode && s.IsActive == true).FirstOrDefault();

                    if (prop == null)
                    {
                        TblFaculty _Region = new TblFaculty
                        {
                            Agency_Id = obj.Agency_Id,
                            FacultyCode = obj.FacultyCode,
                            FacultyName = obj.FacultyName,
                            Email = obj.Email,
                            Mobile = obj.Mobile,
                            IsActive = true,
                            CreatedBy = obj.CreatedBy,
                            CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                            ModifiedBy = null,
                            ModifiedDate = null
                        };

                        context.Entry(_Region).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();


                        TblUser _user = new TblUser
                        {
                            UserName = _Region.FacultyCode,
                            Password = _Region.FacultyCode + "@123",
                            Email = _Region.Email,
                            Role_Id = AgenCyType_Id != 1 ? 5 : 3,
                            Agency_Id = obj.Agency_Id,
                            IsActive = true,
                            CreatedBy = obj.CreatedBy,
                            CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                            ModifiedBy = null,
                            ModifiedDate = null
                        };
                        context.Entry(_user).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();
                    }
                    else
                    {
                        var userDtl = context.TblUsers.Where(x => x.UserName == obj.FacultyCode && x.IsActive == true).FirstOrDefault();
                        if (userDtl != null) {
                            userDtl.Role_Id = AgenCyType_Id != 1 ? 5 : 3;
                            context.Entry(userDtl).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        prop.Agency_Id = obj.Agency_Id;
                        prop.FacultyCode = obj.FacultyCode;
                        prop.FacultyName = obj.FacultyName;
                        prop.Email = obj.Email;
                        prop.IsActive = obj.IsActive;
                        //prop.CreatedBy = 1;
                        prop.Mobile = obj.Mobile;
                        //prop.CreationDate = DateTime.Now;//LocalTimeRegion.GetLocalDate(),
                        prop.ModifiedBy = obj.ModifiedBy;
                        prop.ModifiedDate = DateTime.Now;

                        context.Entry(prop).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }
                return "Success: Faculties Updated successfully";
            }

        }

        public static string DeleteFaculty(FacultyDetailsBLL info)
        {
            using (var context = new CEIDBEntities())
            {
                var prop = context.TblFaculties.Where(s => s.Faculty_Id == info.Faculty_Id && s.IsActive == true).FirstOrDefault();

                if (prop != null)
                {
                    prop.ModifiedBy = info.ModifiedBy;
                    prop.ModifiedDate = DateTime.Now;
                    prop.IsActive = false;
                    context.Entry(prop).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                return "Success: Faculty deleted successfully";
            }
        }

        public static List<SessionIDListForAgency> GetSessionIDListForAgency(int Agency_Id)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetSessionIdListBy_AgencyId(Agency_Id).ToList();

                List<SessionIDListForAgency> objList = null;
                objList = data.Select(s => new SessionIDListForAgency
                {
                    SessionID = s.SessionID
                }).ToList();

                return objList;
            }
        }

        public static List<SessionIDListForAgency> GetSessionIDListForFaculty(int Agency_Id, string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetSessionIdListBy_FacultyCode(Agency_Id, FacultyCode).ToList();

                List<SessionIDListForAgency> objList = null;
                objList = data.Select(s => new SessionIDListForAgency
                {
                    SessionID = s.SessionID,
                    Day = s.Day,
                    EndDate = s.EndDate,
                    StartDate = s.StartDate,
                    IsChecked = false
                }).ToList();

                return objList;
            }
        }

        public static List<CandidateList> GetCandidateList(string SessionId)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetCandidateListBySessionId(SessionId).ToList();

                List<CandidateList> objList = null;
                if (data.Count != 0)
                {
                    objList = data.Select(s => new CandidateList
                    {
                        EndDate = s.EndDate,
                        MSPIN = s.MSPIN,
                        StartDate = s.StartDate,
                        IsChecked = false,
                        IsPresent = s.IsPresent,
                        AgencyCode = s.AgencyCode
                    }).ToList();
                }
                return objList;
            }
        }

        public static List<CandidateList_SSTC> GetCandidateListForSSTC(List<SessionIDListForAgency> List)
        {
            using (var context = new CEIDBEntities())
            {
                List<CandidateList_SSTC> objList = new List<CandidateList_SSTC>();
                foreach (var row in List) {
                    var data = context.SP_GetCandidateListBySessionId_SSTC(row.SessionID, row.Day, null, null).ToList();

                    if (data.Count != 0)
                    {
                        //foreach (var s in data)
                        //{
                        objList.AddRange(data.Select(s => new CandidateList_SSTC
                        {
                            EndDate = s.EndDate,
                            MSPIN = s.MSPIN,
                            Name = s.Name,
                            StartDate = s.StartDate,
                            IsChecked = false,
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

        public static string UpdateAttendance(List<CandidateList> Obj)
        {
            var NewCandidates = Obj.Where(x => x.IsChecked == true).ToList();
            using (var context = new CEIDBEntities())
            {
                string Msg = string.Empty;
                foreach (var St in NewCandidates)
                {
                    var Status = context.sp_Update_InsertIntoTblAttendancePunchIn(St.MSPIN, St.AgencyCode, DateTime.Now, "Updtaed by RTM", null);

                    if (Status < 1) {
                        Msg += (Msg.Equals(string.Empty) ? "" : ",") + ("Attendance not marked for MSPIN: " + St.MSPIN);
                    }
                }
                if (Msg.Equals(string.Empty))
                {
                    SendEmailForAttendanceMarkedByRTM(Obj);
                    return "Success: Updated Successfully!";
                }
                else
                {
                    return "Error: Attendance not updated for " + Msg;
                }
                //return "Success: Updated Successfully!";
            }
        }

        public static List<SessionIDListForAgency> GetSessionIDListForAgency_Mobile(int Agency_Id)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetSessionIdListBy_AgencyId(Agency_Id).ToList();

                List<SessionIDListForAgency> objList = null;
                objList = data.Select(s => new SessionIDListForAgency
                {
                    SessionID = s.SessionID
                }).ToList();

                return objList;
            }
        }

        public static List<CandidateList> GetCandidateList_Mobile(string SessionId)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetCandidateListBySessionId_V2(SessionId).ToList();

                List<CandidateList> objList = null;
                if (data.Count != 0)
                {
                    objList = data.Select(s => new CandidateList
                    {
                        EndDate = s.EndDate,
                        Name = s.Name,
                        MSPIN = s.MSPIN,
                        StartDate = s.StartDate,
                        IsChecked = false,
                        IsPresent = s.IsPresent,
                        AgencyCode = s.AgencyCode
                    }).ToList();
                }
                return objList;
            }
        }

        public static string UpdateAttendance_Mobile(List<CandidateList> Obj)
        {
            var NewCandidates = Obj.Where(x => x.IsChecked == true).ToList();
            using (var context = new CEIDBEntities())
            {
                string Msg = string.Empty;
                foreach (var St in NewCandidates)
                {
                    var Status = context.sp_Update_InsertIntoTblAttendancePunchIn(St.MSPIN, St.AgencyCode, DateTime.Now, "Updated by RTM", null);

                    if (Status < 1)
                    {
                        Msg += (Msg.Equals(string.Empty) ? "" : ",") + ("Attendance not marked for MSPIN: " + St.MSPIN);
                    }
                }
                if (Msg.Equals(string.Empty))
                {
                    SendEmailForAttendanceMarkedByRTM(NewCandidates);
                    return "Success: Updated Successfully!";
                }
                else
                {
                    return "Error: Attendance not updated for " + Msg;
                }
                //foreach (var St in NewCandidates)
                //{
                //    var Status = context.sp_Update_InsertIntoTblAttendancePunchIn(St.MSPIN, St.AgencyCode, DateTime.Now, "Updtaed by RTM", null);
                //}
                //SendEmailForAttendanceMarkedByRTM(Obj);
                //return "Success: Updated Successfully!";
            }
        }

        private static void SendEmailForAttendanceMarkedByRTM(List<CandidateList> Obj)
        {
            using (var context = new CEIDBEntities())
            {
                Obj = Obj.Where(x => x.IsChecked == true).ToList();
                if (Obj.Count != 0) {
                    string AgencyCode = Obj[0].AgencyCode;
                    var RTMDetails = context.TblRTCMasters.Where(x => x.AgencyCode == AgencyCode && x.IsActive == true).FirstOrDefault();
                    var AttnList = context.sp_GetAttendanceUpdatedByRTM(AgencyCode).ToList();
                    string Body = "<html><body><h3>Dear " + RTMDetails.RTMName + " San,</h3><b>Greetings for the day!!</b>";
                    Body += "<p>You have marked the attendance for the following Candidates today: </p>";
                    Body = Body + @"<table border=\"" +1+\""style=\""text- align:center; \""><thead><tr><th>#</th><th>Agency Code</th><th>MSPIN</th></tr></thead>";
                    Body += "<tbody>";
                    int Index = 1;
                    //Obj = Obj.Select(x=>x.ErrorType).Distinct().ToList();
                    foreach (var Q in AttnList)
                    {
                        Body += "<tr><td>";
                        Body += Index.ToString() + "</td><td>";
                        Body += Q.AgencyCode + "</td><td>";
                        Body += Q.MSPIN;
                        Body += "</td></tr>";
                        Index++;
                    }
                    Body += "</tbody></table>";
                    Body += "<p>Thank You.</p><p> Regards </p><p> STEP Portal </p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    string ccMailId = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
                    MailAddressCollection toEmail = new MailAddressCollection();
                    MailAddressCollection ccEmail = new MailAddressCollection();
                    toEmail.Add(RTMDetails.Email);
                    ccEmail.Add(ccMailId);
                    ccEmail.Add("narayan.joshi@phoenixtech.consulting");
                    //ccEmail.Add("amit.kaushik@phoenixtech.consulting");
                    Email.sendEmailForErrorRecords(toEmail, ccEmail, "STEP | Attendance Marked - " + DateTime.Now.ToString("dd-MMM-yyyy"), Body);
                }
            }
        }

        public static List<MultiNominationList> GetMultiNominationListbyAgency (string UserName){
            List<MultiNominationList> multiNominationLists = new List<MultiNominationList>();
            using (var Context = new CEIDBEntities())
            {
                var Reqdata = Context.sp_getMultiNominationListbyAgency(UserName).ToList();
                if (Reqdata.Count != 0) {
                    multiNominationLists = Reqdata.Select(x => new MultiNominationList
                    {
                        AgencyCode=x.AgencyCode,
                        MSPIN=x.MSPIN,
                        Name=x.Name
                    }).ToList();
                }
            }
            return multiNominationLists;
        }
        public static List<MultiNominationDetails> GetMultiNominationDetailsByMSPIN(MultiNominationList Obj)
        {
            List<MultiNominationDetails> multiNominationDetails = null;
            using (var Context = new CEIDBEntities())
            {
                var Data = Context.sp_getMultiNominationDetailsByMSPIN(Obj.MSPIN).ToList();
                multiNominationDetails = Data.Select(x => new MultiNominationDetails {
                    MSPIN=x.MSPIN,
                    AgencyCode=x.AgencyCode,
                    City=x.City,
                    Co_id=x.Co_id,
                    CreatedBy=x.CreatedBy,
                    CreationDate=x.CreationDate,
                    DateofBirth=x.DateofBirth,
                    DealerName=x.DealerName,
                    Dealer_LocationCode=x.Dealer_LocationCode,
                    Duration=x.Duration,
                    EndDate=x.EndDate,
                    FacultyCode=x.FacultyCode,
                    Id=x.Id,
                    IsAccepted=x.IsAccepted,
                    Location=x.Location,
                    MobileNo=x.MobileNo,
                    ModifiedBy=x.ModifiedBy,
                    ModifiedDate=x.ModifiedDate,
                    Name=x.Name,
                    ProgramCode=x.ProgramCode,
                    Region=x.Region,
                    SessionID=x.SessionID,
                    StartDate=x.StartDate,
                    Venue=x.Venue
                }).ToList();
            }
            return multiNominationDetails;
        }
        public static string UpdateMultiNominationListbyAgency(List<MultiNominationDetails> Obj)
        {
            //List<MultiNominationList> multiNominationLists = new List<MultiNominationList>();
            using (var Context = new CEIDBEntities())
            {
                foreach (var item in Obj)
                {
                    var status = Context.sp_Insert_Update_Tbl_Multi_Nomination(item.Co_id, item.Region, item.Venue, item.Dealer_LocationCode, item.DealerName, item.City, item.Location, item.AgencyCode, item.FacultyCode, item.ProgramCode, item.SessionID, item.StartDate, item.EndDate, item.Duration, item.MSPIN, item.Name, item.DateofBirth, item.MobileNo, item.IsAccepted, DateTime.Now);
                }
                Obj = Obj.Where(x => x.IsAccepted == true).ToList();
            }

            //NominationDAL.FilterNomination_MultiNomination(Obj);

            return "Success: Updated Successfully!";
        }
    }
}
