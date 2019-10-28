using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEPDAL.DB;

namespace STEPDAL.CustomDAL
{
    public class AttendancePunch
    {
        public static bool SavePunchInDetailsList(List<AttendancePunchDtl> Objects)
        {
            using (var Context = new CEIDBEntities())
            {
                foreach (var Obj in Objects)
                {   
                    try
                    {
                        int Status = Context.sp_Update_InsertIntoTblAttendancePunchIn(Obj.MSPIN,Obj.AgencyCode,Obj.DateTime,Obj.MachineCode,Obj.MachineId);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return true;


            }
        }
        public static bool SavePunchInDetails(AttendancePunchDtl Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                //foreach (var Obj in Objects)
                //{
                var Check = Context.TblAttendancePunchIns.Where(x => x.MSPIN == Obj.MSPIN && x.DateTime == Obj.DateTime && x.AgencyCode == Obj.AgencyCode && x.IsActive == true).FirstOrDefault();
                try
                {
                    if (Check == null)
                    {
                        TblAttendancePunchIn AP = new TblAttendancePunchIn
                        {
                            DateTime = Obj.DateTime,
                            MSPIN = Obj.MSPIN,
                            AgencyCode = Obj.AgencyCode,
                            CreatedBy = 1,
                            CreationDate = DateTime.Now,
                            IsActive = true,
                            MachineCode = Obj.MachineCode,
                            MachineId = Obj.MachineId
                        };
                        Context.Entry(AP).State = System.Data.Entity.EntityState.Added;
                        Context.SaveChanges();
                    }
                    else
                    {
                        Check.DateTime = Obj.DateTime;
                        Check.ModifiedBy = 1;
                        Check.ModifiedDate = DateTime.Now;
                        Check.IsActive = true;
                        Check.MachineCode = Obj.MachineCode;
                        Check.MachineId = Obj.MachineId;
                        Context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                //}
            }
        }

        public static bool SavePunchInDetails(string MSPIN, string AgencyCode, DateTime DateTime)
        {
            using (var Context = new CEIDBEntities())
            {
                //foreach (var Obj in Objects)
                //{
                var Check = Context.TblAttendancePunchIns.Where(x => x.MSPIN == MSPIN && x.DateTime == DateTime && x.AgencyCode == AgencyCode && x.IsActive == true).FirstOrDefault();
                try
                {
                    if (Check == null)
                    {
                        TblAttendancePunchIn AP = new TblAttendancePunchIn
                        {
                            DateTime = DateTime,
                            MSPIN = MSPIN,
                            AgencyCode = AgencyCode,
                            CreatedBy = 1,
                            CreationDate = DateTime.Now,
                            IsActive = true,
                        };
                        Context.Entry(AP).State = System.Data.Entity.EntityState.Added;
                        Context.SaveChanges();
                    }
                    else
                    {
                        Check.DateTime = DateTime;
                        Check.ModifiedBy = 1;
                        Check.ModifiedDate = DateTime.Now;
                        Check.IsActive = true;
                        //Check.MachineCode = MachineCode;
                        //Check.MachineId = MachineId;
                        Context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                //}
            }
        }

        public static List<CandidatesListForStarlink> GetCandidatesList()
        {
            List<CandidatesListForStarlink> CandidatesList = null;
            using (var Context = new CEIDBEntities())
            {
                var ReqData = Context.sp_GetCandidatesListDayWise().ToList();
                if (ReqData.Count!=0)
                {
                    CandidatesList = ReqData.Select(x => new CandidatesListForStarlink {
                        AgencyCode=x.AgencyCode,
                        DocumentID=x.DocumentID,
                        Duration=x.Duration,
                        EndDate=x.EndDate,
                        IsRegistered=x.IsRegistered,
                        MSPIN=x.MSPIN,
                        Name=x.Name,
                        Picture=x.Picture,
                        ProgramCode=x.ProgramCode,
                        SessionID=x.SessionID,
                        StartDate=x.StartDate

                    }).ToList();
                }
            }
            return CandidatesList;
        }

        public static bool PunchInByRTM(AttendancePunchDtl Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                //foreach (var Obj in Objects)
                //{
                var Check = Context.TblAttendancePunchIns.Where(x => x.MSPIN == Obj.MSPIN && x.DateTime == Obj.DateTime && x.AgencyCode == Obj.AgencyCode && x.IsActive == true).FirstOrDefault();
                try
                {
                    if (Check == null)
                    {
                        TblAttendancePunchIn AP = new TblAttendancePunchIn
                        {
                            DateTime = Obj.DateTime,
                            MSPIN = Obj.MSPIN,
                            AgencyCode = Obj.AgencyCode,
                            CreatedBy = 1,
                            CreationDate = DateTime.Now,
                            IsActive = true,
                            MachineCode = Obj.MachineCode,
                            MachineId = Obj.MachineId
                        };
                        Context.Entry(AP).State = System.Data.Entity.EntityState.Added;
                        Context.SaveChanges();
                    }
                    else
                    {
                        Check.DateTime = Obj.DateTime;
                        Check.ModifiedBy = 1;
                        Check.ModifiedDate = DateTime.Now;
                        Check.IsActive = true;
                        Check.MachineCode = Obj.MachineCode;
                        Check.MachineId = Obj.MachineId;
                        Context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                //}
            }
        }
    }
}
