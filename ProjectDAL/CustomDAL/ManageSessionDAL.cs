using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDAL.CustomDAL
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
                var ReqData = Context.TblNominations.Where(x=>x.SessionID==Obj.SessionID).ToList();
                var FacultyDtl = Context.TblFaculties.Where(x => x.Faculty_Id == Obj.Faculty_Id && x.IsActive==true).FirstOrDefault();
                foreach (var Item in ReqData)
                {
                    Item.FacultyCode = FacultyDtl.FacultyCode;
                    Item.Faculty_Id = Obj.Faculty_Id;
                    Item.ModifiedBy = Obj.User_Id;
                    Item.ModifiedDate = DateTime.Now;
                    Context.Entry(Item).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();

                }
                return "Success: Updated Successfully!";
            }
        }
        public static string UpdateFaculty_Mobile(SessionList Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                var ReqData = Context.TblNominations.Where(x => x.SessionID == Obj.SessionID).ToList();
                var FacultyDtl = Context.TblFaculties.Where(x => x.Faculty_Id == Obj.Faculty_Id && x.IsActive == true).FirstOrDefault();
                foreach (var Item in ReqData)
                {
                    Item.FacultyCode = FacultyDtl.FacultyCode;
                    Item.Faculty_Id = Obj.Faculty_Id;
                    Item.ModifiedBy = Obj.User_Id;
                    Item.ModifiedDate = DateTime.Now;
                    Context.Entry(Item).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();

                }
                return "Success: Updated Successfully!";
            }
        }
    }
}
