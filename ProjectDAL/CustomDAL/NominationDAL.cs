using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using STEPDAL.DB;
using ProjectBLL.CustomModel;

namespace ProjectDAL.CustomDAL
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
                for (int i = 0; i < info.Rows.Count; i++)
                {
                    DataRow Dr = info.Rows[i];
                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                    //{
                        // do something with EF here

                        try
                        {
                            if (Dr["Agency Code"].ToString() != null && Dr["Faculty Code"].ToString() != null && Dr["MSPIN"].ToString() != null && Dr["Start Date"].ToString() != null && Dr["End Date"].ToString() != null)
                            {
                                string Co_id = Dr["Co_id"].ToString().Trim();
                                string AgencyCode = Dr["Agency Code"].ToString().Trim();
                                string FacultyCode = Dr["Faculty Code"].ToString().Trim();
                                string ProgramCode = Dr["Program Code"].ToString().Trim();
                                string SessionID = Dr["Session ID"].ToString().Trim();
                                DateTime StartDate = Convert.ToDateTime(Dr["Start Date"]);
                                DateTime EndDate = Convert.ToDateTime(Dr["End Date"]);
                                int Duration = Convert.ToInt32(Dr["Duration(As per Program Master)"].ToString().Trim());
                                string MSPIN = Dr["MSPIN"].ToString().Trim();
                                string Name = Dr["Name"].ToString().Trim();
                                DateTime DateofBirth = Convert.ToDateTime(Dr["Date of Birth"]);
                                string MobileNo = Dr["Mobile No"].ToString().Trim();

                                var AgencyDtl = AgencyList.Where(x => x.AgencyCode == AgencyCode).FirstOrDefault();
                                var ProgramDtl = ProgramList.Where(x => x.ProgramCode == ProgramCode).FirstOrDefault();
                                //var FacultyDtl = FacultyList.Where(x => x.FacultyCode == FacultyCode).FirstOrDefault();
                                if (Name != null && AgencyDtl != null && MSPIN != null && ProgramDtl != null)
                                {
                                    if (FacultyCode == "" || FacultyCode == null || string.IsNullOrEmpty(FacultyCode))
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
                                            RetMessage += MSPIN + " - Faculty is not defined under Agency code \n";
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    RetMessage += MSPIN + " - Program/Faculty/Agency code is misspelled \n";
                                    continue;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                        //scope.Complete();

                        //transaction.Complete();
                        //transaction.Dispose();

                        //return info;
                    //}
                }
            }
            NominationValidationBLL RetInfo = new NominationValidationBLL();
            RetInfo.Response = RetMessage;
            RetInfo.Datatbl = newInfo;
            return RetInfo;

			
		}

		public static bool BulkInsertDataTable(DataTable dataTable,UserDetailsBLL Obj)
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
                            string pass = Convert.ToDateTime(Dr["Date of Birth"].ToString()).ToString("dd-MM-yyyy");
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

                            string pass = Convert.ToDateTime(Dr["Date of Birth"].ToString()).ToString("dd-MM-yyyy");
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
                finally {
                    int Sataus=context.SP_BatchJobWith_Practical();
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
                var Check = context.TblNominations.Where(x=>x.Nomination_Id==Obj.Nomination_Id && x.IsActive==true).FirstOrDefault();

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
    }
}
