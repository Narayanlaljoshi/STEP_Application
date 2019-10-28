using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEPDAL.DB;
using System.Net;
using System.IO;

namespace STEPDAL.CustomDAL
{
    public class UserDAL
    {
        public static UserDetailsBLL Login(string UserName, string Password)
        {
            UserDetailsBLL UserDetails = new UserDetailsBLL();
            using (var Context = new CEIDBEntities())
            {
                var GetDeatils = Context.SP_GetUserDetails(UserName, Password).FirstOrDefault();
                if (GetDeatils != null)
                {
                    if (GetDeatils.Role_Id != 4)
                    {
                        UserDetails.User_Id = GetDeatils.User_Id; ;
                        UserDetails.UserName = GetDeatils.UserName;
                        UserDetails.Password = GetDeatils.Password;
                        UserDetails.Email = GetDeatils.Email;
                        UserDetails.Zone_Id = GetDeatils.Zone_Id;
                        UserDetails.Region_Id = GetDeatils.Region_Id;
                        UserDetails.City_Id = GetDeatils.City_Id;
                        UserDetails.Role_Id = GetDeatils.Role_Id;
                        UserDetails.DealerGroup_Id = GetDeatils.DealerGroup_Id;
                        UserDetails.DealerOutlet_Id = GetDeatils.DealerOutlet_Id;
                        UserDetails.IsActive = GetDeatils.IsActive;
                        UserDetails.CreationDate = GetDeatils.CreationDate;
                        UserDetails.CreatedBy = GetDeatils.CreatedBy;
                        UserDetails.ModifiedBy = GetDeatils.ModifiedBy;
                        UserDetails.RoleName = GetDeatils.RoleName;
                        UserDetails.Agency_Id = GetDeatils.Agency_Id;
                    }
                }
            }
            return UserDetails;
        }

        public static dynamic LoginStudent(string MSPIN, string Password, string Key)
        {
            if (Key != null && !string.IsNullOrEmpty(Key))
            {
                Key = Key.Replace("/", "");
                //Key = Key.Replace("-", "");
                //Key = Key.Replace(" ", "");
            }
            UserDetailsBLL UserDetails = new UserDetailsBLL();
            using (var Context = new CEIDBEntities())
            {
                string TestCode = Password;
                if (TestCode.Length == 4)
                {
                    StudenttestDetailsBLL GetStudenttestDetails = TestDAL.GetStudenttestDetails(MSPIN);
                    if (GetStudenttestDetails == null)
                    {
                        return null;
                    }
                    else if (GetStudenttestDetails.ErrorMessage != null)
                    {
                        return GetStudenttestDetails.ErrorMessage;
                    }
                    else
                    {
                        var CalenderDtl = Context.sp_GetTblProgramTestCalenderDetails(GetStudenttestDetails.ProgramTestCalenderId).FirstOrDefault();
                        var Attendance = Context.SP_CheckAttendanceByDate(GetStudenttestDetails.AgencyCode, DateTime.Now, MSPIN).FirstOrDefault();
                        if (Attendance.Present != true)
                        {
                            UserDetails.ErrorMsg = "Error: Not Punched In Yet!";
                            return UserDetails;
                        }
                        DateTime ValidTime = GetStudenttestDetails.TestInitiatedDate.Value.AddMinutes(CalenderDtl != null ? CalenderDtl.ValidDuration.Value : 0);
                        if (ValidTime < DateTime.Now)
                        {
                            UserDetails.ErrorMsg = "Error: Time limit expired. Kindly contact admin!";
                            return UserDetails;
                        }
                        string SessionId = GetStudenttestDetails.SessionID;
                        string ActualTestcode = GetStudenttestDetails.TestLoginCode;
                        int Day = GetStudenttestDetails.DayCount.Value;
                        var GetDeatils = Context.SP_GetStudentDetails(MSPIN).FirstOrDefault();
                        if (GetDeatils != null && ActualTestcode == TestCode)
                        {
                            var CheckAlreadyLogin = Context.SP_CheckTblLogReport_Candidates(MSPIN, Day, SessionId).FirstOrDefault();
                            if (CheckAlreadyLogin != null)
                            {
                                if (!CheckAlreadyLogin.LoginKey.Equals(Key))
                                {

                                    if (Key.Equals("undefined") && CheckAlreadyLogin.AllowLoginAgain == true)
                                    {
                                        UserDetails.User_Id = GetDeatils.User_Id;
                                        UserDetails.UserName = GetDeatils.UserName;
                                        UserDetails.Password = GetDeatils.Password;
                                        UserDetails.Email = GetDeatils.Email;
                                        UserDetails.Zone_Id = GetDeatils.Zone_Id;
                                        UserDetails.Region_Id = GetDeatils.Region_Id;
                                        UserDetails.City_Id = GetDeatils.City_Id;
                                        UserDetails.Role_Id = GetDeatils.Role_Id;
                                        UserDetails.DealerGroup_Id = GetDeatils.DealerGroup_Id;
                                        UserDetails.DealerOutlet_Id = GetDeatils.DealerOutlet_Id;
                                        UserDetails.IsActive = GetDeatils.IsActive;
                                        UserDetails.CreationDate = GetDeatils.CreationDate;
                                        UserDetails.CreatedBy = GetDeatils.CreatedBy;
                                        UserDetails.ModifiedBy = GetDeatils.ModifiedBy;
                                        UserDetails.RoleName = GetDeatils.RoleName;
                                        UserDetails.Agency_Id = GetDeatils.Agency_Id;
                                        UserDetails.AlreadyLoggedIn = false;
                                        UserDetails.LogKey = CheckAlreadyLogin.LoginKey;

                                    }
                                    else
                                    {
                                        UserDetails.AlreadyLoggedIn = true;
                                        UserDetails.ErrorMsg = "Error: Alraedy Logged In, Kindly Contact Your Trainer!";
                                    }
                                }

                                else
                                {
                                    UserDetails.User_Id = GetDeatils.User_Id;
                                    UserDetails.UserName = GetDeatils.UserName;
                                    UserDetails.Password = GetDeatils.Password;
                                    UserDetails.Email = GetDeatils.Email;
                                    UserDetails.Zone_Id = GetDeatils.Zone_Id;
                                    UserDetails.Region_Id = GetDeatils.Region_Id;
                                    UserDetails.City_Id = GetDeatils.City_Id;
                                    UserDetails.Role_Id = GetDeatils.Role_Id;
                                    UserDetails.DealerGroup_Id = GetDeatils.DealerGroup_Id;
                                    UserDetails.DealerOutlet_Id = GetDeatils.DealerOutlet_Id;
                                    UserDetails.IsActive = GetDeatils.IsActive;
                                    UserDetails.CreationDate = GetDeatils.CreationDate;
                                    UserDetails.CreatedBy = GetDeatils.CreatedBy;
                                    UserDetails.ModifiedBy = GetDeatils.ModifiedBy;
                                    UserDetails.RoleName = GetDeatils.RoleName;
                                    UserDetails.Agency_Id = GetDeatils.Agency_Id;
                                    UserDetails.AlreadyLoggedIn = false;
                                    //UserDetails.LogKey = CheckAlreadyLogin.LoginKey;
                                    //UserDetails.AlreadyLoggedIn = false;

                                    string LogKey = UserDAL.UpdateLogReport(GetStudenttestDetails);
                                    UserDetails.LogKey = LogKey;
                                }
                            }
                            else
                            {

                                UserDetails.User_Id = GetDeatils.User_Id;
                                UserDetails.UserName = GetDeatils.UserName;
                                UserDetails.Password = GetDeatils.Password;
                                UserDetails.Email = GetDeatils.Email;
                                UserDetails.Zone_Id = GetDeatils.Zone_Id;
                                UserDetails.Region_Id = GetDeatils.Region_Id;
                                UserDetails.City_Id = GetDeatils.City_Id;
                                UserDetails.Role_Id = GetDeatils.Role_Id;
                                UserDetails.DealerGroup_Id = GetDeatils.DealerGroup_Id;
                                UserDetails.DealerOutlet_Id = GetDeatils.DealerOutlet_Id;
                                UserDetails.IsActive = GetDeatils.IsActive;
                                UserDetails.CreationDate = GetDeatils.CreationDate;
                                UserDetails.CreatedBy = GetDeatils.CreatedBy;
                                UserDetails.ModifiedBy = GetDeatils.ModifiedBy;
                                UserDetails.RoleName = GetDeatils.RoleName;
                                UserDetails.Agency_Id = GetDeatils.Agency_Id;
                                UserDetails.AlreadyLoggedIn = false;
                                //UserDetails.LogKey = CheckAlreadyLogin.LoginKey;

                                string LogKey = UserDAL.UpdateLogReport(GetStudenttestDetails);

                                UserDetails.LogKey = LogKey;
                            }
                        }
                    }
                }
            }
            return UserDetails;
        }

        public static string ChangePassword(string userName, string password)
        {
            UserDetailsBLL UserDetails = new UserDetailsBLL();
            using (var Context = new CEIDBEntities())
            {
                var GetDeatils = Context.TblUsers.Where(x => x.IsActive == true && x.UserName.Equals(userName)).FirstOrDefault();
                GetDeatils.Password = password;
                try
                {
                    Context.Entry(GetDeatils).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();
                    return "success";
                }
                catch (Exception ex)
                {
                    return "fail";
                }

            }
        }

        public static int AuthUser(string userName)
        {
            string MobileNumber = "";
            string Name = "";
            int ireturn = 0;
            UserDetailsBLL UserDetails = new UserDetailsBLL();
            using (var Context = new CEIDBEntities())
            {
                var GetDeatils = Context.TblUsers.Where(x => x.IsActive == true && x.UserName.Equals(userName)).FirstOrDefault();
                if (GetDeatils != null)
                {
                    UserDetails.User_Id = GetDeatils.User_Id;
                    UserDetails.UserName = GetDeatils.UserName;
                    UserDetails.Password = GetDeatils.Password;
                    UserDetails.Email = GetDeatils.Email;
                    UserDetails.Zone_Id = GetDeatils.Zone_Id;
                    UserDetails.Region_Id = GetDeatils.Region_Id;
                    UserDetails.City_Id = GetDeatils.City_Id;
                    UserDetails.Role_Id = GetDeatils.Role_Id;
                    UserDetails.DealerGroup_Id = GetDeatils.DealerGroup_Id;
                    UserDetails.DealerOutlet_Id = GetDeatils.DealerOutlet_Id;
                    UserDetails.IsActive = GetDeatils.IsActive;
                    UserDetails.CreationDate = GetDeatils.CreationDate;
                    UserDetails.CreatedBy = GetDeatils.CreatedBy;
                    UserDetails.ModifiedBy = GetDeatils.ModifiedBy;

                    UserDetails.Agency_Id = GetDeatils.Agency_Id;
                }
                else
                {
                    return ireturn;
                }
                if (GetDeatils.Role_Id == 1 || GetDeatils.Role_Id == 2 || GetDeatils.Role_Id == 5 || GetDeatils.Role_Id == 6)
                {

                    GetDeatils.Password = GetDeatils.UserName + "@123";

                    Context.Entry(GetDeatils).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();


                    string subject = "STEP | Password Reset";
                    string Body = "Dear San,<br /><br />Your password has been reset. Login credentails are as follows:<br /> <br />" + "\r\n" + "\r\n" + "Username : " + GetDeatils.UserName + "\r\n" + ",<br /> Password : " + GetDeatils.Password + "<br /> <br />Thanks <br /> STEP Portal <br /><br /> ** this is an auto generated mail, please do not reply.";
                    //string frmEmail = "narayan.joshi@phoenixtech.consulting";
                    string toEmail = GetDeatils.Email;
                    string ccEmail = "";
                    if (toEmail != null)
                    {
                        string str = Email.sendEmailReport("", toEmail, ccEmail, subject, Body);
                        if (str.Equals("Success: Notification sent"))
                        {
                            ireturn = 1;
                        }
                    }
                    else
                    {
                        ireturn = 3;
                    }

                }
                else
                {
                    if (GetDeatils.Role_Id == 4)
                    {
                        var GetUserdetail = Context.TblNominations.Where(t => t.MSPIN.Equals(GetDeatils.UserName)).FirstOrDefault();
                        int date = GetUserdetail.DateofBirth.Value.Day;
                        int month = GetUserdetail.DateofBirth.Value.Month;
                        int year = GetUserdetail.DateofBirth.Value.Year;
                        MobileNumber = GetUserdetail.MobileNo;
                        Name = GetUserdetail.Name;
                        GetDeatils.Password = Convert.ToString(date) + "" + Convert.ToString(month) + "" + Convert.ToString(year);

                        Context.Entry(GetDeatils).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }
                    else
                    {
                        var FacultyDteail = Context.TblFaculties.Where(x => x.FacultyCode.Equals(userName)).FirstOrDefault();
                        MobileNumber = FacultyDteail.Mobile;
                        Name = FacultyDteail.FacultyName;
                        GetDeatils.Password = GetDeatils.UserName + "123";
                        Context.Entry(GetDeatils).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }

                    try
                    {
                        string url = "http://webpostservice.com/sendsms/sendsms.php?username=phonixtec&password=547132&type=TEXT&sender=NPISHM&mobile=" + MobileNumber + "&message=Dear " + Name + ",Your password has been reset." + "\r\n" + "\r\n" + "Username : " + GetDeatils.UserName + "\r\n" + "password : " + GetDeatils.Password;

                        string text = UserDAL.GETResponse(url);
                        if (text.IndexOf("SUCCESS") != -1)
                        {
                            ireturn = 2;
                        }
                        else
                        {
                            ireturn = 3;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        ireturn = 3;
                    }
                }
            }
            return ireturn;
        }


        static string GETResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {

                return null;

            }
        }

        public static string UpdateLogReport(StudenttestDetailsBLL Obj)
        {
            UserDetailsBLL UserDetails = new UserDetailsBLL();
            using (var Context = new CEIDBEntities())
            {
                string MSPIN = Obj.MSPIN;
                int Day = Obj.DayCount.Value;
                string SessionID = Obj.SessionID;

                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < 5; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                //builder.Append(MSPIN);
                var Check = Context.TblLogReport_Candidates.Where(x => x.MSPIN == MSPIN && x.Day == Day && x.SessionID == SessionID).FirstOrDefault();
                if (Check == null)
                {
                    TblLogReport_Candidates TLRC = new TblLogReport_Candidates
                    {
                        SessionID = Obj.SessionID,
                        Day = Obj.DayCount.Value,
                        MSPIN = Obj.MSPIN,
                        LoginDateTime = DateTime.Now,
                        AllowLoginAgain = false,
                        LoginKey = builder.ToString()
                    };
                    Context.Entry(TLRC).State = System.Data.Entity.EntityState.Added;
                    Context.SaveChanges();
                    return builder.ToString();
                }
                else
                {
                    Check.SessionID = Obj.SessionID;
                    Check.Day = Obj.DayCount.Value;
                    Check.MSPIN = Obj.MSPIN;
                    Check.LoginDateTime = DateTime.Now;
                    Check.AllowLoginAgain = false;
                    Context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();
                    return Check.LoginKey;
                }
            }

        }

        public static UserDetailsBLL RTMLogin(string UserName, string Password)
        {
            UserDetailsBLL UserDetails = null;
            using (var Context = new CEIDBEntities())
            {
                var GetDeatils = Context.SP_GetUserDetails(UserName, Password).FirstOrDefault();
                if (GetDeatils != null)
                {
                    if (GetDeatils.Role_Id == 2)
                    {
                        UserDetails = new UserDetailsBLL();
                        UserDetails.User_Id = GetDeatils.User_Id;
                        UserDetails.UserName = GetDeatils.UserName;
                        UserDetails.Password = GetDeatils.Password;
                        UserDetails.Email = GetDeatils.Email;
                        UserDetails.Zone_Id = GetDeatils.Zone_Id;
                        UserDetails.Region_Id = GetDeatils.Region_Id;
                        UserDetails.City_Id = GetDeatils.City_Id;
                        UserDetails.Role_Id = GetDeatils.Role_Id;
                        UserDetails.DealerGroup_Id = GetDeatils.DealerGroup_Id;
                        UserDetails.DealerOutlet_Id = GetDeatils.DealerOutlet_Id;
                        UserDetails.IsActive = GetDeatils.IsActive;
                        UserDetails.CreationDate = GetDeatils.CreationDate;
                        UserDetails.CreatedBy = GetDeatils.CreatedBy;
                        UserDetails.ModifiedBy = GetDeatils.ModifiedBy;
                        UserDetails.RoleName = GetDeatils.RoleName;
                        UserDetails.Agency_Id = GetDeatils.Agency_Id;
                    }
                }
            }
            return UserDetails;
        }

        public static UserDetailsBLL_SSTC SSTCLogin(string UserName, string Password, string Token)
        {
            UserDetailsBLL_SSTC UserDetails = null;
            using (var Context = new CEIDBEntities())
            {
                var GetDeatils = Context.SP_GetUserDetails_VendorTrainer(UserName, Password).FirstOrDefault();
                if (GetDeatils != null)
                {
                    if (GetDeatils.Role_Id == 6)
                    {
                        UserDetails = new UserDetailsBLL_SSTC();
                        UserDetails.User_Id = GetDeatils.User_Id;
                        UserDetails.UserName = GetDeatils.UserName;
                        UserDetails.Password = GetDeatils.Password;
                        UserDetails.Email = GetDeatils.Email;
                        UserDetails.Zone_Id = GetDeatils.Zone_Id;
                        UserDetails.Region_Id = GetDeatils.Region_Id;
                        UserDetails.City_Id = GetDeatils.City_Id;
                        UserDetails.Role_Id = GetDeatils.Role_Id;
                        UserDetails.DealerGroup_Id = GetDeatils.DealerGroup_Id;
                        UserDetails.DealerOutlet_Id = GetDeatils.DealerOutlet_Id;
                        UserDetails.IsActive = GetDeatils.IsActive;
                        UserDetails.CreationDate = GetDeatils.CreationDate;
                        UserDetails.CreatedBy = GetDeatils.CreatedBy;
                        UserDetails.ModifiedBy = GetDeatils.ModifiedBy;
                        UserDetails.RoleName = GetDeatils.RoleName;
                        UserDetails.Agency_Id = GetDeatils.Agency_Id;
                        UserDetails.TrainerName = GetDeatils.TrainerName;
                        UserDetails.VendorName = GetDeatils.VendorName;
                    }

                    int status = Context.sp_Insert_Update_TblUserId_TokenMapping(GetDeatils.User_Id, Token, true, GetDeatils.User_Id);
                }
            }
            return UserDetails;
        }

        public static bool UpdateToken(int User_Id, string Token)
        {
            using (var Context = new CEIDBEntities())
            {
                int status = Context.sp_Insert_Update_TblUserId_TokenMapping(User_Id, Token, true, User_Id);
                if (status >= 1)
                    return true;
                else
                    return false;
            }
        }
    }
}
