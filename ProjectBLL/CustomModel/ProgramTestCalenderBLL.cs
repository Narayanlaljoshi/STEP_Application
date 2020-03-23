using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class ProgramTestCalenderBLL
    {
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> DayCount { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> EvaluationTypeId { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<int> ValidDuration { get; set; }
        public string TestCode { get; set; }
        public string QuestionPaperType { get; set; }
        public Nullable<int> Marks_Question { get; set; }
        public Nullable<int> Total_Marks { get; set; }
        public Nullable<int> Q_Bank { get; set; }

        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<int> PracticalDefaultMarks { get; set; }
        public Nullable<int> PracticalMaxMarks { get; set; }
        public Nullable<int> PracticalMinMarks { get; set; }
    }
    public class QestionLanguageModel
    {
        public int LanguageMasterId { get; set; }
        public int Set_Id { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
    public class QestionLanguagePracticalModel
    {
        public int LanguageMaster_Id { get; set; }
        public int Set_Id { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
    public class QestionDetail_Model
    {
        public int DetailId { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public string QuestionCode { get; set; }
        public string Question { get; set; }
        public string Image { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string AnswerKey { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }


    }


    public class ProgramMaster
    {
        public int ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }


    public class ProgramTestCalenderDetail_Model
    {
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> DayCount { get; set; }
        public Nullable<int> EvaluationTypeId { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public string QuestionPaperType { get; set; }

        public string TestCode { get; set; }
        public Nullable<int> Marks_Question { get; set; }
        public Nullable<int> Total_Marks { get; set; }
        public Nullable<int> Q_Bank { get; set; }
        public Nullable<int> ValidDuration { get; set; }
        public Nullable<int> QuesAdded { get; set; }
        public Nullable<int> PracticalDefaultMarks { get; set; }
        public Nullable<int> PracticalMaxMarks { get; set; }
        public Nullable<int> PracticalMinMarks { get; set; }
    }


    public class ProgramTestModel
    {
        public int ProgramTestCalenderId { get; set; }
        public int Set_Id { get; set; }
        public string Set_Title { get; set; }
        public int TestDuration { get; set; }
    }





    public class ProgramTest_QuestionDetail
    {
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> DayCount { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public string Question { get; set; }
        public string QuestionCode { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string AnswerKey { get; set; }
        public string Image { get; set; }
        public int DetailId { get; set; }
        public bool IsChecked { get; set; }

        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }



    }

    public class Practical_QuestionDetail
    {
        public int Id { get; set; }
        public Nullable<int> Ques_Sequence { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public string QuestionCode { get; set; }
        public string Question { get; set; }
        public string ActionA { get; set; }
        public string ActionB { get; set; }
        public string ActionC { get; set; }
        public string ActionD { get; set; }
        public string ActionE { get; set; }
        public string ActionF { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> Set_Id { get; set; }
        public string ActionA_Image { get; set; }
        public string ActionB_Image { get; set; }
        public string ActionC_Image { get; set; }
        public string ActionD_Image { get; set; }
        public string ActionE_Image { get; set; }
        public string ActionF_Image { get; set; }
        public string Question_Image { get; set; }
        public Nullable<int> Marks_A { get; set; }
        public Nullable<int> Marks_B { get; set; }
        public Nullable<int> Marks_C { get; set; }
        public Nullable<int> Marks_D { get; set; }
        public Nullable<int> Marks_E { get; set; }
        public Nullable<int> Marks_F { get; set; }
        public string QuestionCatagory { get; set; }
    }

    public class Practical_Question_OtherLanguage
    {
        public int Id { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public string QuestionCode { get; set; }
        public string Question { get; set; }
        public string ActionA { get; set; }
        public string ActionB { get; set; }
        public string ActionC { get; set; }
        public string ActionD { get; set; }
        public string ActionE { get; set; }
        public string ActionF { get; set; }
        public string OtherLanguageQuestion { get; set; }
        public string OtherLanguageActionA { get; set; }
        public string OtherLanguageActionB { get; set; }
        public string OtherLanguageActionC { get; set; }
        public string OtherLanguageActionD { get; set; }
        public string OtherLanguageActionE   { get; set; }
        public string OtherLanguageActionF { get; set; }
    }
    public  class GetLanguage_Model
    {
        public int ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> LanguageMaster_Id { get; set; }
        public string Language { get; set; }
    }


    public class QuestionVariable
    {
        public int  id { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public int LangId { get; set; }
        public string MSPIN { get; set; }
        public string SessionID { get; set; }
        public Nullable<int> Day { get; set; }
        public int UserId { get; set; }
        public int ProgramId { get; set; }
        public string TypeOfTest { get; set; }
        public int ProgramType_Id { get; set; }
        public int Set_Id { get; set; }
        public int TimetoExtend { get; set; }
        public int EvaluationType_Id { get; set; }
        public Nullable<int> Position_Id { get; set; }
        public Nullable<int> ExtendedTime { get; set; }
    }

}
