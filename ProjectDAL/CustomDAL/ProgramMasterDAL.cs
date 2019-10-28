using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBLL.CustomModel;
using ProjectDAL.CustomDAL;
using STEPDAL.DB;

namespace ProjectDAL.CustomDAL
{

    public class ProgramMasterDAL
    {
        //public static List<ProgramMasterDetail_Model> GetProgramTypeList()
        //{
        //    using (var LContext = new CEIDBEntities())
        //    {
        //        //  List<ProgramMasterBLL> objlist = null;
        //        var LanguageList = LContext.TblProgramTypeMasters.Where(x=>x.IsActive==true).ToList();

                

        //        return objList;



        //    }
        //}


        public static List<ProgramMasterDetail_Model> GetProgram()
        {
            using (var LContext = new CEIDBEntities())
            {
              //  List<ProgramMasterBLL> objlist = null;
                var ProgramList = LContext.sp_ProgramMaster().ToList();
                
                List<ProgramMasterDetail_Model> objList = new List<ProgramMasterDetail_Model>();

                foreach (var item in ProgramList)
                {
                    //LocationName = item.ManufacturingLocation;
                    var Idx = objList.Where(x => x.ProgramName.Equals(item.ProgramName)).FirstOrDefault();
                    if (Idx == null)
                    {
                        objList.Add(new ProgramMasterDetail_Model()
                        {
                            ProgramName = item.ProgramName,
                            ProgramCode = item.ProgramCode,
                            ProgramId = item.ProgramId,
                            ProgramType_Id = item.ProgramType_Id,
                            Duration = item.Duration != null ? item.Duration.Value : 0,
                            ProgramType = item.ProgramType,
                            SelectedLanguages = ProgramList.Where(x => x.ProgramName.Equals(item.ProgramName)).Select(x => new SelectedLanguages()
                            {
                                Language = x.Language,
                                LanguageMaster_Id = x.LanguageMaster_Id,
                                ProgramMasterDetail_Id = item.ProgramMasterDetail_Id
                            }).ToList(),
                        });
                    }
                }

                 return objList;



            }
        }


        public static int SaveFunction(ProgramMasterBLL obj)
        {

            using (var Context = new CEIDBEntities())
            {
                var ProgramDataSave = Context.TblProgramMasters.Where(x => x.ProgramCode == obj.ProgramCode &&
                x.ProgramName == obj.ProgramName && x.IsActive == true).FirstOrDefault();

                if (ProgramDataSave == null)
                {

                    TblProgramMaster tblProgram = new TblProgramMaster();
                    tblProgram.ProgramName = obj.ProgramName;
                    tblProgram.ProgramCode = obj.ProgramCode;
                    tblProgram.LanguageMaster_Id = obj.LanguageMasterId;
                    tblProgram.IsActive = true;
                    tblProgram.CreationDate = DateTime.Now;
                    tblProgram.Duration = obj.Duration;
                    tblProgram.ProgramType_Id = obj.ProgramType_Id;
                    Context.Entry(tblProgram).State = System.Data.Entity.EntityState.Added;
                    Context.SaveChanges();

                    foreach (var Detail in obj.SelectedLanguages)
                    {
                        tblPrgramMasterDetail tblProgramMaster = new tblPrgramMasterDetail();

                        tblProgramMaster.ProgramId = tblProgram.ProgramId;
                        tblProgramMaster.LanguageMaster_Id = Detail.LanguageMaster_Id;
                        tblProgramMaster.IsDeleted = false;

                        tblProgramMaster.CreatedBy = 1;
                        tblProgramMaster.CreationDate = DateTime.Now;

                        Context.Entry(tblProgramMaster).State = System.Data.Entity.EntityState.Added;
                        Context.SaveChanges();
                    }

                    return 1;
                }
                return 2;

            }


        }


        public static string DeleteProgram(ProgramMasterBLL obj)
        {
            using (var LContext = new CEIDBEntities())
            {
                var ProgramDelete = LContext.TblProgramMasters.Where(c => c.ProgramId == obj.ProgramId).FirstOrDefault();
                if (ProgramDelete != null)
                {


                    ProgramDelete.IsActive = false;
                    ProgramDelete.CreationDate = DateTime.Now;
                    ProgramDelete.ModifiedBy = 1;
                    ProgramDelete.ModifiedDate = DateTime.Now;
                    LContext.Entry(ProgramDelete).State = System.Data.Entity.EntityState.Modified;
                    LContext.SaveChanges();


                    return "success";
                }
                return "error";
            }

        }




        public static int UpdateProgram(ProgramMasterBLL obj)
        {
            using (var context = new CEIDBEntities())
            {
                var UPProgram = context.TblProgramMasters.Where(x => x.ProgramId == obj.ProgramId && x.IsActive == true).FirstOrDefault();

                if (UPProgram != null)
                {
                    var Program = context.TblProgramMasters.Where(c => c.ProgramCode.Equals(obj.ProgramCode)&& c.IsActive == true).FirstOrDefault();
                    if (Program == null)
                    {
                        UPProgram.ProgramCode = obj.ProgramCode;
                        UPProgram.ProgramName = obj.ProgramName;
                        UPProgram.LanguageMaster_Id = obj.LanguageMasterId;
                        UPProgram.Duration = obj.Duration;
                        UPProgram.IsActive = true;
                        UPProgram.CreationDate = DateTime.Now;
                        UPProgram.ModifiedBy = 1;
                        UPProgram.ModifiedDate = DateTime.Now;
                        context.Entry(UPProgram).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        var master = context.tblPrgramMasterDetails.Where(x => x.ProgramId == obj.ProgramId).ToList();
                        //foreach (var Detail in obj.master)
                        //{
                            context.Entry(master).State = System.Data.Entity.EntityState.Deleted;
                            context.SaveChanges();
                        //}


                        foreach (var Detail in obj.SelectedLanguages)
                        {


                            tblPrgramMasterDetail tblProgramMaster = new tblPrgramMasterDetail();

                            tblProgramMaster.ProgramId = obj.ProgramId;
                            tblProgramMaster.LanguageMaster_Id = Detail.LanguageMaster_Id;
                            tblProgramMaster.IsDeleted = false;

                            tblProgramMaster.CreatedBy = obj.CreatedBy;

                            tblProgramMaster.ModifiedBy = obj.ModifiedBy;
                            tblProgramMaster.CreationDate = DateTime.Now;

                            context.Entry(tblProgramMaster).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                           
                        }




                        return 1;
                    }

                    else
                    {
                        var master = context.tblPrgramMasterDetails.Where(x => x.ProgramId == obj.ProgramId).ToList();

                        Program.ProgramCode = obj.ProgramCode;
                        Program.ProgramName = obj.ProgramName;
                        Program.LanguageMaster_Id = obj.LanguageMasterId;
                        Program.Duration = obj.Duration;
                        Program.IsActive = true;
                        Program.CreationDate = DateTime.Now;
                        Program.ModifiedBy = 1;
                        Program.ModifiedDate = DateTime.Now;
                        context.Entry(Program).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        foreach (var Dt in master)
                        {

                            context.Entry(Dt).State = System.Data.Entity.EntityState.Deleted;
                            context.SaveChanges();
                        }

                        foreach (var Detail in obj.SelectedLanguages)
                        {

                            tblPrgramMasterDetail tblProgramMaster = new tblPrgramMasterDetail();

                            tblProgramMaster.ProgramId = obj.ProgramId;
                            tblProgramMaster.LanguageMaster_Id = Detail.LanguageMaster_Id;
                            tblProgramMaster.IsDeleted = false;

                            tblProgramMaster.CreatedBy = obj.CreatedBy;

                            tblProgramMaster.ModifiedBy = obj.ModifiedBy;
                            tblProgramMaster.CreationDate = DateTime.Now;

                            context.Entry(tblProgramMaster).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();

                        }

                        return 1;
                    }




                    return 2;
                }
                else
                {
                    return 2;
                }

            }


        }



    }

}
