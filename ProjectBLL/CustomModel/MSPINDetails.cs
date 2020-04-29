using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class MSPINDetails
    {
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public string AgencyCode { get; set; }
        public byte[] Thumb_1 { get; set; }
        public byte[] Thumb_2 { get; set; }
        public byte[] Candidate_Image { get; set; }
        public byte[] Document_Image { get; set; }
        public string IsRegistered { get; set; }
        public List<CourseDTL> CourseDetails { get; set; }
    }
    public class CourseDTL
    {
        public string SessionID { get; set; }
        public string ProgramName { get; set; }
    }
}
