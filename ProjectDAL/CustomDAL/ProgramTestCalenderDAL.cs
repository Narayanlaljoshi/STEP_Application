using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ProjectBLL.CustomModel;
using STEPDAL.DB;
using ProjectDAL.CustomDAL;
using System.Data;

namespace ProjectDAL.CustomDAL
{
    public  class ProgramTestCalenderDAL
    {
        public static List<ProgramMaster> GetProgramList()
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.TblProgramMasters.Where(x=>x.IsActive==true).ToList();

                List<ProgramMaster> objList = null;
                objList = data.Select(s => new ProgramMaster
                {
                    ProgramId =s.ProgramId,
                    ProgramCode= s.ProgramCode +"-"+s.ProgramName,
                    

                }).ToList();
                return objList;
            }
        }


        public static List<ProgramTestCalenderDetail_Model> GetProgramTestCalenderList(int ProgramId)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.Sp_ProgramTestCalenderDetail(ProgramId).ToList();                
                List<ProgramTestCalenderDetail_Model> objList = new List<ProgramTestCalenderDetail_Model>();
                foreach (var s in data) {
                    int? QuesCount = 0;
                    var QuesDtl = context.sp_GetQuestionCountAgainstProgramTestCalender(s.ProgramTestCalenderId).FirstOrDefault();
                    if (QuesDtl != null) { QuesCount = QuesDtl.QuestionCount; }
                    objList.Add(new ProgramTestCalenderDetail_Model {
                        ProgramId = s.ProgramId,
                        ProgramCode = s.ProgramCode,
                        DayCount = s.DayCount,
                        ProgramName = s.ProgramName,
                        EvaluationTypeId=s.EvaluationTypeId,
                        TypeOfTest = s.TypeOfTest,
                        ProgramTestCalenderId = s.ProgramTestCalenderId,
                        TotalNoQuestion = s.TotalNoQuestion,
                        TestDuration = s.TestDuration,
                        Marks_Question = s.Marks_Question,
                        Q_Bank = s.Q_Bank,
                        TestCode = s.TestCode,
                        Total_Marks = s.Total_Marks,
                        ValidDuration = s.ValidDuration,
                        QuesAdded= QuesCount,
                        PracticalDefaultMarks=s.PracticalDefaultMarks,
                        PracticalMaxMarks=s.PracticalMaxMarks,
                        PracticalMinMarks=s.PracticalMinMarks,
                    });
                    objList.OrderBy(x => x.DayCount);
                }
                return objList;
            }
        }






        public static string AddProgramTestCalender(List<ProgramTestCalenderBLL> List)
        {

            using (var context = new CEIDBEntities())
            {
                foreach (var obj in List)
                {
                    var prop = context.TblProgramTestCalenders.Where(s => s.ProgramId == obj.ProgramId && s.DayCount == obj.DayCount && s.IsActive == true).FirstOrDefault();
                    if (prop == null)
                    {
                        TblProgramTestCalender _ProgramTestCalender = new TblProgramTestCalender
                        {
                            DayCount = obj.DayCount,
                            EvaluationTypeId=obj.EvaluationTypeId,
                            TypeOfTest = obj.TypeOfTest,
                            ProgramId = obj.ProgramId,
                            TestDuration = obj.TestDuration,
                            TotalNoQuestion = obj.TotalNoQuestion,
                            ProgramTestCalenderId = obj.ProgramTestCalenderId,
                            Marks_Question = obj.Marks_Question,
                            Q_Bank = obj.Q_Bank,
                            TestCode = obj.TestCode,
                            Total_Marks = obj.TotalNoQuestion * obj.Marks_Question==null?0: obj.Marks_Question,
                            TestInitiated = false,
                            ValidDuration = obj.ValidDuration,
                            PracticalMinMarks=obj.PracticalMinMarks,
                            PracticalMaxMarks=obj.PracticalMaxMarks,
                            PracticalDefaultMarks=obj.PracticalDefaultMarks,
                            IsActive = obj.IsActive,
                            CreatedBy = 1,
                            CreationDate = DateTime.Now,          //LocalTimeProgramTestCalender.GetLocalDate(),
                            ModifiedBy = null,
                            ModifiedDate = null
                        };

                        context.Entry(_ProgramTestCalender).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();
                        
                    }
                    else
                    {
                        prop.DayCount = obj.DayCount;
                        prop.TypeOfTest = obj.TypeOfTest;
                        prop.ProgramId = obj.ProgramId;
                        prop.TestDuration = obj.TestDuration;
                        prop.TotalNoQuestion = obj.TotalNoQuestion;
                        prop.EvaluationTypeId= obj.EvaluationTypeId;
                        prop.Marks_Question = obj.Marks_Question;
                        prop.Q_Bank = obj.Q_Bank;
                        prop.TestCode = obj.TestCode;
                        prop.Total_Marks = obj.Marks_Question!=null? obj.TotalNoQuestion * obj.Marks_Question:0;
                        prop.PracticalDefaultMarks = obj.PracticalDefaultMarks;
                        prop.PracticalMaxMarks = obj.PracticalMaxMarks;
                        prop.PracticalMinMarks = obj.PracticalMinMarks;
                        prop.TestInitiated = false;
                        prop.ValidDuration = obj.ValidDuration;

                        //prop.IsActive = obj.IsActive;

                        prop.ModifiedBy = 1;
                        prop.ModifiedDate = DateTime.Now;          //LocalTimeProgramTestCalender.GetLocalDate(),
                        context.Entry(prop).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return "Success: Tests created successfully";
            }

        }



        public static string UpdateProgramTestCalender(ProgramTestCalenderBLL info)
        {
            using (var context = new CEIDBEntities())
            {
                var dup = context.TblProgramTestCalenders.Where(x => x.ProgramId == info.ProgramId && x.TypeOfTest == info.TypeOfTest && x.DayCount == info.DayCount && x.IsActive == true && x.ProgramTestCalenderId != info.ProgramTestCalenderId).FirstOrDefault();

                if (dup == null)
                {
                    var Details = context.TblProgramTestCalenders.Where(x => x.ProgramTestCalenderId == info.ProgramTestCalenderId
                    ).FirstOrDefault();
                    if (Details != null)
                    {
                        Details.ProgramId = info.ProgramId;
                        Details.DayCount = info.DayCount;
                        Details.TypeOfTest = info.TypeOfTest;
                        Details.TotalNoQuestion = info.TotalNoQuestion;
                        Details.TestDuration = info.TestDuration;
                        Details.Q_Bank = info.Q_Bank;
                        Details.TestCode = info.TestCode;
                        Details.Marks_Question = info.Marks_Question;
                        Details.Total_Marks = info.TotalNoQuestion * info.Marks_Question;
                        Details.ValidDuration = info.ValidDuration;
                        Details.TestInitiated = false;
                        Details.IsActive = info.IsActive;
                        Details.ModifiedBy = 1;
                        Details.ModifiedDate = DateTime.Now;         // LocalTimeDealerGroup.GetLocalDate();
                    }

                    context.Entry(Details).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return "Success: ProgramTestCalender updated successfully";
                }
                else
                {
                    dup.IsActive = info.IsActive;
                    context.Entry(dup).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return "Success: ProgramTestCalender  updated successfully!";
                }
            }
        }



        public static string UploadList(DataTable dt)
        {
            using (var context = new CEIDBEntities())
            {
                string ProgramCode = "";
                Nullable<int> DayCount = null;
                string TestCode = "";

                int Marks_Question;
                //int Question_Bank;
                int TestDuration;
                string TypeOfTest;
                int TotalNoQuestion;
                int ValidDuration;
                //string Msg="";

                string Msg = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr["Program Code"].Equals(string.Empty) && !dr["Day"].Equals(string.Empty) && !dr["Type Of Test"].Equals(string.Empty) && !dr["Total No Question"].Equals(string.Empty) && !dr["Test Duration"].Equals(string.Empty)) // && !dr["RegistrationNo"].Equals(string.Empty)))
                    {
                        ProgramCode = dr["Program Code"].ToString();
                        TestCode = dr["Test Code"].ToString();
                        Marks_Question = Convert.ToInt32(dr["Marks/Question"]);


                  //      Question_Bank = Convert.ToInt32(dr["Question Bank"]);

                        DayCount = Convert.ToInt32(dr["Day"]);
                        TypeOfTest = dr["Type Of Test"].ToString();
                        TotalNoQuestion = Convert.ToInt32(dr["Total No Question"]);
                        TestDuration = Convert.ToInt32(dr["Test Duration"]);
                        ValidDuration = Convert.ToInt32(dr["Valid Duration"]);
                        string type = "";





                        var Request = context.TblProgramMasters.Where(x => x.ProgramCode == ProgramCode).FirstOrDefault();


                        if(TypeOfTest== "Pre Test")
                        {
                            type = "1";
                        }
                        else if (TypeOfTest == "Daily Test")
                        {
                            type = "2";
                        }
                        else if (TypeOfTest == "Post Test")
                        {
                            type = "3";
                        }


                       


                        if (Request != null)
                        {
                            var dt1 = context.TblProgramTestCalenders.Where(x => x.ProgramId == Request.ProgramId && x.DayCount == DayCount && x.TypeOfTest == TypeOfTest && x.IsActive == true).FirstOrDefault();

                            if(dt1 == null)
                            { 
                            TblProgramTestCalender item = new TblProgramTestCalender();

                            item.ProgramId = Request.ProgramId;
                            item.DayCount = DayCount;
                            item.TestDuration = TestDuration;
                            item.TypeOfTest = type;
                            item.TotalNoQuestion = TotalNoQuestion;

                                item.ValidDuration= ValidDuration;

                                //    item.Q_Bank = Question_Bank;
                                item.TestCode = TestCode;
                                item.Marks_Question = Marks_Question;
                                item.Total_Marks = TotalNoQuestion * Marks_Question;
                                item.TestInitiated = false;

                                item.CreationDate = DateTime.Now;
                            item.CreatedBy = 1;
                            item.ModifiedDate = DateTime.Now;
                            item.ModifiedBy = null;
                            item.IsActive = true;
                            context.Entry(item).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();

                        }

                        }

                        Msg = "Successfully saved..";
                    }

                    else
                    {
                        Msg = "Invalid columns in excel sheet. Please check template";
                    }
                }
                return Msg;

            }
        }

    }
}
