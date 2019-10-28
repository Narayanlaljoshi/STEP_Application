using Newtonsoft.Json;
using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace STEPDAL.CustomDAL
{
    public class ManageSessionDAL
    {
        public static IList<SessionList> GetSessionList(int Agency_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                IList<SessionList> objlist = null;
                var ReqData = Context.sp_GetSessionListAgencyWise(Agency_Id).ToList();
                if (ReqData.Count != 0)
                {
                    objlist = ReqData.Select(x => new SessionList()
                    {
                        EndDate = x.EndDate,
                        FacultyCode = x.FacultyCode,
                        FacultyName = x.FacultyName,
                        Faculty_Id = x.Faculty_Id,
                        SessionID = x.SessionID,
                        StartDate = x.StartDate,
                        Duration = x.Duration,
                        ProgramCode = x.ProgramCode,
                        ProgramName = x.ProgramName
                    }).ToList();
                }
                return objlist;
            }
        }
       
        public static IList<SessionList> GetSessionListByProgramId(int ProgramId)
        {
            using (var Context = new CEIDBEntities())
            {
                IList<SessionList> objlist = null;
                //var ReqData = Context.sp_GetSessionListAgencyWise(Agency_Id).ToList();
                //var GetList = Context.TblNominations.Where(x => x.ProgramId == ProgramId).ToList();
                var GetList = Context.sp_GetSessionListByProgramId(ProgramId).ToList();
                if (GetList.Count != 0)
                {
                    objlist = GetList.Select(x => new SessionList()
                    {
                        SessionID = x.SessionID,
                        EndDate=x.EndDate,
                        StartDate=x.StartDate
                    }).ToList();
                }
                return objlist;
            }
        }

        public static IList<SessionList> GetSessionList_Mobile(int Agency_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                IList<SessionList> objlist = null;
                var ReqData = Context.sp_GetSessionListAgencyWise(Agency_Id).ToList();
                if (ReqData.Count != 0)
                {
                    objlist = ReqData.Select(x => new SessionList()
                    {
                        EndDate = x.EndDate,
                        FacultyCode = x.FacultyCode,
                        FacultyName = x.FacultyName,
                        Faculty_Id = x.Faculty_Id,
                        SessionID = x.SessionID,
                        StartDate = x.StartDate,
                        Duration = x.Duration,
                        ProgramCode = x.ProgramCode,
                        ProgramName = x.ProgramName
                    }).ToList();
                }
                return objlist;
            }
        }
        public static IList<FacultyList> GetFacultyList(int Agency_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                IList<FacultyList> objlist = null;
                var ReqData = Context.sp_GetAgencyWiseFaculty(Agency_Id);
                objlist = ReqData.Select(x => new FacultyList()
                {                   
                    FacultyCode = x.FacultyCode,
                    FacultyName = x.FacultyName,
                    Faculty_Id = x.Faculty_Id,
                    Agency_Id=x.Agency_Id,
                    CreatedBy=x.CreatedBy,
                    CreationDate=x.CreationDate,
                    Email=x.Email,
                    IsActive=x.IsActive,
                    Mobile=x.Mobile,
                    ModifiedBy=x.ModifiedBy,
                    ModifiedDate=x.ModifiedDate,
                }).ToList();
                return objlist;
            }
        }

        public static IList<FacultyList> GetFacultyListForDDL(int Agency_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                //int? AgenCyType = Context.TblRTCMasters.Where(x => x.Agency_Id == Agency_Id && x.IsActive == true).Select(x=>x.AgenyType).FirstOrDefault();
                IList<FacultyList> objlist = null;
                var ReqData = Context.sp_GetAgencyWiseFaculty(Agency_Id);
                objlist = ReqData.Select(x => new FacultyList()
                {
                    FacultyCode = x.FacultyCode,
                    FacultyName ="(" + x.FacultyCode+")-"+ x.FacultyName,
                    Faculty_Id = x.Faculty_Id,
                    Agency_Id = x.Agency_Id,
                    CreatedBy = x.CreatedBy,
                    CreationDate = x.CreationDate,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    Mobile = x.Mobile,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                }).ToList();
                return objlist;
            }
        }
        public static IList<FacultyList> GetFacultyList_External(int Agency_Id,string UserName)
        {
            using (var Context = new CEIDBEntities())
            {
                //int? AgenCyType = Context.TblRTCMasters.Where(x => x.Agency_Id == Agency_Id && x.IsActive == true).Select(x=>x.AgenyType).FirstOrDefault();
                IList<FacultyList> objlist = null;
                var ReqData = Context.sp_GetFacultyList_ExternalAgency(Agency_Id,UserName);
                objlist = ReqData.Select(x => new FacultyList()
                {
                    FacultyCode = x.SubFaculty_Code,
                    FacultyName = "(" + x.SubFaculty_Code + ")-" + x.SubFacultyName,
                    Faculty_Id = x.SubFaculty_Id,
                    Agency_Id = x.Agency_Id,
                    Email = x.Email,
                    Mobile = x.Mobile,
                    FAC_User_Id=x.User_Id
                }).ToList();
                return objlist;
            }
        }
        public static IList<FacultyList> GetFacultyList_Mobile(int Agency_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                IList<FacultyList> objlist = null;
                var ReqData = Context.sp_GetAgencyWiseFaculty(Agency_Id);
                objlist = ReqData.Select(x => new FacultyList()
                {
                    FacultyCode = x.FacultyCode,
                    FacultyName = "(" + x.FacultyCode + ")-" + x.FacultyName,
                    Faculty_Id = x.Faculty_Id,
                    Agency_Id = x.Agency_Id,
                    CreatedBy = x.CreatedBy,
                    CreationDate = x.CreationDate,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    Mobile = x.Mobile,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                }).ToList();
                return objlist;
            }
        }
        public static string UpdateFaculty(SessionList Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                string SessionID = Obj!=null? Obj.SessionID: string.Empty;
                string StartDate = Obj != null ? Obj.StartDate.ToString() :string.Empty;
                var ReqData = Context.TblNominations.Where(x=>x.SessionID==Obj.SessionID).ToList();
                string Venue = ReqData.Count != 0 ? ReqData[0].Venue:string.Empty;
                int? agency_Id = ReqData[0].Agency_Id;
                int? AgencyType = Context.TblRTCMasters.Where(x => x.Agency_Id == agency_Id && x.IsActive == true).Select(x => x.AgenyType).FirstOrDefault();

                if (AgencyType == 1)
                {
                    var FacultyDtl = Context.TblFaculties.Where(x => x.Faculty_Id == Obj.Faculty_Id && x.IsActive == true).FirstOrDefault();
                    //FacultyDtl = Context.sp_GetExternalFacultyDetail(Obj.Faculty_Id).FirstOrDefault();
                    foreach (var Item in ReqData)
                    {
                        StartDate = Item.StartDate != null ? Item.StartDate.Value.ToString("dd-MMM-yyyy") : "-";
                        SessionID = Item.SessionID;
                        Item.FacultyCode = FacultyDtl.FacultyCode;
                        Item.Faculty_Id = Obj.Faculty_Id;
                        Item.ModifiedBy = Obj.User_Id;
                        Item.ModifiedDate = DateTime.Now;
                        Context.Entry(Item).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }
                }
                else {
                    var FacultyDtl = Context.sp_GetExternalFacultyDetail(Obj.Faculty_Id).FirstOrDefault();
                    var TokenDtl = Context.sp_GetToken_By_UserID(FacultyDtl.User_Id).FirstOrDefault();
                    foreach (var Item in ReqData)
                    {
                        StartDate = Item.StartDate != null ? Item.StartDate.Value.ToString("dd-MMM-yyyy") : "-";
                        SessionID = Item.SessionID;
                        Item.FacultyCode = FacultyDtl.TrainerCode;
                        Item.Faculty_Id = Obj.Faculty_Id;
                        Item.ModifiedBy = Obj.User_Id;
                        Item.ModifiedDate = DateTime.Now;
                        Context.Entry(Item).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }
                    if (TokenDtl != null)
                    {
                        List<string> Ids = new List<string>();
                        Ids.Add(TokenDtl.Token);
                        string Title = "A new course is assigned to you";
                        string Content = "Session Id: " + SessionID + " Start date: " + StartDate+". Venue: "+ Venue;
                        SendNotification(Ids, Content, Title, 1, "");

                    }
                }
                return "Success: Updated Successfully!";
            }
        }
        public static string UpdateFaculty_Mobile(SessionList Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                string SessionID = string.Empty;
                var ReqData = Context.TblNominations.Where(x => x.SessionID == Obj.SessionID).ToList();
                var FacultyDtl = Context.TblFaculties.Where(x => x.Faculty_Id == Obj.Faculty_Id && x.IsActive == true).FirstOrDefault();
                var TokenDtl = Context.sp_GetToken_By_UserID(Obj.User_Id).FirstOrDefault();
                foreach (var Item in ReqData)
                {
                    SessionID = Item.SessionID;
                    Item.FacultyCode = FacultyDtl.FacultyCode;
                    Item.Faculty_Id = Obj.Faculty_Id;
                    Item.ModifiedBy = Obj.User_Id;
                    Item.ModifiedDate = DateTime.Now;
                    Context.Entry(Item).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();
                }
                {
                    if (TokenDtl != null)
                    {
                        List<string> Ids = new List<string>();
                        Ids.Add(TokenDtl.Token);
                        SendNotification(Ids, "You Have Assigned A Course: " + SessionID + "", "", 1, "");
                    }

                }
                return "Success: Updated Successfully!";
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
                string AppId = "com.maruti.suzuki.SSTC";
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
