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
    
    public partial class TblProgramTestCalenderDetail_Practical
    {
        public int Id { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> Ques_Sequence { get; set; }
        public string QuestionCatagory { get; set; }
        public string QuestionCode { get; set; }
        public Nullable<int> Set_Id { get; set; }
        public Nullable<int> TestDuration { get; set; }
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
        public string Set_Title { get; set; }
    }
}
