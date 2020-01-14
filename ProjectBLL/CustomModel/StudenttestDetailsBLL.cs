using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class StudenttestDetailsBLL
    {
        public int User_Id { get; set; }
        public int Nomination_Id { get; set; }
        public string Co_id { get; set; }
        public string AgencyCode { get; set; }
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public string DealerName { get; set; }
        public string ProgramCode { get; set; }
        public string SessionID { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public string MSPIN { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string MobileNo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string ProgramName { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> DayCount { get; set; }
        public Nullable<bool> TestInitiated { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public string TypeOfTest { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<int> Agency_Id { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public int Status_Id { get; set; }
        public Nullable<System.DateTime> TestInitiatedDate { get; set; }
        public string TestLoginCode { get; set; }
        public Nullable<int> ProgramType_Id { get; set; }
        public Nullable<int> EvaluationTypeId { get; set; }
        public Nullable<int> Position_Id { get; set; }
        public Nullable<int> ExtendedTime { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class StudentTestDetails
    {
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<bool> TestInitiated { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public string TypeOfTest { get; set; }
        public int DetailId { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> ExtendedTime { get; set; }
        public Nullable<int> Position_Id { get; set; }
        public string MSPIN { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> DayCount { get; set; }
        public string SessionID { get; set; }
        public int Status_Id { get; set; }
        public int RemainingTime { get; set; }
    }
    public class StudentLanguageQuestion
    {
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<bool> TestInitiated { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public string TypeOfTest { get; set; }
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
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string QuestionLanguage { get; set; }
        public string LanguageAnswer1 { get; set; }
        public string LanguageAnswer2 { get; set; }
        public string LanguageAnswer3 { get; set; }
        public string LanguageAnswer4 { get; set; }
        public string AnswerGiven { get; set; }
        public string MSPIN { get; set; }
        public Nullable<int> Day { get; set; }
        public string SessionID { get; set; }        
    }
    public class StudentLanguageQuestion_Practical
    {
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public Nullable<bool> TestInitiated { get; set; }
        public Nullable<int> TotalNoQuestion { get; set; }
        public string TypeOfTest { get; set; }
        public int Id { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> Ques_Sequence { get; set; }
        public string QuestionCode { get; set; }
        public Nullable<int> Set_Id { get; set; }
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
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        //public string LanguageQuestion { get; set; }
        //public string LanguageActionA { get; set; }
        //public string LanguageActionB { get; set; }
        //public string LanguageActionC { get; set; }
        //public string LanguageActionD { get; set; }
        //public string LanguageActionE { get; set; }
        public Nullable<bool> SA_ActionA { get; set; }
        public Nullable<bool> SA_ActionB { get; set; }
        public Nullable<bool> SA_ActionC { get; set; }
        public Nullable<bool> SA_ActionD { get; set; }
        public Nullable<bool> SA_ActionE { get; set; }
        public Nullable<bool> SA_ActionF { get; set; }
        public string ActionTaken { get; set; }
        public string MSPIN { get; set; }
        public long SA_Id { get; set; }
        public string SessionID { get; set; }
        public Nullable<double> RemainingTime { get; set; }
        public int Status_Id { get; set; }
        public Nullable<int> Day { get; set; }
        public string QuestionCategory { get; set; }
    }
    public class StudentTestResponse
    {
        public StudentTestDetails StudentTestDetails { get; set; }
        public List<StudentLanguageQuestion> StudentLanguageQuestion { get; set; }
    }
    public class StudentTestResponse_Practical
    {
        public StudentTestDetails StudentTestDetails { get; set; }
        public List<StudentLanguageQuestion_Practical> StudentLanguageQuestion { get; set; }
    }
    public class StudentTestResponse_PracticalV2
    {
        public StudentTestDetails StudentTestDetails { get; set; }
        public List<StudentCategory> StudentLanguageQuestion { get; set; }
    }
    public class StudentCategory
    {
        public string Catagory { get; set; }
        public List<StudentLanguageQuestion_Practical> StudentLanguageQuestion { get; set; }
    }
    public class StudentTestResponse2
    {
        public StudentTestDetails StudentTestDetails { get; set; }
        public StudentLanguageQuestion StudentLanguageQuestion { get; set; }
    }
    public class StudentTestQuestions_Mobile
    {
        public List<StudentLanguageQuestion> StudentLanguageQuestion { get; set; }
    }
}
