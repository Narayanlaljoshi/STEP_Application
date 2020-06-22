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
    public class QueriesDAL
    {
        public static List<QueriesBLL> GetAllQueries(int User_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                var userDtl = Context.TblUsers.Where(x => x.User_Id == User_Id && x.IsActive==true).FirstOrDefault();
                var list = Context.SP_GetAllQueries(userDtl.Agency_Id).ToList();
                List<QueriesBLL> QList = new List<QueriesBLL>();
                if (list.Count > 0)
                {
                    foreach (var a in list)
                    {
                        //List<Attachment> attachments = null;
                        var AttachmentsList = Context.TblQueryAttachments.Where(s => s.Query_Id == a.Id).ToList();
                        
                        QList.Add(new QueriesBLL
                        {
                            CreationDate = a.CreationDate,
                            Id = a.Id,
                            IsActive = a.IsActive,
                            QueryBody = a.QueryBody,
                            QuerySubject = a.QuerySubject,
                            IsClosed = a.IsClosed,
                            RoleName = a.RoleName,
                            User_Id = a.User_Id,
                            Email = a.Email,
                            AgencyCode = a.AgencyCode,
                            Remarks = a.Remarks,
                            ModifiedDate = a.ModifiedDate,
                            ModifiedBy = a.ModifedBy,
                            Name=a.Name,
                            AttachmentsList = AttachmentsList.Select(x => new Attachments()
                            {
                                AttachmentName = x.AttachmentName,
                                AttachmentPath = x.AttachmentPath,
                                AttachmentType = x.AttachmentType
                            }).ToList(),
                            Status_Id = a.CurrentStatus_Id != null ? a.CurrentStatus_Id.Value : 0,
                            StatusName = a.StatusName
                        });
                    }
                    

                    return QList;
                }
                return QList;
            }
        }

        public static int SaveQuery(SaveQueryBLL obj, ref int Query_Id)
        {
            using (var Context = new CEIDBEntities())
            {

                TblQueryHeader header = new TblQueryHeader()
                {
                    AssignTo = obj.AssignTo,
                    CreatedBy = obj.User_Id,
                    CreationDate = DateTime.Now,
                    IsActive = true,
                    IsClosed = 0,
                    QueryBody = obj.QueryBody,
                    QuerySubject = obj.QuerySubject,
                    CurrentStatus_Id=1

                };
                Context.Entry(header).State = System.Data.Entity.EntityState.Added;
                Context.SaveChanges();


                TblQueryDetail detail = new TblQueryDetail()
                {
                    Header_Id = header.Id,
                    AssignTo = obj.AssignTo,
                    CreatedBy = obj.User_Id,
                    CreationDate = DateTime.Now,
                    IsActive = true,
                    CurrentStatus_Id=1,
                    FutureStatus_Id=5
                };
                Context.Entry(detail).State = System.Data.Entity.EntityState.Added;
                Context.SaveChanges();
                
                //Setting Query_Id
                Query_Id = header.Id;


                return 1;
            }
        }

        public static string UpdateQuery(CloseQueryBLL Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                var QDetail = Context.TblQueryHeaders.Where(x => x.Id == Obj.Query_Id).FirstOrDefault();
                if (QDetail != null)
                {
                    QDetail.IsClosed = Obj.Status_Id==7? 1:0;
                    QDetail.CurrentStatus_Id = Obj.Status_Id;
                    QDetail.Remarks = Obj.Remarks;
                    QDetail.ModifedBy = Obj.User_Id;
                    QDetail.ModifiedDate = DateTime.Now;
                    Context.Entry(QDetail).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();

                    var Qdtl = Context.TblQueryDetails.Where(X => X.Header_Id == QDetail.Id).ToList();

                    if (Obj.Status_Id == 2)
                    {
                        Qdtl[Qdtl.Count - 1].FutureStatus_Id = Obj.Status_Id;
                        Qdtl[Qdtl.Count - 1].ModifiedBy = Obj.User_Id;
                        Qdtl[Qdtl.Count - 1].ModifiedDate = DateTime.Now;
                        Context.Entry(Qdtl[Qdtl.Count - 1]).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }
                    else
                    {
                        TblQueryDetail tblQueryDetail = new TblQueryDetail
                        {
                            CurrentStatus_Id = Obj.Status_Id,
                            FutureStatus_Id = Obj.Status_Id,
                            CreatedBy = Obj.User_Id,
                            CreationDate = DateTime.Now,
                            Header_Id = QDetail.Id,
                            Remarks = Obj.Remarks,
                            IsActive = true
                        };
                        Context.Entry(tblQueryDetail).State = System.Data.Entity.EntityState.Added;
                        Context.SaveChanges();
                    }
                    
                    Query_Update_Email(Obj.Query_Id);
                    return "Success : Query Updated Successfully";
                }

                return "Success : Query can not be closed.";
            }

        }

        public static void UploadQueryAttachement(QueryAttachementBLL obj)
        {

            using (var context = new CEIDBEntities())
            {
                TblQueryAttachment att = new TblQueryAttachment()
                {
                    AttachmentName = obj.FileName,
                    AttachmentPath = obj.Path,
                    AttachmentType = obj.AttachmentType,
                    Query_Id = obj.Query_Id,
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    CreatedBy = obj.User_Id,
                };
                context.Entry(att).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
        }
        public static void Query_Update_Email(int Query_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                var QueryDetail = Context.TblQueryHeaders.Where(x => x.Id == Query_Id).FirstOrDefault();
                var Attachment = Context.TblQueryAttachments.Where(x => x.Query_Id == Query_Id).ToList();
                var UserDetail = Context.sp_GetEmailTrail(QueryDetail.CreatedBy).FirstOrDefault();
                var statusDtl = Context.TblStatus.Where(x => x.Status_Id == QueryDetail.CurrentStatus_Id).FirstOrDefault();
                string ADminEmail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
                string QueryMail = System.Configuration.ConfigurationManager.AppSettings["QueryMail"];
                MailAddressCollection toMail = new MailAddressCollection();
                MailAddressCollection ccMail = new MailAddressCollection();

                NominationValidationBLL Data = new NominationValidationBLL();
                UserDetailsBLL Obj = new UserDetailsBLL();
                try
                {
                    //toMail.Add(ADminEmail);
                    toMail.Add(UserDetail.Email);
                    ccMail.Add(ADminEmail);
                    ccMail.Add(QueryMail);
                    if (UserDetail.RTM_Email!=null)
                    {
                        ccMail.Add(UserDetail.RTM_Email);
                    }
                    string Body = "<html><body><h3>Dear "+ (UserDetail.Name!=null ? UserDetail.Name : UserDetail.USerName) + ",</h3><b>Greetings for the day!!</b>";
                    Body += "<p>Status of query is: " +statusDtl.StatusName +".</p>";
                    //Body += "<p>" + QueryDetail.QueryBody + "</p>";

                    //foreach (var item in Attachment)
                    //{
                    //    Body += "<p>Attachment: " + item.AttachmentPath + "</p>";
                    //}

                    Body += "<p>Thank You.</p><p> Regards </p><p>Step Portal </p><p>marutisuzukistep.co.in</p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    Email.sendEmail("", toMail, ccMail, "STEP | Query Update |" + QueryDetail.QuerySubject, Body);

                    }
                catch (Exception Ex)
                {
                    //toMail.Add("kumar.imus.3@gmail.com");
                    toMail.Add(QueryMail);

                    string Body = "<html><body><h3>Dear,</h3><b>Greetings for the day!!</b>";
                    Body += "<p>An exception has encountered while sending the email." + Ex.ToString() + "</p>";
                    Body += "<p>Thank You.</p><p> Regards </p><p> Step</p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    Email.sendEmail("", toMail, ccMail, "STEP | Query Mail Exception | " + DateTime.Now.ToString("dd-MMM-yyyy"), Body);
                    //throw Ex;
                }
            }
        }

        public static void QueryEmail(int Query_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                var QueryDetail = Context.TblQueryHeaders.Where(x => x.Id == Query_Id).FirstOrDefault();
                var Attachment = Context.TblQueryAttachments.Where(x => x.Query_Id == Query_Id).ToList();
                //var UserDetail = Context.TblUsers.Where(x => x.User_Id == QueryDetail.CreatedBy).FirstOrDefault();
                var UserDetail = Context.sp_GetEmailTrail(QueryDetail.CreatedBy).FirstOrDefault();
                //if (UserDetail != null)
                //{
                string ADminEmail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
                string QueryMail = System.Configuration.ConfigurationManager.AppSettings["QueryMail"];
                MailAddressCollection toMail = new MailAddressCollection();
                MailAddressCollection ccMail = new MailAddressCollection();

                NominationValidationBLL Data = new NominationValidationBLL();
                UserDetailsBLL Obj = new UserDetailsBLL();
                try
                {

                    //toMail.Add(ADminEmail);
                    toMail.Add(QueryMail);
                    ccMail.Add(ADminEmail);
                    ccMail.Add(UserDetail.Email);
                    ccMail.Add(UserDetail.RTM_Email);
                    string Body = "<html><body><h3>Dear Sir,</h3><b>Greetings for the day!!</b>";
                    Body += "<p>" + UserDetail.Name + " has raised the following query: " + "</p>";
                    Body += "<p>" + QueryDetail.QueryBody + "</p>";

                    foreach (var item in Attachment)
                    {
                        Body += "<p>Attachment: " + item.AttachmentPath + "</p>";
                    }

                    Body += "<p>Thank You.</p><p> Regards </p><p> STEP </p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    Email.sendEmail("", toMail, ccMail, "STEP | "+UserDetail.Name+" |"+QueryDetail.QuerySubject, Body);

                }
                catch (Exception Ex)
                {
                    //toMail.Add("kumar.imus.3@gmail.com");
                    toMail.Add(QueryMail);

                    string Body = "<html><body><h3>Dear,</h3><b>Greetings for the day!!</b>";
                    Body += "<p>An exception has encountered while sending the email." + Ex.ToString() + "</p>";
                    Body += "<p>Thank You.</p><p> Regards </p><p> Step</p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    Email.sendEmail("", toMail, ccMail, "STEP | Query Mail Exception | " + DateTime.Now.ToString("dd-MMM-yyyy"), Body);
                    //throw Ex;
                }
            }
        }

        public static List<StatusList> GetStatusList()
        {
            List<StatusList> StatusList = new List<StatusList>();
            using (var context= new CEIDBEntities())
            {
                var Data = context.TblStatus.Where(x => x.IsActive == true).ToList();
                StatusList = Data.Select(x => new ProjectBLL.CustomModel.StatusList {
                    IsActive=x.IsActive,
                    StatusName=x.StatusName,
                    Status_Id=x.Status_Id
                }).ToList();
                return StatusList;
            }
        }

        public static List<SummaryReport> GetSummaryReport(int User_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                var userDtl = Context.TblUsers.Where(x => x.User_Id == User_Id && x.IsActive == true).FirstOrDefault();
                var list = Context.SP_GetAllQueries(userDtl.Agency_Id).ToList();
                var AgencyList = list.Select(x => x.AgencyCode).Distinct().ToList(); ;
                List<SummaryReport> QList = new List<SummaryReport>();
                foreach (var Agency in AgencyList)
                {
                    var ClosedRequests = list.Where(x => x.AgencyCode == Agency && x.CurrentStatus_Id==7).Count();
                    var PendingRequests = list.Where(x => x.AgencyCode == Agency && x.CurrentStatus_Id != 7).Count();
                    QList.Add(new SummaryReport {
                        AgencyCode=Agency,
                        Closed= ClosedRequests,
                        Pending= PendingRequests,
                        TotalQueries=ClosedRequests+ PendingRequests
                    });
                }
                return QList;
            }
        }

        public static EmployeeDetails GetEmployeeDetails(int Query_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                var userDtl = Context.sp_GetEmployeeDetils(Query_Id).FirstOrDefault();
                EmployeeDetails Obj = new EmployeeDetails {
                    EmployeeCode= userDtl.Username,
                    Name= userDtl.Name,
                    Id= userDtl.Id,
                    QuerySubject= userDtl.QuerySubject
                };
                return Obj;
            }
        }

        public static IList<StatusdetailInfo> GetCurrentStatus(int Query_Id)
        {
            using (var context = new CEIDBEntities())
            {
                var fut = context.sp_GetCurrentStatusList(Query_Id).ToList();

                IList<StatusdetailInfo> list = null;

                list = fut.Select(x => new StatusdetailInfo()
                {
                    Status = x.StatusName,
                    Remark = x.Remarks,
                    Added_By = x.Action_Taken_by,
                    Added_Date = x.CreationDate,
                    Query_Id=x.ID

                }).ToList();

                return list;
            }

        }
        public static IList<FutureStatus> GetFutureStatus(int Query_Id)
        {
            using (var context = new CEIDBEntities())
            {
                var fut = context.sp_GetFutureStatus(Query_Id).ToList();
                IList<FutureStatus> list = null;
                list = fut.Select(s => new FutureStatus()
                {
                    Status = s.StatusName,
                    EmployeeCode = s.Action_Taken_by
                }).ToList();
                return list;
            }
        }
    }
}
