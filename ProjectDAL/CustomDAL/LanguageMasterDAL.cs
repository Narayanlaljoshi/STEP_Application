using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBLL.CustomModel;
using STEPDAL.DB;

namespace ProjectDAL.CustomDAL
{

    public class LanguageMasterDAL
    {


        public static List<LanguageMasterBLL> GetLanguage()
        {
            using (var LContext = new CEIDBEntities())
            {
                List<LanguageMasterBLL> objlist = null;
                var LanguageList = LContext.tblLanguageMasters.Where(x => x.IsDeleted == false);
                objlist = LanguageList.Select(x => new LanguageMasterBLL()
                {

                    LanguageMasterId = x.LanguageMaster_Id,
                    Language = x.Language,
                    

                }).ToList();
                return objlist;
            }
        }





        public static int SaveFunction(LanguageMasterBLL obj)
        {

            using (var Context = new CEIDBEntities())
            {
                var LanguageTy = Context.tblLanguageMasters.Where(x => x.Language.Equals(obj.Language) && x.IsDeleted == false).FirstOrDefault();

                if (LanguageTy == null)
                {

                    tblLanguageMaster tblLanguage = new tblLanguageMaster();
                    tblLanguage.Language = obj.Language;
                    tblLanguage.CreatedBy = obj.CreatedBy;
                    tblLanguage.IsDeleted = false;
                    tblLanguage.CreationDate = DateTime.Now;
                    Context.Entry(tblLanguage).State = System.Data.Entity.EntityState.Added;
                    Context.SaveChanges();

                    return 1;
                }
                return 2;

            }


        }




        public static int UpdateLanguage(LanguageMasterBLL obj)
        {
            using (var context = new CEIDBEntities())
            {
                var UpLanguage = context.tblLanguageMasters.Where(x => x.LanguageMaster_Id == obj.LanguageMasterId && x.IsDeleted == false).FirstOrDefault();



                if (UpLanguage != null)
                {
                    var LanguageDelete = context.tblLanguageMasters.Where(c => c.Language.Equals(obj.Language) && c.IsDeleted == false).FirstOrDefault();
                    if (LanguageDelete == null)
                    {

                        UpLanguage.Language = obj.Language;

                        UpLanguage.IsDeleted = false;
                        UpLanguage.CreationDate = DateTime.Now;
                        UpLanguage.ModifiedBy = obj.ModifiedBy;
                        UpLanguage.ModifiedDate = DateTime.Now;
                        context.Entry(UpLanguage).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

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


        public static string DeleteLanguage(LanguageMasterBLL obj)
        {
            using (var LContext = new CEIDBEntities())
            {
                var LanguageDelete = LContext.tblLanguageMasters.Where(c => c.LanguageMaster_Id == obj.LanguageMasterId).FirstOrDefault();
                if (LanguageDelete != null)
                {


                    LanguageDelete.IsDeleted = true;
                    LanguageDelete.CreationDate = DateTime.Now;
                    LanguageDelete.ModifiedBy = obj.ModifiedBy;
                    LanguageDelete.ModifiedDate = DateTime.Now;
                    LContext.Entry(LanguageDelete).State = System.Data.Entity.EntityState.Modified;
                    LContext.SaveChanges();


                    return "success";
                }
                return "error";
            }

        }

    }

}
