using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ProjectUI
{
    /// <summary>
    /// Summary description for BioMetricSolution
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BioMetricSolution : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public bool SinglePunchIn(string MSPIN, string AgencyCode, DateTime DateTime)
        {
            return AttendancePunch.SavePunchInDetails(MSPIN, AgencyCode, DateTime);
        }
        [WebMethod]
        public bool SaveRegistrationDetails(RegistrationDetails Obj)
        {
            return BioMetricDAL.SaveRegistrationDetails(Obj);
        }

        [WebMethod]
        public MSPINDetails CheckMSPIN_Registered(string MSPIN, string AgencyCode)
        {
            return BioMetricDAL.CheckMSPIN_Registered(MSPIN, AgencyCode);
        }

        [WebMethod]
        public UserDetailForBioMetric GuardLogin(string UserName, string Password)
        {
            return BioMetricDAL.GuardLogin(UserName, Password);
        }
        [WebMethod] 
        public BiometricVersiondetails CheckBiometricAppVersion(string Machine_Name)
        {
            return BioMetricDAL.CheckBiometricAppVersion(Machine_Name);
        }
        [WebMethod]
        public BiometricVersiondetails UpdateBiometricVersionMapping(BiometricVersiondetails_V2 Obj)
        {
            return BioMetricDAL.UpdateBiometricVersionMapping(Obj);
        }
    }
}
