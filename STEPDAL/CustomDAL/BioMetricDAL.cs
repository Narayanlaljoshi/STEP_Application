
using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEPDAL.CustomDAL
{
    public class BioMetricDAL
    {
        public static bool SaveRegistrationDetails(RegistrationDetails Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                try
                {
                    //byte[] Thumb_1 = Encoding.ASCII.GetBytes(Obj.Thumb_1);
                    //byte[] Thumb_2 = Encoding.ASCII.GetBytes(Obj.Thumb_2);
                    //byte[] Candidate_Image = Encoding.ASCII.GetBytes(Obj.Candidate_Image);
                    //byte[] Document_Image = Encoding.ASCII.GetBytes(Obj.Document_Image);
                    int Status = Context.SP_InsertIntoTblRegistrationData(Obj.MSPIN, Obj.Thumb_1, Obj.Thumb_2, Obj.Candidate_Image, Obj.Document_Image,Obj.IsActive,Obj.CreatedBy);
                    if (Status > 0)
                    {
                        var CandidateDtl = Context.TblNominations.Where(x => x.MSPIN == Obj.MSPIN && x.IsActive == true).FirstOrDefault();
                        AttendancePunch.SavePunchInDetails(Obj.MSPIN, CandidateDtl != null? CandidateDtl.AgencyCode:"", DateTime.Now);
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false; 
                }                
            }
        }

        public static MSPINDetails CheckMSPIN_Registered(string MSPIN,string AgencyCode)
        {
            using (var Context = new CEIDBEntities())
            {
                MSPINDetails Dtl;
                try
                {
                    var ReqData = Context.SP_CheckMSPIN_Registered_NotRegistered(AgencyCode,MSPIN).FirstOrDefault();
                    //var ReaqData2 = Context.SP_CheckMSPIN_Registered_NotRegistered(AgencyCode, "736121").FirstOrDefault();

                    //bool IsMatched = ReqData.Candidate_Image.Equals(ReaqData2.Candidate_Image);

                    if (ReqData != null)
                    {
                        Dtl = new MSPINDetails {
                            AgencyCode= ReqData.AgencyCode,
                            Candidate_Image= ReqData.Candidate_Image,
                            Document_Image= ReqData.Document_Image,
                            IsRegistered= ReqData.IsRegistered,
                            MSPIN= ReqData.MSPIN,
                            Thumb_1= ReqData.Thumb_1,
                            Thumb_2= ReqData.Thumb_2,
                            Name= ReqData.Name
                        };
                        return Dtl;
                    }                    
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static UserDetailForBioMetric GuardLogin(string UserName, string Password)
        {
            using (var Context = new CEIDBEntities())
            {
                UserDetailForBioMetric Dtl;
                try
                {
                    var ReqData = Context.SP_GETGuardLoginDetails(UserName,Password).FirstOrDefault();
                    if (ReqData != null)
                    {
                        Dtl = new UserDetailForBioMetric
                        {
                            AgencyCode = ReqData.AgencyCode,
                            IsActive= ReqData.IsActive,
                            UserName= ReqData.UserName,
                            Agency_Id= ReqData.Agency_Id,
                            Role_Id= ReqData.Agency_Id,
                            User_Id= ReqData.User_Id
                        };
                        return Dtl;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static BiometricVersiondetails CheckBiometricAppVersion(string Machine_Name)
        {
            using (var Context = new CEIDBEntities())
            {
                BiometricVersiondetails Dtl;
                try
                {
                    var ReqData = Context.sp_CheckBiometricAppVersion(Machine_Name).FirstOrDefault();
                    if (ReqData != null)
                    {
                        Dtl = new BiometricVersiondetails
                        {
                            Current_Version = ReqData.Current_Version,
                            IsCurrentVersionLatest = ReqData.IsCurrentVersionIsLatest,
                            LastUpdatedOn = ReqData.LastUpdatedOn,
                            LatestVersion = ReqData.LatestVersion,
                            Machine_Name = ReqData.Machine_Name,
                            Link = ReqData.Link
                        };
                        return Dtl;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static BiometricVersiondetails UpdateBiometricVersionMapping(BiometricVersiondetails_V2 Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                BiometricVersiondetails Dtl = new BiometricVersiondetails();
                try
                {
                    int Status = Context.sp_Update_Biometric_VersionMapping(Obj.Machine_Name, Obj.Current_Version, Obj.Current_CRC, Obj.Mapped_Version_Id, true, 1);
                    if (Status > 0)
                    {
                        var ReqData = Context.sp_CheckBiometricAppVersion(Obj.Machine_Name).FirstOrDefault();
                        if (ReqData != null)
                        {
                            Dtl = new BiometricVersiondetails
                            {
                                Current_Version = ReqData.Current_Version,
                                IsCurrentVersionLatest = ReqData.IsCurrentVersionIsLatest,
                                LastUpdatedOn = ReqData.LastUpdatedOn,
                                Link = ReqData.Link,
                                LatestVersion = ReqData.LatestVersion,
                                Machine_Name = ReqData.Machine_Name,
                            };
                        }
                        return Dtl;
                    }
                    else
                        return Dtl;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
