using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class StudentTestQuestionsBLL
    {
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<bool> TestInitiated { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public int DetailId { get; set; }
        public string QuestionCode { get; set; }
        public string Question { get; set; }
        public string Image { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string AnswerKey { get; set; }
        public string AnswerGiven { get; set; }
        public string MSPIN { get; set; }
        public string Day { get; set; }
        public string SessionID { get; set; }
        
    }
}
