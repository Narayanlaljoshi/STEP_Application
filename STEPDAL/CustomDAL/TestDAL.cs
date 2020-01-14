using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

using System.IO;
using Newtonsoft.Json;

namespace STEPDAL.CustomDAL
{
    public class TestDAL
    {
        public static string SaveInitiatedTestDetails(FaciltyProgramdetails Obj)
        {
            string Msg = "";
            try
            {
                using (var context = new CEIDBEntities())
                {
                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                    //{
                        // do something with EF here
                        
                        var Check = context.TblProgramTestCalenders.Where(x => x.ProgramTestCalenderId == Obj.ProgramTestCalenderId).FirstOrDefault();

                        Check.TestInitiatedDate = DateTime.Now;
                        Check.TestInitiatedTime = DateTime.Now.TimeOfDay;
                        Check.TestInitiated = true;
                        Check.ModifiedDate = DateTime.Now;
                        Check.ModifiedBy = Obj.User_Id;
                        context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        TblTestHdr th = new TblTestHdr();
                        th.Agency_Id = Obj.Agency_Id;
                        th.Day = Obj.DayCount;
                        th.Duration = Obj.Duration.ToString();
                        th.FacultyCode = Obj.FacultyCode;
                        //th.Day = Obj.DayCount;
                        th.CreationDate = DateTime.Now;
                        th.ProgramTestCalenderId = Obj.ProgramTestCalenderId;
                        th.TestInitiated = true;
                        th.IsActive = true;
                        th.CreatedBy = 1;
                        th.ModifiedDate = DateTime.Now;
                        context.Entry(th).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();

                        var SessionIds = context.TblNominations.Where(x => x.ProgramCode == Obj.ProgramCode && x.IsActive == true).ToList();
                        if (SessionIds.Count != 0)
                        {
                            foreach (var item in SessionIds)
                            {
                                TblTestDtl Dtl = new TblTestDtl
                                {
                                    //Test_Id = th.Test_Id,
                                    //Day = Obj.DayCount,
                                    //IsActive = true,
                                    //CreationDate = DateTime.Now,
                                    //SessionID = item.SessionID,
                                    //CreatedBy = 1
                                };
                                context.Entry(Dtl).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();
                            }
                        }
                        Msg = "Success: Test Initiated Succeccfully";
                        //tr.Complete();
                        //tr.Dispose();
                        //scope.Complete();
                        return Msg;
                    //}

                }

            }
            catch (Exception ex)
            {
                return "Error: Test not Initiated";
            }
        }

        public static StudenttestDetailsBLL GetStudenttestDetails(string MSPin)
        {
            using (var context = new CEIDBEntities())
            {
                StudenttestDetailsBLL GetStudenttestDetails = null;
                //var ReqData2  = context.sp_GetStudentTestDetails(MSPin).First();
                var ReqData = context.sp_GetStudentTestDetails(MSPin).FirstOrDefault();
                if (ReqData != null)
                {
                    int Day = ReqData.Day;
                    string SessionID = ReqData.SessionID;
                    var StudentAnsHdr = context.SP_CheckTblStudentAnswerHdr(MSPin, Day, SessionID).FirstOrDefault();
                    if (StudentAnsHdr != null)
                    {
                        if (StudentAnsHdr.Status_Id == 1)
                        {
                            GetStudenttestDetails = new StudenttestDetailsBLL {
                                ErrorMessage="Error: Already Submitted The Test"
                            };
                            return GetStudenttestDetails;// ;
                        }
                        else
                        {
                            ReqData.TestDuration = Convert.ToInt32(StudentAnsHdr.RemainingTime);
                        }
                    }
                    else
                    {
                        ReqData.TestDuration = Convert.ToInt32(ReqData.TestDuration * 60);
                    }
                    GetStudenttestDetails = new StudenttestDetailsBLL
                    {
                        AgencyCode = ReqData.AgencyCode,
                        Agency_Id = ReqData.Agency_Id,
                        Co_id = ReqData.Co_id,
                        CreatedBy = ReqData.CreatedBy,
                        CreationDate = ReqData.CreationDate,
                        DateofBirth = ReqData.DateofBirth,
                        DayCount = ReqData.Day,
                        Duration = ReqData.Duration,
                        DealerName=ReqData.DealerName,
                        EndDate = ReqData.EndDate,
                        FacultyCode = ReqData.FacultyCode,
                        FacultyName= ReqData.FacultyName,
                        IsActive = ReqData.IsActive,
                        MobileNo = ReqData.MobileNo,
                        MSPIN = ReqData.MSPIN,
                        Name = ReqData.Name,
                        Nomination_Id = ReqData.Nomination_Id,
                        ProgramCode = ReqData.ProgramCode,
                        ProgramName = ReqData.ProgramName,
                        ProgramTestCalenderId = ReqData.ProgramTestCalenderId,
                        SessionID = ReqData.SessionID,
                        StartDate = ReqData.StartDate.Value,
                        TestDuration = ReqData.TestDuration,
                        //TestInitiated = ReqData.TestInitiated,
                        TestInitiatedDate = ReqData.TestInitiatedDate,
                        TotalNoQuestion = ReqData.TotalNoQuestion,
                        TypeOfTest = ReqData.TypeOfTest,
                        ProgramId = ReqData.ProgramId,
                        TestLoginCode = ReqData.TestLoginCode,
                        EvaluationTypeId = ReqData.EvaluationTypeId,
                        ProgramType_Id = ReqData.ProgramType_Id,
                        ExtendedTime= ReqData.ExtendedTime,
                        Position_Id= ReqData.Position
                    };
                    //bool Status=UserDAL.UpdateLogReport(GetStudenttestDetails);
                    //tr.Complete();
                    //tr.Dispose();
                    return GetStudenttestDetails;
                }
                else
                {
                    return null;
                }
            }
        }

        public static StudenttestDetailsBLL GetStudenttestDetails_Evaluation(string MSPin , int Day)
        {
            using (var context = new CEIDBEntities())
            {
                StudenttestDetailsBLL GetStudenttestDetails = null;
                var ReqData = context.sp_GetStudentTestDetails_Evaluation(MSPin,Day).FirstOrDefault();

                if (ReqData != null)
                {
                    //int Day = ReqData.Day;
                    string SessionID = ReqData.SessionID;
                    var StudentAnsHdr = context.SP_CheckTblStudentAnswerHdr(MSPin, Day, SessionID).FirstOrDefault();
                    if (StudentAnsHdr != null)
                    {
                        if (StudentAnsHdr.Status_Id == 1)
                        {
                            return null;
                        }
                        else
                        {
                            ReqData.TestDuration = Convert.ToInt32(StudentAnsHdr.RemainingTime);
                        }
                    }
                    else
                    {
                        ReqData.TestDuration = Convert.ToInt32(ReqData.TestDuration * 60);
                    }
                    GetStudenttestDetails = new StudenttestDetailsBLL
                    {
                        AgencyCode = ReqData.AgencyCode,
                        Agency_Id = ReqData.Agency_Id,
                        Co_id = ReqData.Co_id,
                        CreatedBy = ReqData.CreatedBy,
                        CreationDate = ReqData.CreationDate,
                        DateofBirth = ReqData.DateofBirth,
                        DayCount = ReqData.Day,
                        Duration = ReqData.Duration,
                        EndDate = ReqData.EndDate,
                        FacultyCode = ReqData.FacultyCode,
                        IsActive = ReqData.IsActive,
                        MobileNo = ReqData.MobileNo,
                        MSPIN = ReqData.MSPIN,
                        Name = ReqData.Name,
                        Nomination_Id = ReqData.Nomination_Id,
                        ProgramCode = ReqData.ProgramCode,
                        ProgramName = ReqData.ProgramName,
                        ProgramTestCalenderId = ReqData.ProgramTestCalenderId,
                        SessionID = ReqData.SessionID,
                        StartDate = ReqData.StartDate.Value,
                        TestDuration = ReqData.TestDuration,
                        //TestInitiated = ReqData.TestInitiated,
                        TestInitiatedDate = ReqData.TestInitiatedDate,
                        TotalNoQuestion = ReqData.TotalNoQuestion,
                        TypeOfTest = ReqData.TypeOfTest,
                        ProgramId = ReqData.ProgramId,
                        TestLoginCode = ReqData.TestLoginCode,
                        EvaluationTypeId = ReqData.EvaluationTypeId,
                        ProgramType_Id = ReqData.ProgramType_Id
                    };
                    //bool Status=UserDAL.UpdateLogReport(GetStudenttestDetails);
                    //tr.Complete();
                    //tr.Dispose();
                    return GetStudenttestDetails;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<StudenttestDetailsBLL> GetStudenttestDetails_Practical(string MSPin)
        {
            using (var context = new CEIDBEntities())
            {
                List<StudenttestDetailsBLL> GetStudenttestDetails = null;
                //using (TransactionScope tr = new TransactionScope())
                //{
                var ReqData = context.sp_GetStudentTestDetails(MSPin).ToList();

                if (ReqData.Count != 0)
                {
                    int Day = ReqData[0].Day;
                    string SessionID = ReqData[0].SessionID;
                    var StudentAnsHdr = context.SP_CheckTblStudentAnswerHdr(MSPin, Day, SessionID).FirstOrDefault();
                    if (StudentAnsHdr != null)
                    {
                        if (StudentAnsHdr.Status_Id == 1)
                        {
                            return null;
                        }
                        else
                        {
                            ReqData[0].TestDuration = Convert.ToInt32(StudentAnsHdr.RemainingTime);
                        }
                    }
                    else
                    {
                        ReqData[0].TestDuration = Convert.ToInt32(ReqData[0].TestDuration * 60);
                    }
                    GetStudenttestDetails = ReqData.Select(x => new StudenttestDetailsBLL
                    {
                        AgencyCode = x.AgencyCode,
                        Agency_Id = x.Agency_Id,
                        Co_id = x.Co_id,
                        CreatedBy = x.CreatedBy,
                        CreationDate = x.CreationDate,
                        DateofBirth = x.DateofBirth,
                        DayCount = x.Day,
                        Duration = x.Duration,
                        EndDate = x.EndDate,
                        FacultyCode = x.FacultyCode,
                        IsActive = x.IsActive,
                        MobileNo = x.MobileNo,
                        MSPIN = x.MSPIN,
                        Name = x.Name,
                        Nomination_Id = x.Nomination_Id,
                        ProgramCode = x.ProgramCode,
                        ProgramName = x.ProgramName,
                        ProgramTestCalenderId = x.ProgramTestCalenderId,
                        SessionID = x.SessionID,
                        StartDate = x.StartDate.Value,
                        TestDuration = x.TestDuration,
                        //TestInitiated = x.TestInitiated,
                        TestInitiatedDate = x.TestInitiatedDate,
                        TotalNoQuestion = x.TotalNoQuestion,
                        TypeOfTest = x.TypeOfTest,
                        ProgramId = x.ProgramId,
                        TestLoginCode = x.TestLoginCode,
                        EvaluationTypeId = x.EvaluationTypeId,
                        ProgramType_Id = x.ProgramType_Id
                    }).ToList();

                    //bool Status=UserDAL.UpdateLogReport(GetStudenttestDetails);
                    //tr.Complete();
                    //tr.Dispose();

                    return GetStudenttestDetails;
                }
                else
                {
                    return null;
                }
                //}

            }
        }

        public static List<StudentTestQuestionsBLL> GetStudentTestQuestions(int ProgramTestCalenderId)
        {
            using (var context = new CEIDBEntities())
            {

                List<StudentTestQuestionsBLL> GetStudentTestQuestions = null;
                //using (var scope = new TransactionScope(TransactionScopeOption.Required, new  TransactionOptions
                //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                //{
                    var ReqData = context.sp_GetStudentTestQuestions(ProgramTestCalenderId).ToList();
                    if (ReqData != null)
                    {
                        GetStudentTestQuestions = ReqData.Select(x => new StudentTestQuestionsBLL
                        {
                            TypeOfTest = x.TypeOfTest,
                            Answer1 = x.Answer1,
                            Answer2 = x.Answer2,
                            Answer3 = x.Answer3,
                            Answer4 = x.Answer4,
                            AnswerKey = x.AnswerKey,
                            Image = x.Image,
                            DetailId = x.DetailId,
                            IsActive = x.IsActive,
                            ProgramId = x.ProgramId,
                            ProgramTestCalenderId = x.ProgramTestCalenderId,
                            Question = x.Question,
                            QuestionCode = x.QuestionCode,
                            TestDuration = x.TestDuration,
                            TestInitiated = x.TestInitiated,
                            TotalNoQuestion = x.TotalNoQuestion,
                        }).ToList();
                        //scope.Complete();
                        return GetStudentTestQuestions;
                    }
                    else
                    {
                        //scope.Complete();
                        return null;
                    }
                    
                //}

            }
        }

        public static List<StudentTestQuestionsBLL> GetStudentTestQuestions_Practical(int ProgramTestCalenderId)
        {
            using (var context = new CEIDBEntities())
            {
                List<StudentTestQuestionsBLL> GetStudentTestQuestions = null;
               
                var ReqData = context.sp_GetStudentTestQuestions(ProgramTestCalenderId).ToList();
                if (ReqData != null)
                {
                    GetStudentTestQuestions = ReqData.Select(x => new StudentTestQuestionsBLL
                    {
                        TypeOfTest = x.TypeOfTest,
                        Answer1 = x.Answer1,
                        Answer2 = x.Answer2,
                        Answer3 = x.Answer3,
                        Answer4 = x.Answer4,
                        AnswerKey = x.AnswerKey,
                        Image = x.Image,
                        DetailId = x.DetailId,
                        IsActive = x.IsActive,
                        ProgramId = x.ProgramId,
                        ProgramTestCalenderId = x.ProgramTestCalenderId,
                        Question = x.Question,
                        QuestionCode = x.QuestionCode,
                        TestDuration = x.TestDuration,
                        TestInitiated = x.TestInitiated,
                        TotalNoQuestion = x.TotalNoQuestion,
                    }).ToList();
                    //scope.Complete();
                    return GetStudentTestQuestions;
                }
                else
                {
                    //scope.Complete();
                    return null;
                }

                //}

            }
        }

        public static string SaveTestResponse(StudentTestResponse Obj)
        {
            string Msg = "";
            string QuesCode = "";
            int Processed = 0;
            string StudentMSPIN = string.Empty;
            int? TestDay = null;
            int? TotalQuestion = null;
            int? QuetionReceivedCount = null;
            TblStudentAnswer ObjctToBeSaved = new TblStudentAnswer();
            TblStudentAnswerHdr ObjctToBeSavedHdr = new TblStudentAnswerHdr();

            using (var context = new CEIDBEntities())
            {
                try
                {
                    StudentMSPIN = Obj.StudentTestDetails.MSPIN;
                    TestDay = Obj.StudentTestDetails.Day;
                    TotalQuestion = Obj.StudentTestDetails.TotalNoQuestion.HasValue ? Obj.StudentTestDetails.TotalNoQuestion : null;
                    QuetionReceivedCount = Obj.StudentLanguageQuestion.Count;

                    //var CheckStudentHeader = context.TblStudentAnswerHdrs.Where(x => x.MSPIN == Obj.StudentTestDetails.MSPIN && x.ProgramTestCalenderId == Obj.StudentTestDetails.ProgramTestCalenderId && x.ProgramId == Obj.StudentTestDetails.ProgramId && x.IsActive == true).FirstOrDefault();


                    //if (CheckStudentHeader != null)
                    //    {
                    int Status=context.SP_Insert_Update_TblStudentAnswerHdr(Obj.StudentTestDetails.ProgramTestCalenderId, Obj.StudentTestDetails.ProgramId, Obj.StudentTestDetails.TypeOfTest, Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.RemainingTime, Obj.StudentTestDetails.Day, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Status_Id);
                    TestDAL.LogService("SP_Insert_Update_TblStudentAnswerHdr Status: - "+Status);
                    if (Obj.StudentTestDetails.Status_Id == 1)
                    {
                        context.sp_UpdateTblTestDtl_Evaluation(Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Day);
                    }
                    //CheckStudentHeader.Status_Id = Obj.StudentTestDetails.Status_Id;
                    //CheckStudentHeader.ModifiedDate = DateTime.Now;
                    //CheckStudentHeader.ModifiedBy = Obj.StudentTestDetails.ModifiedBy;
                    //CheckStudentHeader.RemainingTime = Obj.StudentTestDetails.RemainingTime;
                    //for log purpose
                    //ObjctToBeSavedHdr = CheckStudentHeader;

                    //context.Entry(CheckStudentHeader).State = System.Data.Entity.EntityState.Modified;
                    //context.SaveChanges();

                    foreach (var Ques in Obj.StudentLanguageQuestion)
                    {
                        //var check = context.TblStudentAnswers.Where(x => x.MSPIN == Ques.MSPIN && x.QuestionCode == Ques.QuestionCode && x.IsActive == true).FirstOrDefault();

                        //if (check == null)
                        //{

                        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                        QuesCode = Ques.QuestionCode;

                        int StatusDtl=context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, null, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true);
                        TestDAL.LogService("SP_Insert_Update_TblStudentAnswer Status: - " + StatusDtl);
                        //TblStudentAnswer Sa = new TblStudentAnswer
                        //{
                        //    IsActive = true,
                        //    QuestionCode = Ques.QuestionCode,
                        //    MSPIN = Ques.MSPIN,
                        //    AnswerGiven = Ques.AnswerGiven,
                        //    CorrectAnswer = Ques.AnswerKey,
                        //    Day = Ques.Day,
                        //    IsAnswerCorrect = IsCorrect,
                        //    ProgramId = Ques.ProgramId.Value,
                        //    ProgramTestCalenderId = Ques.ProgramTestCalenderId,
                        //    SessionID = Ques.SessionID,
                        //    TypeOfTest = Ques.TypeOfTest,
                        //    CreatedBy = 1,
                        //    CreationDate = DateTime.Now
                        //};
                        //ObjctToBeSaved = Sa;
                        //context.Entry(Sa).State = System.Data.Entity.EntityState.Added;
                        //context.SaveChanges();
                        //    Processed++;
                        //}
                        //else
                        //{
                        //    bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;

                        //    context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, CheckStudentHeader.SA_Id, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true, 1);

                        //    //check.IsActive = true;
                        //    //check.QuestionCode = Ques.QuestionCode;
                        //    //check.MSPIN = Ques.MSPIN;
                        //    //check.AnswerGiven = Ques.AnswerGiven;
                        //    //check.CorrectAnswer = Ques.AnswerKey;
                        //    //check.Day = Ques.Day;
                        //    //check.IsAnswerCorrect = IsCorrect;
                        //    //check.ProgramId = Ques.ProgramId.Value;
                        //    //check.ProgramTestCalenderId = Ques.ProgramTestCalenderId;
                        //    //check.ModifiedBy = 1;
                        //    //check.ModifiedDate = DateTime.Now;
                        //    //check.TypeOfTest = Ques.TypeOfTest;
                        //    //check.SessionID = Ques.SessionID;
                        //    //ObjctToBeSaved = check;
                        //    //context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                        //    //context.SaveChanges();
                        //    Processed++;
                        //}
                    }
                    //}
                    //else
                    //{
                    // context.SP_InertInto_TblStudentAnswerHdr(null, Obj.StudentTestDetails.ProgramTestCalenderId, Obj.StudentTestDetails.ProgramId, Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.RemainingTime, Obj.StudentTestDetails.Day, Obj.StudentTestDetails.TypeOfTest, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Status_Id, true);
                    //TblStudentAnswerHdr SAH = new TblStudentAnswerHdr
                    //{
                    //    MSPIN = Obj.StudentTestDetails.MSPIN,
                    //    ProgramId = Obj.StudentTestDetails.ProgramId,
                    //    ProgramTestCalenderId = Obj.StudentTestDetails.ProgramTestCalenderId,
                    //    Status_Id = Obj.StudentTestDetails.Status_Id,
                    //    SessionID = Obj.StudentTestDetails.SessionID,
                    //    TypeOfTest = Obj.StudentTestDetails.TypeOfTest,

                    //    IsActive = true,
                    //    RemainingTime = Obj.StudentTestDetails.RemainingTime,
                    //    Day = Obj.StudentTestDetails.Day,
                    //    CreatedBy = Obj.StudentTestDetails.CreatedBy,
                    //    CreationDate = DateTime.Now
                    //};
                    //ObjctToBeSavedHdr = SAH;
                    //context.Entry(SAH).State = System.Data.Entity.EntityState.Added;
                    //context.SaveChanges();
                    //foreach (var Ques in Obj.StudentLanguageQuestion)
                    //{

                    //    var check = context.TblStudentAnswers.Where(x => x.MSPIN == Ques.MSPIN && x.QuestionCode == Ques.QuestionCode && x.IsActive == true && x.Day == Ques.Day && x.SessionID == Ques.SessionID).FirstOrDefault();

                    //    if (check == null)
                    //    {
                    //        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                    //        //int? SAid = CheckStudentHeader.SA_Id == null ? null : CheckStudentHeader.SA_Id;
                    //        context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, null, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true, 2);

                    //        //TblStudentAnswer Sa = new TblStudentAnswer
                    //        //{
                    //        //    SA_Id = SAH.SA_Id,
                    //        //    IsActive = true,
                    //        //    QuestionCode = Ques.QuestionCode,
                    //        //    MSPIN = Ques.MSPIN,
                    //        //    AnswerGiven = Ques.AnswerGiven,
                    //        //    CorrectAnswer = Ques.AnswerKey,
                    //        //    Day = Ques.Day,
                    //        //    IsAnswerCorrect = IsCorrect,
                    //        //    ProgramId = Ques.ProgramId.Value,
                    //        //    ProgramTestCalenderId = Ques.ProgramTestCalenderId,
                    //        //    SessionID = Ques.SessionID,
                    //        //    TypeOfTest = Ques.TypeOfTest,
                    //        //    CreatedBy = 1,
                    //        //    CreationDate = DateTime.Now
                    //        //};
                    //        //ObjctToBeSaved = Sa;
                    //        //context.Entry(Sa).State = System.Data.Entity.EntityState.Added;
                    //        //context.SaveChanges();
                    //        Processed++;
                    //    }
                    //    else
                    //    {
                    //        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                    //        context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, CheckStudentHeader.SA_Id, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true, 1);

                    //        //check.IsActive = true;
                    //        //check.QuestionCode = Ques.QuestionCode;
                    //        //check.MSPIN = Ques.MSPIN;
                    //        //check.AnswerGiven = Ques.AnswerGiven;
                    //        //check.CorrectAnswer = Ques.AnswerKey;
                    //        //check.Day = Ques.Day;
                    //        //check.IsAnswerCorrect = IsCorrect;
                    //        //check.ProgramId = Ques.ProgramId.Value;
                    //        //check.ProgramTestCalenderId = Ques.ProgramTestCalenderId;
                    //        //check.ModifiedBy = 1;
                    //        //check.ModifiedDate = DateTime.Now;
                    //        //check.TypeOfTest = Ques.TypeOfTest;
                    //        //check.SessionID = Ques.SessionID;
                    //        //ObjctToBeSaved = check;

                    //        //context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                    //        //context.SaveChanges();
                    //        Processed++;
                    //    }


                    //}
                    //}

                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    string es = string.Empty;
                    while (ex.InnerException != null)
                    {
                        es = es + ex.InnerException;
                    }
                    es = es + "BaseException :-" + ex.GetBaseException();
                    TblLogWhile_AfterSavingQuestions Twas = new TblLogWhile_AfterSavingQuestions
                    {
                        Date = DateTime.Now,
                        Day = TestDay,
                        ExceptionMsg = es + " StackTrace : - " + ex.StackTrace + ex.InnerException.StackTrace + "Total Questions " + JsonConvert.SerializeObject(Obj),// + " Obect : - " + JsonConvert.SerializeObject(ObjctToBeSaved) + " header Object:-" + JsonConvert.SerializeObject(ObjctToBeSavedHdr)
                        MSPIN = StudentMSPIN,
                        ProcessedCount = Processed,
                        QuestionCode = QuesCode,
                        QuestionCount = QuetionReceivedCount,
                        TotalQuestion = TotalQuestion,
                        LineNumber = line
                    };
                    context.Entry(Twas).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                }
                finally
                {
                    GC.Collect();
                    //scope.Complete();
                    //tr.Complete();
                    //tr.Dispose();
                }
                return "Success: Response Saved Successfully !";

                //}
            }

        }

        public static string SaveTestResponse2(StudentTestResponse2 Obj)
        {
            string Msg = "";
            string StudentMSPIN =string.Empty;
            int? TestDay =null;
            int? TotalQuestion =null;
            if (Obj.StudentLanguageQuestion.AnswerGiven == null)
            {
                Obj.StudentLanguageQuestion.AnswerGiven = "";
            }
            using (var context = new CEIDBEntities())
            {
                //using (var scope = new TransactionScope(TransactionScopeOption.Required, new   TransactionOptions
                //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                //{
                    // do something with EF here
                    
                try
                    {
                        StudentMSPIN = Obj.StudentTestDetails.MSPIN;
                        TestDay = Obj.StudentTestDetails.Day;
                        TotalQuestion = Obj.StudentTestDetails.TotalNoQuestion;
                    var Ques = Obj.StudentLanguageQuestion;
                    bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;

                    int HdrStatus=context.SP_Insert_Update_TblStudentAnswerHdr(Obj.StudentTestDetails.ProgramTestCalenderId, Obj.StudentTestDetails.ProgramId, Obj.StudentTestDetails.TypeOfTest, Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.RemainingTime, Obj.StudentTestDetails.Day, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Status_Id);

                    int DtlStatus = context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, null, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true);
                    // && x.ProgramTestCalenderId == Obj.StudentTestDetails.ProgramTestCalenderId && x.ProgramId == Obj.StudentTestDetails.ProgramId
                    if (Obj.StudentTestDetails.Status_Id == 1)
                    {
                        int status=context.sp_UpdateTblTestDtl_Evaluation(Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Day);
                    }
                    //var CheckStudentHeader = context.TblStudentAnswerHdrs.Where(x => x.MSPIN == Obj.StudentTestDetails.MSPIN && x.IsActive == true&& x.SessionID== Obj.StudentTestDetails.SessionID && x.Day== Obj.StudentTestDetails.Day).FirstOrDefault();
                    //if (CheckStudentHeader != null)
                    //{
                    //    context.SP_UpdateTblStudentAnswerHdr(CheckStudentHeader.SA_Id, Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.RemainingTime, Obj.StudentTestDetails.Day, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Status_Id,true);
                    //    #region 
                    //    //CheckStudentHeader.Status_Id = Obj.StudentTestDetails.Status_Id;
                    //    //CheckStudentHeader.ModifiedDate = DateTime.Now;
                    //    //CheckStudentHeader.ModifiedBy = Obj.StudentTestDetails.ModifiedBy;
                    //    //CheckStudentHeader.RemainingTime = Obj.StudentTestDetails.RemainingTime;
                    //    //context.Entry(CheckStudentHeader).State = System.Data.Entity.EntityState.Modified;
                    //    //context.SaveChanges();
                    //    #endregion

                    //    var Ques = Obj.StudentLanguageQuestion;
                    //    var check = context.TblStudentAnswers.Where(x => x.MSPIN == Ques.MSPIN && x.QuestionCode == Ques.QuestionCode && x.IsActive == true).FirstOrDefault();

                    //    if (check == null)
                    //    {
                    //        //var Ques = Obj.StudentLanguageQuestion;
                    //        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                    //        context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, CheckStudentHeader.SA_Id,Ques.MSPIN,Ques.QuestionCode,Ques.AnswerGiven,Ques.AnswerKey,Ques.Day,Ques.TypeOfTest,Ques.SessionID,IsCorrect,true,2);
                    //        #region
                    //        //TblStudentAnswer Sa = new TblStudentAnswer
                    //        //{
                    //        //    IsActive = true,
                    //        //    QuestionCode = Ques.QuestionCode,
                    //        //    MSPIN = Ques.MSPIN,
                    //        //    AnswerGiven = Ques.AnswerGiven,
                    //        //    CorrectAnswer = Ques.AnswerKey,
                    //        //    Day = Ques.Day,
                    //        //    IsAnswerCorrect = IsCorrect,
                    //        //    ProgramId = Ques.ProgramId.Value,
                    //        //    ProgramTestCalenderId = Ques.ProgramTestCalenderId,
                    //        //    SessionID = Ques.SessionID,
                    //        //    TypeOfTest = Ques.TypeOfTest,
                    //        //    CreatedBy = Obj.StudentTestDetails.CreatedBy,
                    //        //    CreationDate = DateTime.Now
                    //        //};
                    //        //context.Entry(Sa).State = System.Data.Entity.EntityState.Added;
                    //        //context.SaveChanges();
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        // var Ques = Obj.StudentLanguageQuestion;
                    //        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                    //        context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, CheckStudentHeader.SA_Id, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true, 1);
                    //        #region
                    //        //check.IsActive = true;
                    //        //check.QuestionCode = Ques.QuestionCode;
                    //        //check.MSPIN = Ques.MSPIN;
                    //        //check.AnswerGiven = Ques.AnswerGiven;
                    //        //check.CorrectAnswer = Ques.AnswerKey;
                    //        //check.Day = Ques.Day;
                    //        //check.IsAnswerCorrect = IsCorrect;
                    //        //check.ProgramId = Ques.ProgramId.Value;
                    //        //check.ProgramTestCalenderId = Ques.ProgramTestCalenderId;
                    //        //check.ModifiedBy = Obj.StudentTestDetails.CreatedBy;
                    //        //check.ModifiedDate = DateTime.Now;
                    //        //check.TypeOfTest = Ques.TypeOfTest;
                    //        //check.SessionID = Ques.SessionID;
                    //        //context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                    //        //context.SaveChanges();
                    //        #endregion
                    //    }

                    //}
                    //else
                    //{
                    //    context.SP_InertInto_TblStudentAnswerHdr(null, Obj.StudentTestDetails.ProgramTestCalenderId, Obj.StudentTestDetails.ProgramId, Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.TestDuration, Obj.StudentTestDetails.Day, Obj.StudentTestDetails.TypeOfTest, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Status_Id,true);
                    //    #region
                    //    //TblStudentAnswerHdr SAH = new TblStudentAnswerHdr
                    //    //{
                    //    //    MSPIN = Obj.StudentTestDetails.MSPIN,
                    //    //    ProgramId = Obj.StudentTestDetails.ProgramId,
                    //    //    ProgramTestCalenderId = Obj.StudentTestDetails.ProgramTestCalenderId,
                    //    //    Status_Id = Obj.StudentTestDetails.Status_Id,
                    //    //    SessionID = Obj.StudentTestDetails.SessionID,
                    //    //    TypeOfTest = Obj.StudentTestDetails.TypeOfTest,
                    //    //    IsActive = true,
                    //    //    RemainingTime = Obj.StudentTestDetails.TestDuration,
                    //    //    Day = Obj.StudentTestDetails.Day,
                    //    //    CreatedBy = Obj.StudentTestDetails.CreatedBy,
                    //    //    CreationDate = DateTime.Now
                    //    //};
                    //    //context.Entry(SAH).State = System.Data.Entity.EntityState.Added;
                    //    //context.SaveChanges();
                    //    #endregion
                    //    var Ques = Obj.StudentLanguageQuestion;
                    //    var check = context.TblStudentAnswers.Where(x => x.MSPIN == Ques.MSPIN && x.QuestionCode == Ques.QuestionCode && x.IsActive == true).FirstOrDefault();

                    //    if (check == null)
                    //    {
                    //        // var Ques = Obj.StudentLanguageQuestion;
                    //        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                    //        context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, CheckStudentHeader.SA_Id, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true, 2);
                    //        #region 
                    //        //TblStudentAnswer Sa = new TblStudentAnswer
                    //        //{
                    //        //    SA_Id = SAH.SA_Id,
                    //        //    IsActive = true,
                    //        //    QuestionCode = Ques.QuestionCode,
                    //        //    MSPIN = Ques.MSPIN,
                    //        //    AnswerGiven = Ques.AnswerGiven,
                    //        //    CorrectAnswer = Ques.AnswerKey,
                    //        //    Day = Ques.Day,
                    //        //    IsAnswerCorrect = IsCorrect,
                    //        //    ProgramId = Ques.ProgramId.Value,
                    //        //    ProgramTestCalenderId = Ques.ProgramTestCalenderId,
                    //        //    SessionID = Ques.SessionID,
                    //        //    TypeOfTest = Ques.TypeOfTest,
                    //        //    CreatedBy = 1,
                    //        //    CreationDate = DateTime.Now
                    //        //};
                    //        //context.Entry(Sa).State = System.Data.Entity.EntityState.Added;
                    //        //context.SaveChanges();
                    //        #endregion

                    //    }
                    //    else
                    //    {
                    //        // var Ques = Obj.StudentLanguageQuestion;
                    //        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                    //        context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, CheckStudentHeader.SA_Id, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true, 1);
                    //        #region
                    //        //check.IsActive = true;
                    //        //check.QuestionCode = Ques.QuestionCode;
                    //        //check.MSPIN = Ques.MSPIN;
                    //        //check.AnswerGiven = Ques.AnswerGiven;
                    //        //check.CorrectAnswer = Ques.AnswerKey;
                    //        //check.Day = Ques.Day;
                    //        //check.IsAnswerCorrect = IsCorrect;
                    //        //check.ProgramId = Ques.ProgramId.Value;
                    //        //check.ProgramTestCalenderId = Ques.ProgramTestCalenderId;
                    //        //check.ModifiedBy = 1;
                    //        //check.ModifiedDate = DateTime.Now;
                    //        //check.TypeOfTest = Ques.TypeOfTest;
                    //        //check.SessionID = Ques.SessionID;
                    //        //context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                    //        //context.SaveChanges();
                    //        #endregion
                    //    }
                    //}

                    return "Success: Response Saved Successfully !";
                    }
                    catch (Exception ex)
                    {
                        var st = new StackTrace(ex, true);
                        var frame = st.GetFrame(0);
                        var line = frame.GetFileLineNumber();
                        string es = string.Empty;
                        while (ex.InnerException != null)
                        {
                            es = es + ex.InnerException;
                        }
                        es = es +"BaseException :-"+ ex.GetBaseException();
                        TblLogWhile_AfterSavingQuestions Twas = new TblLogWhile_AfterSavingQuestions
                        {
                            Date = DateTime.Now,
                            Day = TestDay,
                            ExceptionMsg ="While Saving one by one question"+ es + " StackTrace : - " + ex.StackTrace + "ex.InnerException : -" + ex.InnerException + " Obect : - " + JsonConvert.SerializeObject(Obj) ,
                            MSPIN = StudentMSPIN,
                            LineNumber = line
                        };
                        context.Entry(Twas).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();

                        return "Error: Response Not Saved!";
                    }
                    finally
                    {
                        //scope.Complete();
                        //tr.Complete();
                        //tr.Dispose();
                    }
                    
               // }
            }
        }

        public static List<StudenttestDetailsBLL> Get_Check(string MSPin, int Day)
        {
            using (var context = new CEIDBEntities())
            {
                //Nullable<int> DayCount = Day;
                List<StudenttestDetailsBLL> GetStudenttestDetails = new List<StudenttestDetailsBLL>();
                //using (TransactionScope tr = new TransactionScope())
                //{
                    DateTime CurrentTime = DateTime.Now;
                    DateTime TestInitiatedTime;
                    //var ReqData = context.Sp_CheckStatus(MSPin, Day).ToList();
                    var ReqData = context.TblStudentAnswerHdrs.Where(x => x.MSPIN == MSPin && x.Day == Day && x.Status_Id == 1).FirstOrDefault();
                    var GetTimeDetails = context.sp_GetStudentTestDetails(MSPin).FirstOrDefault();
                    if (GetTimeDetails != null)
                    {
                        //CurrentTime = DateTime.Now.TimeOfDay;
                        TestInitiatedTime = GetTimeDetails.TestInitiatedDate.Value;
                        TestInitiatedTime = TestInitiatedTime.AddMinutes(GetTimeDetails.ValidDuration.Value);
                        if (CurrentTime > TestInitiatedTime)
                        {
                            //return null;
                            GetStudenttestDetails.Add(new StudenttestDetailsBLL
                            {
                                DayCount = 0,
                                MSPIN = MSPin

                            });
                            return GetStudenttestDetails;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (ReqData != null)
                    {
                        GetStudenttestDetails.Add(new StudenttestDetailsBLL
                        {
                            DayCount = ReqData.Day,
                            MSPIN = ReqData.MSPIN

                        });

                        return GetStudenttestDetails;
                    }

                    else
                    {

                        return null;
                    }

                //}

            }
        }

        public static List<StudentPostTestScoresBLL> GetStudentPostTestScores(string MSpin)
        {
            using (var context = new CEIDBEntities())
            {
                List<StudentPostTestScoresBLL> StudentPostTestScores = null;

                var ReqData = context.sp_GetStudentPostTestScores(MSpin).ToList();
                if (ReqData.Count != 0)
                {
                    StudentPostTestScores = ReqData.Select(x => new StudentPostTestScoresBLL
                    {
                        Day = x.Day,
                        MSPIN = x.MSPIN,
                        PostTestMaxMarks = x.PostTestMaxMarks,
                        PostTest_MarksObtained = x.PostTest_MarksObtained,
                        ProgramId = x.ProgramId,
                        SessionID = x.SessionID,
                        TypeOfTest = x.TypeOfTest,

                    }).ToList();

                    return StudentPostTestScores;
                }
                else
                {
                    return null;
                }


            }
        }

        public static StudentTestResponse GetStudentQuestionFormatedList(QuestionVariable data)
        {
            string SessionId = data.SessionID;
            StudentTestResponse StudentTestResponse = new StudentTestResponse();
            /*Initialize Sub BLL Models*/
            StudentTestResponse.StudentLanguageQuestion = new List<StudentLanguageQuestion>();
            StudentTestResponse.StudentTestDetails = new StudentTestDetails();
            //StudentTestResponse.StudentTestDetails.RemainingTime=data.
            StudentTestResponse.StudentTestDetails.CreatedBy = data.UserId;
            StudentTestResponse.StudentTestDetails.CreationDate = DateTime.Now;
            StudentTestResponse.StudentTestDetails.Day = data.Day;
            StudentTestResponse.StudentTestDetails.MSPIN = data.MSPIN;
            StudentTestResponse.StudentTestDetails.ProgramTestCalenderId = data.ProgramTestCalenderId;
            StudentTestResponse.StudentTestDetails.ProgramId = data.ProgramId;
            StudentTestResponse.StudentTestDetails.Status_Id = 3;
            StudentTestResponse.StudentTestDetails.TypeOfTest = data.TypeOfTest;
            StudentTestResponse.StudentTestDetails.SessionID = data.SessionID;

            using (var context = new CEIDBEntities())
            {
                //List<StudentLanguageQuestion> objList = new List<StudentLanguageQuestion>();
                // var dtl = context.Sp_GetStudentTestLanguageWiseList(data.ProgramTestCalenderId, data.LangId).ToList();
                //var dtl = context.Sp_GetStudentTestLanguageWiseList(data.ProgramTestCalenderId, data.LangId).ToList();
                //var dtl = context.Sp_GetStudentTestLanguageWiseList(data.ProgramTestCalenderId, 0).ToList();
                //var IfExist = context.SP_GetQuestionListIfExist(data.MSPIN, data.Day, SessionId).ToList();
                //if (IfExist.Count != 0)
                //{
                //    for (int i = 0; i < IfExist.Count; i++)
                //    {
                //        var QuesDtl = dtl.Where(x => x.QuestionCode == IfExist[i].QuestionCode).FirstOrDefault();
                //        StudentTestResponse.StudentLanguageQuestion.Add(new StudentLanguageQuestion
                //        {
                //            TypeOfTest = QuesDtl.TypeOfTest,
                //            Answer1 = QuesDtl.Answer1,
                //            Answer2 = QuesDtl.Answer2,
                //            Answer3 = QuesDtl.Answer3,
                //            Answer4 = QuesDtl.Answer4,
                //            AnswerKey = QuesDtl.AnswerKey,
                //            AnswerGiven = IfExist[i].AnswerGiven,
                //            Image = QuesDtl.Image,
                //            DetailId = QuesDtl.DetailId,
                //            IsActive = QuesDtl.IsActive,
                //            ProgramId = QuesDtl.ProgramId,
                //            ProgramTestCalenderId = QuesDtl.ProgramTestCalenderId,
                //            Question = QuesDtl.Question,
                //            QuestionCode = QuesDtl.QuestionCode,
                //            TestDuration = QuesDtl.TestDuration,
                //            TestInitiated = QuesDtl.TestInitiated,
                //            TotalNoQuestion = QuesDtl.TotalNoQuestion,
                //            LanguageAnswer1 = QuesDtl.LanguageAnswer1,
                //            LanguageAnswer2 = QuesDtl.LanguageAnswer2,
                //            LanguageAnswer3 = QuesDtl.LanguageAnswer3,
                //            LanguageAnswer4 = QuesDtl.LanguageAnswer4,
                //            QuestionLanguage = QuesDtl.QuestionLanguage,
                //            MSPIN = data.MSPIN,
                //            SessionID = data.SessionID,
                //            Day = data.Day
                //        });
                //    }
                //}
                var IfExist = context.SP_GetQuestionListFromTblSA(data.MSPIN, SessionId,data.Day,data.LangId).ToList();
                if (IfExist.Count != 0)
                {
                    List<StudentLanguageQuestion> QuesList = null;

                    //var QuesDtl = dtl.Where(x => x.QuestionCode == IfExist[i].QuestionCode).FirstOrDefault();
                    QuesList = IfExist.Select(x => new StudentLanguageQuestion
                    {
                        TypeOfTest = x.TypeOfTest,
                        Answer1 = x.Answer1,
                        Answer2 = x.Answer2,
                        Answer3 = x.Answer3,
                        Answer4 = x.Answer4,
                        AnswerKey = x.AnswerKey,
                        AnswerGiven = x.AnswerGiven,
                        Image = x.Image,
                        DetailId = x.DetailId,
                        IsActive = x.IsActive,
                        ProgramId = x.ProgramId,
                        ProgramTestCalenderId = x.ProgramTestCalenderId,
                        Question = x.Question,
                        QuestionCode = x.QuestionCode,
                        TestDuration = x.TestDuration,
                        TestInitiated = x.TestInitiated,
                        TotalNoQuestion = x.TotalNoQuestion,
                        LanguageAnswer1 = x.LanguageAnswer1,
                        LanguageAnswer2 = x.LanguageAnswer2,
                        LanguageAnswer3 = x.LanguageAnswer3,
                        LanguageAnswer4 = x.LanguageAnswer4,
                        QuestionLanguage = x.QuestionLanguage,
                        MSPIN = data.MSPIN,
                        SessionID = data.SessionID,
                        Day = data.Day
                    }).ToList();
                    StudentTestResponse.StudentLanguageQuestion = QuesList;

                    return StudentTestResponse;
                }
                else {
                    return null;
                }
                #region
                //else
                //{
                //    var TestDetails = context.sp_GetStudentTestDetails(data.MSPIN).FirstOrDefault();
                //    int TotalNoOfTestQuestions = Convert.ToInt32(TestDetails.TotalNoQuestion);
                //    var random = new Random();
                //    StudentTestResponse.StudentTestDetails.RemainingTime = dtl[0].TestDuration * 60;
                //    var result = Enumerable.Range(0, dtl.Count - 1)
                //                            .OrderBy(i => random.Next(TotalNoOfTestQuestions))
                //                            .Take(TotalNoOfTestQuestions)
                //                            .OrderBy(i => i).ToList();
                //    if (result.Count < TotalNoOfTestQuestions)
                //    {

                //        result = Enumerable.Range(0, dtl.Count - 1)
                //                            .OrderBy(i => random.Next(TotalNoOfTestQuestions))
                //                            .Take(TotalNoOfTestQuestions)
                //                            .OrderBy(i => i).ToList();
                //    }

                //    for (int i = 0; i < TotalNoOfTestQuestions; i++)
                //    {
                //        int RandomIndex = result[i];
                //        StudentTestResponse.StudentLanguageQuestion.Add(new StudentLanguageQuestion
                //        {
                //            TypeOfTest = dtl[RandomIndex].TypeOfTest,
                //            Answer1 = dtl[RandomIndex].Answer1,
                //            Answer2 = dtl[RandomIndex].Answer2,
                //            Answer3 = dtl[RandomIndex].Answer3,
                //            Answer4 = dtl[RandomIndex].Answer4,
                //            AnswerKey = dtl[RandomIndex].AnswerKey,
                //            AnswerGiven = null,
                //            Image = dtl[RandomIndex].Image,
                //            DetailId = dtl[RandomIndex].DetailId,
                //            IsActive = dtl[RandomIndex].IsActive,
                //            ProgramId = dtl[RandomIndex].ProgramId,
                //            ProgramTestCalenderId = dtl[RandomIndex].ProgramTestCalenderId,
                //            Question = dtl[RandomIndex].Question,
                //            QuestionCode = dtl[RandomIndex].QuestionCode,
                //            TestDuration = dtl[RandomIndex].TestDuration,
                //            TestInitiated = dtl[RandomIndex].TestInitiated,
                //            TotalNoQuestion = dtl[RandomIndex].TotalNoQuestion,
                //            LanguageAnswer1 = dtl[RandomIndex].LanguageAnswer1,
                //            LanguageAnswer2 = dtl[RandomIndex].LanguageAnswer2,
                //            LanguageAnswer3 = dtl[RandomIndex].LanguageAnswer3,
                //            LanguageAnswer4 = dtl[RandomIndex].LanguageAnswer4,
                //            QuestionLanguage = dtl[RandomIndex].QuestionLanguage,
                //            MSPIN = data.MSPIN,
                //            SessionID = data.SessionID,
                //            Day = data.Day
                //        });
                //    }

                //    TblLogBeforeSaveQuestion Tlbs = new TblLogBeforeSaveQuestion
                //    {
                //        Date = DateTime.Now,
                //        TotalQuestion = TotalNoOfTestQuestions,
                //        MSPIN = data.MSPIN,
                //        QuestionListCount = StudentTestResponse.StudentLanguageQuestion.Count,
                //        RandomLogicCount = result.Count
                //    };
                //    context.Entry(Tlbs).State = System.Data.Entity.EntityState.Added;
                //    context.SaveChanges();


                //    TestDAL.SaveTestResponse(StudentTestResponse);
                //    //TestDAL.LogService("Before SaveTestResponse StudentTestResponse.StudentLanguageQuestion.Count : " + StudentTestResponse.StudentLanguageQuestion.Count);

                //    //TestDAL.LogService("After SaveTestResponse");
                //    //while (true)
                //    //{

                //    //    TestDAL.SaveTestResponse(StudentTestResponse);
                //    //    GetStudentQuestionFormatedList(data);
                //    //}
                //}
                #endregion
                
            }

        }

        public static StudentTestResponse_Practical GetStudentQuestionFormatedList_Practical_old(QuestionVariable data)
        {
            string SessionId = data.SessionID;
            StudentTestResponse_Practical StudentTestResponse = new StudentTestResponse_Practical();
            /*Initialize Sub BLL Models*/
            StudentTestResponse.StudentLanguageQuestion = new List<StudentLanguageQuestion_Practical>();
            StudentTestResponse.StudentTestDetails = new StudentTestDetails();
            //StudentTestResponse.StudentTestDetails.RemainingTime=data.
            StudentTestResponse.StudentTestDetails.CreatedBy = data.UserId;
            StudentTestResponse.StudentTestDetails.CreationDate = DateTime.Now;
            StudentTestResponse.StudentTestDetails.Day = data.Day;
            StudentTestResponse.StudentTestDetails.MSPIN = data.MSPIN;
            StudentTestResponse.StudentTestDetails.ProgramTestCalenderId = data.ProgramTestCalenderId;
            StudentTestResponse.StudentTestDetails.ProgramId = data.ProgramId;
            StudentTestResponse.StudentTestDetails.Status_Id = 3;
            StudentTestResponse.StudentTestDetails.TypeOfTest = data.TypeOfTest;
            StudentTestResponse.StudentTestDetails.SessionID = data.SessionID;

            using (var context = new CEIDBEntities())
            {
                var IfExist = context.SP_GetQuestionListFromTblSA_Practical(data.MSPIN, SessionId, data.Day, data.LangId).ToList();
                if (IfExist.Count == 0)
                {
                    int Status = context.sp_GeneratePracticalQuestions(data.MSPIN,data.ProgramId,data.ProgramTestCalenderId,data.SessionID,data.Day,data.LangId,data.TypeOfTest,data.Set_Id);

                    IfExist= context.SP_GetQuestionListFromTblSA_Practical(data.MSPIN, SessionId, data.Day, data.LangId).ToList();

                    if (IfExist.Count != 0)
                    {
                        List<StudentLanguageQuestion_Practical> QuesList = null;
                        QuesList = IfExist.Select(x => new StudentLanguageQuestion_Practical
                        {
                            TypeOfTest = x.TypeOfTest,
                            ActionA = x.ActionA,
                            ActionA_Image = x.ActionA_Image,
                            ActionB = x.ActionB,
                            ActionB_Image = x.ActionB_Image,
                            ActionC = x.ActionC,
                            ActionC_Image = x.ActionC_Image,
                            ActionD = x.ActionD,
                            ActionD_Image = x.ActionD_Image,
                            ActionE = x.ActionE,
                            ActionE_Image = x.ActionE_Image,
                            ActionF = x.ActionF,
                            ActionF_Image = x.ActionF_Image,
                            ActionTaken = null,
                            CreatedBy = x.CreatedBy,
                            CreationDate = x.CreationDate,
                            Id = x.Id,
                            SA_Id = x.SA_Id,
                            MSPIN = x.MSPIN,
                            //LanguageActionA=x.LanguageActionA,
                            //LanguageActionB=x.LanguageActionB,
                            //LanguageActionC=x.LanguageActionC,
                            //LanguageActionD=x.LanguageActionD,
                            //LanguageActionE=x.LanguageActionE,
                            //LanguageQuestion=x.LanguageQuestion,
                            Marks_A = x.Marks_A,
                            Marks_B = x.Marks_B,
                            Marks_C = x.Marks_C,
                            Marks_D = x.Marks_D,
                            Marks_E = x.Marks_E,
                            Marks_F = x.Marks_F,
                            ModifiedBy = x.ModifiedBy,
                            ModifiedDate = x.ModifiedDate,
                            Question_Image = x.Question_Image,
                            SA_ActionA = x.SA_ActionA,
                            SA_ActionB = x.SA_ActionB,
                            SA_ActionC = x.SA_ActionC,
                            SA_ActionD = x.SA_ActionD,
                            SA_ActionE = x.SA_ActionE,
                            SA_ActionF = x.SA_ActionF,
                            Set_Id = x.Set_Id,
                            SessionID = x.SessionID,
                            IsActive = x.IsActive,
                            ProgramId = x.ProgramId,
                            ProgramTestCalenderId = x.ProgramTestCalenderId,
                            Question = x.Question,
                            QuestionCode = x.QuestionCode,
                            QuestionCategory = x.QuestionCatagory,
                            TestDuration = x.TestDuration,
                            TestInitiated = x.TestInitiated,
                            TotalNoQuestion = x.TotalNoQuestion,
                            RemainingTime = x.RemainingTime,
                            Status_Id = x.Status_Id,
                            Day = x.Day
                        }).ToList();
                        StudentTestResponse.StudentLanguageQuestion = QuesList;
                        return StudentTestResponse;
                    }
                    else
                        return null;
                }
                else
                {
                    List<StudentLanguageQuestion_Practical> QuesList = null;
                    QuesList = IfExist.Select(x => new StudentLanguageQuestion_Practical
                    {
                        TypeOfTest = x.TypeOfTest,
                        ActionA = x.ActionA,
                        ActionA_Image = x.ActionA_Image,
                        ActionB = x.ActionB,
                        ActionB_Image = x.ActionB_Image,
                        ActionC = x.ActionC,
                        ActionC_Image = x.ActionC_Image,
                        ActionD = x.ActionD,
                        ActionD_Image = x.ActionD_Image,
                        ActionE = x.ActionE,
                        ActionE_Image = x.ActionE_Image,
                        ActionF = x.ActionF,
                        ActionF_Image = x.ActionF_Image,
                        ActionTaken = null,
                        CreatedBy = x.CreatedBy,
                        CreationDate = x.CreationDate,
                        Id = x.Id,
                        SA_Id = x.SA_Id,
                        MSPIN = x.MSPIN,
                        //LanguageActionA=x.LanguageActionA,
                        //LanguageActionB=x.LanguageActionB,
                        //LanguageActionC=x.LanguageActionC,
                        //LanguageActionD=x.LanguageActionD,
                        //LanguageActionE=x.LanguageActionE,
                        //LanguageQuestion=x.LanguageQuestion,
                        Marks_A = x.Marks_A,
                        Marks_B = x.Marks_B,
                        Marks_C = x.Marks_C,
                        Marks_D = x.Marks_D,
                        Marks_E = x.Marks_E,
                        Marks_F = x.Marks_F,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        Question_Image = x.Question_Image,
                        SA_ActionA = x.SA_ActionA,
                        SA_ActionB = x.SA_ActionB,
                        SA_ActionC = x.SA_ActionC,
                        SA_ActionD = x.SA_ActionD,
                        SA_ActionE = x.SA_ActionE,
                        SA_ActionF = x.SA_ActionF,
                        Set_Id = x.Set_Id,
                        SessionID = x.SessionID,
                        IsActive = x.IsActive,
                        ProgramId = x.ProgramId,
                        ProgramTestCalenderId = x.ProgramTestCalenderId,
                        Question = x.Question,
                        QuestionCode = x.QuestionCode,
                        QuestionCategory = x.QuestionCatagory,
                        TestDuration = x.TestDuration,
                        TestInitiated = x.TestInitiated,
                        TotalNoQuestion = x.TotalNoQuestion,
                        RemainingTime = x.RemainingTime,
                        Status_Id = x.Status_Id,
                        Day = x.Day
                    }).ToList();
                    StudentTestResponse.StudentLanguageQuestion = QuesList;
                    return StudentTestResponse;
                }
            }
        }

        public static StudentTestResponse_PracticalV2 GetStudentQuestionFormatedList_Practical(QuestionVariable data)
        {
            string SessionId = data.SessionID;
            StudentTestResponse_Practical StudentTestResponse = new StudentTestResponse_Practical();
            /*Initialize Sub BLL Models*/
            StudentTestResponse.StudentLanguageQuestion = new List<StudentLanguageQuestion_Practical>();
            StudentTestResponse.StudentTestDetails = new StudentTestDetails();
            //StudentTestResponse.StudentTestDetails.RemainingTime=data.
            StudentTestResponse.StudentTestDetails.CreatedBy = data.UserId;
            StudentTestResponse.StudentTestDetails.CreationDate = DateTime.Now;
            StudentTestResponse.StudentTestDetails.Day = data.Day;
            StudentTestResponse.StudentTestDetails.MSPIN = data.MSPIN;
            StudentTestResponse.StudentTestDetails.ProgramTestCalenderId = data.ProgramTestCalenderId;
            StudentTestResponse.StudentTestDetails.ProgramId = data.ProgramId;
            StudentTestResponse.StudentTestDetails.Status_Id = 3;
            StudentTestResponse.StudentTestDetails.TypeOfTest = data.TypeOfTest;
            StudentTestResponse.StudentTestDetails.SessionID = data.SessionID;
            StudentTestResponse.StudentTestDetails.Position_Id = data.Position_Id;
            StudentTestResponse.StudentTestDetails.ExtendedTime = data.ExtendedTime;

            using (var context = new CEIDBEntities())

            {
                var IfExist = context.SP_GetQuestionListFromTblSA_Practical(data.MSPIN, SessionId, data.Day, data.LangId).ToList();
                if (IfExist.Count == 0)
                {
                    int Status = context.sp_GeneratePracticalQuestions(data.MSPIN, data.ProgramId, data.ProgramTestCalenderId, data.SessionID, data.Day, data.LangId, data.TypeOfTest, data.Set_Id);
                    //Set Testduration time
                    TestDAL.UpdateTestTime_PositionForPractical(data);

                    IfExist = context.SP_GetQuestionListFromTblSA_Practical(data.MSPIN, SessionId, data.Day, data.LangId).ToList();

                    if (IfExist.Count != 0)
                    {
                        List<StudentLanguageQuestion_Practical> QuesList = null;
                        QuesList = IfExist.Select(x => new StudentLanguageQuestion_Practical
                        {
                            Ques_Sequence = x.Ques_Sequence,
                            TypeOfTest = x.TypeOfTest,
                            ActionA = x.ActionA,
                            ActionA_Image = x.ActionA_Image,
                            ActionB = x.ActionB,
                            ActionB_Image = x.ActionB_Image,
                            ActionC = x.ActionC,
                            ActionC_Image = x.ActionC_Image,
                            ActionD = x.ActionD,
                            ActionD_Image = x.ActionD_Image,
                            ActionE = x.ActionE,
                            ActionE_Image = x.ActionE_Image,
                            ActionF = x.ActionF,
                            ActionF_Image = x.ActionF_Image,
                            ActionTaken = null,
                            CreatedBy = x.CreatedBy,
                            CreationDate = x.CreationDate,
                            Id = x.Id,
                            SA_Id = x.SA_Id,
                            MSPIN = x.MSPIN,
                            //LanguageActionA=x.LanguageActionA,
                            //LanguageActionB=x.LanguageActionB,
                            //LanguageActionC=x.LanguageActionC,
                            //LanguageActionD=x.LanguageActionD,
                            //LanguageActionE=x.LanguageActionE,
                            //LanguageQuestion=x.LanguageQuestion,
                            Marks_A = x.Marks_A,
                            Marks_B = x.Marks_B,
                            Marks_C = x.Marks_C,
                            Marks_D = x.Marks_D,
                            Marks_E = x.Marks_E,
                            Marks_F = x.Marks_F,
                            ModifiedBy = x.ModifiedBy,
                            ModifiedDate = x.ModifiedDate,
                            Question_Image = x.Question_Image,
                            SA_ActionA = x.SA_ActionA,
                            SA_ActionB = x.SA_ActionB,
                            SA_ActionC = x.SA_ActionC,
                            SA_ActionD = x.SA_ActionD,
                            SA_ActionE = x.SA_ActionE,
                            SA_ActionF = x.SA_ActionF,
                            Set_Id = x.Set_Id,
                            SessionID = x.SessionID,
                            IsActive = x.IsActive,
                            ProgramId = x.ProgramId,
                            ProgramTestCalenderId = x.ProgramTestCalenderId,
                            Question = x.Question,
                            QuestionCode = x.QuestionCode,
                            QuestionCategory = x.QuestionCatagory,
                            TestDuration = x.TestDuration,
                            TestInitiated = x.TestInitiated,
                            TotalNoQuestion = x.TotalNoQuestion,
                            RemainingTime = x.RemainingTime,
                            Status_Id = x.Status_Id,
                            Day = x.Day
                        }).ToList();
                        StudentTestResponse.StudentLanguageQuestion = QuesList;
                        StudentTestResponse.StudentTestDetails.RemainingTime =Convert.ToInt32(IfExist[0].RemainingTime);
                        StudentTestResponse.StudentTestDetails.TestDuration = Convert.ToInt32(IfExist[0].RemainingTime);
                        var RtnList = GetNewListForQuestion(data, StudentTestResponse);
                        return RtnList;
                    }
                    else
                        return null;
                }
                else
                {
                    List<StudentLanguageQuestion_Practical> QuesList = null;

                    StudentTestResponse.StudentTestDetails.RemainingTime = Convert.ToInt32(IfExist[0].RemainingTime);
                    StudentTestResponse.StudentTestDetails.TestDuration = Convert.ToInt32(IfExist[0].RemainingTime);

                    QuesList = IfExist.Select(x => new StudentLanguageQuestion_Practical
                    {
                        Ques_Sequence=x.Ques_Sequence,
                        TypeOfTest = x.TypeOfTest,
                        ActionA = x.ActionA,
                        ActionA_Image = x.ActionA_Image,
                        ActionB = x.ActionB,
                        ActionB_Image = x.ActionB_Image,
                        ActionC = x.ActionC,
                        ActionC_Image = x.ActionC_Image,
                        ActionD = x.ActionD,
                        ActionD_Image = x.ActionD_Image,
                        ActionE = x.ActionE,
                        ActionE_Image = x.ActionE_Image,
                        ActionF = x.ActionF,
                        ActionF_Image = x.ActionF_Image,
                        ActionTaken = null,
                        CreatedBy = x.CreatedBy,
                        CreationDate = x.CreationDate,
                        Id = x.Id,
                        SA_Id = x.SA_Id,
                        MSPIN = x.MSPIN,
                        //LanguageActionA=x.LanguageActionA,
                        //LanguageActionB=x.LanguageActionB,
                        //LanguageActionC=x.LanguageActionC,
                        //LanguageActionD=x.LanguageActionD,
                        //LanguageActionE=x.LanguageActionE,
                        //LanguageQuestion=x.LanguageQuestion,
                        Marks_A = x.Marks_A,
                        Marks_B = x.Marks_B,
                        Marks_C = x.Marks_C,
                        Marks_D = x.Marks_D,
                        Marks_E = x.Marks_E,
                        Marks_F = x.Marks_F,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        Question_Image = x.Question_Image,
                        SA_ActionA = x.SA_ActionA,
                        SA_ActionB = x.SA_ActionB,
                        SA_ActionC = x.SA_ActionC,
                        SA_ActionD = x.SA_ActionD,
                        SA_ActionE = x.SA_ActionE,
                        SA_ActionF = x.SA_ActionF,
                        Set_Id = x.Set_Id,
                        SessionID = x.SessionID,
                        IsActive = x.IsActive,
                        ProgramId = x.ProgramId,
                        ProgramTestCalenderId = x.ProgramTestCalenderId,
                        Question = x.Question,
                        QuestionCode = x.QuestionCode,
                        QuestionCategory = x.QuestionCatagory,
                        TestDuration = x.TestDuration,
                        TestInitiated = x.TestInitiated,
                        TotalNoQuestion = x.TotalNoQuestion,
                        RemainingTime = x.RemainingTime,
                        Status_Id = x.Status_Id,
                        Day = x.Day
                    }).ToList();
                    StudentTestResponse.StudentLanguageQuestion = QuesList;
                    var RtnList = GetNewListForQuestion(data, StudentTestResponse);
                    //return StudentTestResponse;
                    return RtnList;
                }
            }
        }

        public static void UpdateTestTime_PositionForPractical(QuestionVariable data)
        {
            //bool Ret = true;
            using (var Context = new CEIDBEntities())
            {
                var PTCD_Practical = Context.TblProgramTestCalenderDetail_Practical.Where(x => x.ProgramTestCalenderId == data.ProgramTestCalenderId && x.Set_Id == data.Set_Id && x.IsActive == true).FirstOrDefault();

                if (PTCD_Practical!=null)
                {
                    int TestDuration = (data.TimetoExtend * 60)+ (PTCD_Practical.TestDuration!=null? PTCD_Practical.TestDuration.Value*60:0);
                    int Status = Context.SP_Insert_Update_TblStudentAnswerHdr_V2(data.ProgramTestCalenderId, data.ProgramId, data.TypeOfTest, data.MSPIN, TestDuration, data.Day, data.SessionID, 3,data.TimetoExtend,data.Position_Id);
                }
            }
        }

        public static StudentTestResponse_PracticalV2 GetNewListForQuestion(QuestionVariable data, StudentTestResponse_Practical StudentTestResponse)
        {

            using (var Context = new CEIDBEntities())
            {
                StudentTestResponse_PracticalV2 List = new StudentTestResponse_PracticalV2();
                var DistinctCategory = StudentTestResponse.StudentLanguageQuestion.Select(x => x.QuestionCategory).Distinct().ToList();
                List<string> lst = DistinctCategory;
                List<StudentCategory> stuCat = new List<StudentCategory>();
                foreach (var catg in lst)
                {
                    StudentCategory STObj = new StudentCategory();

                    List<StudentLanguageQuestion_Practical> StDataList = new List<StudentLanguageQuestion_Practical>();
                    STObj.Catagory = catg;
                    StDataList = StudentTestResponse.StudentLanguageQuestion.Where(x => x.QuestionCategory == catg).ToList();
                    //foreach (var st in StudentTestResponse.StudentLanguageQuestion)
                    //{
                    //    StudentLanguageQuestion_Practical StData = new StudentLanguageQuestion_Practical();
                    //    if (catg == st.QuestionCategory)
                    //    {
                    //        StData = st;

                    //        StDataList.Add(StData);

                    //    }

                    //}
                    STObj.StudentLanguageQuestion = StDataList;
                    stuCat.Add(STObj);

                }
                List.StudentTestDetails = StudentTestResponse.StudentTestDetails;
                List.StudentLanguageQuestion = stuCat;

                return List;
            }

        }

        public static string SaveTestInitiationDetails(List<SessionIDListBLL> Obj)
        {
            string Msg = "";
            string TestCode = "";
            var random = new Random();
            var result = Enumerable.Range(0, 9).OrderBy(i => random.Next()).Take(4).OrderBy(i => i).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                TestCode += result[i].ToString();
            }
            try
            {
                using (var context = new CEIDBEntities())
                {
                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot
                    //}))
                    //{
                        foreach (var item in Obj)
                        {
                            item.day += 1;
                            var Check = context.TblTestDtls.Where(x => x.SessionID == item.SessionID && x.Day == item.day && x.IsActive == true).FirstOrDefault();

                            if (Check == null && item.IsChecked == true)
                            {

                                TblTestDtl Dtl = new TblTestDtl
                                {
                                    Day = item.day,
                                    IsActive = true,
                                    CreationDate = DateTime.Now,
                                    SessionID = item.SessionID,
                                    CreatedBy = item.CreatedBy,
                                    TestCode = TestCode
                                };
                                context.Entry(Dtl).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();
                                Msg = "Success: Test Initiated Successfully \n TestCode is :- " + TestCode;
                            }
                            else
                            {

                                Msg = "Error: already initiated";
                            }
                        }
                        //scope.Complete();
                        //tr.Complete();
                        //tr.Dispose();
                        return Msg;
                    //}

                }

            }
            catch (Exception ex)
            {
                return "Error: Test not Initiated";
            }
        }

        public static string SaveTestInitiationDetailsWithAttendance(AttendanceBLL Obj)
        {
            string Msg = "";
            string TestCode = "";
            var random = new Random();
            var result = Enumerable.Range(0, 9).OrderBy(i => random.Next()).Take(4).OrderBy(i => i).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                TestCode += result[i].ToString();
            }
            try
            {
                using (var context = new CEIDBEntities())
                {
                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot
                    //}))
                    //{
                        foreach (var item in Obj.SessionIDList)
                        {
                            item.day += 1;
                            var Check = context.TblTestDtls.Where(x => x.SessionID == item.SessionID && x.Day == item.day && x.IsActive == true).FirstOrDefault();

                            if (Check == null && item.IsChecked == true)
                            {

                                TblTestDtl Dtl = new TblTestDtl
                                {
                                    Day = item.day,
                                    IsActive = true,
                                    CreationDate = DateTime.Now,
                                    SessionID = item.SessionID,
                                    CreatedBy = item.CreatedBy,
                                    TestCode = TestCode,
                                    SameDayTestInitiation = false
                                };
                                context.Entry(Dtl).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();
                                Msg = "Success: Test Initiated Successfully \n TestCode is :- " + TestCode;
                            }
                            else
                            {

                                Msg = "Error: already initiated";
                            }
                        }
                        foreach (var item in Obj.StudentsList)
                        {
                            //item.Day += 1; 
                            var Check = context.TblAttendances.Where(x => x.MSPIN == item.MSPIN && x.SessionID == item.SessionID && x.Day == item.Day && x.IsActive == true).FirstOrDefault();

                            if (Check == null && item.IsChecked == true)
                            {

                                TblAttendance TA = new TblAttendance
                                {
                                    IsPresent = true,
                                    MSPIN = item.MSPIN,
                                    Day = item.Day.Value,
                                    IsActive = true,
                                    CreationDate = DateTime.Now,
                                    SessionID = item.SessionID,
                                    CreatedBy = item.CreatedBy,
                                };
                                context.Entry(TA).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();

                            }

                        }
                        //scope.Complete();
                        //tr.Complete();
                        //tr.Dispose();
                        return Msg;
                    //}

                }

            }
            catch (Exception ex)
            {
                return "Error: Test not Initiated";
            }
        }

        public static void LogService(string content)
        {
            string Path = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["LogFilePath"]);
            Path = Path + "LogFile.txt";
            FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }

        public static StudenttestDetailsBLL GetStudenttestDetails_Mobile(string MSPin)
        {
            using (var context = new CEIDBEntities())
            {
                StudenttestDetailsBLL GetStudenttestDetails = null;
                //using (TransactionScope tr = new TransactionScope())
                //{
                var ReqData = context.sp_GetStudentTestDetails(MSPin).FirstOrDefault();

                if (ReqData != null)
                {
                    int Day = ReqData.Day;
                    string SessionID = ReqData.SessionID;
                    var StudentAnsHdr = context.SP_CheckTblStudentAnswerHdr(MSPin, Day, SessionID).FirstOrDefault();
                    if (StudentAnsHdr != null)
                    {
                        if (StudentAnsHdr.Status_Id == 1)
                        {
                            return null;
                        }
                        else
                        {
                            ReqData.TestDuration = Convert.ToInt32(StudentAnsHdr.RemainingTime);
                        }
                    }
                    else
                    {
                        ReqData.TestDuration = Convert.ToInt32(ReqData.TestDuration * 60);
                    }
                    GetStudenttestDetails =  new StudenttestDetailsBLL
                    {
                        AgencyCode = ReqData.AgencyCode,
                        Agency_Id = ReqData.Agency_Id,
                        Co_id = ReqData.Co_id,
                        CreatedBy = ReqData.CreatedBy,
                        CreationDate = ReqData.CreationDate,
                        DateofBirth = ReqData.DateofBirth,
                        DayCount = ReqData.Day,
                        Duration = ReqData.Duration,
                        EndDate = ReqData.EndDate,
                        FacultyCode = ReqData.FacultyCode,
                        IsActive = ReqData.IsActive,
                        MobileNo = ReqData.MobileNo,
                        MSPIN = ReqData.MSPIN,
                        Name = ReqData.Name,
                        Nomination_Id = ReqData.Nomination_Id,
                        ProgramCode = ReqData.ProgramCode,
                        ProgramName = ReqData.ProgramName,
                        ProgramTestCalenderId = ReqData.ProgramTestCalenderId,
                        SessionID = ReqData.SessionID,
                        StartDate = ReqData.StartDate.Value,
                        TestDuration = ReqData.TestDuration,
                        //TestInitiated = ReqData.TestInitiated,
                        TestInitiatedDate = ReqData.TestInitiatedDate,
                        TotalNoQuestion = ReqData.TotalNoQuestion,
                        TypeOfTest = ReqData.TypeOfTest,
                        ProgramId = ReqData.ProgramId,
                        Status_Id= StudentAnsHdr.Status_Id,
                        TestLoginCode = ReqData.TestLoginCode
                    };

                    //bool Status=UserDAL.UpdateLogReport(GetStudenttestDetails);
                    //tr.Complete();
                    //tr.Dispose();

                    return GetStudenttestDetails;
                }
                else
                {
                    return null;
                }
                //}

            }
        }

        public static List<StudentLanguageQuestion> GetStudentQuestionFormatedList_Mobile(QuestionVariable data)
        {
            string SessionId = data.SessionID;

            using (var context = new CEIDBEntities())
            {                
                var IfExist = context.SP_GetQuestionListFromTblSA(data.MSPIN, SessionId, data.Day, data.LangId).ToList();
                if (IfExist.Count != 0)
                {
                    List<StudentLanguageQuestion> QuesList = null;

                    QuesList = IfExist.Select(x => new StudentLanguageQuestion
                    {
                        TypeOfTest = x.TypeOfTest,
                        Answer1 = x.Answer1,
                        Answer2 = x.Answer2,
                        Answer3 = x.Answer3,
                        Answer4 = x.Answer4,
                        AnswerKey = x.AnswerKey,
                        AnswerGiven = x.AnswerGiven,
                        Image = x.Image,
                        DetailId = x.DetailId,
                        IsActive = x.IsActive,
                        ProgramId = x.ProgramId,
                        ProgramTestCalenderId = x.ProgramTestCalenderId,
                        Question = x.Question,
                        QuestionCode = x.QuestionCode,
                        TestDuration = x.TestDuration,
                        TestInitiated = x.TestInitiated,
                        TotalNoQuestion = x.TotalNoQuestion,
                        LanguageAnswer1 = x.LanguageAnswer1,
                        LanguageAnswer2 = x.LanguageAnswer2,
                        LanguageAnswer3 = x.LanguageAnswer3,
                        LanguageAnswer4 = x.LanguageAnswer4,
                        QuestionLanguage = x.QuestionLanguage,
                        MSPIN = data.MSPIN,
                        SessionID = data.SessionID,
                        Day = data.Day
                    }).ToList();

                    QuesList = QuesList.OrderBy(a => Guid.NewGuid()).ToList();

                    return QuesList;
                }
                else
                {
                    return null;
                }
               
            }

        }

        public static string SaveTestResponse_Mobile(StudentTestResponse Obj)
        {
            string Msg = "";
            string QuesCode = "";
            int Processed = 0;
            string StudentMSPIN = string.Empty;
            int? TestDay = null;
            int? TotalQuestion = null;
            int? QuetionReceivedCount = null;
            TblStudentAnswer ObjctToBeSaved = new TblStudentAnswer();
            TblStudentAnswerHdr ObjctToBeSavedHdr = new TblStudentAnswerHdr();

            using (var context = new CEIDBEntities())
            {
                try
                {
                    StudentMSPIN = Obj.StudentTestDetails.MSPIN;
                    TestDay = Obj.StudentTestDetails.DayCount;
                    TotalQuestion = Obj.StudentTestDetails.TotalNoQuestion.HasValue ? Obj.StudentTestDetails.TotalNoQuestion : null;
                    QuetionReceivedCount = Obj.StudentLanguageQuestion.Count;
                    Obj.StudentTestDetails.Status_Id = 1;
                    int Status = context.SP_Insert_Update_TblStudentAnswerHdr(Obj.StudentTestDetails.ProgramTestCalenderId, Obj.StudentTestDetails.ProgramId, Obj.StudentTestDetails.TypeOfTest, Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.RemainingTime, TestDay, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.Status_Id);
                    TestDAL.LogService("SP_Insert_Update_TblStudentAnswerHdr Status: - " + Status);
                    if (Obj.StudentTestDetails.Status_Id == 1)
                    {
                        int status=context.sp_UpdateTblTestDtl_Evaluation(Obj.StudentTestDetails.MSPIN, Obj.StudentTestDetails.SessionID, Obj.StudentTestDetails.DayCount);
                    }

                    foreach (var Ques in Obj.StudentLanguageQuestion)
                    {
                        bool IsCorrect = Ques.AnswerGiven == Ques.AnswerKey ? true : false;
                        QuesCode = Ques.QuestionCode;
                        int StatusDtl = context.SP_Insert_Update_TblStudentAnswer(Ques.ProgramTestCalenderId, Ques.ProgramId, null, Ques.MSPIN, Ques.QuestionCode, Ques.AnswerGiven, Ques.AnswerKey, Ques.Day, Ques.TypeOfTest, Ques.SessionID, IsCorrect, true);
                        TestDAL.LogService("SP_Insert_Update_TblStudentAnswer Status: - " + StatusDtl);
                    }

                    Msg = "Success: Response Saved Successfully !";
                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    string es = string.Empty;
                    while (ex.InnerException != null)
                    {
                        es = es + ex.InnerException;
                    }
                    es = es + "BaseException :-" + ex.GetBaseException();
                    TblLogWhile_AfterSavingQuestions Twas = new TblLogWhile_AfterSavingQuestions
                    {
                        Date = DateTime.Now,
                        Day = TestDay,
                        ExceptionMsg = es + " StackTrace : - " + ex.StackTrace + ex.InnerException.StackTrace + "Total Questions " + JsonConvert.SerializeObject(Obj),// + " Obect : - " + JsonConvert.SerializeObject(ObjctToBeSaved) + " header Object:-" + JsonConvert.SerializeObject(ObjctToBeSavedHdr)
                        MSPIN = StudentMSPIN,
                        ProcessedCount = Processed,
                        QuestionCode = QuesCode,
                        QuestionCount = QuetionReceivedCount,
                        TotalQuestion = TotalQuestion,
                        LineNumber = line
                    };
                    context.Entry(Twas).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    Msg = "Error: " + ex.Message.ToString();
                }
                return Msg;
            }
        }

        public static string SaveTestResponse_Practical(StudentLanguageQuestion_Practical Obj)
        {
            string Msg = string.Empty;
            using (var context = new CEIDBEntities())
            {
                try
                {
                    int Status = context.SP_Insert_Update_TblStudentAnswerHdr(Obj.ProgramTestCalenderId, Obj.ProgramId, Obj.TypeOfTest, Obj.MSPIN, Obj.RemainingTime, Obj.Day, Obj.SessionID, Obj.Status_Id);
                    TestDAL.LogService("SP_Insert_Update_TblStudentAnswerHdr Status: - " + Status);
                    int StatusDtl = context.SP_Insert_Update_TblStudentAnswer_Practical(Obj.SA_Id,Obj.ProgramTestCalenderId,Obj.ProgramId,Obj.MSPIN,Obj.QuestionCode,Obj.Day,Obj.SessionID,Obj.TypeOfTest,Obj.SA_ActionA,Obj.SA_ActionB, Obj.SA_ActionC, Obj.SA_ActionD, Obj.SA_ActionE, null,Obj.SA_ActionA==true? Obj.Marks_A:null, Obj.SA_ActionB == true ? Obj.Marks_B:null, Obj.SA_ActionC == true ? Obj.Marks_C:null, Obj.SA_ActionD == true ? Obj.Marks_D:null, Obj.SA_ActionE == true ? Obj.Marks_E:null, null,true);
                    if (Obj.Status_Id==1)
                    {
                        context.sp_UpdateTblTestDtl_Evaluation(Obj.MSPIN,Obj.SessionID,Obj.Day);
                    }
                    Msg = "Success: Response Saved Successfully !";
                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    string es = string.Empty;
                    while (ex.InnerException != null)
                    {
                        es = es + ex.InnerException;
                    }
                    es = es + "BaseException :-" + ex.GetBaseException();
                    TblLogWhile_AfterSavingQuestions Twas = new TblLogWhile_AfterSavingQuestions
                    {
                        Date = DateTime.Now,
                        Day = Obj.Day,
                        ExceptionMsg = es + " StackTrace : - " + ex.StackTrace + ex.InnerException.StackTrace + "Total Questions " + JsonConvert.SerializeObject(Obj),// + " Obect : - " + JsonConvert.SerializeObject(ObjctToBeSaved) + " header Object:-" + JsonConvert.SerializeObject(ObjctToBeSavedHdr)
                        MSPIN = Obj.MSPIN,
                        //ProcessedCount = Processed,
                        QuestionCode = Obj.QuestionCode,
                        //QuestionCount = QuetionReceivedCount,
                        TotalQuestion = Obj.TotalNoQuestion,
                        LineNumber = line
                    };
                    context.Entry(Twas).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    Msg = "Error: " + ex.Message.ToString();
                }
                return Msg;
            }
        }

        public static string SaveCompleteTestResponse_Practical(StudentTestResponse_PracticalV2 Object)
        {
            string Msg = string.Empty;
            using (var context = new CEIDBEntities())
            {
                foreach (var Category in Object.StudentLanguageQuestion) {
                    foreach (var Obj in Category.StudentLanguageQuestion)
                    {
                        if (Obj.ActionTaken != null)
                        {
                            switch (Obj.ActionTaken != null ? Obj.ActionTaken.ToLower() : Obj.ActionTaken)
                            {
                                case "a":
                                    Obj.SA_ActionA = true;
                                    break;
                                case "b":
                                    Obj.SA_ActionB = true;
                                    break;
                                case "c":
                                    Obj.SA_ActionC = true;
                                    break;
                                case "d":
                                    Obj.SA_ActionD = true;
                                    break;
                                case "e":
                                    Obj.SA_ActionE = true;
                                    break;
                                case "f":
                                    Obj.SA_ActionF = true;
                                    break;
                                default:
                                    break;
                            }
                            try
                            {
                                Obj.Status_Id = 1;
                                int Status = context.SP_Insert_Update_TblStudentAnswerHdr(Obj.ProgramTestCalenderId, Obj.ProgramId, Obj.TypeOfTest, Obj.MSPIN, Object.StudentTestDetails.RemainingTime, Obj.Day, Obj.SessionID, Obj.Status_Id);
                                //TestDAL.LogService("SP_Insert_Update_TblStudentAnswerHdr Status: - " + Status);
                                int StatusDtl = context.SP_Insert_Update_TblStudentAnswer_Practical(Obj.SA_Id, Obj.ProgramTestCalenderId, Obj.ProgramId, Obj.MSPIN, Obj.QuestionCode, Obj.Day, Obj.SessionID, Obj.TypeOfTest, Obj.SA_ActionA, Obj.SA_ActionB, Obj.SA_ActionC, Obj.SA_ActionD, Obj.SA_ActionE, Obj.SA_ActionF, Obj.ActionTaken == "a" ? Obj.Marks_A : null, Obj.ActionTaken == "b" ? Obj.Marks_B : null, Obj.ActionTaken == "c" ? Obj.Marks_C : null, Obj.ActionTaken == "d" ? Obj.Marks_D : null, Obj.ActionTaken == "e" ? Obj.Marks_E : null, Obj.ActionTaken == "f" ? Obj.Marks_F : null, true);
                                if (Obj.Status_Id >= 1)
                                {
                                    context.sp_UpdateTblTestDtl_Evaluation(Obj.MSPIN, Obj.SessionID, Obj.Day);
                                }
                                Msg = "Success: Response Saved Successfully !";
                            }
                            catch (Exception ex)
                            {
                                var st = new StackTrace(ex, true);
                                var frame = st.GetFrame(0);
                                var line = frame.GetFileLineNumber();
                                string es = string.Empty;
                                while (ex.InnerException != null)
                                {
                                    es = es + ex.InnerException;
                                }
                                es = es + "BaseException :-" + ex.GetBaseException();
                                TblLogWhile_AfterSavingQuestions Twas = new TblLogWhile_AfterSavingQuestions
                                {
                                    Date = DateTime.Now,
                                    Day = Obj.Day,
                                    ExceptionMsg = es + " StackTrace : - " + ex.StackTrace + ex.InnerException + "Total Questions " + JsonConvert.SerializeObject(Obj),// + " Obect : - " + JsonConvert.SerializeObject(ObjctToBeSaved) + " header Object:-" + JsonConvert.SerializeObject(ObjctToBeSavedHdr)
                                    MSPIN = Obj.MSPIN,
                                    //ProcessedCount = Processed,
                                    QuestionCode = Obj.QuestionCode,
                                    //QuestionCount = QuetionReceivedCount,
                                    TotalQuestion = Obj.TotalNoQuestion,
                                    LineNumber = line
                                };
                                context.Entry(Twas).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();
                                Msg = "Error: " + ex.Message;
                            }
                        }
                    }
                }
                return Msg;
            }
        }

        public static string InitiateTestForEvaluation_Theory(List<EligibleCandidatesForEvaluation> Obj)
        {
            using (var Context = new CEIDBEntities()) {
                
                string TestCode = "";
                var random = new Random();
                var result = Enumerable.Range(0, 9).OrderBy(i => random.Next()).Take(4).OrderBy(i => i).ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    TestCode += result[i].ToString();
                }
                List<EligibleCandidatesForEvaluation> List = Obj.Where(x => x.IsChecked == true).ToList();
                foreach (var pt in List)
                {
                    var check = Context.TblTestDtl_Evaluation.Where(x => x.MSPIN == pt.MSPIN && x.Day == pt.Day && x.SessionID == pt.SessionID && x.IsActive == true).FirstOrDefault();
                    if (check == null) {
                        TblTestDtl_Evaluation DT = new TblTestDtl_Evaluation {
                            IsActive=true,
                            CreatedBy=1,
                            CreationDate=DateTime.Now,
                            Day=pt.Day,
                            MSPIN=pt.MSPIN,
                            SessionID=pt.SessionID,
                            TestCode= TestCode
                        };
                        Context.Entry(DT).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        check.IsActive = true;
                        check.CreatedBy = 1;
                        check.CreationDate = DateTime.Now;
                        check.Day = pt.Day;
                        check.MSPIN = pt.MSPIN;
                        check.SessionID = pt.SessionID;
                        check.TestCode = TestCode;
                        Context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                try
                {
                    Context.SaveChanges();
                    return "Success: Test Initiated and Test Code is - " + TestCode;
                }
                catch (Exception ex) { return "Test Not Initiated"; }
                finally { GC.Collect(); }
            }
        }

        public static string InitiateTestForEvaluation_Practical(EligibleCandidatesForEvaluation Obj)
        {
            using (var Context = new CEIDBEntities())
            {
                string TestCode = "";
                var random = new Random();
                var result = Enumerable.Range(0, 9).OrderBy(i => random.Next()).Take(4).OrderBy(i => i).ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    TestCode += result[i].ToString();
                }

                var check = Context.TblTestDtl_Evaluation.Where(x => x.MSPIN == Obj.MSPIN && x.Day == Obj.Day && x.SessionID == Obj.SessionID).FirstOrDefault();
                if (check == null)
                {
                    TblTestDtl_Evaluation DT = new TblTestDtl_Evaluation
                    {
                        IsActive = true,
                        CreatedBy = Obj.UserId,
                        CreationDate = DateTime.Now,
                        Day = Obj.Day,
                        MSPIN = Obj.MSPIN,
                        SessionID = Obj.SessionID,
                        TestCode = TestCode
                    };
                    Context.Entry(DT).State = System.Data.Entity.EntityState.Added;
                }
                else if (check.IsActive==false)
                {
                    return "Warning: Already Submitted";
                }
                else if (check.IsActive == false)
                {
                    check.IsActive = true;
                    check.ModifiedBy = Obj.UserId;
                    check.ModifiedDate = DateTime.Now;
                    check.Day = Obj.Day;
                    check.MSPIN = Obj.MSPIN;
                    check.SessionID = Obj.SessionID;
                    check.TestCode = TestCode;
                    Context.Entry(check).State = System.Data.Entity.EntityState.Modified;
                }
                try
                {
                    Context.SaveChanges();
                    return "Success: Test Initiated and Test Code is - " + TestCode;
                }
                catch (Exception ex) { return "Test Not Initiated"; }
                finally { GC.Collect(); }
            }
        }

        public static string CheckIfAnyTestIsGoingOn(EligibleCandidatesForEvaluation Obj)
        {
            using (var Context= new CEIDBEntities())
            {
                var Check = Context.sp_CheckIfAnyTestIsGoingOn(Obj.MSPIN,Obj.SessionID).FirstOrDefault();
                if (Check != null)
                {
                    return "Warning: Already one test is going on.";
                }
                else
                {
                    return "Success";
                }
            }
        }

        public static string CheckIfAnyTestIsGoingOn(List<EligibleCandidatesForEvaluation> Obj)
        {
            string ReturnMessage = "";
            using (var Context = new CEIDBEntities())
            {
                Obj = Obj.Where(x => x.IsChecked == true).ToList();
                foreach (var item in Obj)
                {
                    var Check = Context.sp_CheckIfAnyTestIsGoingOn(item.MSPIN, item.SessionID).FirstOrDefault();
                    if (Check != null)
                    {
                        return "Warning: Already one test is going on for MSPIN : " + item.MSPIN + "<br />";
                    }
                    else {
                        return "Success";
                    }
                }
                return ReturnMessage;
            }
        }

        public static List<SetSequenceForProgramTestCalenderId> GetSetSequenceByProgramTestCalanderId(int Id)
        {
            List<SetSequenceForProgramTestCalenderId> List = null;
            using (var Context = new CEIDBEntities())
            {
                var ReqData = Context.sp_GetSetSequenceByProgramTestCalenderId(Id).ToList();

                if (ReqData.Count != 0)
                {
                    List = ReqData.Select(x => new SetSequenceForProgramTestCalenderId
                    {
                        ProgramId=x.ProgramId,
                        ProgramTestCalenderId=x.ProgramTestCalenderId,
                        Set_Id=x.Set_Id,
                        Set_Title=x.Set_Title
                    }).ToList();
                }
                return List;
            }

        }

        public static string ClosePracticalSession(List<SessionIDListBLL>Obj)
        {
            string Msg = string.Empty;
            using (var Context = new CEIDBEntities())
            {
                foreach (var item in Obj)
                {
                    var Check = Context.TblTestDtls.Where(x => x.SessionID == item.SessionID && x.Day == item.day+1).FirstOrDefault();
                    try
                    {
                        if (Check == null)
                        {
                            TblTestDtl TD = new TblTestDtl
                            {
                                Day = item.day + 1,
                                CreatedBy = item.CreatedBy,
                                SessionID = item.SessionID,
                                IsActive = true,
                                CreationDate = DateTime.Now,
                                SameDayTestInitiation = false,
                                TestCode = "Practical",
                            };
                            Context.Entry(TD).State = System.Data.Entity.EntityState.Added;
                            Context.SaveChanges();
                            //return "Success! Practical Session Closed";
                        }
                    }
                    catch (Exception Ex)
                    {
                        Msg = "Error: " + Ex.Message.ToString();
                    }
                }
                return Msg;
            }
        }
    }
}
