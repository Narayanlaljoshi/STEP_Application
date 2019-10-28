using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class RegistrationDetails
    {
        public long Registration_Id { get; set; }
        public string MSPIN { get; set; }
        public byte[] Thumb_1 { get; set; }
        public byte[] Thumb_2 { get; set; }
        public byte[] Candidate_Image { get; set; }
        public byte[] Document_Image { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
