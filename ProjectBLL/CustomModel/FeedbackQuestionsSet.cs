using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class FeedbackQuestionsSet
    {
        public long FeedbackQuestion_Id { get; set; }
        public int ProgramId { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public string SessionID { get; set; }
        public string MSPIN { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }

    public class FeedbackModelData
    {
        public int ProgramId { get; set; }
        public int UserId { get; set; }
    }
}
