using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STEPDAL.CustomDAL;
using ProjectBLL.CustomModel;

namespace Project.Controllers
{
    public class ManageSessionController : ApiController
    {
        [HttpGet]
        public IList<SessionList> GetSessionList(int Agency_Id)
        {
            return ManageSessionDAL.GetSessionList(Agency_Id);
        }

        [HttpGet]
        public IList<SessionList> GetSessionListByProgramId(int ProgramId)
        {
            return ManageSessionDAL.GetSessionListByProgramId(ProgramId);
        }

        [HttpGet]
        public IList<SessionList> GetSessionList_Mobile(int Agency_Id)
        {
            return ManageSessionDAL.GetSessionList(Agency_Id);
        }
        [HttpGet]
        public IList<FacultyList> GetFacultyList(int Agency_Id)
        {
            return ManageSessionDAL.GetFacultyListForDDL(Agency_Id);
        }
        [HttpGet]
        public IList<FacultyList> GetFacultyList_External(int Agency_Id, string UserName)
        {
            return ManageSessionDAL.GetFacultyList_External(Agency_Id,UserName);
        }
        [HttpPost]
        public string UpdateFaculty(SessionList Obj)
        {
            return ManageSessionDAL.UpdateFaculty(Obj);
        }
        [HttpPost]
        public string UpdateFaculty_Mobile(SessionList Obj)
        {
            return ManageSessionDAL.UpdateFaculty_Mobile(Obj);
        }
        [HttpGet]
        public IList<FacultyList> GetFacultyList_Mobile(int Agency_Id)
        {
            return ManageSessionDAL.GetFacultyList_Mobile(Agency_Id);
        }
    }
}