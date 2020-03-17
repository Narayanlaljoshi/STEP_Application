using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ProjectBLL.CustomModel;
using STEPDAL.DB;

namespace STEPDAL.CustomDAL
{
    public class Vendors
    {
        public static List<VendorList> GetVendorList()
        {
            List<VendorList> V_List = new List<VendorList>();
            using (var Context = new CEIDBEntities())
            {
                var GetData = Context.sp_GetVendorsList().ToList();
                if (GetData.Count!=0)
                {
                    V_List = GetData.Select(x => new VendorList {
                        CreatedBy=x.CreatedBy,
                        CreationDate=x.CreationDate,
                        FAC_Code=x.FAC_Code,
                        Id=x.Id,
                        IsActive=x.IsActive,
                        ManagerEmail=x.ManagerEmail,
                        ManagerID=x.ManagerID,
                        ManagerMobile=x.ManagerMobile,
                        ManagerName=x.ManagerName,
                        ModifiedBy=x.ModifiedBy,
                        ModifiedDate=x.ModifiedDate,
                        VendorName=x.VendorName
                    }).ToList();
                }
                return V_List;
            }
        }

        public static string AddVendorList(VendorList Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                try
                {
                    var Check = Context.TblVendorMasters.Where(x => x.FAC_Code == Obj.FAC_Code && x.ManagerID == Obj.ManagerID && x.IsActive == true).FirstOrDefault();
                    if (Check == null)
                    {
                        int status = Context.sp_Insert_Update_TblVendorMaster(Obj.Id, Obj.FAC_Code, Obj.VendorName, Obj.ManagerName, Obj.ManagerEmail, Obj.ManagerMobile, Obj.ManagerID, true, 1);
                        if (status > 0)
                        {

                            TblUser Usr = new TblUser
                            {
                                Agency_Id = 27,
                                IsActive=true,
                                CreatedBy = 1,
                                CreationDate = DateTime.Now,
                                Email = Obj.ManagerEmail,
                                Password = Obj.ManagerID + "@123",
                                Role_Id = 5,
                                UserName = Obj.ManagerID
                            };
                            Context.Entry(Usr).State = System.Data.Entity.EntityState.Added;
                            Context.SaveChanges();

                            return "Success: Vendor Added!";
                        }

                        else
                            return "Error: Vendor Not Added!";
                    }

                    else {
                        return "Error: Duplicate Manager ID is not allowed";
                    }
                }
                catch (Exception Ex) {
                    return "Error: " + Ex.Message.ToString();
                }
                    
            }
        }
        
        public static List<VendorTrainerList> GetVendorTrainerList(int User_Id)
        {
            List<VendorTrainerList> V_List = new List<VendorTrainerList>();
            using (var Context = new CEIDBEntities())
            {
                var GetData = Context.sp_GetVendorTrainerMaster(User_Id).ToList();
                if (GetData.Count != 0)
                {
                    V_List = GetData.Select(x => new VendorTrainerList
                    {
                        CreatedBy = x.CreatedBy,
                        CreationDate = x.CreationDate,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        TrainerCode=x.TrainerCode,
                        TrainerEmail=x.TrainerEmail,
                        TrainerMobile=x.TrainerMobile,
                        TrainerName=x.TrainerName,
                        Vendor_Id=x.Vendor_Id
                    }).ToList();
                }
                return V_List;
            }
        }
        
        public static string UpdateVendorTrainerList(List<VendorTrainerList> List)
        {
            using (var Context = new CEIDBEntities())
            {
                try
                {
                    int index = 0;
                    foreach (var Obj in List)
                    {
                        index++;
                        string Code = "";
                        var vandorDtl = Context.TblVendorMasters.Where(x => x.ManagerID == Obj.VendorName && x.IsActive == true).FirstOrDefault();
                        Obj.Vendor_Id = vandorDtl != null ? vandorDtl.Id: Obj.Vendor_Id;
                        if (Obj.TrainerCode == null) {
                            char[] whitespace = new char[] { ' ', '\t' };
                            string[] ssizes = vandorDtl.VendorName.Split(whitespace);
                            foreach (string s in ssizes) { Code = Code+ s.Substring(0, 1).ToUpper(); }
                            Code = Code + "0" + index;
                            Obj.TrainerCode = Code;
                        }
                        int status = Context.sp_Insert_Update_TblVendorTrainerMaster(Obj.Id, Obj.Vendor_Id, Obj.TrainerCode, Obj.TrainerName, Obj.TrainerMobile, Obj.TrainerEmail,Obj.IsActive, Obj.CreatedBy);

                        if (Obj.Id==0 && status>0) {
                            TblUser Usr = new TblUser
                            {
                                Agency_Id = 27,
                                IsActive = true,
                                CreatedBy = 1,
                                CreationDate = DateTime.Now,
                                Email = Obj.TrainerEmail,
                                Password = Obj.TrainerCode + "@123",
                                Role_Id = 6,
                                UserName = Obj.TrainerCode
                            };
                            Context.Entry(Usr).State = System.Data.Entity.EntityState.Added;
                            Context.SaveChanges();
                        }
                    }
                    return "Success: Trainer Updated!";
                }
                catch (Exception Ex)
                {
                    return "Error: " + Ex.Message.ToString();
                }

            }
        }

        public static IList<SessionListForStepAgencyManager> GetSessionList_StepManager(Filter_STEP_Agency Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                var UserDetail = Context.TblUsers.Where(x => x.UserName == Obj.UserName && x.IsActive == true).FirstOrDefault();
                var VendorDtl = Context.TblVendorMasters.Where(x => x.ManagerID == Obj.UserName && x.IsActive == true).FirstOrDefault();
                IList<SessionListForStepAgencyManager> objlist = null;
                var ReqData = Context.sp_GetSessionListForStepAgencyManager(UserDetail.Agency_Id, VendorDtl.Id, Obj.Month,Obj.ProgramCode).ToList();
                if (ReqData.Count != 0)
                {
                    objlist = ReqData.Select(x => new SessionListForStepAgencyManager()
                    {
                        Id = x.Id,
                        Duration = x.Duration,
                        EndDate = x.EndDate,
                        FacultyCode = x.FacultyCode,
                        ProgramCode = x.ProgramCode,
                        ProgramName = x.ProgramName,
                        SessionID = x.SessionID,
                        StartDate = x.StartDate,
                        TrainerName = x.TrainerName,
                        TrainerCode = x.TrainerCode,
                        Region=x.Region,
                        Venue=x.Venue
                    }).ToList();
                }
                return objlist;
            }
        }

        public static IList<ActiveTrainerForVendor> GetActiveTrainerForVendor(string UserName)
        {
            using (var Context = new CEIDBEntities())
            {
                //var UserDetail = Context.TblUsers.Where(x => x.UserName == UserName && x.IsActive == true).FirstOrDefault();
                var VendorDtl = Context.TblVendorMasters.Where(x => x.ManagerID == UserName && x.IsActive == true).FirstOrDefault();
                IList<ActiveTrainerForVendor> objlist = null;
                var ReqData = Context.sp_GetActiveTrainerForVendor(VendorDtl.Id).ToList();
                if (ReqData.Count != 0)
                {
                    objlist = ReqData.Select(x => new ActiveTrainerForVendor()
                    {
                        Id = x.Id,
                        TrainerName = x.TrainerName,
                        TrainerCode = x.TrainerCode,
                        Vendor_Id=x.Vendor_Id,
                        IsActive=x.IsActive,
                        CreatedBy=x.CreatedBy,
                        CreationDate=x.CreationDate,
                        ModifiedBy=x.ModifiedBy,
                        ModifiedDate=x.ModifiedDate,
                        TrainerEmail=x.TrainerEmail,
                        TrainerMobile=x.TrainerMobile
                    }).ToList();
                }
                return objlist;
            }
        }

        public static string UpdateFaculty(SessionListForStepAgencyManager Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                List<TblNomination> ReqData = new List<TblNomination>();
                string SessionID = Obj != null ? Obj.SessionID : string.Empty;
                string StartDate = Obj != null ? Obj.StartDate.ToString() : string.Empty;
                string Venue=Obj.Venue;
                int? agency_Id = 27;
                int? AgencyType = Context.TblRTCMasters.Where(x => x.Agency_Id == agency_Id && x.IsActive == true).Select(x => x.AgenyType).FirstOrDefault();
                int user_id = Context.TblUsers.Where(x => x.UserName == Obj.TrainerCode && x.IsActive == true).Select(x => x.User_Id).FirstOrDefault();
                var TrainerDtl = Context.sp_GetExternalFacultyDetail(Obj.Id).FirstOrDefault();
                var TokenDtl = Context.sp_GetToken_By_UserID(TrainerDtl.User_Id).FirstOrDefault();
                if (Obj.StartDate>DateTime.Now)
                {
                    var CLNDR_Data = Context.sp_UpdateTrainer_TblCalendar(Obj.SessionID,Obj.ProgramCode,Obj.StartDate,Obj.EndDate,Obj.Venue, Obj.Region,TrainerDtl.TrainerCode,Obj.User_Id);
                }
                else
                {
                    ReqData = Context.TblNominations.Where(x => x.SessionID == Obj.SessionID).ToList();
                    Venue = ReqData.Count != 0 ? ReqData[0].Venue : string.Empty;

                    foreach (var Item in ReqData)
                    {
                        StartDate = Item.StartDate != null ? Item.StartDate.Value.ToString("dd-MMM-yyyy") : "-";
                        SessionID = Item.SessionID;
                        Item.FacultyCode = TrainerDtl.TrainerCode;
                        Item.Faculty_Id = Obj.Id;
                        Item.ModifiedBy = Obj.User_Id;
                        Item.ModifiedDate = DateTime.Now;
                        Context.Entry(Item).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }                   
                }
                if (TokenDtl != null)
                {
                    List<string> Ids = new List<string>();
                    Ids.Add(TokenDtl.Token);
                    string Title = "A new course is assigned to you";
                    string Content = "Session Id: " + SessionID + " Start date: " + StartDate + ". Venue: " + Venue;
                    SendNotification(Ids, Content, Title, 1, "");
                }
                return "Success: Updated Successfully!";
            }
        }

        public static string AddCandidate_Mobile(AddCandidateBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 1500;

                //foreach (var Obj in CandidatesList)
                //{
                //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                //{
                // do something with EF here                        
                var Check = context.TblNominations.Where(x => x.Name == Obj.Name && x.MobileNo == Obj.MobileNo && x.IsActive == true).FirstOrDefault();
                try
                {
                    Obj.MSPIN = FacultyAgencyLevel.GenerateMSPIN(Obj.OrgType);// Obj.MSPIN.Trim().Equals(string.Empty) ? GenerateMSPIN(Obj.OrgType) : Obj.MSPIN.Trim();
                    var Details = context.TblNominations.Where(x => x.SessionID == Obj.SessionID && x.IsActive == true).FirstOrDefault();
                    TblNomination tn = new TblNomination
                    {
                        SessionID = Obj.SessionID,
                        IsActive = true,
                        AgencyCode = Details.AgencyCode,
                        Agency_Id = Details.Agency_Id,
                        Co_id = Details.Co_id,
                        CreatedBy = Obj.CreatedBy,
                        CreationDate = DateTime.Now,
                        DateofBirth = Obj.DateofBirth,
                        Duration = Details.Duration,
                        EndDate = Details.EndDate,
                        FacultyCode = Details.FacultyCode,
                        Faculty_Id = Details.Faculty_Id,
                        MobileNo = Obj.MobileNo,
                        MSPIN = Obj.MSPIN,
                        Location = Obj.Location,
                        DealerName = Obj.Organization,
                        Name = Obj.Name,
                        ProgramCode = Details.ProgramCode,
                        ProgramId = Details.ProgramId,
                        StartDate = Details.StartDate,
                        Venue= Details.Venue,
                        Region= Details.Region
                    };
                    context.Entry(tn).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();

                    string pass = Convert.ToDateTime(Obj.DateofBirth).ToString("dd-MM-yyyy");
                    pass = pass.Replace("/", "");
                    pass = pass.Replace("-", "");
                    pass = pass.Replace(" ", "");
                    TblUser tu = new TblUser
                    {
                        Agency_Id = Details.Agency_Id,
                        CreatedBy = Obj.CreatedBy.Value,
                        CreationDate = DateTime.Now,
                        //Email=Obj.Equals
                        IsActive = true,
                        Role_Id = 4,
                        Password = pass,
                        UserName = Obj.MSPIN,
                    };
                    context.Entry(tu).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    Obj.Msg = "Added";
                    int Status = context.SP_BatchJobMSPIN_And_SessionId_Wise(tn.MSPIN, tn.SessionID);
                }
                catch (Exception ex)
                {
                    return "Error : " + ex.Message.ToString();
                }
                finally
                {
                    GC.Collect();
                }
                return "Success : Candidate added successfully";
            }
        }

        private static string SendNotification(List<string> deviceRegIds, string message, string title, long id, string PageName)
        {
            try
            {
                //string iconpath = "http://marutisuzukistep.co.in/dist/img/maruti-logo1.jpg";// System.Configuration.ConfigurationManager.AppSettings["icon"];
                string regIds = string.Join("", "", deviceRegIds);
                //string regIds = string.Join("\",\"", deviceRegIds);

                //Android
                //string AppId = "com.maruti.suzuki.SSTC";
                string Server_Key = "AAAAXfncR6Q:APA91bGkPOuqXrdSU9CW4HAc_UsXch6hZu8lZSZRuI6NiKIfOAXMeS6l5RicnGKO3pbVHsWqNWcYoL8lAOx6dVX7k9gBfI1Bdb7WpH1v6yLWrUDu0ShCtawilz6KlpAjG6WWjV3fsVDq";
                var SenderId = "403623921572";



                //IOS
                //string AppId = "Hansa.Renkie.0501";
                //string Server_Key = "AAAAU3H_pG0:APA91bE32Hif_QUEC02AwfAsDd3fxOb4VQJiByJF5NZydeDaSxFmdE0xFUDJ-lHJcObiAkJU6nzfORofqICGkz3_TNLbniqMd8wHbfK3dnXCTqZOjnAEnp0WFpr28xD1zvWt8bX8YUrf";
                //var SenderId = "358394864749";


                Mobile_Notification nm = new Mobile_Notification();
                nm.Title = title;
                nm.Message = message;
                nm.ItemId = 1;

                var value = new JavaScriptSerializer().Serialize(nm);
                WebRequest wRequest;
                //wRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                wRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                wRequest.Method = "post";
                wRequest.ContentType = " application/json;charset=UTF-8";
                wRequest.Headers.Add(string.Format("Authorization: key={0}", Server_Key));

                wRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));

                //string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + value + ",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + regIds + "\"]}";

                //deviceRegIds.Add("fibTXqZVeeI:APA91bG-Ufa-_7N0Z1KBHfdBTPNEdZhcWUgTAvdiFLX0gAsJJeWCGfoJTkJ7Xhqo40ooL79WG3rOKYg3CE_GjiqWxKb0jYru1MWtk1zKP-btCazNz1Kq1eO_dvfhHe4dBBF_u1nFxUF4");
                //deviceRegIds.Add("e8cO7F5gi54:APA91bE8RyRHvb9UdXiTnPtZTRxw1hLHZ6UvovOBnOdzIA81FepbAYYriq_gySnIsnzvkESkCcz1lI3pUEHUwK1XoLSQpoXcdZjxqrNu_stWpX98biGF8KBD_AWWTFVg-9od2ZnUAKkz");

                var payload = new
                {
                    registration_ids = deviceRegIds,
                    foreground = true,
                    priority = "high",
                    content_available = true,
                    led = "#FFFFFF",
                    //led = new { color = "#FFFFFF", on = 500, off = 500 },
                    show_in_foreground = true,
                    sound = "default",
                    forceShow = true,
                    // badge = 3,
                    data = new
                    {
                        title = title,
                        body = message,
                        PageName = PageName,
                        //badge = 3
                    },
                    notification = new
                    {
                        body = message,
                        color = "#f53d3d",
                        forceShow = true,
                        title = title,
                        priority = "high",
                        content_available = true,
                        show_in_foreground = true,
                        foreground = true,
                        sound = "default",
                        led = "#FFFFFF",
                        //led = new { color = "#FFFFFF", on = 500, off = 500 },
                        icon = "notification_icon",
                        PageName = PageName,
                        click_action = "FCM_PLUGIN_ACTIVITY",
                        // badge = 3
                    }
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] bytes = Encoding.UTF8.GetBytes(postbody);

                //   Byte[] bytes = Encoding.UTF8.GetBytes(postData);
                wRequest.ContentLength = bytes.Length;

                Stream stream = wRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                WebResponse wResponse = wRequest.GetResponse();

                stream = wResponse.GetResponseStream();

                StreamReader reader = new StreamReader(stream);

                String response = reader.ReadToEnd();

                HttpWebResponse httpResponse = (HttpWebResponse)wResponse;
                string status = httpResponse.StatusCode.ToString();

                reader.Close();
                stream.Close();
                wResponse.Close();

                if (status == "")
                {
                    return response;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
