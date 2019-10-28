using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
	public class ProgramMasterBLL
	{

		public int ProgramId { get; set; }
		public string ProgramCode { get; set; }
		public string ProgramName { get; set; }
        public Nullable<int> ProgramType_Id { get; set; }
        public string Language { get; set; }
		public Nullable<int> LanguageMasterId { get; set; }
		public bool IsActive { get; set; }
		public System.DateTime CreationDate { get; set; }
		public int CreatedBy { get; set; }
		public Nullable<System.DateTime> ModifiedDate { get; set; }
		public Nullable<int> ModifiedBy { get; set; }
        public List<SelectedLanguages> SelectedLanguages { get; set; }
        public Nullable<int> Duration { get; set; }
    }


   


    public  class ProgramMasterDetail_Model
    {
        public string Language { get; set; }
        public Nullable<int> ProgramType_Id { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public int LanguageMaster_Id { get; set; }
        public int ProgramId { get; set; }
        public int Duration { get; set; }
        public string ProgramType { get; set; }

        public List<SelectedLanguages> SelectedLanguages { get; set; }
    }

    public class SelectedLanguages
    {
        public int LanguageMaster_Id { get; set; }
        public string Language { get; set; }
        public int ProgramMasterDetail_Id { get; set; }
    }


}
