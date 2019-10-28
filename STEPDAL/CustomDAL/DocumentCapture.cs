using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEPDAL.DB;

namespace STEPDAL.CustomDAL
{
    public class DocumentCapture
    {
        public static string SaveDocuments(DocumentsBLL Obj)
        {
            string ServerIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
            string DocumentsPath = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"]);
            using (var Context = new CEIDBEntities())
            {
                try
                {
                    var Check = Context.TblPhotoDocumentDtls.Where(x => x.MSPIN == Obj.MSPIN).FirstOrDefault();//&&x.IsActive==true
                    string Uid = DateTime.Now.Ticks.ToString();
                    if (Check == null)
                    {
                        //Obj.MSPIN = Obj.MSPIN.Remove(@"#/");
                        Obj.MSPIN = Obj.MSPIN.Replace("/", "");
                        Obj.MSPIN = Obj.MSPIN.Replace("#", "");
                        //Key = Key.Replace(" ", "");
                        string Picture = DocumentsPath + Obj.MSPIN + "_PIC_"+ Uid + ".jpeg";
                        string DocumentID = DocumentsPath + Obj.MSPIN + "_DOC_" + Uid + ".jpeg";
                        string[] PictureBase64 = Obj.Picture.Split(',');
                        string[] DocumentIDBase64 = Obj.Document.Split(',');
                        byte[] imgByteArray = Convert.FromBase64String(PictureBase64[1]);
                        byte[] docByteArray = Convert.FromBase64String(DocumentIDBase64[1]);
                        File.WriteAllBytes(Picture, imgByteArray);
                        File.WriteAllBytes(DocumentID, docByteArray);

                        TblPhotoDocumentDtl PD = new TblPhotoDocumentDtl {
                            CreatedBy=1,
                            CreationDate=DateTime.Now,
                            DocumentID= ServerIP+ Obj.MSPIN + "_DOC_" + Uid + ".jpeg",
                            IsActive=true,
                            MSPIN=Obj.MSPIN,
                            Picture= ServerIP+ Obj.MSPIN + "_PIC_" + Uid + ".jpeg"
                        };
                        Context.Entry(PD).State = System.Data.Entity.EntityState.Added;
                        Context.SaveChanges();
                    }
                    else
                    {
                        string Picture = DocumentsPath + Obj.MSPIN + "_PIC_" + Uid + ".jpeg";
                        string DocumentID = DocumentsPath + Obj.MSPIN + "_DOC_" + Uid + ".jpeg";
                        string[] PictureBase64 = Obj.Picture.Split(',');
                        string[] DocumentIDBase64 = Obj.Document.Split(',');
                        byte[] imgByteArray = Convert.FromBase64String(PictureBase64[1]);
                        byte[] docByteArray = Convert.FromBase64String(DocumentIDBase64[1]);
                        File.WriteAllBytes(Picture, imgByteArray);
                        File.WriteAllBytes(DocumentID, docByteArray);

                        Check.ModifiedBy = 1;
                        Check.ModifiedDate = DateTime.Now;
                        Check.DocumentID = ServerIP + Obj.MSPIN + "_DOC_" + Uid + ".jpeg";
                        Check.IsActive = true;
                        Check.Picture = ServerIP + Obj.MSPIN + "_PIC_" + Uid + ".jpeg";

                        Context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        Context.SaveChanges();
                    }
                    return "Success: Data Saved Successfully !";
                }
                catch (Exception Ex)
                {
                    return "Error: " + Ex.ToString();
                }
            }
            
        }
    }
}
