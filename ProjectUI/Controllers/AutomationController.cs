using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            NominationValidationBLL Data = new NominationValidationBLL();
            UserDetailsBLL Obj = new UserDetailsBLL();
          
            ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
            Client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 0, 600);
            DataTable NominTionFile = Client.GetNominationFile(1, Convert.ToDateTime(DateTime.Now));//"13-Aug-19"
            if (NominTionFile.Rows.Count>0)
            {
                //DataTable nDT = NominTionFile.Clone();//
                //int rowId = 0;
                //for (int i = 0; i <= NominTionFile.Rows.Count - 1; i++)
                //{
                //    NominTionFile.Rows[i][3] = NominTionFile.Rows[i][3].ToString().Replace("FAC00489", "FAC00407");
                //}
                //foreach (DataRow dr in NominTionFile.Rows)
                //{
                //    string MD = dr["MSPIN"].ToString();
                //    if (MD == "522425"|| MD == "682700"|| MD == "650032"|| MD == "728450")
                //    {
                //        //dr.Delete();

                //    }
                //    else
                //    {
                //        continue;
                //        //nDT.Rows.Add(dr.ItemArray);
                //    }
                //}
                Automation.BulkInsert_RawData(NominTionFile);
                Automation.FilterNominationData(NominTionFile);
            }
        }
        [HttpGet]
        public void GetCalendarFile()
        {
            ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
            DataTable CalendarFile  = Client.GetCalendarFile(1,DateTime.Now);
            Automation.SaveCalanderFile(CalendarFile);
        }
        /*Attendance report as per DMS*/
        [HttpGet]
        public List<AttendanceDMSAutomation> GetAttendanceReport()
        {
            ReportInputBLL Obj = new ReportInputBLL {
                EndDate = Convert.ToDateTime("02-Sep-2019")
            };
            List<AttendanceReportBLL> List= ReportsDAL.GetAttendanceReport(Obj);
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

            ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
            //Client.InnerChannel.OperationTimeout =new TimeSpan(0,10,15000);
            //Client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 0, 600);
            DataTable Return_Dt = Client.UploadAttendance(Dt);
            DataTable Return_DtForBatchJob = Client.RunBatchJob_Attendance(Obj.EndDate.Value.ToString("dd-MMM-yyyy"));
            return DMS_List;
        }

        /*Marks report as per DMS*/
        [HttpGet]
        public List<ScorreDMSAutomation> GetMarksReportAsperDMS()
        {
            List<ScorreDMSAutomation> DMSLIst = new List<ScorreDMSAutomation>();
            SessionIDListBLL Obj = new SessionIDListBLL
            {
                EndDate = Convert.ToDateTime("02-Sep-2019")//DateTime.Now
            };
            List< MarksReportBLL> MarksList= ReportsDAL.GetMarksReportAsperDMS_V2(Obj);
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

            ServiceReference1.STEP_NominationSoapClient Client = new STEP_NominationSoapClient();
            DataTable Return_Dt=Client.UploadScore(Dt);
            DataTable Return_DtForBatchJob = Client.RunBatchJob_Marks();
            return DMSLIst;
        }
    }
}
