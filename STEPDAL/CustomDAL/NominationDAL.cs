using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using STEPDAL.DB;
using ProjectBLL.CustomModel;

namespace STEPDAL.CustomDAL
{
    public class NominationDAL
    {
        public static int getLastEntryCount()
        {
            try
            {
                using (var context = new CEIDBEntities())
                {
                    var count = context.TblNominations.OrderByDescending(x => x.Nomination_Id).First();
                    return count.Nomination_Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //public static bool Truncate()
        //{
        //	try
        //	{
        //		using (var context = new CEIDBEntities())
        //		{
        //			context.sp_TruncateTblNomination();
        //			return true;

        //		}


        //	}
        //	catch
        //	{
        //		return false;
        //	}
        //}
        public static NominationValidationBLL FilterDataTable(DataTable info, ref int countIfProccessed, UserDetailsBLL Obj)
        {
            DataTable newInfo = new DataTable();
            newInfo = info.Clone();
            string RetMessage = "";
            int totalCount = info.Rows.Count;
            using (var context = new CEIDBEntities())
            {
                var ProgramList = context.TblProgramMasters.Where(x => x.IsActive == true).ToList();
                var AgencyList = context.TblRTCMasters.Where(x => x.IsActive == true).ToList();
                var FacultyList = context.TblFaculties.Where(x => x.IsActive == true).ToList();
                var VendorFAC_List = context.TblVendorMasters.Where(x => x.IsActive == true).ToList();
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
                    string Venue = string.Empty;
                    string DealerCode = string.Empty;
                    string DealerName = string.Empty;
                    string Location = string.Empty;
                    try
                    {
                        Co_id = Dr["Co_id"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        AgencyCode = Dr["Agency Code"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        FacultyCode = Dr["Faculty Code"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        ProgramCode = Dr["Program Code"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        SessionID = Dr["Session ID"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempStartDate;
                        if (DateTime.TryParse(Dr["Start Date"].ToString(), out TempStartDate))
                        {
                            StartDate = TempStartDate;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempEndDate;
                        if (DateTime.TryParse(Dr["End Date"].ToString(), out TempEndDate))
                        {
                            EndDate = TempEndDate;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Int32 TempDuration;
                        if (Int32.TryParse(Dr["Duration(As per Program Master)"].ToString(), out TempDuration))
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
                        Name = Dr["Name"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DateTime TempDateofBirth;
                        if (DateTime.TryParse(Dr["Date of Birth"].ToString(), out TempDateofBirth))
                        {
                            DateofBirth = TempDateofBirth;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        MobileNo = Dr["Mobile No"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Region = Dr["Region"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Venue = Dr["Venue"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DealerCode = Dr["Dealer Code"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        DealerName = Dr["Dealer Name"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Location = Dr["Location"].ToString().Trim();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (!Name.Equals(string.Empty) && !AgencyCode.Equals(string.Empty) && !FacultyCode.Equals(string.Empty) && !MSPIN.Equals(string.Empty) && StartDate != null && EndDate != null && !ProgramCode.Equals(string.Empty))
                        {
                            var AgencyDtl = AgencyList.Where(x => x.AgencyCode == AgencyCode).FirstOrDefault();
                            var ProgramDtl = ProgramList.Where(x => x.ProgramCode == ProgramCode).FirstOrDefault();
                            if (AgencyDtl != null && ProgramDtl != null)
                            {
                                if (AgencyDtl.AgencyCode == "AG017")//AG017
                                {
                                    var FacultyDtl = VendorFAC_List.Where(x => x.FAC_Code == FacultyCode && x.IsActive == true).FirstOrDefault();
                                    if (FacultyDtl != null)
                                    {
                                        var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                        if (Check == null)
                                        {
                                            Dr["Co_id"] = Co_id;
                                            Dr["Agency Code"] = AgencyCode;
                                            Dr["Faculty Code"] = FacultyDtl.ManagerID;
                                            Dr["Program Code"] = ProgramCode;
                                            Dr["Session ID"] = SessionID;
                                            Dr["Start Date"] = StartDate;
                                            Dr["End Date"] = EndDate;
                                            Dr["Duration(As per Program Master)"] = Duration;
                                            Dr["MSPIN"] = MSPIN;
                                            Dr["Name"] = Name;
                                            Dr["Date of Birth"] = DateofBirth;
                                            Dr["Mobile No"] = MobileNo;
                                            Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                            Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                            Dr["Faculty_Id"] = Convert.ToInt32(FacultyDtl.Id);
                                            Dr["Region"] = Region;
                                            Dr["Venue"] = Venue;
                                            Dr["Dealer Code"] = DealerCode;
                                            Dr["Dealer Name"] = DealerName;
                                            Dr["Location"] = Location;
                                            newInfo.Rows.Add(Dr.ItemArray);
                                            countIfProccessed++;

                                            //continue;
                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = FacultyDtl.ManagerID,
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = FacultyDtl.Id,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = Obj.User_Id,
                                                CreationDate = DateTime.Now,
                                                DealerName = DealerName,
                                                Dealer_LocationCode = DealerCode,
                                                Location = Location,
                                                Region = Region,
                                                Venue = Venue
                                            };
                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                        }
                                        else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                        {
                                            Check.IsActive = false;
                                            Check.ModifiedDate = DateTime.Now;
                                            Check.ModifiedBy = Obj.User_Id;
                                            context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();

                                            Dr["Co_id"] = Co_id;
                                            Dr["Agency Code"] = AgencyCode;
                                            Dr["Faculty Code"] = FacultyDtl.ManagerID;
                                            Dr["Program Code"] = ProgramCode;
                                            Dr["Session ID"] = SessionID;
                                            Dr["Start Date"] = StartDate;
                                            Dr["End Date"] = EndDate;
                                            Dr["Duration(As per Program Master)"] = Duration;
                                            Dr["MSPIN"] = MSPIN;
                                            Dr["Name"] = Name;
                                            Dr["Date of Birth"] = DateofBirth;
                                            Dr["Mobile No"] = MobileNo;
                                            Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                            Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                            Dr["Faculty_Id"] = Convert.ToInt32(FacultyDtl.Id);
                                            Dr["Region"] = Region;
                                            Dr["Venue"] = Venue;
                                            Dr["Dealer Code"] = DealerCode;
                                            Dr["Dealer Name"] = DealerName;
                                            Dr["Location"] = Location;
                                            newInfo.Rows.Add(Dr.ItemArray);
                                            countIfProccessed++;

                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = FacultyDtl.ManagerID,
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = FacultyDtl.Id,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = Obj.User_Id,
                                                CreationDate = DateTime.Now,
                                                DealerName = DealerName,
                                                Dealer_LocationCode = DealerCode,
                                                Location = Location,
                                                Region = Region,
                                                Venue = Venue
                                            };

                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                        }
                                        else if (Check.EndDate.Value.Date >= DateTime.Now.Date)
                                        {
                                            var IsAbsenDtl = context.sp_Get_IfAbesnt(Check.MSPIN, Check.SessionID).Where(x => x.IsPresent == "A").FirstOrDefault();
                                            if (IsAbsenDtl != null)
                                            {
                                                Check.IsActive = false;
                                                Check.ModifiedDate = DateTime.Now.AddDays(-1);
                                                Check.ModifiedBy = Obj.User_Id;
                                                context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                                context.SaveChanges();

                                                Dr["Co_id"] = Co_id;
                                                Dr["Agency Code"] = AgencyCode;
                                                Dr["Faculty Code"] = FacultyDtl.ManagerID;
                                                Dr["Program Code"] = ProgramCode;
                                                Dr["Session ID"] = SessionID;
                                                Dr["Start Date"] = StartDate;
                                                Dr["End Date"] = EndDate;
                                                Dr["Duration(As per Program Master)"] = Duration;
                                                Dr["MSPIN"] = MSPIN;
                                                Dr["Name"] = Name;
                                                Dr["Date of Birth"] = DateofBirth;
                                                Dr["Mobile No"] = MobileNo;
                                                Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                                Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                                Dr["Faculty_Id"] = Convert.ToInt32(FacultyDtl.Id);
                                                Dr["Region"] = Region;
                                                Dr["Venue"] = Venue;
                                                Dr["Dealer Code"] = DealerCode;
                                                Dr["Dealer Name"] = DealerName;
                                                Dr["Location"] = Location;
                                                newInfo.Rows.Add(Dr.ItemArray);
                                                countIfProccessed++;

                                                TblNomination NM = new TblNomination
                                                {
                                                    FacultyCode = FacultyDtl.ManagerID,
                                                    IsActive = true,
                                                    MSPIN = MSPIN,
                                                    AgencyCode = AgencyCode,
                                                    Co_id = Co_id,
                                                    Agency_Id = AgencyDtl.Agency_Id,
                                                    DateofBirth = DateofBirth,
                                                    Duration = Duration,
                                                    EndDate = EndDate,
                                                    MobileNo = MobileNo,
                                                    Name = Name,
                                                    ProgramCode = ProgramCode,
                                                    ProgramId = ProgramDtl.ProgramId,
                                                    Faculty_Id = FacultyDtl.Id,
                                                    SessionID = SessionID,
                                                    StartDate = StartDate,
                                                    CreatedBy = Obj.User_Id,
                                                    CreationDate = DateTime.Now,
                                                    DealerName = DealerName,
                                                    Dealer_LocationCode = DealerCode,
                                                    Location = Location,
                                                    Region = Region,
                                                    Venue = Venue
                                                };

                                                context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                context.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                        }
                                    }
                                    else
                                    {
                                        RetMessage += MSPIN + " - Faculty is not defined under Agency code \n";
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (FacultyCode.Equals(string.Empty) || string.IsNullOrEmpty(FacultyCode))
                                    {
                                        var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                        if (Check == null)
                                        {
                                            Dr["Co_id"] = Co_id;
                                            Dr["Agency Code"] = AgencyCode;
                                            Dr["Faculty Code"] = System.DBNull.Value;
                                            Dr["Program Code"] = ProgramCode;
                                            Dr["Session ID"] = SessionID;
                                            Dr["Start Date"] = StartDate;
                                            Dr["End Date"] = EndDate;
                                            Dr["Duration(As per Program Master)"] = Duration;
                                            Dr["MSPIN"] = MSPIN;
                                            Dr["Name"] = Name;
                                            Dr["Date of Birth"] = DateofBirth;
                                            Dr["Mobile No"] = MobileNo;
                                            Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                            Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                            Dr["Faculty_Id"] = System.DBNull.Value;
                                            newInfo.Rows.Add(Dr.ItemArray);
                                            countIfProccessed++;

                                            //continue;
                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = "",
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = null,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = Obj.User_Id,
                                                CreationDate = DateTime.Now
                                            };
                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                        }
                                        else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                        {
                                            Check.IsActive = false;
                                            Check.ModifiedDate = DateTime.Now;
                                            Check.ModifiedBy = Obj.User_Id;
                                            context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();

                                            Dr["Co_id"] = Co_id;
                                            Dr["Agency Code"] = AgencyCode;
                                            Dr["Faculty Code"] = null;
                                            Dr["Program Code"] = ProgramCode;
                                            Dr["Session ID"] = SessionID;
                                            Dr["Start Date"] = StartDate;
                                            Dr["End Date"] = EndDate;
                                            Dr["Duration(As per Program Master)"] = Duration;
                                            Dr["MSPIN"] = MSPIN;
                                            Dr["Name"] = Name;
                                            Dr["Date of Birth"] = DateofBirth;
                                            Dr["Mobile No"] = MobileNo;
                                            Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                            Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                            Dr["Faculty_Id"] = null;
                                            newInfo.Rows.Add(Dr.ItemArray);
                                            countIfProccessed++;

                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = FacultyCode,
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = null,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = Obj.User_Id,
                                                CreationDate = DateTime.Now
                                            };

                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                        }
                                    }
                                    else
                                    {
                                        var FacultyDtl = FacultyList.Where(x => x.FacultyCode == FacultyCode && x.Agency_Id == AgencyDtl.Agency_Id && x.IsActive == true).FirstOrDefault();
                                        if (FacultyDtl != null)
                                        {
                                            var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                            if (Check == null)
                                            {
                                                Dr["Co_id"] = Co_id;
                                                Dr["Agency Code"] = AgencyCode;
                                                Dr["Faculty Code"] = FacultyCode;
                                                Dr["Program Code"] = ProgramCode;
                                                Dr["Session ID"] = SessionID;
                                                Dr["Start Date"] = StartDate;
                                                Dr["End Date"] = EndDate;
                                                Dr["Duration(As per Program Master)"] = Duration;
                                                Dr["MSPIN"] = MSPIN;
                                                Dr["Name"] = Name;
                                                Dr["Date of Birth"] = DateofBirth;
                                                Dr["Mobile No"] = MobileNo;
                                                Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                                Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                                Dr["Faculty_Id"] = Convert.ToInt32(FacultyDtl.Faculty_Id);
                                                Dr["Region"] = Region;
                                                Dr["Venue"] = Venue;
                                                Dr["Dealer Code"] = DealerCode;
                                                Dr["Dealer Name"] = DealerName;
                                                Dr["Location"] = Location;
                                                newInfo.Rows.Add(Dr.ItemArray);
                                                countIfProccessed++;

                                                //continue;
                                                TblNomination NM = new TblNomination
                                                {
                                                    FacultyCode = FacultyCode,
                                                    IsActive = true,
                                                    MSPIN = MSPIN,
                                                    AgencyCode = AgencyCode,
                                                    Co_id = Co_id,
                                                    Agency_Id = AgencyDtl.Agency_Id,
                                                    DateofBirth = DateofBirth,
                                                    Duration = Duration,
                                                    EndDate = EndDate,
                                                    MobileNo = MobileNo,
                                                    Name = Name,
                                                    ProgramCode = ProgramCode,
                                                    ProgramId = ProgramDtl.ProgramId,
                                                    Faculty_Id = FacultyDtl.Faculty_Id,
                                                    SessionID = SessionID,
                                                    StartDate = StartDate,
                                                    CreatedBy = Obj.User_Id,
                                                    CreationDate = DateTime.Now,
                                                    DealerName = DealerName,
                                                    Dealer_LocationCode = DealerCode,
                                                    Location = Location,
                                                    Region = Region,
                                                    Venue = Venue
                                                };
                                                context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                context.SaveChanges();
                                            }
                                            else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                            {
                                                Check.IsActive = false;
                                                Check.ModifiedDate = DateTime.Now;
                                                Check.ModifiedBy = Obj.User_Id;
                                                context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                                context.SaveChanges();

                                                Dr["Co_id"] = Co_id;
                                                Dr["Agency Code"] = AgencyCode;
                                                Dr["Faculty Code"] = FacultyCode;
                                                Dr["Program Code"] = ProgramCode;
                                                Dr["Session ID"] = SessionID;
                                                Dr["Start Date"] = StartDate;
                                                Dr["End Date"] = EndDate;
                                                Dr["Duration(As per Program Master)"] = Duration;
                                                Dr["MSPIN"] = MSPIN;
                                                Dr["Name"] = Name;
                                                Dr["Date of Birth"] = DateofBirth;
                                                Dr["Mobile No"] = MobileNo;
                                                Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                                Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                                Dr["Faculty_Id"] = Convert.ToInt32(FacultyDtl.Faculty_Id);
                                                Dr["Region"] = Region;
                                                Dr["Venue"] = Venue;
                                                Dr["Dealer Code"] = DealerCode;
                                                Dr["Dealer Name"] = DealerName;
                                                Dr["Location"] = Location;
                                                newInfo.Rows.Add(Dr.ItemArray);
                                                countIfProccessed++;

                                                TblNomination NM = new TblNomination
                                                {
                                                    FacultyCode = FacultyCode,
                                                    IsActive = true,
                                                    MSPIN = MSPIN,
                                                    AgencyCode = AgencyCode,
                                                    Co_id = Co_id,
                                                    Agency_Id = AgencyDtl.Agency_Id,
                                                    DateofBirth = DateofBirth,
                                                    Duration = Duration,
                                                    EndDate = EndDate,
                                                    MobileNo = MobileNo,
                                                    Name = Name,
                                                    ProgramCode = ProgramCode,
                                                    ProgramId = ProgramDtl.ProgramId,
                                                    Faculty_Id = FacultyDtl.Faculty_Id,
                                                    SessionID = SessionID,
                                                    StartDate = StartDate,
                                                    CreatedBy = Obj.User_Id,
                                                    CreationDate = DateTime.Now,
                                                    DealerName = DealerName,
                                                    Dealer_LocationCode = DealerCode,
                                                    Location = Location,
                                                    Region = Region,
                                                    Venue = Venue
                                                };

                                                context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                context.SaveChanges();
                                            }
                                            else if (Check.EndDate.Value.Date > DateTime.Now.Date)
                                            {
                                                var IsAbsenDtl = context.sp_Get_IfAbesnt_v2(Check.MSPIN, Check.SessionID).Where(x => x.IsPresent == "A").FirstOrDefault();
                                                if (IsAbsenDtl != null)
                                                {
                                                    Check.IsActive = false;
                                                    Check.ModifiedDate = DateTime.Now;
                                                    Check.ModifiedBy = Obj.User_Id;
                                                    context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                                    context.SaveChanges();

                                                    Dr["Co_id"] = Co_id;
                                                    Dr["Agency Code"] = AgencyCode;
                                                    Dr["Faculty Code"] = FacultyCode;
                                                    Dr["Program Code"] = ProgramCode;
                                                    Dr["Session ID"] = SessionID;
                                                    Dr["Start Date"] = StartDate;
                                                    Dr["End Date"] = EndDate;
                                                    Dr["Duration(As per Program Master)"] = Duration;
                                                    Dr["MSPIN"] = MSPIN;
                                                    Dr["Name"] = Name;
                                                    Dr["Date of Birth"] = DateofBirth;
                                                    Dr["Mobile No"] = MobileNo;
                                                    Dr["ProgramId"] = Convert.ToInt32(ProgramDtl.ProgramId);
                                                    Dr["Agency_Id"] = Convert.ToInt32(AgencyDtl.Agency_Id);
                                                    Dr["Faculty_Id"] = Convert.ToInt32(FacultyDtl.Faculty_Id);
                                                    Dr["Region"] = Region;
                                                    Dr["Venue"] = Venue;
                                                    Dr["Dealer Code"] = DealerCode;
                                                    Dr["Dealer Name"] = DealerName;
                                                    Dr["Location"] = Location;
                                                    newInfo.Rows.Add(Dr.ItemArray);
                                                    countIfProccessed++;

                                                    TblNomination NM = new TblNomination
                                                    {
                                                        FacultyCode = FacultyCode,
                                                        IsActive = true,
                                                        MSPIN = MSPIN,
                                                        AgencyCode = AgencyCode,
                                                        Co_id = Co_id,
                                                        Agency_Id = AgencyDtl.Agency_Id,
                                                        DateofBirth = DateofBirth,
                                                        Duration = Duration,
                                                        EndDate = EndDate,
                                                        MobileNo = MobileNo,
                                                        Name = Name,
                                                        ProgramCode = ProgramCode,
                                                        ProgramId = ProgramDtl.ProgramId,
                                                        Faculty_Id = FacultyDtl.Faculty_Id,
                                                        SessionID = SessionID,
                                                        StartDate = StartDate,
                                                        CreatedBy = Obj.User_Id,
                                                        CreationDate = DateTime.Now,
                                                        DealerName = DealerName,
                                                        Dealer_LocationCode = DealerCode,
                                                        Location = Location,
                                                        Region = Region,
                                                        Venue = Venue
                                                    };

                                                    context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                            }
                                        }
                                        else
                                        {
                                            RetMessage += MSPIN + " - Faculty is not defined under Agency code \n";
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                TblUploadError UE = new TblUploadError
                                {
                                    ColumnName = ProgramDtl == null ? "Program Code" : (AgencyDtl == null ? "Agency Code" : ""),
                                    CreatedBy = Obj.User_Id,
                                    CreationDate = DateTime.Now,
                                    IsActive = true,
                                    Value = ProgramDtl == null ? ProgramCode : (AgencyDtl == null ? AgencyCode : "")
                                };
                                context.Entry(UE).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();
                                RetMessage = RetMessage + MSPIN + " - " + (ProgramDtl == null ? "(" + ProgramCode + ") " + "Program Code Does Not Exist <br />" : (AgencyDtl == null ? "(" + AgencyCode + ") " + "Agency Code is Does Not Exist <br />" : ""));
                                continue;
                            }
                        }
                        else
                        {
                            TblUploadError UE = new TblUploadError
                            {
                                ColumnName = Name.Equals(string.Empty) ? "Name" : (AgencyCode.Equals(string.Empty) ? "AgencyCode" : (FacultyCode.Equals(string.Empty) ? "FacultyCode" : (MSPIN.Equals(string.Empty) ? "MSPIN" : (StartDate == null ? "StartDate" : (EndDate == null ? "EndDate" : (ProgramCode.Equals(string.Empty) ? "ProgramCode" : "")))))),
                                CreatedBy = Obj.User_Id,
                                CreationDate = DateTime.Now,
                                IsActive = true,
                                Value = Name.Equals(string.Empty) ? Name : (AgencyCode.Equals(string.Empty) ? AgencyCode : (FacultyCode.Equals(string.Empty) ? FacultyCode : (MSPIN.Equals(string.Empty) ? MSPIN : (StartDate == null ? "" : (EndDate == null ? "" : (ProgramCode.Equals(string.Empty) ? ProgramCode : "")))))),
                            };
                            context.Entry(UE).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                            RetMessage += MSPIN + " - " + (Name.Equals(string.Empty) ? "Name Is Empty <br />" : (AgencyCode.Equals(string.Empty) ? "AgencyCode  Is Empty<br />" : (FacultyCode.Equals(string.Empty) ? "FacultyCode Is Empty <br />" : (MSPIN.Equals(string.Empty) ? "MSPIN Is Empty <br />" : (StartDate == null ? "StartDate Is Empty <br />" : (EndDate == null ? "EndDate Is Empty <br />" : (ProgramCode.Equals(string.Empty) ? "ProgramCode Is Empty <br />" : "")))))));
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
            NominationValidationBLL RetInfo = new NominationValidationBLL();
            RetInfo.Response = RetMessage;
            RetInfo.Datatbl = newInfo;
            return RetInfo;


        }

        public static bool BulkInsertDataTable(DataTable dataTable, UserDetailsBLL Obj)
        {
            bool isSuccuss;
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 1500;
                try
                {
                    foreach (DataRow Dr in dataTable.Rows)// (int i=0;i< dataTable.Rows.Count;i++)
                    {
                        string MSPIN = Dr["MSPIN"].ToString();
                        int? Agency_Id = Convert.ToInt32(Dr["Agency_Id"]);
                        var check = context.TblUsers.Where(x => x.UserName == MSPIN && x.Agency_Id == Agency_Id && x.IsActive == true).FirstOrDefault();
                        if (check == null)
                        {
                            //password changed from ddmmyyyy to mspin because there are cases in which DOB is null or not format.
                            string pass = MSPIN;// Convert.ToDateTime(Dr["Date of Birth"].ToString()).ToString("dd-MM-yyyy");
                            pass = pass.Replace("/", "");
                            pass = pass.Replace("-", "");
                            pass = pass.Replace(" ", "");

                            TblUser _user = new TblUser
                            {
                                UserName = MSPIN,
                                Password = pass,
                                Role_Id = 4,
                                Agency_Id = Agency_Id,
                                IsActive = true,
                                CreatedBy = Obj.User_Id,
                                CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                                ModifiedBy = null,
                                ModifiedDate = null
                            };
                            context.Entry(_user).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                        }
                        else
                        {

                            check.IsActive = false;
                            check.ModifiedBy = Obj.User_Id;
                            check.ModifiedDate = DateTime.Now;

                            context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();

                            string pass = MSPIN;// Convert.ToDateTime(Dr["Date of Birth"].ToString()).ToString("dd-MM-yyyy");
                            pass = pass.Replace("/", "");
                            pass = pass.Replace("-", "");
                            pass = pass.Replace(" ", "");

                            TblUser _user = new TblUser
                            {
                                UserName = MSPIN,
                                Password = pass,
                                Role_Id = 4,
                                Agency_Id = Agency_Id,
                                IsActive = true,
                                CreatedBy = Obj.User_Id,
                                CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                                ModifiedBy = null,
                                ModifiedDate = null

                            };
                            context.Entry(_user).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                        }
                    }


                    isSuccuss = true;
                }
                catch (Exception ex)
                {
                    isSuccuss = false;
                }
                finally
                {
                    int Sataus = context.SP_BatchJobWith_Practical();
                }
            }
            return isSuccuss;
        }

        public static DataTable ToDataTable<DataRow>(List<DataRow> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(DataRow));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (DataRow iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static List<NominationsList> GetNominationsList()
        {
            List<NominationsList> NominationsList = null;
            using (var context = new CEIDBEntities())
            {
                var ReqData = context.sp_SessionIDsForManage().ToList();
                if (ReqData != null)
                {
                    NominationsList = ReqData.Select(x => new NominationsList
                    {
                        AgencyCode = x.AgencyCode,
                        //Agency_Id = x.Agency_Id,
                        Co_id = x.Co_id,
                        //CreatedBy = x.CreatedBy,
                        //CreationDate = x.CreationDate,
                        //DateofBirth = x.DateofBirth,
                        Duration = x.Duration,
                        EndDate = x.EndDate,
                        FacultyCode = x.FacultyCode,
                        //Faculty_Id = x.Faculty_Id,
                        //IsActive = x.IsActive,
                        //MobileNo = x.MobileNo,
                        //ModifiedBy = x.ModifiedBy,
                        //ModifiedDate = x.ModifiedDate,
                        //MSPIN = x.MSPIN,
                        //Name = x.Name,
                        //Nomination_Id = x.Nomination_Id,
                        ProgramCode = x.ProgramCode,
                        //ProgramId = x.ProgramId,
                        SessionID = x.SessionID,
                        StartDate = x.StartDate
                    }).ToList();
                }
                return NominationsList;
            }

        }

        public static string UpdateNominationsList(NominationsList Obj)
        {

            using (var context = new CEIDBEntities())
            {
                var Check = context.TblNominations.Where(x => x.Nomination_Id == Obj.Nomination_Id && x.IsActive == true).FirstOrDefault();

                context.sp_UpdateNomination(Obj.SessionID, Obj.EndDate);
                //if (Check != null)
                //{
                //    Check.EndDate = Obj.EndDate;
                //    Check.ModifiedBy = Obj.ModifiedBy;
                //    Check.ModifiedDate = DateTime.Now;
                //    context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                //    context.SaveChanges();
                //}
                return "Success: Updated Successfully";
            }

        }

        public static string FilterDownloadedNomination()
        {
            string RetMessage = string.Empty;
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 1500;
                var GetDownloadedData = context.sp_GetNominationListFromDownload(DateTime.Now).ToList();
                //GetDownloadedData= GetDownloadedData.Where(x=>x.SessionID == "SSI19188295").ToList();
                var ProgramList = context.TblProgramMasters.Where(x => x.IsActive == true).ToList();
                var AgencyList = context.TblRTCMasters.Where(x => x.IsActive == true).ToList();
                var FacultyList = context.TblFaculties.Where(x => x.IsActive == true).ToList();
                var VendorFAC_List = context.TblVendorMasters.Where(x => x.IsActive == true).ToList();
                //var SubFacultyList = context.TblSubFacultyMasters.Where(x => x.IsActive == true).ToList();
                List<TblNomination> FilteredList = new List<TblNomination>();
                foreach (var row in GetDownloadedData)
                {
                    string Co_id = row.Co_id;
                    string AgencyCode = row.AgencyCode;
                    string FacultyCode = row.FacultyCode;
                    string ProgramCode = row.ProgramCode;
                    string SessionID = row.SessionID;
                    DateTime? StartDate = row.StartDate;
                    DateTime? EndDate = row.EndDate;
                    int? Duration = row.Duration;
                    string MSPIN = row.MSPIN;
                    string Name = row.Name;
                    DateTime? DateofBirth = row.DateofBirth;
                    string MobileNo = row.MobileNo;
                    string Region = row.Region;
                    string Venue = row.Venue;
                    string DealerCode = row.Dealer_LocationCode;
                    string DealerName = row.DealerName;
                    string Location = row.Location;
                    //if (row.AgencyCode=="AG017")
                    //{ }
                    try
                    {
                        if (!Name.Equals(string.Empty) && !AgencyCode.Equals(string.Empty) && !FacultyCode.Equals(string.Empty) && !MSPIN.Equals(string.Empty) && StartDate != null && EndDate != null && !ProgramCode.Equals(string.Empty))
                        {
                            var AgencyDtl = AgencyList.Where(x => x.AgencyCode == AgencyCode).FirstOrDefault();
                            var ProgramDtl = ProgramList.Where(x => x.ProgramCode == ProgramCode).FirstOrDefault();
                            if (AgencyDtl != null && ProgramDtl != null)
                            {
                                if (AgencyDtl.AgencyCode == "AG017")//AG017
                                {
                                    var CalendarTrainerDtl = context.sp_GetTrainerDetails_TblCalendar(SessionID).FirstOrDefault();
                                    var FacultyDtl = VendorFAC_List.Where(x => x.FAC_Code == FacultyCode && x.IsActive == true).FirstOrDefault();
                                    if (FacultyDtl != null)
                                    {
                                        var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                        if (Check == null)
                                        {
                                            //continue;
                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = CalendarTrainerDtl != null ? CalendarTrainerDtl.TrainerCode: FacultyDtl.ManagerID,
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = CalendarTrainerDtl!=null? CalendarTrainerDtl.Id: FacultyDtl.Id,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = 1,
                                                CreationDate = DateTime.Now,
                                                DealerName = DealerName,
                                                Dealer_LocationCode = DealerCode,
                                                Location = Location,
                                                Region = Region,
                                                Venue = Venue
                                            };
                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                            FilteredList.Add(NM);
                                        }
                                        else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                        {
                                            Check.IsActive = false;
                                            Check.ModifiedDate = DateTime.Now;
                                            Check.ModifiedBy = 1;
                                            context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();
                                            
                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = CalendarTrainerDtl != null ? CalendarTrainerDtl.TrainerCode : FacultyDtl.ManagerID,
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = CalendarTrainerDtl != null ? CalendarTrainerDtl.Id : FacultyDtl.Id,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = 1,
                                                CreationDate = DateTime.Now,
                                                DealerName = DealerName,
                                                Dealer_LocationCode = DealerCode,
                                                Location = Location,
                                                Region = Region,
                                                Venue = Venue
                                            };

                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                            FilteredList.Add(NM);
                                        }
                                        else if (Check.EndDate.Value.Date >= DateTime.Now.Date)
                                        {
                                            var IsAbsenDtl = context.sp_Get_IfAbesnt_v2(Check.MSPIN, Check.SessionID).Where(x => x.IsPresent == "A").FirstOrDefault();
                                            if (IsAbsenDtl != null)
                                            {
                                                Check.IsActive = false;
                                                Check.ModifiedDate = DateTime.Now.AddDays(-1);
                                                Check.ModifiedBy = 1;
                                                context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                                context.SaveChanges();

                                                TblNomination NM = new TblNomination
                                                {
                                                    FacultyCode = CalendarTrainerDtl != null ? CalendarTrainerDtl.TrainerCode : FacultyDtl.ManagerID,
                                                    IsActive = true,
                                                    MSPIN = MSPIN,
                                                    AgencyCode = AgencyCode,
                                                    Co_id = Co_id,
                                                    Agency_Id = AgencyDtl.Agency_Id,
                                                    DateofBirth = DateofBirth,
                                                    Duration = Duration,
                                                    EndDate = EndDate,
                                                    MobileNo = MobileNo,
                                                    Name = Name,
                                                    ProgramCode = ProgramCode,
                                                    ProgramId = ProgramDtl.ProgramId,
                                                    Faculty_Id = CalendarTrainerDtl != null ? CalendarTrainerDtl.Id : FacultyDtl.Id,
                                                    SessionID = SessionID,
                                                    StartDate = StartDate,
                                                    CreatedBy = 1,
                                                    CreationDate = DateTime.Now,
                                                    DealerName = DealerName,
                                                    Dealer_LocationCode = DealerCode,
                                                    Location = Location,
                                                    Region = Region,
                                                    Venue = Venue
                                                };

                                                context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                context.SaveChanges();
                                                FilteredList.Add(NM);
                                            }
                                        }
                                        else
                                        {
                                            RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                        }
                                    }
                                    else
                                    {
                                        RetMessage += MSPIN + " - Faculty is not defined under Agency code \n";
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (FacultyCode.Equals(string.Empty) || string.IsNullOrEmpty(FacultyCode))
                                    {
                                        var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                        if (Check == null)
                                        {
                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = "",
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = null,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = 1,
                                                CreationDate = DateTime.Now
                                            };
                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                            FilteredList.Add(NM);
                                        }
                                        else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                        {
                                            Check.IsActive = false;
                                            Check.ModifiedDate = DateTime.Now;
                                            Check.ModifiedBy = 1;
                                            context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();

                                            TblNomination NM = new TblNomination
                                            {
                                                FacultyCode = FacultyCode,
                                                IsActive = true,
                                                MSPIN = MSPIN,
                                                AgencyCode = AgencyCode,
                                                Co_id = Co_id,
                                                Agency_Id = AgencyDtl.Agency_Id,
                                                DateofBirth = DateofBirth,
                                                Duration = Duration,
                                                EndDate = EndDate,
                                                MobileNo = MobileNo,
                                                Name = Name,
                                                ProgramCode = ProgramCode,
                                                ProgramId = ProgramDtl.ProgramId,
                                                Faculty_Id = null,
                                                SessionID = SessionID,
                                                StartDate = StartDate,
                                                CreatedBy = 1,
                                                CreationDate = DateTime.Now
                                            };

                                            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                            context.SaveChanges();
                                            FilteredList.Add(NM);
                                        }
                                        else
                                        {
                                            RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                        }
                                    }
                                    else
                                    {
                                        var FacultyDtl = FacultyList.Where(x => x.FacultyCode == FacultyCode && x.Agency_Id == AgencyDtl.Agency_Id && x.IsActive == true).FirstOrDefault();
                                        if (FacultyDtl != null)
                                        {
                                            var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                            if (Check == null)
                                            {
                                                TblNomination NM = new TblNomination
                                                {
                                                    FacultyCode = FacultyCode,
                                                    IsActive = true,
                                                    MSPIN = MSPIN,
                                                    AgencyCode = AgencyCode,
                                                    Co_id = Co_id,
                                                    Agency_Id = AgencyDtl.Agency_Id,
                                                    DateofBirth = DateofBirth,
                                                    Duration = Duration,
                                                    EndDate = EndDate,
                                                    MobileNo = MobileNo,
                                                    Name = Name,
                                                    ProgramCode = ProgramCode,
                                                    ProgramId = ProgramDtl.ProgramId,
                                                    Faculty_Id = FacultyDtl.Faculty_Id,
                                                    SessionID = SessionID,
                                                    StartDate = StartDate,
                                                    CreatedBy = 1,
                                                    CreationDate = DateTime.Now,
                                                    DealerName = DealerName,
                                                    Dealer_LocationCode = DealerCode,
                                                    Location = Location,
                                                    Region = Region,
                                                    Venue = Venue
                                                };
                                                context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                context.SaveChanges();
                                                FilteredList.Add(NM);
                                            }
                                            else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                            {
                                                Check.IsActive = false;
                                                Check.ModifiedDate = DateTime.Now;
                                                Check.ModifiedBy = 1;
                                                context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                                context.SaveChanges();

                                                TblNomination NM = new TblNomination
                                                {
                                                    FacultyCode = FacultyCode,
                                                    IsActive = true,
                                                    MSPIN = MSPIN,
                                                    AgencyCode = AgencyCode,
                                                    Co_id = Co_id,
                                                    Agency_Id = AgencyDtl.Agency_Id,
                                                    DateofBirth = DateofBirth,
                                                    Duration = Duration,
                                                    EndDate = EndDate,
                                                    MobileNo = MobileNo,
                                                    Name = Name,
                                                    ProgramCode = ProgramCode,
                                                    ProgramId = ProgramDtl.ProgramId,
                                                    Faculty_Id = FacultyDtl.Faculty_Id,
                                                    SessionID = SessionID,
                                                    StartDate = StartDate,
                                                    CreatedBy = 1,
                                                    CreationDate = DateTime.Now,
                                                    DealerName = DealerName,
                                                    Dealer_LocationCode = DealerCode,
                                                    Location = Location,
                                                    Region = Region,
                                                    Venue = Venue
                                                };

                                                context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                context.SaveChanges();
                                                FilteredList.Add(NM);
                                            }
                                            else if (Check.EndDate.Value.Date >= DateTime.Now.Date)
                                            {
                                                var IsAbsenDtl = context.sp_Get_IfAbesnt_v2(Check.MSPIN, Check.SessionID).Where(x => x.IsPresent == "A").FirstOrDefault();
                                                if (IsAbsenDtl != null)
                                                {
                                                    Check.IsActive = false;
                                                    Check.ModifiedDate = DateTime.Now;
                                                    Check.ModifiedBy = 1;
                                                    context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                                    context.SaveChanges();

                                                    TblNomination NM = new TblNomination
                                                    {
                                                        FacultyCode = FacultyCode,
                                                        IsActive = true,
                                                        MSPIN = MSPIN,
                                                        AgencyCode = AgencyCode,
                                                        Co_id = Co_id,
                                                        Agency_Id = AgencyDtl.Agency_Id,
                                                        DateofBirth = DateofBirth,
                                                        Duration = Duration,
                                                        EndDate = EndDate,
                                                        MobileNo = MobileNo,
                                                        Name = Name,
                                                        ProgramCode = ProgramCode,
                                                        ProgramId = ProgramDtl.ProgramId,
                                                        Faculty_Id = FacultyDtl.Faculty_Id,
                                                        SessionID = SessionID,
                                                        StartDate = StartDate,
                                                        CreatedBy = 1,
                                                        CreationDate = DateTime.Now,
                                                        DealerName = DealerName,
                                                        Dealer_LocationCode = DealerCode,
                                                        Location = Location,
                                                        Region = Region,
                                                        Venue = Venue
                                                    };

                                                    context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                                    context.SaveChanges();
                                                    FilteredList.Add(NM);
                                                }
                                            }
                                            else
                                            {
                                                RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                            }
                                        }
                                        else
                                        {
                                            RetMessage += MSPIN + " - Faculty is not defined under Agency code \n";
                                            continue;
                                        }
                                    }
                                }

                                #region MyRegion
                                //if (FacultyCode.Equals(string.Empty) || string.IsNullOrEmpty(FacultyCode))
                                //{
                                //    var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                //    if (Check == null)
                                //    {
                                //        TblNomination NM = new TblNomination
                                //        {
                                //            FacultyCode = "",
                                //            IsActive = true,
                                //            MSPIN = MSPIN,
                                //            AgencyCode = AgencyCode,
                                //            Co_id = Co_id,
                                //            Agency_Id = AgencyDtl.Agency_Id,
                                //            DateofBirth = DateofBirth,
                                //            Duration = Duration,
                                //            EndDate = EndDate,
                                //            MobileNo = MobileNo,
                                //            Name = Name,
                                //            ProgramCode = ProgramCode,
                                //            ProgramId = ProgramDtl.ProgramId,
                                //            Faculty_Id = null,
                                //            SessionID = SessionID,
                                //            StartDate = StartDate,
                                //            CreatedBy = 1,
                                //            CreationDate = DateTime.Now
                                //        };
                                //        context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                //        context.SaveChanges();
                                //        FilteredList.Add(NM);
                                //    }
                                //    else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                //    {
                                //        Check.IsActive = false;
                                //        Check.ModifiedDate = DateTime.Now;
                                //        Check.ModifiedBy = 1;
                                //        context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                //        context.SaveChanges();

                                //        TblNomination NM = new TblNomination
                                //        {
                                //            FacultyCode = FacultyCode,
                                //            IsActive = true,
                                //            MSPIN = MSPIN,
                                //            AgencyCode = AgencyCode,
                                //            Co_id = Co_id,
                                //            Agency_Id = AgencyDtl.Agency_Id,
                                //            DateofBirth = DateofBirth,
                                //            Duration = Duration,
                                //            EndDate = EndDate,
                                //            MobileNo = MobileNo,
                                //            Name = Name,
                                //            ProgramCode = ProgramCode,
                                //            ProgramId = ProgramDtl.ProgramId,
                                //            Faculty_Id = null,
                                //            SessionID = SessionID,
                                //            StartDate = StartDate,
                                //            CreatedBy = 1,
                                //            CreationDate = DateTime.Now
                                //        };

                                //        context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                //        context.SaveChanges();
                                //        FilteredList.Add(NM);
                                //    }
                                //    else
                                //    {
                                //        //RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                //    }
                                //}
                                //else
                                //{
                                //    var FacultyDtl = FacultyList.Where(x => x.FacultyCode == FacultyCode && x.Agency_Id == AgencyDtl.Agency_Id && x.IsActive == true).FirstOrDefault();
                                //    if (FacultyDtl != null)
                                //    {
                                //        var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN && x.IsActive == true).FirstOrDefault();
                                //        if (Check == null)
                                //        {
                                //            TblNomination NM = new TblNomination
                                //            {
                                //                FacultyCode = AgencyDtl.AgenyType != 1 ? "" : FacultyCode,
                                //                IsActive = true,
                                //                MSPIN = MSPIN,
                                //                AgencyCode = AgencyCode,
                                //                Co_id = Co_id,
                                //                Agency_Id = AgencyDtl.Agency_Id,
                                //                DateofBirth = DateofBirth,
                                //                Duration = Duration,
                                //                EndDate = EndDate,
                                //                MobileNo = MobileNo,
                                //                Name = Name,
                                //                ProgramCode = ProgramCode,
                                //                ProgramId = ProgramDtl.ProgramId,
                                //                Faculty_Id = AgencyDtl.AgenyType != 1 ? 0 : FacultyDtl.Faculty_Id,
                                //                SessionID = SessionID,
                                //                StartDate = StartDate,
                                //                CreatedBy = 1,
                                //                CreationDate = DateTime.Now,
                                //                DealerName = DealerName,
                                //                Dealer_LocationCode = DealerCode,
                                //                Location = Location,
                                //                Region = Region,
                                //                Venue = Venue
                                //            };
                                //            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                //            context.SaveChanges();
                                //            FilteredList.Add(NM);
                                //        }
                                //        else if (Check.EndDate.Value.Date < DateTime.Now.Date)
                                //        {
                                //            Check.IsActive = false;
                                //            Check.ModifiedDate = DateTime.Now;
                                //            Check.ModifiedBy = 1;
                                //            context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                //            context.SaveChanges();

                                //            TblNomination NM = new TblNomination
                                //            {
                                //                FacultyCode = FacultyCode,
                                //                IsActive = true,
                                //                MSPIN = MSPIN,
                                //                AgencyCode = AgencyCode,
                                //                Co_id = Co_id,
                                //                Agency_Id = AgencyDtl.Agency_Id,
                                //                DateofBirth = DateofBirth,
                                //                Duration = Duration,
                                //                EndDate = EndDate,
                                //                MobileNo = MobileNo,
                                //                Name = Name,
                                //                ProgramCode = ProgramCode,
                                //                ProgramId = ProgramDtl.ProgramId,
                                //                Faculty_Id = FacultyDtl.Faculty_Id,
                                //                SessionID = SessionID,
                                //                StartDate = StartDate,
                                //                CreatedBy = 1,
                                //                CreationDate = DateTime.Now,
                                //                DealerName = DealerName,
                                //                Dealer_LocationCode = DealerCode,
                                //                Location = Location,
                                //                Region = Region,
                                //                Venue = Venue
                                //            };

                                //            context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                //            context.SaveChanges();
                                //            FilteredList.Add(NM);
                                //        }
                                //        else if (Check.EndDate.Value.Date >= DateTime.Now.Date)
                                //        {
                                //            var IsAbsenDtl = context.sp_Get_IfAbesnt_v2(Check.MSPIN, Check.SessionID).Where(x => x.IsPresent == "A").FirstOrDefault();
                                //            if (IsAbsenDtl != null)
                                //            {
                                //                if (IsAbsenDtl.IsPresent == "A")
                                //                {
                                //                    Check.IsActive = false;
                                //                    Check.ModifiedDate = DateTime.Now;
                                //                    Check.ModifiedBy = 1;
                                //                    context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                                //                    context.SaveChanges();
                                //                    //var CheckAttendance = context.sp_Get_IfAbesnt(Check.MSPIN, Check.SessionID).FirstOrDefault();
                                //                    context.sp_DeleteQuestionForMSPINandSessionID(Check.MSPIN, Check.SessionID, Check.ProgramId, IsAbsenDtl.Day);

                                //                    TblNomination NM = new TblNomination
                                //                    {
                                //                        FacultyCode = FacultyCode,
                                //                        IsActive = true,
                                //                        MSPIN = MSPIN,
                                //                        AgencyCode = AgencyCode,
                                //                        Co_id = Co_id,
                                //                        Agency_Id = AgencyDtl.Agency_Id,
                                //                        DateofBirth = DateofBirth,
                                //                        Duration = Duration,
                                //                        EndDate = EndDate,
                                //                        MobileNo = MobileNo,
                                //                        Name = Name,
                                //                        ProgramCode = ProgramCode,
                                //                        ProgramId = ProgramDtl.ProgramId,
                                //                        Faculty_Id = FacultyDtl.Faculty_Id,
                                //                        SessionID = SessionID,
                                //                        StartDate = StartDate,
                                //                        CreatedBy = 1,
                                //                        CreationDate = DateTime.Now,
                                //                        DealerName = DealerName,
                                //                        Dealer_LocationCode = DealerCode,
                                //                        Location = Location,
                                //                        Region = Region,
                                //                        Venue = Venue
                                //                    };
                                //                    context.Entry(NM).State = System.Data.Entity.EntityState.Added;
                                //                    context.SaveChanges();
                                //                    FilteredList.Add(NM);
                                //                }
                                //            }
                                //        }
                                //        else
                                //        {
                                //            //RetMessage += MSPIN + " - Currently under training at Agency : " + AgencyCode + "\n";
                                //        }
                                //    }
                                //    else
                                //    {
                                //        //RetMessage += MSPIN + " - Faculty is not defined under Agency code \n";
                                //        continue;
                                //    }
                                //}
                                #endregion

                            }
                            else
                            {
                                TblUploadError UE = new TblUploadError
                                {
                                    ColumnName = ProgramDtl == null ? "Program Code" : (AgencyDtl == null ? "Agency Code" : ""),
                                    CreatedBy = 1,
                                    CreationDate = DateTime.Now,
                                    IsActive = true,
                                    Value = ProgramDtl == null ? ProgramCode : (AgencyDtl == null ? AgencyCode : "")
                                };
                                context.Entry(UE).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();
                                //RetMessage = RetMessage + MSPIN + " - " + (ProgramDtl == null ? "(" + ProgramCode + ") " + "Program Code Does Not Exist <br />" : (AgencyDtl == null ? "(" + AgencyCode + ") " + "Agency Code is Does Not Exist <br />" : ""));
                                continue;
                            }
                        }
                        else
                        {
                            TblUploadError UE = new TblUploadError
                            {
                                ColumnName = Name.Equals(string.Empty) ? "Name" : (AgencyCode.Equals(string.Empty) ? "AgencyCode" : (FacultyCode.Equals(string.Empty) ? "FacultyCode" : (MSPIN.Equals(string.Empty) ? "MSPIN" : (StartDate == null ? "StartDate" : (EndDate == null ? "EndDate" : (ProgramCode.Equals(string.Empty) ? "ProgramCode" : "")))))),
                                CreatedBy = 1,
                                CreationDate = DateTime.Now,
                                IsActive = true,
                                Value = Name.Equals(string.Empty) ? Name : (AgencyCode.Equals(string.Empty) ? AgencyCode : (FacultyCode.Equals(string.Empty) ? FacultyCode : (MSPIN.Equals(string.Empty) ? MSPIN : (StartDate == null ? "" : (EndDate == null ? "" : (ProgramCode.Equals(string.Empty) ? ProgramCode : "")))))),
                            };
                            context.Entry(UE).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                            //RetMessage += MSPIN + " - " + (Name.Equals(string.Empty) ? "Name Is Empty <br />" : (AgencyCode.Equals(string.Empty) ? "AgencyCode  Is Empty<br />" : (FacultyCode.Equals(string.Empty) ? "FacultyCode Is Empty <br />" : (MSPIN.Equals(string.Empty) ? "MSPIN Is Empty <br />" : (StartDate == null ? "StartDate Is Empty <br />" : (EndDate == null ? "EndDate Is Empty <br />" : (ProgramCode.Equals(string.Empty) ? "ProgramCode Is Empty <br />" : "")))))));
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }

                bool Status = CreateUserIds(FilteredList);
            }

            return RetMessage;


        }

        public static bool CreateUserIds(List<TblNomination> FilteredList)
        {
            bool isSuccuss;
            UserDetailsBLL Obj = new UserDetailsBLL
            {
                CreatedBy = 1,
                CreationDate = DateTime.Now
            };
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 15000;
                try
                {
                    foreach (var Dr in FilteredList)// (int i=0;i< dataTable.Rows.Count;i++)
                    {
                        string MSPIN = Dr.MSPIN;
                        int? Agency_Id = Dr.Agency_Id;
                        var check = context.TblUsers.Where(x => x.UserName == MSPIN && x.Agency_Id == Agency_Id && x.IsActive == true).FirstOrDefault();
                        if (check == null)
                        {
                            string pass = "1234";// Dr.DateofBirth!=null? Dr.DateofBirth.Value.ToString("dd-MM-yyyy"):"1234";
                            pass = pass.Replace("/", "");
                            pass = pass.Replace("-", "");
                            pass = pass.Replace(" ", "");

                            TblUser _user = new TblUser
                            {
                                UserName = MSPIN,
                                Password = pass,
                                Role_Id = 4,
                                Agency_Id = Agency_Id,
                                IsActive = true,
                                CreatedBy = Obj.User_Id,
                                CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                                ModifiedBy = null,
                                ModifiedDate = null
                            };
                            context.Entry(_user).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                        }
                        else
                        {
                            check.IsActive = false;
                            check.ModifiedBy = Obj.User_Id;
                            check.ModifiedDate = DateTime.Now;

                            context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();

                            string pass = "1234";//  Convert.ToDateTime(Dr["Date of Birth"].ToString()).ToString("dd-MM-yyyy");
                            pass = pass.Replace("/", "");
                            pass = pass.Replace("-", "");
                            pass = pass.Replace(" ", "");

                            TblUser _user = new TblUser
                            {
                                UserName = MSPIN,
                                Password = pass,
                                Role_Id = 4,
                                Agency_Id = Agency_Id,
                                IsActive = true,
                                CreatedBy = Obj.User_Id,
                                CreationDate = DateTime.Now,//LocalTimeRegion.GetLocalDate(),
                                ModifiedBy = null,
                                ModifiedDate = null
                            };
                            context.Entry(_user).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                        }
                    }
                    isSuccuss = true;
                }
                catch (Exception ex)
                {
                    isSuccuss = false;
                }
                finally
                {
                    int Sataus = context.SP_BatchJobWith_Practical();
                }
            }
            return isSuccuss;
        }
    }
}
