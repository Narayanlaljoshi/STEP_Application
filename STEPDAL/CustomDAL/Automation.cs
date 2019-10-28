using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace STEPDAL.CustomDAL
{
    public class Automation
    {
        public static void FilterNominationData(DataTable info)
        {
            //int countIfProccessed = 0;
            DataTable newInfo = info.Clone();
            string RetMessage = string.Empty ;
            bool IsErroredRecords = false;
            List<TblAutomationErrorLog> ErrList = new List<TblAutomationErrorLog>();
            using (var context = new CEIDBEntities())
            {
                var ProgramList = context.TblProgramMasters.Where(x => x.IsActive == true).ToList();
                var AgencyList = context.TblRTCMasters.Where(x => x.IsActive == true).ToList();
                var FacultyList = context.TblFaculties.Where(x => x.IsActive == true).ToList();
                for (int i = 0; i < info.Rows.Count; i++)
                {
                    DataRow Dr = info.Rows[i];
                    string Co_id = string.Empty;
                    string AgencyCode = string.Empty;
                    string FacultyCode = string.Empty;
                    string ProgramCode = string.Empty;
                    string SessionID = string.Empty;
                    DateTime? StartDate = null;
                    DateTime? EndDate = null;
                    int? Duration = null;
                    string MSPIN = string.Empty;
                    string Name = string.Empty;
                    DateTime? DateofBirth = null;
                    string MobileNo = string.Empty;
                    string Region = string.Empty;
                    string City = string.Empty;
                    string Venue = string.Empty;
                    string DealerCode = string.Empty;
                    string DealerName = string.Empty;
                    string Location = string.Empty;
                    try
                    {
                        Co_id = "1";// Dr["Co_id"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        AgencyCode = Dr["AGENCY_CD"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        FacultyCode = Dr["FAC_CD"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        ProgramCode = Dr["PRG_ID"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        SessionID = Dr["CALNDR_ID"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempStartDate;
                        if (DateTime.TryParse(Dr["RO_FRM_DATE"].ToString(), out TempStartDate))
                        {
                            StartDate = TempStartDate;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempEndDate;
                        if (DateTime.TryParse(Dr["RO_TO_DATE"].ToString(), out TempEndDate))
                        {
                            EndDate = TempEndDate;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Int32 TempDuration;
                        if (Int32.TryParse(Dr["PROG_DURATION"].ToString(), out TempDuration))
                        {
                            Duration = TempDuration;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        MSPIN = Dr["MSPIN"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Name = Dr["PARTICIPANTS_NAME"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempDateofBirth;
                        if (DateTime.TryParse(Dr["EMP_DOB"].ToString(), out TempDateofBirth))
                        {
                            DateofBirth = TempDateofBirth;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        MobileNo = Dr["Mobile"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Region = Dr["region_cd"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Venue = Dr["prg_venue"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        City = Dr["city"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DealerCode = Dr["DEALER_CD"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DealerName = Dr["DEALER_NAME"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Location = Dr["LOC_DESC"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        //if (MSPIN == "585099")// || SessionID == "SSI19167355" || SessionID == "SSI19167355"
                        //{ //DateofBirth = Convert.ToDateTime(DateofBirth.Value.Day+ DateofBirth.Value.Month+DateTime.Now.year); 
                        //}
                        //else { continue; }
                        if (!Name.Equals(string.Empty) && !AgencyCode.Equals(string.Empty) && !FacultyCode.Equals(string.Empty) && !MSPIN.Equals(string.Empty) && StartDate != null && EndDate != null && !ProgramCode.Equals(string.Empty))
                        {
                            var AgencyDtl = AgencyList.Where(x => x.AgencyCode == AgencyCode).FirstOrDefault();
                            var ProgramDtl = ProgramList.Where(x => x.ProgramCode == ProgramCode).FirstOrDefault();
                            var FacultyDtl = FacultyList.Where(x => x.FacultyCode == FacultyCode).FirstOrDefault();
                            if (AgencyDtl != null && ProgramDtl != null && FacultyDtl != null)
                            {
                                try
                                {
                                    var Status = context.sp_InsertUpdate_TblNominationDownload(Co_id, Region, Venue, DealerCode, DealerName, City, Location, AgencyCode, FacultyCode, ProgramCode, SessionID, StartDate, EndDate, Duration, MSPIN, Name, DateofBirth, MobileNo, true, DateTime.Now, 1);
                                }
                                catch (Exception Ex) {
                                    var Status = context.sp_InsertUpdate_TblNominationDownload(Co_id, Region, Venue, DealerCode, DealerName, City, Location, AgencyCode, FacultyCode, ProgramCode, SessionID, StartDate, EndDate, Duration, MSPIN, Name, DateTime.Now.Date, MobileNo, true, DateTime.Now, 1);
                                }
                            }
                            else
                            {
                                IsErroredRecords = true;
                                ErrList.Add(new TblAutomationErrorLog {
                                    IsActive = true,
                                    CreatedBy = 1,
                                    CreationDate = DateTime.Now,
                                    ErrorType = (AgencyDtl == null ? "Agency Not Present in STEP Portal, " : "") + (ProgramDtl == null ? " Program Code Not Present in STEP Portal, " : "") + (FacultyDtl == null ? "Faculty Not Registered in STEP Portal, " : ""),
                                    Value =( AgencyDtl == null ? "AgencyCode=" + AgencyCode+", " : "") + (ProgramDtl == null ? " ProgramCode=" + ProgramCode + ", " : "") + (FacultyDtl == null ? " FacultyCode=" + FacultyCode + ", " : "")
                                });
                                
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        IsErroredRecords = true;
                        ErrList.Add(new TblAutomationErrorLog
                        {
                            IsActive = true,
                            CreatedBy = 1,
                            CreationDate = DateTime.Now,
                            ErrorType = "Exception",
                            Value = MSPIN+" Exception:"+e.Message.ToString()
                        });
                        continue;
                    }
                }
                if (IsErroredRecords) {
                    //Save Err records
                    try { SaveErrorRecords(ErrList); }
                    catch (Exception Ex) { }
                    //Send Email to Admin of the Error Records
                    try { SendEmailForErrorRecords(); }
                    catch(Exception Ex) { }
                }
            }
        }

        public static void SaveCalanderFile(DataTable info)
        {
            DataTable newInfo = info.Clone();
            string RetMessage = string.Empty;
            using (var context = new CEIDBEntities())
            {
                for (int i = 0; i < info.Rows.Count; i++)
                {
                    DataRow Dr = info.Rows[i];
                    string CALNDR_ID = string.Empty;
                    string PRG_ID = string.Empty;
                    DateTime? RO_FRM_DATE = null;
                    DateTime? RO_TO_DATE=null;
                    string prg_venue = string.Empty;
                    string city = string.Empty;
                    string AGENCY_CD = string.Empty;
                    string FAC_CD = string.Empty;
                    string region_cd = string.Empty;
                    string prg_status = string.Empty;
                    string shift = string.Empty;
                    int? Count_EMP_CD = null;
                    try
                    {
                        CALNDR_ID = Dr["CALNDR_ID"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        PRG_ID = Dr["PRG_ID"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prg_venue = Dr["prg_venue"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        city = Dr["city"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        AGENCY_CD = Dr["AGENCY_CD"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        FAC_CD = Dr["FAC_CD"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        region_cd = Dr["region_cd"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prg_status = Dr["prg_status"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        shift = Dr["shift"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempStartDate;
                        if (DateTime.TryParse(Dr["RO_FRM_DATE"].ToString(), out TempStartDate))
                        {
                            RO_FRM_DATE = TempStartDate;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempEndDate;
                        if (DateTime.TryParse(Dr["RO_TO_DATE"].ToString(), out TempEndDate))
                        {
                            RO_TO_DATE = TempEndDate;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Int32 TempCount_EMP_CD;
                        if (Int32.TryParse(Dr["COUNT(EMP_CD)"].ToString(), out TempCount_EMP_CD))
                        {
                            Count_EMP_CD = TempCount_EMP_CD;
                        }
                    }
                    catch (Exception ex) { }
                    
                    try
                    {
                        var status = context.sp_InsertUpdate_TblCalendarDownload(CALNDR_ID,PRG_ID, RO_FRM_DATE, RO_TO_DATE, prg_venue, city, AGENCY_CD, FAC_CD, region_cd, prg_status, shift, Count_EMP_CD, true, 1);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
                
            }
        }

        private static void SaveErrorRecords(List<TblAutomationErrorLog> Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                foreach (var Item in Obj)
                {
                    int status = Context.sp_InsertIntoTblAutomationErrorLogs(Item.ErrorType, Item.Value, Item.IsActive,DateTime.Now, Item.CreatedBy);
                }
            }
        }

        private static void SendEmailForErrorRecords()
        {
            using (var Context = new CEIDBEntities())
            {
                var Obj = Context.sp_GetAutomationErrorRecords(DateTime.Now).ToList();

                string Body = "<html><body><h3>Dear San,</h3><b>Greetings for the day!!</b>";
                Body += "<p>There were some following error records in Nomination file upload, Kindly resolve them: </p>";
                Body = Body + @"<table border=\"" +1+\""style=\""text- align:center; \""><thead><tr><th>#</th><th>Error</th><th>Value</th><th>Total Records</th></tr></thead>";
                Body += "<tbody>";
                int Index = 1;
                //Obj = Obj.Select(x=>x.ErrorType).Distinct().ToList();
                foreach (var Q in Obj)
                {
                    Body += "<tr><td>";
                    Body += Index.ToString() + "</td><td>";
                    Body += Q.ErrorType + "</td><td>";
                    Body += Q.Value+ "</td><td>";
                    Body += Q.NumberofCandidates;
                    Body += "</td></tr>";
                    Index++;
                }
                Body += "</tbody></table>";
                Body += "<p>Thank You.</p><p> Regards </p><p> STEP Portal </p>";
                Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

                string MailId = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
                MailAddressCollection toEmail = new MailAddressCollection();
                MailAddressCollection ccEmail = new MailAddressCollection();
                toEmail.Add(MailId);
                ccEmail.Add("narayan.joshi@phoenixtech.consulting");
                //ccEmail.Add("amit.kaushik@phoenixtech.consulting");
                Email.sendEmailForErrorRecords(toEmail, ccEmail, "STEP | Nomination Upload Errors - " + DateTime.Now.ToString("dd-MMM-yyyy"), Body);
            }
            
        }


        public static bool BulkInsert_RawData(DataTable dataTable)
        {
            bool isSuccuss;
            DataColumn IsActive = new DataColumn("IsActive",typeof(bool));
            IsActive.DefaultValue = true;
            dataTable.Columns.Add(IsActive);
            DataColumn CreatedBy = new DataColumn("CreatedBy", typeof(Int32));
            CreatedBy.DefaultValue = 1;
            dataTable.Columns.Add(CreatedBy);
            DataColumn CreationDate = new DataColumn("CreationDate", typeof(DateTime));
            CreationDate.DefaultValue = DateTime.Now;
            dataTable.Columns.Add(CreationDate);
            
            string connectionStringTarget = System.Configuration.ConfigurationManager.AppSettings["BulkUploadConnectionString"].ToString();
            //string connectionStringTarget = "data source=.;initial catalog=ProductivityDashboard;integrated security=True;";
            using (SqlConnection SqlConnectionObj = new SqlConnection(connectionStringTarget))
            {

                try
                {
                    SqlConnectionObj.Open();
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlConnectionObj, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                    bulkCopy.DestinationTableName = "[dbo].[TblNominationDownload_Raw]";

                    bulkCopy.ColumnMappings.Add(dataTable.Columns[0].ColumnName, "Co_id");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[1].ColumnName, "Region");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[2].ColumnName, "AgencyCode");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[3].ColumnName, "FacultyCode");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[4].ColumnName, "ProgramCode");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[5].ColumnName, "SessionID");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[6].ColumnName, "Venue");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[7].ColumnName, "City");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[8].ColumnName, "StartDate");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[9].ColumnName, "EndDate");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[10].ColumnName, "Duration");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[11].ColumnName, "Dealer_LocationCode");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[12].ColumnName, "DealerName");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[13].ColumnName, "Location");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[14].ColumnName, "MSPIN");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[15].ColumnName, "Name");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[16].ColumnName, "DateofBirth");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[17].ColumnName, "MobileNo");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[18].ColumnName, "IsActive");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[19].ColumnName, "CreatedBy");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[20].ColumnName, "CreationDate");
                    bulkCopy.WriteToServer(dataTable);
                    isSuccuss = true;
                }

                catch (Exception ex)
                {
                    isSuccuss = false;
                }
                finally
                {
                    SqlConnectionObj.Close();
                    GC.Collect();
                }
            }
            return isSuccuss;
        }


    }
}
