using System;
using System.Collections.Generic;
using System.Linq;
using STEPDAL.DB;
namespace ProjectBLL.CustomModel
{
    public class FacultyAgencyLevel
    {
        public static FaciltyProgram GetFaciltyProgramdetails(string FacultyCode)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.sp_FacultyProgramDetails(FacultyCode).FirstOrDefault();
                var StudentData = context.sp_GetStudentList(data.FacultyCode, data.ProgramCode).ToList();
                FaciltyProgram FaciltyProgram = new FaciltyProgram();
                List<StudentList> StudentList = new List<StudentList>();
                FaciltyProgramdetails objList = new FaciltyProgramdetails
                {
                    AgencyCode = data.AgencyCode,
                    Agency_Id = data.Agency_Id,
                    Duration = data.Duration,
                    EndDate = data.EndDate,
                    FacultyCode = data.FacultyCode,
                    IsActive = data.IsActive,
                    MSPIN = data.MSPIN,
                    ProgramCode = data.ProgramCode,
                    ProgramId = data.ProgramId,
                    ProgramName = data.ProgramName,
                    //SessionID=data.SessionID,
                    StartDate = data.StartDate.Value,
                    ProgramTestCalenderId = data.ProgramTestCalenderId,
                    DayCount = data.DayCount,
                    TestDuration = data.TestDuration,
                    TotalNoQuestion = data.TotalNoQuestion,
                    TypeOfTest = data.TypeOfTest,
                    LastTestDate = data.LastTestDate,

                };
                foreach (var x in StudentData)
                {
                    int IsPresentInPostTest = 0;
                    int IsPresentInPreTest = 0;
                    var CheckForPreTest = context.TblStudentAnswers.Where(p => p.MSPIN == x.MSPIN && p.TypeOfTest == "1" && p.IsActive == true).FirstOrDefault();
                    var CheckForPostTest = context.TblStudentAnswers.Where(p => p.MSPIN == x.MSPIN && p.TypeOfTest == "3" && p.IsActive == true).FirstOrDefault();
                    if (CheckForPreTest != null)
                    {
                        IsPresentInPreTest = 1;
                    }
                    if (CheckForPostTest != null)
                    {
                        IsPresentInPostTest = 1;
                    }

                    StudentList.Add(new StudentList
                    {
                        ProgramCode = x.ProgramCode,
                        FacultyCode = x.FacultyCode,
                        AgencyCode = x.AgencyCode,
                        DateofBirth = x.DateofBirth,
                        Duration = x.Duration,
                        EndDate = x.EndDate,
                        StartDate = x.StartDate.Value,
                        IsActive = x.IsActive,
                        MSPIN = x.MSPIN,
                        Name = x.Name,
                        Nomination_Id = x.Nomination_Id,
                        SessionID = x.SessionID,
                        PreTest_MarksObtained = x.PreTest_MarksObtained,
                        PostTest_MarksObtained = x.PostTest_MarksObtained,
                        MobileNo = x.MobileNo,
                        PreTestMaxMarks = x.PreTestMaxMarks,
                        PostTestMaxMarks = x.PostTestMaxMarks,
                        IsPresentInPostTest = IsPresentInPostTest,
                        IsPresentInPreTest = IsPresentInPreTest
                    });
                }
                //StudentList = StudentData.Select(x => new StudentList {
                //    ProgramCode=x.ProgramCode,
                //    FacultyCode=x.FacultyCode,
                //    AgencyCode=x.AgencyCode,
                //    DateofBirth=x.DateofBirth,
                //    Duration=x.Duration,
                //    EndDate=x.EndDate,
                //    StartDate=x.StartDate,
                //    IsActive=x.IsActive,
                //    MSPIN=x.MSPIN,
                //    Name=x.Name,
                //    Nomination_Id=x.Nomination_Id,
                //    SessionID=x.SessionID,
                //    PreTest_MarksObtained=x.PreTest_MarksObtained,
                //    PostTest_MarksObtained=x.PostTest_MarksObtained,
                //    MobileNo=x.MobileNo,
                //    PreTestMaxMarks=x.PreTestMaxMarks,
                //    PostTestMaxMarks=x.PostTestMaxMarks,
                //    IsPresentInPostTest= IsPresentInPostTest,


                //}).ToList();

                FaciltyProgram.StudentList = StudentList;
                FaciltyProgram.FaciltyProgramdetails = objList;

                return FaciltyProgram;
            }
        }

        public static string GetFacultyName(string userName)
        {
            using (var context = new CEIDBEntities())
            {
                var data = context.TblFaculties.Where(x => x.FacultyCode == userName).FirstOrDefault();

                string objName = data.FacultyName;

                return objName;
            }


        }

        public static List<SessionIDListBLL> GetSessionIdList(int Agency_Id, string FacultyCode)
        {
            List<SessionIDListBLL> SessionIdList = null;
            using (var context = new CEIDBEntities())
            {
                var data = context.sp_GetSessionIDList(Agency_Id, FacultyCode).ToList();
                //var StudentData = context.sp_GetStudentList(data.FacultyCode, data.ProgramCode).ToList();
                SessionIdList = data.Select(x => new SessionIDListBLL
                {
                    AgencyCode = x.AgencyCode,
                    day = x.day,
                    Duration = x.Duration,
                    EndDate = x.EndDate,
                    ProgramCode = x.ProgramCode,
                    ProgramId = x.ProgramId,
                    SessionID = x.SessionID,
                    StartDate = x.StartDate,
                    TestAssignable = x.TestAssignable,
                    TotalStudents = x.TotalStudents,
                    ProgramName = x.ProgramName,
                    IsChecked = false,
                    NextTestEvaluationType=x.NextTestEvaluationType,
                    NextTestEvaluationType_Id=x.NextTestEvaluationType_Id,
                    ProgramType_Id=x.ProgramType_Id,
                    TestCode = x.TestCode,
                    SameDayTestInitiation = x.SameDayTestInitiation,
                    TestIniateDate = x.CreationDate,
                    CurrentTestEvaluationType_Id=x.CurrentTestEvaluationType_Id,
                    ProgramTestCalenderId=x.ProgramTestCalenderId
                }).ToList();

                return SessionIdList;
            }
        }

        
        //public static List<ProgramMasterBLL> GetProgramsList_Evaluation_Mobile()
        //{
        //    List<ProgramMasterBLL> ProgramsList = null;
        //    using (var context = new CEIDBEntities())
        //    {
        //        var data = context.SP_GetEvaluationTypePrograms().ToList();
        //        //var StudentData = context.sp_GetStudentList(data.FacultyCode, data.ProgramCode).ToList();
        //        ProgramsList = data.Select(x => new ProgramMasterBLL
        //        {
        //            ProgramType_Id = x.ProgramType_Id,
        //            ProgramCode = x.ProgramCode,
        //            ProgramId = x.ProgramId,
        //            Duration = x.Duration
        //        }).ToList();

        //        return ProgramsList;
        //    }
        //}
        public static List<ProgramMasterBLL> GetProgramsList_Evaluation()
        {
            List<ProgramMasterBLL> ProgramsList = null;
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetEvaluationTypePrograms().ToList();
                //var StudentData = context.sp_GetStudentList(data.FacultyCode, data.ProgramCode).ToList();
                ProgramsList = data.Select(x => new ProgramMasterBLL
                {
                   ProgramType_Id=x.ProgramType_Id,
                   ProgramCode=x.ProgramCode,
                   ProgramId=x.ProgramId,
                   Duration=x.Duration
                }).ToList();

                return ProgramsList;
            }
        }
        public static List<TestCodes> GetTestCodes(ProgramMasterBLL Obj)
        {
            List<TestCodes> TestCodes = null;
            using (var context = new CEIDBEntities())
            {
                var data = context.SP_GetTestCodesByProgramId(Obj.ProgramId).ToList();
                //var StudentData = context.sp_GetStudentList(data.FacultyCode, data.ProgramCode).ToList();
                TestCodes = data.Select(x => new TestCodes
                {
                    ProgramId=x.ProgramId,
                    EvaluationTypeId=x.EvaluationTypeId,
                    EvaluationType=x.EvaluationType,
                    ProgramTestCalenderId=x.ProgramTestCalenderId,
                    TestCode=x.TestCode,
                    Day=x.DayCount
                }).ToList();

                return TestCodes;
            }
        }
        public static List<EligibleCandidatesForEvaluation> GetEligibleCandidatesForEvaluation(TestCodes Obj)
        {
            List<EligibleCandidatesForEvaluation> EligibleCandidatesForEvaluation = null;
            using (var context = new CEIDBEntities())
            {
                var data = context.sp_GetEligibleCandidatesForEvaluation(Obj.ProgramTestCalenderId).ToList();
                //var StudentData = context.sp_GetStudentList(data.FacultyCode, data.ProgramCode).ToList();
                EligibleCandidatesForEvaluation = data.Select(x => new EligibleCandidatesForEvaluation
                {
                    Day=x.Day,
                    MSPIN=x.MSPIN,
                    SessionID=x.SessionID,
                    Name=x.Name,
                    IsChecked=false,
                    IsEligible=x.IsEligible,
                    IsResumable=x.IsResumable
                }).ToList();

                return EligibleCandidatesForEvaluation;
            }
        }
        public static List<StudentsList> GetSessionWiseStudentsList(List<SessionIDListBLL> Obj)
        {

            string SessionIDs = "";
            List<StudentsList> SessionList = new List<StudentsList>();
            //for (int i = 0; i < Obj.Count; i++)
            //{
            //    if (i == 0) { SessionIDs = "And SessionID in ("+"'" +Obj[i].SessionID+ "'"; }
            //    //else if (i== Obj.Count-1) { SessionIDs += Obj[i].SessionID; }
            //    else if(i== Obj.Count-1) { SessionIDs += ","+ "'" + Obj[i].SessionID + "'"+")"; }                 
            //    else { SessionIDs += "," + "'" + Obj[i].SessionID + "'"; }
            //    if (i == 0 && Obj.Count == 1) { SessionIDs +=  ")"; }
            //}
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 1500;

                foreach (var item in Obj)
                {
                    var data = context.sp_GetSessionIDWiseStudentList(item.SessionID).ToList();
                    foreach (var row in data)
                    {
                        SessionList.Add(new StudentsList
                        {

                            SessionID = row.SessionID,
                            AgencyCode = row.AgencyCode,
                            Agency_Id = row.Agency_Id,
                            Co_id = row.Co_id,
                            CreatedBy = row.CreatedBy,
                            CreationDate = row.CreationDate,
                            DateofBirth = row.DateofBirth,
                            Duration = row.Duration,
                            EndDate = row.EndDate,
                            FacultyCode = row.FacultyCode,
                            Faculty_Id = row.Faculty_Id,
                            IsActive = row.IsActive,
                            MobileNo = row.MobileNo,
                            ModifiedBy = row.ModifiedBy,
                            ModifiedDate = row.ModifiedDate,
                            MSPIN = row.MSPIN,
                            Name = row.Name,
                            Nomination_Id = row.Nomination_Id,
                            ProgramCode = row.ProgramCode,
                            ProgramId = row.ProgramId,
                            StartDate = row.StartDate,
                            IsChecked = null,
                            LogDay = row.LogDay,
                            LoginDateTime = row.LoginDateTime,
                            Status_Id = row.Status_Id
                        });
                    }
                }

                //var StudentData = context.sp_GetStudentList(data.FacultyCode, data.ProgramCode).ToList();
                //if (data != null)
                //{
                //    SessionList = data.Select(x => new StudentsList
                //    {
                //        SessionID = x.SessionID,
                //        AgencyCode = x.AgencyCode,
                //        Agency_Id = x.Agency_Id,
                //        Co_id = x.Co_id,
                //        CreatedBy = x.CreatedBy,
                //        CreationDate = x.CreationDate,
                //        DateofBirth = x.DateofBirth,
                //        Duration = x.Duration,
                //        EndDate = x.EndDate,
                //        FacultyCode = x.FacultyCode,
                //        Faculty_Id = x.Faculty_Id,
                //        IsActive = x.IsActive,
                //        MobileNo = x.MobileNo,
                //        ModifiedBy = x.ModifiedBy,
                //        ModifiedDate = x.ModifiedDate,
                //        MSPIN = x.MSPIN,
                //        Name = x.Name,
                //        Nomination_Id = x.Nomination_Id,
                //        ProgramCode = x.ProgramCode,
                //        ProgramId = x.ProgramId,
                //        StartDate = x.StartDate
                //    }).ToList();
                //}
                return SessionList;
            }
        }
        public static string AddCandidate(List<AddCandidateBLL> CandidatesList)
        {
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 1500;

                foreach (var Obj in CandidatesList)
                {
                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                    //{
                    // do something with EF here                        
                    var Check = context.TblNominations.Where(x => x.Name == Obj.Name && x.MobileNo == Obj.MobileNo && x.IsActive == true).FirstOrDefault();
                    try
                    {
                        //Obj.MSPIN = GenerateMSPIN(Obj.OrgType);
                        Obj.MSPIN = GenerateMSPIN(Obj.OrgType);// Obj.MSPIN.Trim().Equals(string.Empty) ? GenerateMSPIN(Obj.OrgType) : Obj.MSPIN.Trim();
                        var Details = context.TblNominations.Where(x => x.SessionID == Obj.SessionID && x.IsActive == true).FirstOrDefault();
                        TblNomination tn = new TblNomination
                        {
                            SessionID = Obj.SessionID,
                            IsActive = true,
                            AgencyCode = Details.AgencyCode,
                            Agency_Id = Details.Agency_Id,
                            Co_id = Details.Co_id,
                            CreatedBy = Obj.CreatedBy,
                            CreationDate = DateTime.Now,
                            DateofBirth = Obj.DateofBirth,
                            Duration = Details.Duration,
                            EndDate = Details.EndDate,
                            FacultyCode = Details.FacultyCode,
                            Faculty_Id = Details.Faculty_Id,
                            MobileNo = Obj.MobileNo,
                            MSPIN = Obj.MSPIN,
                            Name = Obj.Name,
                            ProgramCode = Details.ProgramCode,
                            ProgramId = Details.ProgramId,
                            StartDate = Details.StartDate,
                        };
                        context.Entry(tn).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();

                        string pass = Convert.ToDateTime(Obj.DateofBirth).ToString("dd-MM-yyyy");
                        pass = pass.Replace("/", "");
                        pass = pass.Replace("-", "");
                        pass = pass.Replace(" ", "");
                        TblUser tu = new TblUser
                        {
                            Agency_Id = Details.Agency_Id,
                            CreatedBy = Obj.CreatedBy.Value,
                            CreationDate = DateTime.Now,
                            //Email=Obj.Equals
                            IsActive = true,
                            Role_Id = 4,
                            Password = pass,
                            UserName = Obj.MSPIN,

                        };
                        context.Entry(tu).State = System.Data.Entity.EntityState.Added;
                        context.SaveChanges();
                        Obj.Msg = "Added";
                        //Transtion.Complete();
                        //Transtion.Dispose();
                        int Status = context.SP_BatchJobMSPIN_And_SessionId_Wise(tn.MSPIN,tn.SessionID);

                    }
                    catch (Exception ex)
                    {
                        Obj.Msg = ex.Message.ToString();
                        continue;
                    }
                    finally
                    {
                        GC.Collect();
                        //scope.Complete();
                    }

                    //}
                }



                return "Success : Candidates added successfully";
            }
        }
        public static string AddCandidate_Mobile(AddCandidateBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 1500;

                //foreach (var Obj in CandidatesList)
                //{
                //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                //{ IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
                //{
                // do something with EF here                        
                var Check = context.TblNominations.Where(x => x.Name == Obj.Name && x.MobileNo == Obj.MobileNo && x.IsActive == true).FirstOrDefault();
                try
                {
                    Obj.MSPIN = GenerateMSPIN(Obj.OrgType);// Obj.MSPIN.Trim().Equals(string.Empty) ? GenerateMSPIN(Obj.OrgType) : Obj.MSPIN.Trim();
                    var Details = context.TblNominations.Where(x => x.SessionID == Obj.SessionID && x.IsActive == true).FirstOrDefault();
                    TblNomination tn = new TblNomination
                    {
                        SessionID = Obj.SessionID,
                        IsActive = true,
                        AgencyCode = Details.AgencyCode,
                        Agency_Id = Details.Agency_Id,
                        Co_id = Details.Co_id,
                        CreatedBy = Obj.CreatedBy,
                        CreationDate = DateTime.Now,
                        DateofBirth = Obj.DateofBirth,
                        Duration = Details.Duration,
                        EndDate = Details.EndDate,
                        FacultyCode = Details.FacultyCode,
                        Faculty_Id = Details.Faculty_Id,
                        MobileNo = Obj.MobileNo,
                        MSPIN = Obj.MSPIN,
                        Location=Obj.Location,
                        DealerName=Obj.Organization,
                        Name = Obj.Name,
                        ProgramCode = Details.ProgramCode,
                        ProgramId = Details.ProgramId,
                        StartDate = Details.StartDate,
                    };
                    context.Entry(tn).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();

                    string pass = Convert.ToDateTime(Obj.DateofBirth).ToString("dd-MM-yyyy");
                    pass = pass.Replace("/", "");
                    pass = pass.Replace("-", "");
                    pass = pass.Replace(" ", "");
                    TblUser tu = new TblUser
                    {
                        Agency_Id = Details.Agency_Id,
                        CreatedBy = Obj.CreatedBy.Value,
                        CreationDate = DateTime.Now,
                        //Email=Obj.Equals
                        IsActive = true,
                        Role_Id = 4,
                        Password = pass,
                        UserName = Obj.MSPIN,
                    };
                    context.Entry(tu).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    Obj.Msg = "Added";
                    int Status = context.SP_BatchJobMSPIN_And_SessionId_Wise(tn.MSPIN, tn.SessionID);
                }
                catch (Exception ex)
                {
                    return "Error : " + ex.Message.ToString();
                }
                finally
                {
                    GC.Collect();
                }
                return "Success : Candidate added successfully";
            }
        }
        public static string GenerateMSPIN(string OrgType)
        {
            using (var context = new CEIDBEntities())
            {
                string MSPIN = OrgType.Substring(0, 1);
                //var random = new Random();
                double Id = context.TblNominations.Max(x => x.Nomination_Id);
                //var result = Enumerable.Range(0, 9).OrderBy(i => random.Next()).Take(5).OrderBy(i => i).ToList();
                //for (int i = 0; i < 5; i++)
                //{
                //    MSPIN += result[i].ToString();
                //}
                MSPIN = MSPIN + "00" + Id;
                var Check = context.TblNominations.Where(x => x.MSPIN == MSPIN).FirstOrDefault();
                if (Check == null)
                {
                    return MSPIN;
                }
                else
                {
                    return GenerateMSPIN(OrgType);
                }
            }
        }
        public static string ResetStudentLogin(StudentsList Obj)
        {
            using (var context = new CEIDBEntities())
            {
                string MSPIN = Obj.MSPIN;
                int Day = Obj.LogDay;
                var ReqData = context.TblLogReport_Candidates.Where(x => x.MSPIN == MSPIN && x.Day == Day).FirstOrDefault();
                if (ReqData != null)
                {
                    int Status=context.sp_ResetStudentLogin(Obj.MSPIN, Day);
                    //ReqData.AllowLoginAgain = true;

                    //context.Entry(ReqData).State = System.Data.Entity.EntityState.Modified;
                    //context.SaveChanges();

                    return "Success: Login reset for MSPIN : " + Obj.MSPIN;
                }
                else
                {
                    return "Error: Not logged in yet";
                }
            }
        }

    }
}
