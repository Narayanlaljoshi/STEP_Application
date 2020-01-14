using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class RegistrationInfoBLL
    {
        public long Registration_Id { get; set; }
        public string MSPIN { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public byte[] Candidate_Image { get; set; }
        public byte[] Document_Image { get; set; }
    }
}
