using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class ProgramTestCalenderDetail_Practical
    {
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> Ques_Sequence { get; set; }
        public string QuestionCatagory { get; set; }
        public string QuestionCode { get; set; }
        public Nullable<int> Set_Id { get; set; }
        public int TimeDuration { get; set; }
        public string Question { get; set; }
        public Nullable<int> Marks_A { get; set; }
        public Nullable<int> Marks_B { get; set; }
        public Nullable<int> Marks_C { get; set; }
        public Nullable<int> Marks_D { get; set; }
        public Nullable<int> Marks_E { get; set; }
        public Nullable<int> Marks_F { get; set; }
        public string ActionA { get; set; }
        public string ActionB { get; set; }
        public string ActionC { get; set; }
        public string ActionD { get; set; }
        public string ActionE { get; set; }
        public string ActionF { get; set; }
        public string ActionA_Image { get; set; }
        public string ActionB_Image { get; set; }
        public string ActionC_Image { get; set; }
        public string ActionD_Image { get; set; }
        public string ActionE_Image { get; set; }
        public string ActionF_Image { get; set; }
        public string Question_Image { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsValidEntry { get; set; }
    }
}
