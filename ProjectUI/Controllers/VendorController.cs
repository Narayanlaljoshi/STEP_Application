using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectUI.Controllers
{
    public class VendorController : ApiController
    {
        [HttpGet]
        public  List<VendorList> GetVendorList()
        {
            return Vendors.GetVendorList();
        }
        [HttpPost]
        public string AddVendorList(VendorList Obj)
        {
            return Vendors.AddVendorList(Obj);
        }

        [HttpGet]
        public List<VendorTrainerList> GetVendorTrainerList(int User_Id)
        {
            return Vendors.GetVendorTrainerList(User_Id);
        }

        [HttpPost]
        public string UpdateVendorTrainerList(List<VendorTrainerList> List)
        {
            return Vendors.UpdateVendorTrainerList(List);
        }

        [HttpPost]
        public IList<SessionListForStepAgencyManager> GetSessionList_StepManager(Filter_STEP_Agency Obj)
        {
            return Vendors.GetSessionList_StepManager(Obj);
        }

        [HttpGet]
        public IList<ActiveTrainerForVendor> GetActiveTrainerForVendor(string UserName)
        {
            return Vendors.GetActiveTrainerForVendor(UserName);
        }

        [HttpPost]
        public string UpdateFaculty(SessionListForStepAgencyManager Obj)
        {
            return Vendors.UpdateFaculty(Obj);
        }
        [HttpPost]
        public string AddCandidate_Mobile(AddCandidateBLL Obj)
        {
            return Vendors.AddCandidate_Mobile(Obj);
        }
    }
}
