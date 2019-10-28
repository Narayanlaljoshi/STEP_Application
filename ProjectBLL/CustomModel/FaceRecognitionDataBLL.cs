using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class FaceRecognitionDataBLL
    {
        public long Id { get; set; }
        public string MSPIN { get; set; }
        public byte[] Image { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
