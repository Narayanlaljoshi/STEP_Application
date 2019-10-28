using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;
using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;

namespace Project.Controllers
{
	public class ProgramMasterController : ApiController
	{
		[System.Web.Http.HttpGet]
		public List<ProgramMasterDetail_Model> GetProgram()
		{
			return ProgramMasterDAL.GetProgram();
		}
        //[HttpGet]
        //public List<ProgramMasterDetail_Model> GetProgramType()
        //{
        //    return ProgramMasterDAL.GetProgramType();
        //}
        [System.Web.Http.HttpPost]
		public string DeleteProgram(ProgramMasterBLL obj)
		{
			return ProgramMasterDAL.DeleteProgram(obj);
		}
		[HttpPost]
		public int SaveFunction([FromBody] ProgramMasterBLL obj)
		{
			return ProgramMasterDAL.SaveFunction(obj);
		}

        //[HttpPost]
        //public void SaveFunction(List<ProgramMasterBLL> obj)
        //{
        //    return ProgramMasterDAL.SaveFunction(obj);
        //}

        [HttpPost]
		public int UpdateProgram(ProgramMasterBLL obj)
		{
			return ProgramMasterDAL.UpdateProgram(obj);
		}
	}
}