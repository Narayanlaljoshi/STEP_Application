using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ProjectDAL.CustomDAL
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
                        AgenyType=s.AgenyType

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
                    
                    var prop = context.TblRTCMasters.Where(s => s.Agency_Id.Equals(obj.Agency_Id)&& s.IsActive==true).FirstOrDefault();

                    if (prop == null)
                    {

                        TblRTCMaster _Region = new TblRTCMaster
                        {
                            AgencyCode = obj.AgencyCode,
                            AgencyName = obj.AgencyName,
                            RTMName = obj.RTMName,
                            IsActive = true,
                            AgenyType=obj.AgenyType,
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
                            Role_Id = obj.AgenyType==1?2:5,
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
                    else if (prop.RTMCode!= RTMCode && prop.Agency_Id==obj.Agency_Id) {
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
                    FacultyCode=s.FacultyCode,
                    FacultyName=s.FacultyName,
                    Faculty_Id=s.Faculty_Id,
                    Mobile=s.Mobile,
                    IsActive=s.IsActive,
                    CreatedBy=s.CreatedBy,
                    CreationDate=s.CreationDate,
                    Agency_Id = s.Agency_Id,
                    Email = s.Email,

                }).ToList();

                return objList;
            }
        }

        public static string SubmitFacultyList(List<FacultyDetailsBLL> info)
        {

            using (var context = new CEIDBEntities())
            {
                foreach (var obj in info)
                {
                    var prop = context.TblFaculties.Where(s => s.Agency_Id.Equals(obj.Agency_Id)&& s.FacultyCode==obj.FacultyCode && s.IsActive==true).FirstOrDefault();

                    if (prop == null)
                    {
                        TblFaculty _Region = new TblFaculty
                        {
                            Agency_Id = obj.Agency_Id,
                            FacultyCode = obj.FacultyCode,
                            FacultyName = obj.FacultyName,
                            Email=obj.Email,
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
                            Role_Id =obj.Role_Id==5? 6:3,
                            Agency_Id= obj.Agency_Id,
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
                var prop = context.TblFaculties.Where(s => s.Faculty_Id==info.Faculty_Id && s.IsActive==true).FirstOrDefault();
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
                   SessionID=s.SessionID
                }).ToList();

                return objList;
            }
        }

        public static List<SessionIDListForAgency> GetSessionIDListForFaculty(int Agency_Id,string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetSessionIdListBy_FacultyCode(Agency_Id, FacultyCode).ToList();

                List<SessionIDListForAgency> objList = null;
                objList = data.Select(s => new SessionIDListForAgency
                {
                    SessionID = s.SessionID,
                    Day=s.Day,
                    EndDate=s.EndDate,
                    StartDate=s.StartDate,
                    IsChecked=false
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
                       EndDate=s.EndDate,
                       MSPIN=s.MSPIN,
                       StartDate=s.StartDate,
                       IsChecked=false,
                       IsPresent=s.IsPresent,
                       AgencyCode=s.AgencyCode
                    }).ToList();
                }
                return objList;
            }
        }

        public static List<CandidateList_SSTC> GetCandidateListForSSTC(string SessionId,int Day)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetCandidateListBySessionId_SSTC(SessionId, Day).ToList();

                List<CandidateList_SSTC> objList = null;
                if (data.Count != 0)
                {
                    objList = data.Select(s => new CandidateList_SSTC
                    {
                        EndDate = s.EndDate,
                        MSPIN = s.MSPIN,
                        StartDate = s.StartDate,
                        IsChecked = false,
                        IsPresent = s.IsPresent,
                        AgencyCode = s.AgencyCode,
                        Day=s.Day,
                        Date=s.Date,
                        SessionID=s.SessionID
                    }).ToList();
                }
                return objList;
            }
        }

        public static string UpdateAttendance(List<CandidateList> Obj)
        {
            var NewCandidates = Obj.Where(x => x.IsChecked == true).ToList();
            using (var context = new CEIDBEntities())
            {
                foreach (var St in NewCandidates)
                {
                    var Status = context.sp_Update_InsertIntoTblAttendancePunchIn(St.MSPIN,St.AgencyCode,DateTime.Now,"Updtaed by RTM",null);
                }
                return "Success: Updated Successfully!";
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

        public static string UpdateAttendance_Mobile(List<CandidateList> Obj)
        {
            var NewCandidates = Obj.Where(x => x.IsChecked == true).ToList();
            using (var context = new CEIDBEntities())
            {
                foreach (var St in NewCandidates)
                {
                    var Status = context.sp_Update_InsertIntoTblAttendancePunchIn(St.MSPIN, St.AgencyCode, DateTime.Now, "Updtaed by RTM", null);
                }
                return "Success: Updated Successfully!";
            }
        }
    }
}
