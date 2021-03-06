﻿using System;
using System.Net.Mail;
using System.Net;


namespace ProjectDAL.CustomDAL
{
    public class Email
    {

        public static string sendEmail(string FromMailAddress, MailAddressCollection toEmail, MailAddressCollection ccEmail, string Subject, string Body)
        {
            string SMTPEmailHost = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
            string SMTPusername = System.Configuration.ConfigurationManager.AppSettings["SmtpUserName"];
            string SMTPpass = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];
            string AllowEmail = System.Configuration.ConfigurationManager.AppSettings["AllowEmail"];
            //string frmEmail = UserDAL.GetEmail(FromMailAddress);



            if (AllowEmail == "true")
            {

                

                try
                {
                    SmtpClient smtp = new SmtpClient(SMTPEmailHost);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(SMTPusername, SMTPpass);
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(FromMailAddress);


                    //message.CC.Add("tiwarih521@gmail.com");

                    //message.To.Add("tiwarih521@gmail.com");
                    message.Bcc.Add("himanshu.tiwari@phoenixtech.consulting");



                    if (toEmail != null)
                    {
                        foreach (var mail in toEmail)
                        {
                             message.To.Add(mail);
                           
                        }
                    }
                    if (ccEmail != null)
                    {
                        foreach (var mail in ccEmail)
                        {
                            message.CC.Add(mail);
                           
                        }
                    }
                    
                  
                    message.IsBodyHtml = true;
                    message.Subject = Subject;
                    message.Body = Body;
                    //message.Attachments.Add(new Attachment(@"F:\ATS Reports\Saksham Business Tracker as on 19.2.2018.xlsx"));
                  //  message.Attachments.Add(new Attachment(AttachMentPath));
                    smtp.Send(message);

                    return "Success: Notification sent";
                }
                catch (Exception Ex)
                {

                    return "Failed: Email ";
                }
            }
            else
            {
                return "Email not allowed";
            }
        }

        public static string sendEmailReport(string RequestedBy, string RequestAssignedTo, string RequestCreatedBy, string subject, string body)
        {
            try
            {

                // string frmEmail = UserDAL.GetEmail(RequestedBy);
                // string toEmail = UserDAL.GetEmail(RequestAssignedTo);
                // string ccEmail = UserDAL.GetEmail(RequestCreatedBy);
                string frmEmail = RequestedBy;
                string toEmail = RequestAssignedTo;
                string ccEmail = RequestCreatedBy;


                MailAddressCollection m = new MailAddressCollection();

                 m.Add(RequestAssignedTo);

                MailAddressCollection cc = new MailAddressCollection();

                //cc.Add(RequestCreatedBy);
                string str = sendEmail(frmEmail, m, cc, subject, body);
                return str;
            }
            catch
            {
                return "Error";
            }
        }



    }
}
