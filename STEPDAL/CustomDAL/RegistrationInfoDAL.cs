using ProjectBLL;
using ProjectBLL.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEPDAL.CustomDAL
{
    public class RegistrationInfoDal
    {
         
        public static long Registration_Id { get; private set; }

        public static RegistrationInfoBLL GetData(string mspin)
        {
            RegistrationInfoBLL list = new RegistrationInfoBLL();
            using (var context = new DB.CEIDBEntities())
            {
                var data = context.sp_GetRegistrationInfo(mspin).FirstOrDefault();
                if (data != null)
                {
                    list.MSPIN = data.MSPIN;
                    list.Registration_Id = data.Registration_Id;
                    list.CreationDate = data.CreationDate;
                    list.IsActive = data.IsActive;
                    list.Candidate_Image = data.Candidate_Image;
                    list.Document_Image = data.Document_Image;
                }
                return list;
            }
        }

        public static string Reset(RegistrationInfoBLL Obj)
        {
            try
            {
                using (var context = new DB.CEIDBEntities())
                {
                    int Status = context.sp_UpdateRegistrationInfo(Obj.Registration_Id, Obj.IsActive);

                    return Status > 0 ? "Success" : "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
    }
}

