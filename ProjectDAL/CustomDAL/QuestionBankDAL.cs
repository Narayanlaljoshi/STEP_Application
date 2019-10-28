using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ProjectBLL.CustomModel;
using STEPDAL.DB;
using ProjectDAL.CustomDAL;
using System.Data;
using System.Data.SqlClient;

namespace ProjectDAL.CustomDAL
{
    public class QuestionBankDAL
    {
        public static string UploadList(DataTable dt, ProgramTestModel obj)
        {
            using (var context = new CEIDBEntities())
            {
                //string QuestionCode = "";
                string Question = "";
                string Answer1 = "";
                string Answer2 = "";
                string Answer3 = "";
                string Answer4 = "";
                string AnswerKey = "";
                //string Msg="";

                string Msg = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr["Question"].Equals(string.Empty) && !dr["AnswerKey"].Equals(string.Empty)) // && !dr["RegistrationNo"].Equals(string.Empty)))
                    {
                        //QuestionCode = dr["Question Code"].ToString();
                        Question = dr["Question"].ToString();
                        Answer1 = dr["a"].ToString();
                        Answer2 = dr["b"].ToString();
                        Answer3 = dr["c"].ToString();
                        Answer4 = dr["d"].ToString();
                        AnswerKey = dr["AnswerKey"].ToString();



                        var dt1 = context.TblProgramTestCalenderDetails.Where(x => x.ProgramTestCalenderId == obj.ProgramTestCalenderId && x.Question == Question && x.IsActive == true).FirstOrDefault();



                        TblProgramTestCalenderDetail item = new TblProgramTestCalenderDetail();

                        item.ProgramTestCalenderId = obj.ProgramTestCalenderId;
                        item.Question = Question;
                        // item.QuestionCode = QuestionCode;
                        item.Answer1 = Answer1;      //"(a)." +
                        item.Answer2 = Answer2;           //"(b)." +
                        item.Answer3 = Answer3;
                        item.Answer4 = Answer4;

                        item.AnswerKey = AnswerKey;


                        item.CreationDate = DateTime.Now;
                        item.CreatedBy = 1;
                        item.ModifiedDate = DateTime.Now;
                        item.ModifiedBy = null;
                        item.IsActive = true;
                        context.Entry(item).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();

                        item.QuestionCode = "QUE-" + item.DetailId;
                        context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();





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
        public static string UploadList_Practical(DataTable dt, ProgramTestModel obj)
        {
            string ReturnMessage = string.Empty;
            List<ProgramTestCalenderDetail_Practical> List = new List<ProgramTestCalenderDetail_Practical>();
            
            using (var context = new CEIDBEntities())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int rowCount = 0;
                    string QuestionCatagory = string.Empty;
                    string Question = string.Empty;
                    string ActionA = string.Empty;
                    string ActionB = string.Empty;
                    string ActionC = string.Empty;
                    string ActionD = string.Empty;
                    string ActionE = string.Empty;
                    string ActionF = string.Empty;
                    int? MarksActionA = null;
                    int? MarksActionB = null;
                    int? MarksActionC = null;
                    int? MarksActionD = null;
                    int? MarksActionE = null;
                    int? MarksActionF = null;
                    bool IsValidEntry = true;
                    try
                    {
                        QuestionCatagory = dr["Question Category"].ToString();
                        Question = dr["Question"].ToString();
                        ActionA = dr["Action A"].ToString();
                        ActionB = dr["Action B"].ToString();
                        ActionC = dr["Action C"].ToString();
                        ActionD = dr["Action D"].ToString();
                        ActionE = dr["Action E"].ToString();
                        ActionF = dr["Action F"].ToString();
                    }
                    catch (Exception ex) { }
                    int TempMarksActionA;
                    int TempMarksActionB;
                    int TempMarksActionC;
                    int TempMarksActionD;
                    int TempMarksActionE;
                    int TempMarksActionF;
                    try
                    {
                        if (Int32.TryParse(dr["A"].ToString(), out TempMarksActionA))
                        {
                            MarksActionA = TempMarksActionA;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Int32.TryParse(dr["B"].ToString(), out TempMarksActionB))
                        {
                            MarksActionB = TempMarksActionB;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Int32.TryParse(dr["C"].ToString(), out TempMarksActionC))
                        {
                            MarksActionC = TempMarksActionC;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Int32.TryParse(dr["D"].ToString(), out TempMarksActionD))
                        {
                            MarksActionD = TempMarksActionD;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Int32.TryParse(dr["E"].ToString(), out TempMarksActionE))
                        {
                            MarksActionE = TempMarksActionE;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Int32.TryParse(dr["F"].ToString(), out TempMarksActionF))
                        {
                            MarksActionF = TempMarksActionF;
                        }
                    }
                    catch (Exception ex) { }

                    if (Question.Equals(string.Empty))
                    {
                        IsValidEntry = false;
                    }
                    if ((ActionA.Equals(string.Empty) && MarksActionA != null) || (!ActionA.Equals(string.Empty) && MarksActionA == null))
                    {
                        IsValidEntry = false;
                    }
                    if ((ActionB.Equals(string.Empty) && MarksActionB != null) || (!ActionB.Equals(string.Empty) && MarksActionB == null))
                    {
                        IsValidEntry = false;
                    }
                    if ((ActionC.Equals(string.Empty) && MarksActionC != null) || (!ActionC.Equals(string.Empty) && MarksActionC == null))
                    {
                        IsValidEntry = false;
                    }
                    if ((ActionD.Equals(string.Empty) && MarksActionD != null) || (!ActionD.Equals(string.Empty) && MarksActionD == null))
                    {
                        IsValidEntry = false;
                    }
                    if ((ActionE.Equals(string.Empty) && MarksActionE != null) || (!ActionE.Equals(string.Empty) && MarksActionE == null))
                    {
                        IsValidEntry = false;
                    }
                    if ((ActionF.Equals(string.Empty) && MarksActionF != null) || (!ActionF.Equals(string.Empty) && MarksActionF == null))
                    {
                        IsValidEntry = false;
                    }

                    //var Check = context.TblProgramTestCalenderDetail_Practical.Where(x => x.ProgramTestCalenderId == obj.ProgramTestCalenderId && x.Set_Id == obj.Set_Id && x.Question == Question && x.ActionA == ActionA && x.ActionB == ActionB && x.ActionC == ActionC && x.ActionD == ActionD && x.ActionE == ActionE && x.IsActive == true).FirstOrDefault();
                    //if (Check == null)
                    //{
                        List.Add(new ProgramTestCalenderDetail_Practical
                        {
                            ProgramTestCalenderId = obj.ProgramTestCalenderId,
                            QuestionCatagory=QuestionCatagory,
                            Question = Question,
                            CreationDate = DateTime.Now,
                            CreatedBy = 1,
                            IsActive = true,
                            ActionA = ActionA,
                            ActionB = ActionB,
                            ActionC = ActionC,
                            ActionD = ActionD,
                            ActionE = ActionE,
                            ActionF = ActionF,
                            Set_Id = obj.Set_Id,
                            Marks_A = MarksActionA,
                            Marks_B = MarksActionB,
                            Marks_C = MarksActionC,
                            Marks_D = MarksActionD,
                            Marks_E = MarksActionE,
                            Marks_F = MarksActionF,
                            IsValidEntry = IsValidEntry
                        });
                        
                    //}
                }
                int index = List.FindIndex(a => a.IsValidEntry == false);
                if (index == -1)
                {
                    foreach (var row in List)
                    {
                        var Check = context.TblProgramTestCalenderDetail_Practical.Where(x => x.ProgramTestCalenderId == obj.ProgramTestCalenderId && x.Set_Id == obj.Set_Id && x.QuestionCatagory == row.QuestionCatagory && x.Question == row.Question&& x.ActionA == row.ActionA && x.ActionB == row.ActionB && x.ActionC == row.ActionC && x.ActionD == row.ActionD && x.ActionE == row.ActionE && x.ActionF == row.ActionF && x.IsActive == true).FirstOrDefault();
                        if (Check != null)
                        {
                            Check.QuestionCatagory = row.QuestionCatagory;
                            Check.ProgramTestCalenderId = obj.ProgramTestCalenderId;
                            Check.Question = row.Question;
                            Check.CreationDate = DateTime.Now;
                            Check.CreatedBy = 1;
                            Check.ModifiedDate = DateTime.Now;
                            Check.ModifiedBy = null;
                            Check.IsActive = true;
                            Check.ActionA = row.ActionA;
                            Check.ActionB = row.ActionB;
                            Check.ActionC = row.ActionC;
                            Check.ActionD = row.ActionD;
                            Check.ActionE = row.ActionE;
                            Check.ActionF = row.ActionF;
                            Check.Set_Id = obj.Set_Id;
                            Check.Marks_A = row.Marks_A;
                            Check.Marks_B = row.Marks_B;
                            Check.Marks_C = row.Marks_C;
                            Check.Marks_D = row.Marks_D;
                            Check.Marks_E = row.Marks_E;
                            Check.Marks_F = row.Marks_F;

                            context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            ReturnMessage = "Success: Data Uploaded Successfully";
                        }
                        else
                        {
                            TblProgramTestCalenderDetail_Practical item = new TblProgramTestCalenderDetail_Practical
                            {
                                ProgramTestCalenderId = obj.ProgramTestCalenderId,
                                QuestionCatagory=row.QuestionCatagory,
                                Question = row.Question,
                                CreationDate = DateTime.Now,
                                CreatedBy = 1,
                                ModifiedDate = DateTime.Now,
                                ModifiedBy = null,
                                IsActive = true,
                                ActionA = row.ActionA,
                                ActionB = row.ActionB,
                                ActionC = row.ActionC,
                                ActionD = row.ActionD,
                                ActionE = row.ActionE,
                                ActionF = row.ActionF,
                                Set_Id = obj.Set_Id,
                                Marks_A = row.Marks_A,
                                Marks_B = row.Marks_B,
                                Marks_C = row.Marks_C,
                                Marks_D = row.Marks_D,
                                Marks_E = row.Marks_E,
                                Marks_F = row.Marks_F,
                            };

                            context.Entry(item).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();

                            item.QuestionCode = "QUE_P-" + item.Id;
                            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            ReturnMessage = "Success: Data Uploaded Successfully";
                        }
                    }
                }
                else
                {
                    ReturnMessage = "Error: Invalid Data Entry at Row: " + (index+3);
                }
                
            }
           
            return ReturnMessage;
            #region
            //using (var context = new CEIDBEntities())
            //{
            //    var Check = context.TblProgramTestCalenderDetail_Practical.Where(x => x.ProgramTestCalenderId == obj.ProgramTestCalenderId && x.Set_Id == obj.Set_Id && x.Question == Question && x.ActionA == ActionA && x.ActionB == ActionB && x.ActionC == ActionC && x.ActionD == ActionD && x.ActionE == ActionE && x.IsActive == true).FirstOrDefault();
            //    //string QuestionCode = "";
            //    string Question = string.Empty;
            //    string ActionA = string.Empty;
            //    string ActionB = string.Empty;
            //    string ActionC = string.Empty;
            //    string ActionD = string.Empty;
            //    string ActionE = string.Empty;
            //    string ActionF = string.Empty;
            //    int? MarksActionA = null;
            //    int? MarksActionB = null;
            //    int? MarksActionC = null;
            //    int? MarksActionD = null;
            //    int? MarksActionE = null;
            //    int? MarksActionF = null;
            //    string Msg = string.Empty;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        bool IsValidEntry = false;
            //        try
            //        {
            //            Question = dr["Question"].ToString();
            //            ActionA = dr["Action A"].ToString();
            //            ActionB = dr["Action B"].ToString();
            //            ActionC = dr["Action C"].ToString();
            //            ActionD = dr["Action D"].ToString();
            //            ActionE = dr["Action E"].ToString();
            //            ActionF = dr["Action F"].ToString();
            //        }
            //        catch (Exception ex) { }
            //        int TempMarksActionA;
            //        int TempMarksActionB;
            //        int TempMarksActionC;
            //        int TempMarksActionD;
            //        int TempMarksActionE;
            //        //int TempMarksActionF;
            //        try
            //        {
            //            if (Int32.TryParse(dr["A"].ToString(), out TempMarksActionA))
            //            {
            //                MarksActionA = TempMarksActionA;
            //            }
            //        }
            //        catch (Exception ex) { }
            //        try
            //        {
            //            if (Int32.TryParse(dr["B"].ToString(), out TempMarksActionB))
            //            {
            //                MarksActionB = TempMarksActionB;
            //            }
            //        }
            //        catch (Exception ex) { }
            //        try
            //        {
            //            if (Int32.TryParse(dr["C"].ToString(), out TempMarksActionC))
            //            {
            //                MarksActionC = TempMarksActionC;
            //            }
            //        }
            //        catch (Exception ex) { }
            //        try
            //        {
            //            if (Int32.TryParse(dr["D"].ToString(), out TempMarksActionD))
            //            {
            //                MarksActionD = TempMarksActionD;
            //            }
            //        }
            //        catch (Exception ex) { }
            //        try
            //        {
            //            if (Int32.TryParse(dr["E"].ToString(), out TempMarksActionE))
            //            {
            //                MarksActionE = TempMarksActionE;
            //            }
            //        }
            //        catch (Exception ex) { }
            //        try
            //        {
            //            if (Int32.TryParse(dr["F"].ToString(), out TempMarksActionA))
            //            {
            //                MarksActionA = TempMarksActionA;
            //            }
            //        }
            //        catch (Exception ex) { }

            //        if (Question.Equals(string.Empty)) { }


            //        if (!Question.Equals(string.Empty) && !ActionA.Equals(string.Empty) && !ActionB.Equals(string.Empty) && !ActionC.Equals(string.Empty) && !ActionD.Equals(string.Empty) && !ActionE.Equals(string.Empty) && MarksActionA != null && MarksActionB != null && MarksActionC != null && MarksActionD != null && MarksActionE != null)
            //        {
            //            var Check = context.TblProgramTestCalenderDetail_Practical.Where(x => x.ProgramTestCalenderId == obj.ProgramTestCalenderId && x.Set_Id == obj.Set_Id && x.Question == Question && x.ActionA == ActionA && x.ActionB == ActionB && x.ActionC == ActionC && x.ActionD == ActionD && x.ActionE == ActionE && x.IsActive == true).FirstOrDefault();
            //            if (Check != null)
            //            {

            //                Check.ProgramTestCalenderId = obj.ProgramTestCalenderId;
            //                Check.Question = Question;
            //                Check.CreationDate = DateTime.Now;
            //                Check.CreatedBy = 1;
            //                Check.ModifiedDate = DateTime.Now;
            //                Check.ModifiedBy = null;
            //                Check.IsActive = true;
            //                Check.ActionA = ActionA;
            //                Check.ActionB = ActionB;
            //                Check.ActionC = ActionC;
            //                Check.ActionD = ActionD;
            //                Check.ActionE = ActionE;
            //                Check.Set_Id = obj.Set_Id;
            //                Check.Marks_A = MarksActionA;
            //                Check.Marks_B = MarksActionB;
            //                Check.Marks_C = MarksActionC;
            //                Check.Marks_D = MarksActionD;
            //                Check.Marks_E = MarksActionE;

            //                context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
            //                context.SaveChanges();
            //            }
            //            else
            //            {
            //                TblProgramTestCalenderDetail_Practical item = new TblProgramTestCalenderDetail_Practical
            //                {
            //                    ProgramTestCalenderId = obj.ProgramTestCalenderId,
            //                    Question = Question,
            //                    CreationDate = DateTime.Now,
            //                    CreatedBy = 1,
            //                    ModifiedDate = DateTime.Now,
            //                    ModifiedBy = null,
            //                    IsActive = true,
            //                    ActionA = ActionA,
            //                    ActionB = ActionB,
            //                    ActionC = ActionC,
            //                    ActionD = ActionD,
            //                    ActionE = ActionE,
            //                    Set_Id = obj.Set_Id,
            //                    Marks_A = MarksActionA,
            //                    Marks_B = MarksActionB,
            //                    Marks_C = MarksActionC,
            //                    Marks_D = MarksActionD,
            //                    Marks_E = MarksActionE,
            //                    //Marks_F = MarksActionA,
            //                };

            //                context.Entry(item).State = System.Data.Entity.EntityState.Added;
            //                context.SaveChanges();

            //                item.QuestionCode = "QUE_P-" + item.Id;
            //                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            //                context.SaveChanges();
            //                Msg = "Successfully saved..";
            //            }
            //        }
            //        else
            //        {
            //            return "Error: Invalid columns in excel sheet. Please check template";
            //        }
            //    }
            //    return Msg;

            //}
            #endregion
        }
        private static string BulkInsertDataTable(DataTable dataTable)
        {
            string isSuccuss;
            string connectionStringTarget = System.Configuration.ConfigurationManager.AppSettings["BulkUploadConnectionString"].ToString();
            //string connectionStringTarget = "data source=.;initial catalog=ProductivityDashboard;integrated security=True;";
            using (SqlConnection SqlConnectionObj = new SqlConnection(connectionStringTarget))
            {
                try
                {

                    SqlConnectionObj.Open();
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlConnectionObj, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                    bulkCopy.DestinationTableName = "[dbo].[TblProgramTestCalenderDetail_Practical]";
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[0].ColumnName, "Person_In_Charge");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[1].ColumnName, "Branch_Name");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[2].ColumnName, "Branch_Code");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[3].ColumnName, "DMS_Id");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[4].ColumnName, "Customer_Name");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[5].ColumnName, "Tel_No");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[6].ColumnName, "Mobile_No");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[7].ColumnName, "Sales_Staff");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[8].ColumnName, "Service_Advisor");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[9].ColumnName, "Katashiki");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[10].ColumnName, "Vin");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[11].ColumnName, "Vin");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[12].ColumnName, "Registration_No");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[13].ColumnName, "Last_Service_In_Date");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[14].ColumnName, "IsActive");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[15].ColumnName, "CreatedBy");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[16].ColumnName, "CreationDate");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[17].ColumnName, "CreationDate");
                    bulkCopy.ColumnMappings.Add(dataTable.Columns[18].ColumnName, "ProgramTestCalenderId");

                    bulkCopy.WriteToServer(dataTable);
                    isSuccuss = "Success: Data Uploaded Successfully";
                }
                catch (Exception ex)
                {
                    isSuccuss = "Error: " + ex.Message.ToString();
                }
                finally
                {
                    SqlConnectionObj.Close();
                    GC.Collect();
                }
            }
            return isSuccuss;
        }
        public static string UploadLanguageList(DataTable dt, QestionLanguageModel obj)
        {
            using (var context = new CEIDBEntities())
            {
                string QuestionCode = "";
                string Question = "";
                string Answer1 = "";
                string Answer2 = "";
                string Answer3 = "";
                string Answer4 = "";
                string AnswerKey = "";



                //string Msg="";

                string Msg = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr["QuestionCode"].Equals(string.Empty) && !dr["QueInOther"].Equals(string.Empty)) // && !dr["RegistrationNo"].Equals(string.Empty)))
                    {
                        QuestionCode = dr["QuestionCode"].ToString();
                        Question = dr["QueInOther"].ToString();//QueInOther=question in other language
                        Answer1 = dr["AnswerA"].ToString();
                        Answer2 = dr["AnswerB"].ToString();
                        Answer3 = dr["AnswerC"].ToString();
                        Answer4 = dr["AnswerD"].ToString();
                        //AnswerKey = dr["AnswerKey"].ToString();



                        var dt1 = context.TblProgramTestCalenderDetails.Where(x => x.QuestionCode == QuestionCode && x.IsActive == true).FirstOrDefault();

                        if (dt1 != null)
                        {
                            var lang = context.TblQuestionLanguageDetails.Where(x => x.LanguageMaster_Id == obj.LanguageMasterId && x.ProgramTestCalenderId == dt1.ProgramTestCalenderId && x.DetailId == dt1.DetailId && x.IsActive == true).FirstOrDefault();

                            if (lang == null)
                            {

                                var DT = context.Sp_InsertQuestionLanguage(dt1.DetailId, dt1.ProgramTestCalenderId, Question, Answer1, Answer2, Answer3, Answer4, AnswerKey, obj.LanguageMasterId, obj.CreatedBy);

                            }
                            else
                            {
                                var Dtl = context.Sp_UpdateQuestionLanguage(dt1.DetailId, dt1.ProgramTestCalenderId, Question, Answer1, Answer2, Answer3, Answer4, AnswerKey, obj.LanguageMasterId, obj.ModifiedBy);
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

        public static List<ProgramTest_QuestionDetail> GetQuestionBankList(int id)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.Sp_Question_Detail(id).ToList();

                List<ProgramTest_QuestionDetail> objList = null;
                objList = data.Select(x => new ProgramTest_QuestionDetail
                {
                    Answer1 = x.Answer1,
                    Answer2 = x.Answer2,
                    Answer3 = x.Answer3,
                    Answer4 = x.Answer4,
                    AnswerKey = x.AnswerKey,
                    DayCount = x.DayCount,
                    Image = x.Image,
                    DetailId = x.DetailId,
                    ProgramId = x.ProgramId,
                    ProgramTestCalenderId = x.ProgramTestCalenderId,
                    Question = x.Question,
                    ProgramCode = x.ProgramCode,
                    ProgramName = x.ProgramName,
                    QuestionCode = x.QuestionCode,
                    TestDuration = x.TestDuration,
                    TotalNoQuestion = x.TotalNoQuestion,
                    TypeOfTest = x.TypeOfTest,
                    IsChecked = false

                }).ToList();
                return objList;

            }

        }
        public static List<Practical_QuestionDetail> GetPracticalQuestionBankList(int id, int Set_Id)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.Sp_PracticalQuestion_Detail(id, Set_Id, 1).ToList();

                List<Practical_QuestionDetail> objList = null;
                objList = data.Select(x => new Practical_QuestionDetail
                {
                    QuestionCatagory=x.QuestionCatagory,
                    Question_Image = x.Question_Image,
                    Question = x.Question,
                    ActionA = x.ActionA,
                    ActionB = x.ActionB,
                    ActionC = x.ActionC,
                    ActionD = x.ActionD,
                    ActionE = x.ActionE,
                    ActionF = x.ActionF,
                    ActionA_Image = x.ActionA_Image,
                    Marks_A = x.Marks_A,
                    ActionB_Image = x.ActionB_Image,
                    ActionC_Image = x.ActionC_Image,
                    ActionD_Image = x.ActionD_Image,
                    ActionE_Image = x.ActionE_Image,
                    ActionF_Image = x.ActionF_Image,
                    CreatedBy = x.CreatedBy,
                    CreationDate = x.CreationDate,
                    Marks_B = x.Marks_B,
                    Marks_D = x.Marks_D,
                    Marks_F = x.Marks_F,
                    Marks_C = x.Marks_C,
                    Marks_E = x.Marks_E,
                    Set_Id = x.Set_Id,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    ProgramTestCalenderId = x.ProgramTestCalenderId,
                    QuestionCode = x.QuestionCode

                }).ToList();
                return objList;

            }

        }

        public static List<Practical_QuestionDetail> GetPracticalQuestionBankList(FilterBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.Sp_PracticalQuestion_Detail(Obj.ProgramTestCalenderId, Obj.Set_Id, Obj.LanguageMaster_Id == null ? 1 : Obj.LanguageMaster_Id).ToList();

                List<Practical_QuestionDetail> objList = null;
                objList = data.Select(x => new Practical_QuestionDetail
                {
                    QuestionCatagory=x.QuestionCatagory,
                    Question_Image = x.Question_Image,
                    Question = x.Question,
                    ActionA = x.ActionA,
                    ActionB = x.ActionB,
                    ActionC = x.ActionC,
                    ActionD = x.ActionD,
                    ActionE = x.ActionE,
                    ActionF = x.ActionF,
                    ActionA_Image = x.ActionA_Image,
                    Marks_A = x.Marks_A,
                    ActionB_Image = x.ActionB_Image,
                    ActionC_Image = x.ActionC_Image,
                    ActionD_Image = x.ActionD_Image,
                    ActionE_Image = x.ActionE_Image,
                    ActionF_Image = x.ActionF_Image,
                    CreatedBy = x.CreatedBy,
                    CreationDate = x.CreationDate,
                    Marks_B = x.Marks_B,
                    Marks_D = x.Marks_D,
                    Marks_F = x.Marks_F,
                    Marks_C = x.Marks_C,
                    Marks_E = x.Marks_E,
                    Set_Id = x.Set_Id,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    ProgramTestCalenderId = x.ProgramTestCalenderId,
                    QuestionCode = x.QuestionCode

                }).ToList();
                return objList;

            }

        }
        public static List<Practical_Question_OtherLanguage> GetPracticalQuestionTemplate(FilterBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.Sp_PracticalQuestion_Detail(Obj.ProgramTestCalenderId, Obj.Set_Id, Obj.LanguageMaster_Id == null ? 1 : Obj.LanguageMaster_Id).ToList();

                List<Practical_Question_OtherLanguage> objList = null;
                objList = data.Select(x => new Practical_Question_OtherLanguage
                {
                    Question = x.Question,
                    ActionA = x.ActionA,
                    ActionB = x.ActionB,
                    ActionC = x.ActionC,
                    ActionD = x.ActionD,
                    ActionE = x.ActionE,
                    ActionF = x.ActionF,
                    OtherLanguageActionA = "",
                    OtherLanguageActionB = "",
                    OtherLanguageActionC = "",
                    OtherLanguageActionD = "",
                    OtherLanguageActionE = "",
                    OtherLanguageActionF = "",
                    OtherLanguageQuestion = "",
                    Id = x.Id,
                    ProgramTestCalenderId = x.ProgramTestCalenderId,
                    QuestionCode = x.QuestionCode

                }).ToList();
                return objList;

            }

        }
        public static List<ProgramTest_QuestionDetail> GetQuestionFormatedList(QuestionVariable data)
        {
            using (var context = new CEIDBEntities())
            {
                var dtl = context.Sp_GetLanguageWiseQuestionList(data.id, data.LangId).ToList();

                List<ProgramTest_QuestionDetail> objList = null;
                objList = dtl.Select(x => new ProgramTest_QuestionDetail
                {
                    Answer1 = x.Answer1,
                    Answer2 = x.Answer2,
                    Answer3 = x.Answer3,
                    Answer4 = x.Answer4,
                    AnswerKey = x.AnswerKey,
                    DayCount = x.DayCount,
                    Image = x.Image,
                    DetailId = x.DetailId,
                    ProgramId = x.ProgramId,
                    ProgramTestCalenderId = x.ProgramTestCalenderId,
                    Question = x.Question,
                    ProgramCode = x.ProgramCode,
                    ProgramName = x.ProgramName,
                    QuestionCode = x.QuestionCode,
                    TestDuration = x.TestDuration,
                    TotalNoQuestion = x.TotalNoQuestion,
                    TypeOfTest = x.TypeOfTest,


                }).ToList();
                return objList;

            }

        }

        public static List<GetLanguage_Model> GetLanguage(int id)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.Sp_GetLanguage(id).ToList();

                List<GetLanguage_Model> objList = null;
                objList = data.Select(x => new GetLanguage_Model
                {

                    ProgramCode = x.ProgramCode,
                    ProgramName = x.ProgramName,
                    Language = x.Language,
                    LanguageMaster_Id = x.LanguageMaster_Id,
                    ProgramId = x.ProgramId

                }).ToList();
                return objList;

            }

        }

        public static List<GetLanguage_Model> GetLanguageWithoutEnglish(int id)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.Sp_GetLanguage(id).ToList();

                List<GetLanguage_Model> objList = null;
                objList = data.Select(x => new GetLanguage_Model
                {

                    ProgramCode = x.ProgramCode,
                    ProgramName = x.ProgramName,
                    Language = x.Language,
                    LanguageMaster_Id = x.LanguageMaster_Id,
                    ProgramId = x.ProgramId

                }).ToList();
                return objList.Where(x => x.Language != "English").ToList();

            }

        }

        public static string SaveQuestion(QestionDetail_Model data)
        {

            using (var context = new CEIDBEntities())
            {
                //var dtl = context.TblProgramTestCalenderDetails.Where(x => x.Question == data.Question && x.ProgramTestCalenderId==data.ProgramTestCalenderId && x.IsActive==true).FirstOrDefault();
                var dtl = context.sp_CheckQuestionDuplicacy(data.ProgramTestCalenderId, data.Question, data.Answer1, data.Answer2, data.Answer3, data.Answer4).FirstOrDefault();
                if (dtl == null)
                {
                    TblProgramTestCalenderDetail dup = new TblProgramTestCalenderDetail()
                    {
                        Question = data.Question,
                        // QuestionCode = "Que",
                        Answer1 = data.Answer1,
                        Answer2 = data.Answer2,
                        Answer3 = data.Answer3,
                        Answer4 = data.Answer4,
                        AnswerKey = data.AnswerKey,
                        Image = data.Image,
                        ProgramTestCalenderId = data.ProgramTestCalenderId,
                        IsActive = true,
                        CreatedBy = data.CreatedBy,
                        CreationDate = DateTime.Now,

                    };
                    context.Entry(dup).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();


                    dup.QuestionCode = "QUE-" + dup.DetailId;
                    context.Entry(dup).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();




                    return "Sucess: Saved Sucessfully";
                }
                else
                {
                    var dtll = context.TblProgramTestCalenderDetails.Where(x => x.Question == data.Question && x.ProgramTestCalenderId == data.ProgramTestCalenderId && x.QuestionCode == data.QuestionCode && x.IsActive == true).FirstOrDefault();
                    dtll.Image = data.Image;
                    dtll.ModifiedBy = data.ModifiedBy;
                    dtll.ModifiedDate = DateTime.Now;
                    context.Entry(dtll).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();



                    return "Sucess: Image Updated";
                }
            }
        }

        public static string UpdateImage_Parctical(Practical_QuestionDetail Obj)
        {

            using (var context = new CEIDBEntities())
            {
                var dtl = context.TblProgramTestCalenderDetail_Practical.Where(x => x.Id == Obj.Id).FirstOrDefault();
                if (dtl == null)
                {
                    //TblProgramTestCalenderDetail dup = new TblProgramTestCalenderDetail()
                    //{

                    //};
                    //context.Entry(dup).State = System.Data.Entity.EntityState.Added;
                    //context.SaveChanges();

                    //dup.QuestionCode = "QUE-" + dup.DetailId;
                    //context.Entry(dup).State = System.Data.Entity.EntityState.Modified;
                    //context.SaveChanges();

                    return "Sucess: Saved Sucessfully";
                }
                else
                {
                    dtl.Question_Image = Obj.Question_Image;
                    dtl.ActionA_Image = Obj.ActionA_Image;
                    dtl.ActionB_Image = Obj.ActionB_Image;
                    dtl.ActionC_Image = Obj.ActionC_Image;
                    dtl.ActionD_Image = Obj.ActionD_Image;
                    dtl.ActionE_Image = Obj.ActionE_Image;
                    //dtl.ActionA_Image = Obj.ActionA_Image;
                    context.Entry(dtl).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    return "Sucess: Image Updated";
                }
            }
        }

        public static string DeleteQuestion(List<int> Ids)
        {
            using (var context = new CEIDBEntities())
            {
                foreach (var id in Ids)
                {
                    var data = context.TblProgramTestCalenderDetails.Where(x => x.DetailId == id && x.IsActive == true).FirstOrDefault();

                    if (data != null)
                    {
                        data.IsActive = false;


                        context.Entry(data).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return "Sucess : sucessfully updated";
            }

        }

        public static List<SetSequenceBll> GetSetSequenceList()
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.TblSetMasters.Where(x => x.IsActive == true).ToList();

                List<SetSequenceBll> objList = null;
                objList = data.Select(x => new SetSequenceBll
                {
                    Set_Id = x.Set_Id,
                    SetNumber = x.SetNumber

                }).ToList();
                return objList;
            }
        }

        public static string UploadLanguageWiseQuestion_Practical(DataTable dt, QestionLanguagePracticalModel obj)
        {
            using (var context = new CEIDBEntities())
            {
                string QuestionCode = string.Empty;
                string Question = string.Empty;
                string ActionA = string.Empty;
                string ActionB = string.Empty;
                string ActionC = string.Empty;
                string ActionD = string.Empty;
                string ActionE = string.Empty;
                string ActionF = string.Empty;
                string Msg = string.Empty;
                int RowNumber = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    RowNumber++;
                    QuestionCode = dr["QuestionCode"].ToString();
                    Question = dr["OtherLanguageQuestion"].ToString();
                    ActionA = dr["OtherLanguageActionA"].ToString();
                    ActionB = dr["OtherLanguageActionB"].ToString();
                    ActionC = dr["OtherLanguageActionC"].ToString();
                    ActionD = dr["OtherLanguageActionD"].ToString();
                    ActionE = dr["OtherLanguageActionE"].ToString();

                    if (!QuestionCode.Equals(string.Empty) && !Question.Equals(string.Empty))
                    {
                        var Check = context.TblProgramTestCalenderDetail_Practical.Where(x => x.QuestionCode == QuestionCode && x.IsActive == true).FirstOrDefault();
                        if (Check != null)
                        {
                            var QuesCheck = context.TblQuestionLanguageDetail_Practical.Where(x => x.Detail_Id == Check.Id && x.IsActive == true).FirstOrDefault();

                            if (QuesCheck != null)
                            {
                                int Status = context.Sp_UpdateQuestionLanguage_Practical(Check.Id, Check.ProgramTestCalenderId, Check.QuestionCode, obj.LanguageMaster_Id, Check.Set_Id, Question, ActionA, ActionB, ActionC, ActionD, ActionE, ActionF, obj.CreatedBy);
                            }
                            else
                            {
                                int Status = context.Sp_InsertQuestionLanguage_Practical(Check.Id, Check.ProgramTestCalenderId, Check.QuestionCode, obj.LanguageMaster_Id, Check.Set_Id, Question, ActionA, ActionB, ActionC, ActionD, ActionE, ActionF, obj.CreatedBy);
                            }
                        }
                        else
                        {
                            Msg = Msg + "Row: " + RowNumber + " Question Code: " + QuestionCode + " Does not exists.<br />";
                        }
                    }
                    else
                    {
                        Msg = Msg + "Row: " + RowNumber + " Contains Empty Fields.<br />";
                    }
                }
                return Msg.Equals(string.Empty)?"Success: Data Uploaded!":Msg;
            }

        }
    }
}
