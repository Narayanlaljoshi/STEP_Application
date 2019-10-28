using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBLL.CustomModel;
using STEPDAL.DB;

namespace STEPDAL.CustomDAL
{
    public class FaceRecognitionData
    {
        public static FaceRecognitionDataBLL GetFaceRecognitionData(string MSPIN)
        {
            FaceRecognitionDataBLL Obj;
            using (var context = new CEIDBEntities())
            {
                var ReqData = context.sp_GetFaceRecognitionData(MSPIN).FirstOrDefault();
                if (ReqData != null)
                {
                    Obj = new FaceRecognitionDataBLL
                    {
                        CreatedBy= ReqData.CreatedBy,
                        CreationDate= ReqData.CreationDate,
                        Id= ReqData.Id,
                        IsActive= ReqData.IsActive,
                        ModifiedBy= ReqData.ModifiedBy,
                        ModifiedDate= ReqData.ModifiedDate,
                        MSPIN= ReqData.MSPIN,
                        Image=ReqData.Image
                    };
                    return Obj;
                }
                else
                    return null;
                
            }
        }
        public static string UpdateFaceRecognitionData(FaceRecognitionDataBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                var Check = context.TblFaceRecognitionDatas.Where(x => x.MSPIN == Obj.MSPIN && x.IsActive == true).FirstOrDefault();
                try
                {
                    if (Check == null)
                    {
                        TblFaceRecognitionData FD = new TblFaceRecognitionData
                        {
                            IsActive = true,
                            MSPIN = Obj.MSPIN,
                            CreatedBy = Obj.CreatedBy,
                            CreationDate = DateTime.Now,
                            Image = Obj.Image,
                        };
                        context.Entry(FD).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();
                        return "Success";
                    }
                    else
                    {
                        TblFaceRecognitionData FD = new TblFaceRecognitionData
                        {
                            IsActive = true,
                            MSPIN = Obj.MSPIN,
                            ModifiedBy = Obj.ModifiedBy,
                            ModifiedDate = DateTime.Now,
                            Image = Obj.Image,
                        };
                        context.Entry(FD).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return "Success";
                    }
                }
                catch (Exception Ex) {
                    return Ex.ToString();
                }
            }
        }
    }
}
