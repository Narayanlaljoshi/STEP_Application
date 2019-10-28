using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ProjectBLL.CustomModel;
using ProjectDAL.CustomDAL;

namespace Project.Controllers
{
	public class LanguageMasterController  : ApiController
	{
		[HttpGet]
		public List<LanguageMasterBLL> GetLanguage()
		{
			return LanguageMasterDAL.GetLanguage();

		}
		[HttpPost]
		public int UpdateLanguage(LanguageMasterBLL obj)
		{
			return LanguageMasterDAL.UpdateLanguage(obj);
		}

		[HttpPost]
		public int SaveFunction([FromBody] LanguageMasterBLL obj)
		{
			return LanguageMasterDAL.SaveFunction(obj);
		}

		[HttpPost]
		public string DeleteLanguage(LanguageMasterBLL obj)
		{
			return LanguageMasterDAL.DeleteLanguage(obj);
		}

	}
}