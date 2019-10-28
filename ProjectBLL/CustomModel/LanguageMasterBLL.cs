using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
	public class LanguageMasterBLL
	{
		
			public int LanguageMasterId { get; set; }
			public string Language { get; set; }
			public string UploadFile { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        


    }
}
