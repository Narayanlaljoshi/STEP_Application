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
    using System.Collections.Generic;
    
    public partial class TblQuestionLanguageDetail_Practical
    {
        public int Id { get; set; }
        public int Detail_Id { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public string QuestionCatagory { get; set; }
        public string QuestionCode { get; set; }
        public Nullable<int> LanguageMaster_Id { get; set; }
        public Nullable<int> Set_Id { get; set; }
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
    }
}
