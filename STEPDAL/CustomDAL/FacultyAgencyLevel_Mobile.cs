using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEPDAL.CustomDAL
{
    public class FacultyAgencyLevel_Mobile
    {
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
                    NextTestEvaluationType = x.NextTestEvaluationType,
                    NextTestEvaluationType_Id = x.NextTestEvaluationType_Id,
                    ProgramType_Id = x.ProgramType_Id,
                    TestCode = x.TestCode,
                    SameDayTestInitiation = x.SameDayTestInitiation,
                    TestIniateDate = x.CreationDate,
                    CurrentTestEvaluationType_Id = x.CurrentTestEvaluationType_Id,
                    ProgramTestCalenderId = x.ProgramTestCalenderId
                }).ToList();

                return SessionIdList;
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

        public static List<StudentsList> GetSessionWiseStudentsList(List<SessionIDListBLL> Obj)
        {
            List<StudentsList> SessionList = new List<StudentsList>();
          
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
                return SessionList;
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
                    int Status = context.sp_ResetStudentLogin(Obj.MSPIN, Day);
                   

                    return "Success: Login reset for MSPIN : " + Obj.MSPIN;
                }
                else
                {
                    return "Error: Not logged in yet";
                }
            }
        }

        public static List<DayWiseReportBLL> GetMarksReportForFaculty(List<SessionIDListBLL> Object)
        {
            using (var context = new CEIDBEntities())
            {
                List<DayWiseReportBLL> objList = new List<DayWiseReportBLL>();

                //var data = context.sp_GetMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();
                foreach (var Obj in Object)
                {
                    var data = context.sp_GetDayWiseMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();

                    if (data.Count != 0)
                    {
                        var MSPinList = data.Select(x => x.MSPIN).Distinct().ToList();

                        foreach (var Mspin in MSPinList)
                        {
                            var StDtl = data.Where(x => x.MSPIN == Mspin).FirstOrDefault();
                            var ReqData = data.Where(x => x.MSPIN == Mspin).ToList();
                            objList.Add(new DayWiseReportBLL
                            {
                                MSPIN = Mspin,
                                AgencyName = StDtl.AgencyName,
                                AgencyCode = StDtl.AgencyCode,
                                Agency_Id = StDtl.Agency_Id,
                                FacultyName = StDtl.FacultyName,
                                Name = StDtl.Name,
                                ProgramCode = StDtl.ProgramCode,
                                SessionID = StDtl.SessionID,
                                StartDate = StDtl.StartDate,
                                ProgramTestCalenderId = StDtl.ProgramTestCalenderId,
                                DayWiseScore = ReqData.Select(x => new DayWiseScoreBLL
                                {
                                    Day = x.Day,
                                    DayCount = x.DayCount,
                                    IsPresent = x.IsPresent != null ? 1 : 0,
                                    studentmrks = x.studentmrks,
                                    TotalQuestion = x.TotalQuestion,
                                    Total_Marks = x.Total_Marks,
                                    TypeOfTest = x.TypeOfTest

                                }).ToList()
                            });
                        }
                    }
                }
                foreach (var Row in objList)
                {
                    if (Row.DayWiseScore.Count < 15)
                    {
                        for (int i = Row.DayWiseScore.Count + 1; i <= 15; i++)
                        {
                            Row.DayWiseScore.Add(new DayWiseScoreBLL
                            {
                                Day = null,
                                DayCount = i,
                                IsPresent = 0,
                                studentmrks = null,
                                TotalQuestion = null,
                                Total_Marks = null,
                                TypeOfTest = null
                            });
                        }

                    }
                }
                return objList;
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

        public static List<MarksReportBLL> GetMarksReport(List<SessionIDListBLL> Object)
        {
            using (var context = new CEIDBEntities())
            {
                List<MarksReportBLL> objList = new List<MarksReportBLL>();
                foreach (var Obj in Object)
                {
                    var data = context.sp_GetStudentMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();

                    if (data.Count != 0)
                    {
                        var MSPinList = data.Select(x => x.MSPIN).Distinct().ToList();

                        foreach (var Mspin in MSPinList)
                        {
                            var TestDetails = data.Where(x => x.MSPIN == Mspin).FirstOrDefault();
                            var preTestDetails = data.Where(x => x.MSPIN == Mspin && x.TypeOfTest == "1" && x.SessionID == Obj.SessionID).FirstOrDefault();
                            var postTestDetails = data.Where(x => x.MSPIN == Mspin && x.TypeOfTest == "3" && x.SessionID == Obj.SessionID).FirstOrDefault();

                            objList.Add(new MarksReportBLL
                            {
                                IsPresentInPreTest = preTestDetails != null ? 1 : 0,
                                IsPresentInPostTest = postTestDetails != null ? 1 : 0,
                                SessionID = TestDetails.SessionID,
                                StartDate = TestDetails.StartDate,
                                MSPIN = TestDetails.MSPIN,
                                Name = TestDetails.Name,
                                PostTest_MarksObtained = postTestDetails != null ? postTestDetails.studentmrks : null,
                                PreTest_MarksObtained = preTestDetails != null ? preTestDetails.studentmrks : null,
                                ProgramCode = TestDetails.ProgramCode,
                                PostTestPercentage = postTestDetails != null ? postTestDetails.Percentage : null,
                                PreTestPercentage = preTestDetails != null ? preTestDetails.Percentage : null,
                            });
                        }
                    }
                }
                return objList;
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
                    return Msg;

                }

            }
            catch (Exception ex)
            {
                return "Error: Test not Initiated";
            }
        }

    }
}
