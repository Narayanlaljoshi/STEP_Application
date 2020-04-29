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

                var list = Context.SP_GetAllQueries().ToList();
                List<QueriesBLL> QList = null;
                if (list.Count > 0)
                {
                    QList = list.Select(x => new QueriesBLL()
                    {
                        CreationDate = x.CreationDate,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        QueryBody = x.QueryBody,
                        QuerySubject = x.QuerySubject,
                        IsClosed = x.IsClosed,
                        RoleName = x.RoleName,
                        User_Id = x.User_Id,
                        Email = x.Email,
                        AgencyType = x.AgencyType,
                        Remarks = x.Remarks,
                        ModifiedDate = x.ModifiedDate,
                        ModifiedBy = x.ModifedBy,
                        AttachmentName = x.AttachmentName,
                        AttachmentPath = x.AttachmentPath,
                        AttachmentType = x.AttachmentType
                    }).ToList();

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

                };
                Context.Entry(detail).State = System.Data.Entity.EntityState.Added;
                Context.SaveChanges();


                //Setting Query_Id
                Query_Id = header.Id;


                return 1;
            }
        }

        public static string CloseQuery(CloseQueryBLL Obj)
        {
            using (var Context = new CEIDBEntities())
            {

                var QDetail = Context.TblQueryHeaders.Where(x => x.Id == Obj.Query_Id).FirstOrDefault();
                if (QDetail != null)
                {
                    QDetail.IsClosed = 1;
                    QDetail.Remarks = Obj.Remarks;
                    QDetail.ModifedBy = Obj.User_Id;
                    QDetail.ModifiedDate = DateTime.Now;

                    Context.Entry(QDetail).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();


                    return "Success : Query Closed Successfully";
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

        public static void QueryEmail(int Query_Id)
        {
            using (var Context = new CEIDBEntities())
            {
                var QueryDetail = Context.TblQueryHeaders.Where(x => x.Id == Query_Id).FirstOrDefault();
                var Attachment = Context.TblQueryAttachments.Where(x => x.Query_Id == Query_Id).FirstOrDefault();
                var UserDetail = Context.TblUsers.Where(x => x.User_Id == QueryDetail.CreatedBy).FirstOrDefault();

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
                    //toMail.Add("kumar.imus.3@gmail.com");
                    //ccMail.Add(UserDetail.Email);
                    string Body = "<html><body><h3>Dear San,</h3><b>Greetings for the day!!</b>";
                    Body += "<p>" + QueryDetail.QueryBody + "</p>";
                    if (Attachment != null)
                    {
                        Body += "<p>Attachment: " + Attachment.AttachmentPath + "</p>";
                    }
                    Body += "<p>Thank You.</p><p> Regards </p><p>" + UserDetail.UserName + " </p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    Email.sendEmail("", toMail, ccMail, "STEP | Query  | " + QueryDetail.QuerySubject, Body);

                }
                catch (Exception Ex)
                {
                    toMail.Add("kumar.imus.3@gmail.com");
                    //ccMail.Add("narayan.joshi@phoenixtech.consulting");

                    string Body = "<html><body><h3>Dear,</h3><b>Greetings for the day!!</b>";
                    Body += "<p>An exception has encountered while sending the email." + Ex.ToString() + "</p>";
                    Body += "<p>Thank You.</p><p> Regards </p><p> " + UserDetail.UserName + "  </p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    Email.sendEmail("", toMail, ccMail, "STEP | Query Mail Exception | " + DateTime.Now.ToString("dd-MMM-yyyy"), Body);
                    //throw Ex;

                }
            }
        }
    }
}
