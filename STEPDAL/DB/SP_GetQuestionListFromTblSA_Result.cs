//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STEPDAL.DB
{
    using System;
    
    public partial class SP_GetQuestionListFromTblSA_Result
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
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string AnswerGiven { get; set; }
        public string QuestionLanguage { get; set; }
        public string LanguageAnswer1 { get; set; }
        public string LanguageAnswer2 { get; set; }
        public string LanguageAnswer3 { get; set; }
        public string LanguageAnswer4 { get; set; }
        public Nullable<bool> EnableJumbling { get; set; }
        public Nullable<int> Set_Id { get; set; }
    }
}
