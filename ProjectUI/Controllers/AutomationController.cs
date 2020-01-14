using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using ProjectBLL.CustomModel;
using ProjectUI.ServiceReference1;
using STEPDAL.CustomDAL;

namespace ProjectUI.Controllers
{
    public class AutomationController : ApiController
    {
        [HttpGet]
        public void GetNomiantionFile()
        {
            //int countIfProccessed = 0;
            string ADminEmail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
            MailAddressCollection toMail = new MailAddressCollection();
            MailAddressCollection ccMail = new MailAddressCollection();

            NominationValidationBLL Data = new NominationValidationBLL();
            UserDetailsBLL Obj = new UserDetailsBLL();
            try
            {
                ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
                Client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 0, 600);
                DataTable NominTionFile = Client.GetNominationFile(1, Convert.ToDateTime(DateTime.Now));//"13-Aug-19"
                
                if (NominTionFile.Rows.Count > 0)
                {
                    Automation.BulkInsert_RawData(NominTionFile);
                    Automation.FilterNominationData(NominTionFile);
                }
                else
                {
                    toMail.Add(ADminEmail);
                    ccMail.Add("narayan.joshi@phoenixtech.consulting");

                    string Body = "<html><body><h3>Dear San,</h3><b>Greetings for the day!!</b>";
                    Body += "<p>No Nominations Received for Today. </p>";
                    Body += "<p>Thank You.</p><p> Regards </p><p> STEP Portal </p>";
                    Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                    Email.sendEmail("",toMail,ccMail, "STEP | Nomination  | " + DateTime.Now.ToString("dd-MMM-yyyy"),Body);
                }
            }
            catch (Exception Ex)
            {
                toMail.Add(ADminEmail);
                ccMail.Add("narayan.joshi@phoenixtech.consulting");

                string Body = "<html><body><h3>Dear San,</h3><b>Greetings for the day!!</b>";
                Body += "<p>An exception has encountered while downloading nomination file from DMS."+Ex.ToString()+ "</p>";
                Body += "<p>Thank You.</p><p> Regards </p><p> STEP Portal </p>";
                Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                Email.sendEmail("", toMail, ccMail, "STEP | Nomination Download Exception | " + DateTime.Now.ToString("dd-MMM-yyyy"), Body);
                throw Ex;
            }
        }

        [HttpGet]
        public void GetCalendarFile()
        {
            string ADminEmail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
            MailAddressCollection toMail = new MailAddressCollection();
            MailAddressCollection ccMail = new MailAddressCollection();

            try
            {
                ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
                DataTable CalendarFile = Client.GetCalendarFile(1, DateTime.Now);
                Automation.SaveCalanderFile(CalendarFile);
            }
            catch (Exception Ex)
            {
                toMail.Add(ADminEmail);
                ccMail.Add("narayan.joshi@phoenixtech.consulting");

                string Body = "<html><body><h3>Dear San,</h3><b>Greetings for the day!!</b>";
                Body += "<p>An exception has encountered while downloading calendar file from DMS. </p>";
                Body += "<p>Thank You.</p><p> Regards </p><p> STEP Portal </p>";
                Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                Email.sendEmail("", toMail, ccMail, "STEP | Calendar Download Exception | "+DateTime.Now.ToString("dd-MMM-yyyy"), Body);
                throw Ex;
            }
            
        }
        /*Attendance report as per DMS*/
        [HttpGet]
        public void PushAttendanceReport(DateTime EndDate)
        {
            string MailBody = string.Empty;
            string ADminEmail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
            MailAddressCollection toMail = new MailAddressCollection();
            MailAddressCollection ccMail = new MailAddressCollection();

            toMail.Add(ADminEmail);

            ReportInputBLL Obj = new ReportInputBLL
            {
                EndDate = EndDate
            };

            List<AttendanceReportBLL> List= ReportsDAL.GetAttendanceReport(Obj);
            List<string> SessionIDs = List.Select(x => x.SessionID).Distinct().ToList();
            DataTable Dt = new DataTable("myTable");
            Dt.Columns.Add("LV_FACULTY");
            Dt.Columns.Add("LV_PROGRAM");
            Dt.Columns.Add("LV_SESSION_ID");
            Dt.Columns.Add("LV_FROM_DATE");
            Dt.Columns.Add("LV_MSPIN");
            Dt.Columns.Add("LV_DAYS");
            Dt.Columns.Add("LV_ATTENDANCE");
                List<AttendanceDMSAutomation> DMS_List = List.Select(x => new AttendanceDMSAutomation {
                LV_ATTENDANCE=x.P_A,
                LV_DAYS=x.Day,
                LV_FACULTY=x.FacultyCode,
                LV_FROM_DATE=x.StartDate,
                LV_MSPIN=x.MSPIN,
                LV_PROGRAM=x.ProgramCode,
                LV_SESSION_ID=x.SessionID
            }).ToList();
            int rowIndex = 0;

            string[] formats = { "dd/MM/yyyy"};
            foreach (var item in DMS_List) {
                //DateTime dt;
                //DateTime.TryParseExact(item.LV_FROM_DATE.ToString(), formats, null, DateTimeStyles.None, out dt);
                Dt.Rows.Add();
                Dt.Rows[rowIndex][0] = item.LV_FACULTY;
                Dt.Rows[rowIndex][1] = item.LV_PROGRAM;
                Dt.Rows[rowIndex][2] = item.LV_SESSION_ID;
                Dt.Rows[rowIndex][3] = Convert.ToDateTime(item.LV_FROM_DATE).ToString("dd-MMM-yyyy");
                Dt.Rows[rowIndex][4] = item.LV_MSPIN;
                Dt.Rows[rowIndex][5] = item.LV_DAYS;
                Dt.Rows[rowIndex][6] = item.LV_ATTENDANCE;
                rowIndex++;
            }
            Automation.Save_Report_ATNDC_DMS(Dt);
            //LogService(DateTime.Now.ToString() + " ATNDC Bulk cpy done");
            ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
            //Client.InnerChannel.OperationTimeout =new TimeSpan(0,10,15000);
            Client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 10, 60);
            try
            {
                DataTable Return_Dt = Client.UploadAttendance(Dt);
                //LogService(DateTime.Now.ToString() + " Bulk cpy done");
                Automation.Update_Log_Table(Obj.EndDate,"ATNDC",Dt.Rows.Count, Return_Dt.Rows.Count,null);
                if (Return_Dt.Rows.Count != 0)
                {
                    MailBody = GenerateHtml(Return_Dt, "Error While Uploading The attendance Sheet. Number of error records - "+ Return_Dt.Rows.Count, false, SessionIDs);
                    Email.sendEmail("", toMail, ccMail, "STEP | Attendance Sheet Push Failed | " + Obj.EndDate.Value.ToString("dd-MMM-yyyy"), MailBody);
                    return;
                }
                else
                {
                    DataTable Return_DtForBatchJob = Client.RunBatchJob_Attendance(Obj.EndDate.Value.ToString("dd-MMM-yyyy"));
                    //LogService(DateTime.Now.ToString() + " ATNDC Batch Job Done");
                    //Return_DtForBatchJob = Return_DtForBatchJob.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull ||string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();
                    Automation.Update_Log_Table(Obj.EndDate, "ATNDC", Dt.Rows.Count, Return_DtForBatchJob.Rows.Count,"Batch Job");
                    if (Return_DtForBatchJob.Rows.Count != 0)
                    {
                        MailBody = GenerateHtml(Return_DtForBatchJob, "Error while running the batch job SP after uploading attendance sheet. Number of error records - " + Return_DtForBatchJob.Rows.Count, false, SessionIDs);
                        Email.sendEmail("", toMail, ccMail, "STEP | Attendance Sheet Push Failed | " + Obj.EndDate.Value.ToString("dd-MMM-yyyy"), MailBody);
                        //PushMarksReportAsperDMS();
                    }
                    else
                    {
                        MailBody = GenerateHtml(null, "Attendance Sheet Uploaded Successfully", true, SessionIDs);
                        Email.sendEmail("", toMail, ccMail, "STEP | Attendance Sheet Push Successful | " + Obj.EndDate.Value.ToString("dd-MMM-yyyy"), MailBody);
                        //PushMarksReportAsperDMS();
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                LogService(DateTime.Now.ToString() + " EXCEPTION " +Ex.ToString());
                MailBody = GenerateHtml(null, "Exception Occurred While Uploading The attendance Sheet i.e. - " + Ex.Message.ToString(), false, SessionIDs);
                Email.sendEmail("", toMail, ccMail, "STEP | Attendance Sheet Push Failed | " + DateTime.Now.ToString("dd-MMM-yyyy"), MailBody);

                throw Ex;
            }
        }

        /*Marks report as per DMS*/
        [HttpGet]
        public string PushMarksReportAsperDMS(DateTime EndDate)
        {

            string MailBody = string.Empty;
            string ADminEmail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
            MailAddressCollection toMail = new MailAddressCollection();
            MailAddressCollection ccMail = new MailAddressCollection();
            toMail.Add(ADminEmail);
            ccMail.Add("narayan.joshi@phoenixtech.consulting");

            List<ScorreDMSAutomation> DMSLIst = new List<ScorreDMSAutomation>();
            SessionIDListBLL Obj = new SessionIDListBLL
            {
                EndDate = EndDate
            };
            List< MarksReportBLL> MarksList= ReportsDAL.GetMarksReportAsperDMS_V2(Obj);
            List<string> SessionIDs = MarksList.Select(x => x.SessionID).Distinct().ToList();
            DataTable Dt = new DataTable("myTable");
            Dt.Columns.Add("LV_FACULTY");
            Dt.Columns.Add("LV_PROGRAM");
            Dt.Columns.Add("LV_SESSION_ID");
            Dt.Columns.Add("LV_FROM_DATE");
            Dt.Columns.Add("LV_MSPIN");
            Dt.Columns.Add("LV_PRE_SCORE");
            Dt.Columns.Add("LV_POST_SCORE");
            Dt.Columns.Add("LV_FIN_YEAR");
            Dt.Columns.Add("PN_NAME");

            if (MarksList.Count != 0)
            {
                DMSLIst = MarksList.Select(x => new ScorreDMSAutomation {
                    LV_FACULTY=x.FacultyCode,
                    LV_FROM_DATE=x.StartDate,
                    LV_MSPIN=x.MSPIN,
                    LV_POST_SCORE=x.PostTest_MarksObtained,
                    LV_PRE_SCORE=x.PreTest_MarksObtained,
                    LV_PROGRAM=x.ProgramCode,
                    LV_SESSION_ID=x.SessionID,
                    LV_FIN_YEAR=x.StartDate.Value.Year,
                    PN_NAME=x.Name
                }).ToList();
            }

            int rowIndex = 0;
            foreach (var item in DMSLIst)
            {
                Dt.Rows.Add();
                Dt.Rows[rowIndex][0] = item.LV_FACULTY;
                Dt.Rows[rowIndex][1] = item.LV_PROGRAM;
                Dt.Rows[rowIndex][2] = item.LV_SESSION_ID;
                Dt.Rows[rowIndex][3] = Convert.ToDateTime(item.LV_FROM_DATE).ToString("dd-MMM-yyyy");
                Dt.Rows[rowIndex][4] = item.LV_MSPIN;
                Dt.Rows[rowIndex][5] = item.LV_PRE_SCORE;
                Dt.Rows[rowIndex][6] = item.LV_POST_SCORE;
                Dt.Rows[rowIndex][7] = item.LV_FIN_YEAR;
                Dt.Rows[rowIndex][8] = item.PN_NAME;
                rowIndex++;
            }

            Automation.Save_Report_SCORE_DMS(Dt);
            //LogService(DateTime.Now.ToString() + " Score Bulk Cpy Done");

            ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
            Client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 10, 60);
            try
            {
                DataTable Return_Dt = Client.UploadScore(Dt);
                //LogService(DateTime.Now.ToString() + " Score Data Uploaded");
                Automation.Update_Log_Table(Obj.EndDate, "SCORE", Dt.Rows.Count, Return_Dt.Rows.Count,null);
                if (Return_Dt.Rows.Count != 0)
                {
                    MailBody = GenerateHtml(Return_Dt, "Error While Uploading The Score Sheet. Number of error records are: "+ Return_Dt.Rows.Count, false, SessionIDs);
                    Email.sendEmail("", toMail, ccMail, "STEP | Score Sheet Push Failed | " + DateTime.Now.ToString("dd-MMM-yyyy"), MailBody);
                    return "Error While Uploading The Score Sheet";
                }
                else
                {
                    DataTable Return_DtForBatchJob = Client.RunBatchJob_Marks();
                    //LogService(DateTime.Now.ToString() + " Score Batch Job Done");
                    Automation.Update_Log_Table(Obj.EndDate, "SCORE", Dt.Rows.Count, Return_DtForBatchJob.Rows.Count,"Batch Job");
                    if (Return_DtForBatchJob.Rows.Count != 0)
                    {
                        MailBody = GenerateHtml(Return_DtForBatchJob, "Error while running the batch job SP after uploading Score sheet. Number of error records are: "+ Return_DtForBatchJob.Rows.Count, false, SessionIDs);
                        Email.sendEmail("", toMail, ccMail, "STEP | Score Sheet Push Failed | " + DateTime.Now.ToString("dd-MMM-yyyy"), MailBody);
                        return "Error while running the batch job SP after uploading Score sheet.";
                    }
                    else
                    {
                        MailBody = GenerateHtml(null, "Score Sheet Uploaded Successfully", true, SessionIDs);
                        Email.sendEmail("", toMail, ccMail, "STEP | Score Sheet Push Successful | " + DateTime.Now.ToString("dd-MMM-yyyy"), MailBody);
                        return "Score Sheet Uploaded Successfully";
                    }
                }
            }
            catch (Exception Ex)
            {
                LogService(DateTime.Now.ToString() + " EXCEPTION " + Ex.ToString());
                Automation.Update_Log_Table(Obj.EndDate, "SCORE", Dt.Rows.Count, null, Ex.ToString());
                MailBody = GenerateHtml(null, "Exception Occurred While Uploading The score Sheet i.e. - " + Ex.Message.ToString(), false, SessionIDs);
                Email.sendEmail("", toMail, ccMail, "STEP | Score Sheet Push Failed | " + DateTime.Now.ToString("dd-MMM-yyyy"), MailBody);
                return "Error "+ Ex.Message.ToString();
            }
        }

        private static string GenerateHtml(DataTable Dt, string HeadingLine, bool IsSuccess, List<string> SessionIDs)
        {
            string Body = "<html><body><h3>Dear San,</h3><b>Greetings for the day!!</b>";
            Body += "<p>" + HeadingLine + " </p>";
            DataColumnCollection dataColumns;
            if (Dt != null)
            {
                Body = Body + @"<table border=\"" +1+\""style=\""text- align:center; \""><thead><tr>";

                dataColumns = Dt.Columns;

                foreach (var col in dataColumns)
                {
                    Body = Body + @"<th>" + col + "</th>";
                }

                Body = Body + "</tr></thead><tbody>";

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    Body = Body + @"<tr>";
                    for (int j = 0; j < dataColumns.Count; j++)
                    {
                        Body = Body + @"<td>" + Dt.Rows[i][j].ToString() + "</td>";
                    }
                    Body = Body + @"</tr>";
                }
            }
            Body += "<p>Thank You.</p><p> Regards </p><p> STEP Portal </p>";
            Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";
            return Body;
        }

        private void LogService(string content)
        {
            FileStream fs = new FileStream("C:\\STEP\\STEP_DMS_Data_Push.txt", FileMode.OpenOrCreate, FileAccess.Write);
            //FileStream fs = new FileStream("E:\\STEP_AutomationServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(string.Format(content, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
            sw.Flush();
            sw.Close();
        }
    }
}
