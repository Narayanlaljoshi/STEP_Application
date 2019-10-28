using OfficeOpenXml;
using ProjectBLL.CustomModel;
using STEPDAL.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace STEPDAL.CustomDAL
{
    public class ReportsDAL
    {

        public static List<AttendanceReportBLL> GetAttendanceReport(ReportInputBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                List<AttendanceReportBLL> objList = new List<AttendanceReportBLL>();
                var ProgramList = context.sp_GetProgramListByEndDate_V2(Obj.EndDate).ToList();
                if (ProgramList.Count != 0)
                {
                    foreach (var prg in ProgramList)
                    {
                        if (prg.ProgramType_Id == 1)
                        {
                            //if(prg.sessionID== "SSI19158582")
                            //{ }

                            var data = context.sp_GetAttendanceReport(prg.StartDate, prg.EndDate, Obj.Agency_Id, prg.sessionID, Obj.Faculty_Id, prg.ProgramID).ToList();

                            foreach (var x in data)
                            {
                                objList.Add(new AttendanceReportBLL
                                {
                                    AgencyCode = x.AgencyCode,
                                    Co_id = x.Co_id,
                                    Day = x.Day,
                                    FacultyCode = x.FacultyCode,
                                    MSPIN = x.MSPIN,
                                    ProgramCode = x.ProgramCode,
                                    ProgramId = x.ProgramId,
                                    P_A = x.P_A,
                                    SessionID = x.SessionID,
                                    StartDate = x.StartDate.Value,
                                    Duration = x.Duration,
                                    EndDate = x.EndDate,
                                    Agency_Id = x.Agency_Id,
                                    MobileNo = x.MobileNo,
                                    Name = x.Name
                                });
                            }
                        }
                        else
                        {
                            var data = context.sp_GetAttendanceReport_Evaluation(prg.StartDate, prg.EndDate, Obj.Agency_Id, prg.sessionID, Obj.Faculty_Id, prg.ProgramID).ToList();
                            int? CourseDuration = data.Count != 0 ? data[0].Duration : 0;
                            var MSPINList = data.Select(x => x.MSPIN).Distinct();
                            //objList = new List<AttendanceReportBLL>();
                            foreach (var MSPIN in MSPINList)
                            {
                                var Details = data.Where(x => x.MSPIN == MSPIN).ToList();
                                int Day = 1;
                                for (int i = 0; i < CourseDuration; i++)
                                {
                                    if (Details.Count >= i + 1)
                                    {
                                        objList.Add(new AttendanceReportBLL
                                        {
                                            AgencyCode = Details[i].AgencyCode,
                                            Co_id = Details[i].Co_id,
                                            Day = Day,
                                            FacultyCode = Details[i].FacultyCode,
                                            MSPIN = Details[i].MSPIN,
                                            ProgramCode = Details[i].ProgramCode,
                                            ProgramId = Details[i].ProgramId,
                                            P_A = Details[i].P_A,
                                            SessionID = Details[i].SessionID,
                                            StartDate = Details[i].StartDate.Value,
                                            Duration = Details[i].Duration,
                                            EndDate = Details[i].EndDate,
                                            Agency_Id = Details[i].Agency_Id,
                                            MobileNo = Details[i].MobileNo,
                                            Name = Details[i].Name
                                        });
                                    }
                                    else
                                    {
                                        objList.Add(new AttendanceReportBLL
                                        {
                                            AgencyCode = Details[0].AgencyCode,
                                            Co_id = Details[0].Co_id,
                                            Day = Day,
                                            FacultyCode = Details[0].FacultyCode,
                                            MSPIN = Details[0].MSPIN,
                                            ProgramCode = Details[0].ProgramCode,
                                            ProgramId = Details[0].ProgramId,
                                            P_A = "A",
                                            SessionID = Details[0].SessionID,
                                            StartDate = Details[0].StartDate.Value,
                                            Duration = Details[0].Duration,
                                            EndDate = Details[0].EndDate,
                                            Agency_Id = Details[0].Agency_Id,
                                            MobileNo = Details[0].MobileNo,
                                            Name = Details[0].Name
                                        });
                                    }
                                    Day++;
                                }
                            }
                        }
                    }
                }
                return objList;
            }
        }
        public static ReportFilterBLL GetReportFilter()
        {
            ReportFilterBLL ReportFilter = new ReportFilterBLL();
            using (var context = new CEIDBEntities())
            {
                List<AgencyListForreportFilterBLL> AgencyList = null;
                List<ProgramListForReportInput> ProgramList = null;
                List<FacultyList> FacultyList = null;
                List<SessionListForReportFilter> SessionList = null;
                //var AgencyListData = context.sp_GetAgencyList().ToList();
                //var FacultyListData = context.sp_GetFacultyList(null).ToList();
                var ProgramListData = context.sp_GetProgramList().ToList();
                //var SessionListData = context.sp_GetSessionList().ToList();
                //if (AgencyListData!=null) {
                //    AgencyList = AgencyListData.Select(x => new AgencyListForreportFilterBLL {
                //        AgencyCode=x.AgencyCode,
                //        AgencyName=x.AgencyName,
                //        Agency_Id=x.Agency_Id
                //    }).ToList();
                //}
                //else {
                //    AgencyList = null;
                //}
                if (ProgramListData != null)
                {
                    ProgramList = ProgramListData.Select(x => new ProgramListForReportInput
                    {
                        ProgramCode = x.ProgramCode,
                        ProgramId = x.ProgramId,
                        ProgramName = x.ProgramName,
                        ProgramType_Id = x.ProgramType_Id
                    }).ToList();
                }
                else
                {
                    ProgramList = null;
                }
                //if (FacultyListData!=null) { 
                //FacultyList = FacultyListData.Select(x => new FacultyList {
                //    Faculty_Id=x.Faculty_Id,
                //    FacultyName=x.FacultyName,
                //    FacultyCode=x.FacultyCode,
                //}).ToList();
                //}
                //else {
                //    FacultyList = null;
                //}
                //if (SessionListData!=null) { 
                //SessionList = SessionListData.Select(x => new SessionListForReportFilter {
                //SessionID=x.SessionID,
                //}).ToList();
                //}
                //else {
                //    SessionList = null;
                //}

                ReportFilter.AgencyList = AgencyList;
                ReportFilter.FacultyList = FacultyList;
                ReportFilter.SessionList = SessionList;
                ReportFilter.ProgramList = ProgramList;

                return ReportFilter;
            }

        }

        public static List<MarksReportBLL> GetMarksReport(List<SessionIDListBLL> Object)
        {
            using (var context = new CEIDBEntities())
            {
                List<MarksReportBLL> objList = new List<MarksReportBLL>();
                foreach (var Obj in Object)
                {
                    //var data = context.sp_GetMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();
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


                            //int? PostTest_MarksObtained = 0;
                            //int? PostTestMaxMarks = 0;
                            //int? PreTestMaxMarks = 0;
                            //int? PreTest_MarksObtained = 0;
                            //if (postTestDetails.Count != 0)
                            //{
                            //    PostTest_MarksObtained = postTestDetails.Where(x => x.IsAnswerCorrect == true).Sum(x => x.MarksScored);
                            //    //postTestDetails = postTestDetails.Select(x => x.Day ,x.Total_Marks).Distinct();
                            //    PostTestMaxMarks = postTestDetails.Sum(x => x.Total_Marks);
                            //}

                            //if (preTestDetails.Count != 0)
                            //{
                            //    PreTestMaxMarks = preTestDetails[0].Total_Marks;
                            //    PreTest_MarksObtained = preTestDetails.Where(x => x.IsAnswerCorrect == true).Sum(x => x.MarksScored);
                            //}

                            //objList.Add(new MarksReportBLL
                            //{
                            //    IsPresentInPreTest = preTestDetails.Count != 0 ? 1 : 0,
                            //    IsPresentInPostTest = postTestDetails.Count != 0 ? 1 : 0,
                            //    SessionID = TestDetails.SessionID,
                            //    StartDate = TestDetails.StartDate,
                            //    MSPIN = TestDetails.MSPIN,
                            //    Duration = TestDetails.Duration,
                            //    EndDate = TestDetails.EndDate,
                            //    FacultyCode = TestDetails.FacultyCode,
                            //    Name = TestDetails.Name,
                            //    PostTestMaxMarks = PostTestMaxMarks,
                            //    PreTestMaxMarks = PreTestMaxMarks,
                            //    PostTest_MarksObtained = PostTest_MarksObtained.Value,
                            //    PreTest_MarksObtained = PreTest_MarksObtained.Value,
                            //    ProgramCode = TestDetails.ProgramCode,
                            //    PreTotalMarks = TestDetails.TypeOfTest == "1" ? TestDetails.Total_Marks : 0,
                            //    PostTotalMarks = TestDetails.TypeOfTest == "3" ? TestDetails.Total_Marks : 0
                            //});
                        }
                    }
                }
                return objList;
            }
        }
        public static List<MarksReportBLL> GetMarksReportAsperDMS(SessionIDListBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                List<MarksReportBLL> objList = new List<MarksReportBLL>();

                //var data = context.sp_GetMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();
                var data = context.sp_GetStudentMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();

                if (data.Count != 0)
                {
                    var MSPinList = data.Select(x => x.MSPIN).Distinct().ToList();

                    foreach (var Mspin in MSPinList)
                    {
                        var TestDetails = data.Where(x => x.MSPIN == Mspin).FirstOrDefault();
                        var preTestDetails = data.Where(x => x.MSPIN == Mspin && x.TypeOfTest == "1").FirstOrDefault();
                        var postTestDetails = data.Where(x => x.MSPIN == Mspin && x.TypeOfTest == "3").FirstOrDefault();

                        objList.Add(new MarksReportBLL
                        {
                            Co_id = TestDetails.Co_id,
                            IsPresentInPreTest = preTestDetails != null ? 1 : 0,
                            IsPresentInPostTest = postTestDetails != null ? 1 : 0,
                            SessionID = TestDetails.SessionID,
                            StartDate = TestDetails.StartDate,
                            MSPIN = TestDetails.MSPIN,
                            Name = TestDetails.Name,
                            FacultyCode = TestDetails.FacultyCode,
                            PostTest_MarksObtained = postTestDetails != null ? postTestDetails.studentmrks : null,
                            PreTest_MarksObtained = preTestDetails != null ? preTestDetails.studentmrks : null,
                            ProgramCode = TestDetails.ProgramCode,
                            PostTestPercentage = postTestDetails != null ? postTestDetails.Percentage : null,
                            PreTestPercentage = preTestDetails != null ? preTestDetails.Percentage : null,
                        });


                        //int? PostTest_MarksObtained = 0;
                        //int? PostTestMaxMarks = 0;
                        //int? PreTestMaxMarks = 0;
                        //int? PreTest_MarksObtained = 0;
                        //if (postTestDetails.Count != 0)
                        //{
                        //    PostTest_MarksObtained = postTestDetails.Where(x => x.IsAnswerCorrect == true).Sum(x => x.MarksScored);
                        //    //postTestDetails = postTestDetails.Select(x => x.Day ,x.Total_Marks).Distinct();
                        //    PostTestMaxMarks = postTestDetails.Sum(x => x.Total_Marks);
                        //}

                        //if (preTestDetails.Count != 0)
                        //{
                        //    PreTestMaxMarks = preTestDetails[0].Total_Marks;
                        //    PreTest_MarksObtained = preTestDetails.Where(x => x.IsAnswerCorrect == true).Sum(x => x.MarksScored);
                        //}

                        //objList.Add(new MarksReportBLL
                        //{
                        //    IsPresentInPreTest = preTestDetails.Count != 0 ? 1 : 0,
                        //    IsPresentInPostTest = postTestDetails.Count != 0 ? 1 : 0,
                        //    SessionID = TestDetails.SessionID,
                        //    StartDate = TestDetails.StartDate,
                        //    MSPIN = TestDetails.MSPIN,
                        //    Duration = TestDetails.Duration,
                        //    EndDate = TestDetails.EndDate,
                        //    FacultyCode = TestDetails.FacultyCode,
                        //    Name = TestDetails.Name,
                        //    PostTestMaxMarks = PostTestMaxMarks,
                        //    PreTestMaxMarks = PreTestMaxMarks,
                        //    PostTest_MarksObtained = PostTest_MarksObtained.Value,
                        //    PreTest_MarksObtained = PreTest_MarksObtained.Value,
                        //    ProgramCode = TestDetails.ProgramCode,
                        //    PreTotalMarks = TestDetails.TypeOfTest == "1" ? TestDetails.Total_Marks : 0,
                        //    PostTotalMarks = TestDetails.TypeOfTest == "3" ? TestDetails.Total_Marks : 0
                        //});
                    }
                }

                return objList;
            }
        }
        public static List<MarksReportBLL> GetMarksReportAsperDMS_V2(SessionIDListBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                List<MarksReportBLL> objList = new List<MarksReportBLL>();
                ReportInputBLL RpIn = new ReportInputBLL { EndDate = Obj.EndDate };
                var data = context.sp_GetStudentMarksReport_V2(Obj.EndDate).ToList();
                var AttRept = ReportsDAL.GetAttendanceReport(RpIn);
                if (data.Count != 0)
                {
                    var Session_MSPinList = context.sp_GetStudentMarksReport_DistinctSession_MSPIN(Obj.EndDate).ToList();
                    foreach (var item in Session_MSPinList)
                    {
                        //if (item.MSPIN == "774589")
                        //{ }
                        var CheckIfAbsent = AttRept.Where(x => x.MSPIN == item.MSPIN && x.SessionID == item.SessionID && x.P_A == "A").FirstOrDefault();
                        var TestDetails = data.Where(x => x.MSPIN == item.MSPIN && x.SessionID == item.SessionID).FirstOrDefault();
                        var preTestDetails = data.Where(x => x.MSPIN == item.MSPIN && x.SessionID == item.SessionID && x.TypeOfTest == "1").ToList();
                        var postTestDetails = data.Where(x => x.MSPIN == item.MSPIN && x.SessionID == item.SessionID && x.TypeOfTest == "3").ToList();
                        //var CheckIfAbsent2 = AttRept.Where(x => x.MSPIN == item.Mspin && x.SessionID == item.SessionID && x.P_A == "A").ToList();
                        if (CheckIfAbsent == null)
                        {
                            objList.Add(new MarksReportBLL
                            {
                                Co_id = TestDetails.Co_id,
                                IsPresentInPreTest = preTestDetails != null ? 1 : 0,
                                IsPresentInPostTest = postTestDetails != null ? 1 : 0,
                                SessionID = TestDetails.SessionID,
                                StartDate = TestDetails.StartDate,
                                MSPIN = TestDetails.MSPIN,
                                Name = TestDetails.Name,
                                FacultyCode = TestDetails.FacultyCode,
                                PostTest_MarksObtained = postTestDetails.Count != 0 ? postTestDetails.Sum(x => x.StudentMarks) : 0,
                                PreTest_MarksObtained = preTestDetails.Count != 0 ? preTestDetails.Sum(x => x.StudentMarks) : 0,
                                ProgramCode = TestDetails.ProgramCode,
                                PostTestPercentage = postTestDetails.Count != 0 ? postTestDetails.Sum(x => x.Percentage) : 0,
                                PreTestPercentage = preTestDetails.Count != 0 ? preTestDetails.Sum(x => x.Percentage) : 0,
                            });
                        }
                        else
                        {
                            objList.Add(new MarksReportBLL
                            {
                                Co_id = TestDetails.Co_id,
                                IsPresentInPreTest = preTestDetails != null ? 1 : 0,
                                IsPresentInPostTest = postTestDetails != null ? 1 : 0,
                                SessionID = TestDetails.SessionID,
                                StartDate = TestDetails.StartDate,
                                MSPIN = TestDetails.MSPIN,
                                Name = TestDetails.Name,
                                FacultyCode = TestDetails.FacultyCode,
                                PostTest_MarksObtained = 0,
                                PreTest_MarksObtained = 0,
                                ProgramCode = TestDetails.ProgramCode,
                                PostTestPercentage = 0,
                                PreTestPercentage = 0,
                            });
                        }
                    }
                }

                return objList;
            }
        }
        public static Boolean GenerateMarksReportAsperDMS_Phase_2()
        {
            //bool IsGenerated = false;
            using (var context = new CEIDBEntities())
            {
                int Status = context.sp_generateMarksReportDMS();
                if (Status >= 1)
                    return true;
                else
                    return false;
            }
        }
        public static List<DayWiseReportBLL> GetMarksReportForAdmin(SessionIDListBLL Obj)
        {
            using (var context = new CEIDBEntities())
            {
                context.Database.CommandTimeout = 1000000;
                List<DayWiseReportBLL> objList = new List<DayWiseReportBLL>();

                //var data = context.sp_GetMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();
                if (Obj.FacultyCode != null)
                {
                    Obj.Faculty_Id = context.TblFaculties.Where(x => x.FacultyCode == Obj.FacultyCode).Select(x => x.Faculty_Id).FirstOrDefault();
                }
                var data = context.sp_GetDayWiseMarksReport(Obj.Agency_Id, Obj.Faculty_Id, Obj.SessionID, Obj.ProgramId, Obj.StartDate, Obj.EndDate).ToList();

                if (data.Count != 0)
                {
                    var MSPinList = data.Select(x => x.MSPIN).Distinct().ToList();

                    foreach (var Mspin in MSPinList)
                    {
                        //if (Mspin=="777241")
                        //{ }
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

        public static PracticalMarksAndExcel GetMarksReport(EligibleCandidatesForEvaluation Obj)
        {
            using (var context = new CEIDBEntities())
            {
                PracticalMarksAndExcel PracticalMarksAndExcel = new PracticalMarksAndExcel();
                List<DayWisePracticalMarkSheet> objList = new List<DayWisePracticalMarkSheet>();

                var data = context.sp_GetDayWisePracticalMarkSheet(Obj.MSPIN, Obj.SessionID, Obj.Day).ToList();

                if (data.Count != 0)
                {
                    var MarksEarned = data.Sum(x => x.Total);

                    MarksEarned = MarksEarned + data[0].PracticalDefaultMarks;
                    if (MarksEarned > data[0].PracticalMaxMarks)
                    { MarksEarned = data[0].PracticalMaxMarks; }
                    else if (MarksEarned < data[0].PracticalMinMarks)
                    { MarksEarned = data[0].PracticalMinMarks; }

                    objList = data.Select(x => new DayWisePracticalMarkSheet
                    {
                        Day = x.Day,
                        SessionID = x.SessionID,
                        MSPIN = x.MSPIN,
                        ActionA = x.ActionA,
                        ActionB = x.ActionB,
                        ActionC = x.ActionC,
                        ActionD = x.ActionD,
                        ActionE = x.ActionE,
                        ActionF = x.ActionF,
                        Marks_A = x.Marks_A,
                        Marks_B = x.Marks_B,
                        Marks_C = x.Marks_C,
                        Marks_D = x.Marks_D,
                        Marks_E = x.Marks_E,
                        Marks_F = x.Marks_F,
                        PracticalDefaultMarks = x.PracticalDefaultMarks,
                        PracticalMaxMarks = x.PracticalMaxMarks,
                        PracticalMinMarks = x.PracticalMinMarks,
                        ProgramTestCalenderId = x.ProgramTestCalenderId,
                        Question = x.Question,
                        Total = x.Total
                    }).ToList();
                    string ExcelUrl = GenerateExcel(objList);
                    PracticalMarksAndExcel.ExcelUrl = ExcelUrl;
                    PracticalMarksAndExcel.DayWisePracticalMarkSheet = objList;
                    PracticalMarksAndExcel.MarksEarned = Convert.ToInt32(MarksEarned);
                }
                return PracticalMarksAndExcel;
            }
        }

        public static string GenerateExcel(List<DayWisePracticalMarkSheet> List)
        {
            var ServerIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"].ToString();
            var Path = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelPath"].ToString());
            string DTics = DateTime.Now.Ticks.ToString();
            if (List.Count != 0)
            {
                ExcelPackage ExcelPkg = new ExcelPackage();
                ExcelWorksheet wsSheet1 = ExcelPkg.Workbook.Worksheets.Add("Sheet1");
                //using (ExcelRange Rng = wsSheet1.Cells[2, 2, 2, 2])
                //{
                //    Rng.Value = "Welcome to Everyday be coding - tutorials for beginners";
                //    Rng.Style.Font.Size = 16;
                //    Rng.Style.Font.Bold = true;
                //    Rng.Style.Font.Italic = true;
                //}
                var MarksEarned = List.Sum(x => x.Total);
                MarksEarned = MarksEarned + List[0].PracticalDefaultMarks;
                if (MarksEarned > List[0].PracticalMaxMarks)
                { MarksEarned = List[0].PracticalMaxMarks; }
                else if (MarksEarned < List[0].PracticalMinMarks)
                { MarksEarned = List[0].PracticalMinMarks; }

                ExcelRange Rng = wsSheet1.Cells[1, 2, 1, 2];
                Rng.Value = "MSPIN: " + List[0].MSPIN;
                Rng.Style.Font.Size = 16;
                Rng = wsSheet1.Cells[1, 5, 1, 5];
                Rng.Value = "Practical Max Marks: " + List[0].PracticalMaxMarks;
                Rng.Style.Font.Size = 16;
                Rng = wsSheet1.Cells[2, 2, 2, 2];
                Rng.Value = "Marks Earned: " + MarksEarned;
                Rng.Style.Font.Size = 16;
                Rng = wsSheet1.Cells[2, 5, 2, 5];
                Rng.Value = "Default Marks: " + List[0].PracticalDefaultMarks;
                Rng.Style.Font.Size = 16;
                wsSheet1.Protection.IsProtected = false;
                wsSheet1.Protection.AllowSelectLockedCells = false;
                wsSheet1.Cells[4, 1].Value = "Question";
                wsSheet1.Cells[4, 2].Value = "ActionA";
                wsSheet1.Cells[4, 3].Value = "ActionB";
                wsSheet1.Cells[4, 4].Value = "ActionC";
                wsSheet1.Cells[4, 5].Value = "ActionD";
                wsSheet1.Cells[4, 6].Value = "ActionE";
                wsSheet1.Cells[4, 7].Value = "ActionF";
                wsSheet1.Cells[4, 8].Value = "Marks_A";
                wsSheet1.Cells[4, 9].Value = "Marks_B";
                wsSheet1.Cells[4, 10].Value = "Marks_C";
                wsSheet1.Cells[4, 11].Value = "Marks_D";
                wsSheet1.Cells[4, 12].Value = "Marks_E";
                wsSheet1.Cells[4, 13].Value = "Marks_F";
                wsSheet1.Cells[4, 14].Value = "Total";
                wsSheet1.Cells[4, 1, 4, 14].Style.Font.Bold = true;
                int i = 5;
                foreach (var item in List)
                {
                    wsSheet1.Cells[i, 1].Value = item.Question;
                    wsSheet1.Cells[i, 2].Value = item.ActionA;
                    wsSheet1.Cells[i, 3].Value = item.ActionB;
                    wsSheet1.Cells[i, 4].Value = item.ActionC;
                    wsSheet1.Cells[i, 5].Value = item.ActionD;
                    wsSheet1.Cells[i, 6].Value = item.ActionE;
                    wsSheet1.Cells[i, 7].Value = item.ActionF;
                    wsSheet1.Cells[i, 8].Value = item.Marks_A;
                    wsSheet1.Cells[i, 9].Value = item.Marks_B;
                    wsSheet1.Cells[i, 10].Value = item.Marks_C;
                    wsSheet1.Cells[i, 11].Value = item.Marks_D;
                    wsSheet1.Cells[i, 12].Value = item.Marks_E;
                    wsSheet1.Cells[i, 13].Value = item.Marks_F;
                    wsSheet1.Cells[i, 14].Value = item.Total;
                    i++;
                }
                wsSheet1.Cells[i, 1].Value = "Total";
                wsSheet1.Cells[i, 14].Value = List.Sum(x => x.Total);
                wsSheet1.Cells[i, 1, i, 14].Style.Font.Bold = true;
                wsSheet1.Cells[1, 1, i, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                wsSheet1.Cells[1, 1, i, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                wsSheet1.Cells[5, 1, i, 14].Style.WrapText = true;
                ExcelPkg.SaveAs(new FileInfo(Path + List[0].MSPIN + "_" + DTics + ".xlsx"));
                ExcelPkg.Dispose();
                return ServerIP + "/Uploaded/" + List[0].MSPIN + "_" + DTics + ".xlsx";
            }
            return string.Empty;
        }


        public static List<DayWiseReportVendor> GetVendorAttendanceReport(ReportFilter_Vendor Obj)
        {
            using (var context = new CEIDBEntities())
            {
                List<DayWiseReportVendor> objList = new List<DayWiseReportVendor>();

                var SessionList = context.sp_SessionIDsFor_AgencyManagerReport(Obj.ManagerID, Obj.StartDate, Obj.EndDate, Obj.ProgramId, Obj.Trainer_Id, Obj.SessionID).ToList();
                if (SessionList.Count != 0)
                {
                    foreach (var item in SessionList)
                    {
                        var data = context.sp_GetVendorReport_Attendance(item.SessionID).ToList();

                        if (data.Count != 0)
                        {

                            var MSPINList = data.Select(x => x.MSPIN).Distinct().ToList();
                            foreach (var MSPIN in MSPINList)
                            {
                                var DayList = data.Where(x => x.MSPIN == MSPIN).ToList();
                                DayWiseReportVendor Tc = new DayWiseReportVendor
                                {
                                    SessionID = DayList[0].SessionID,
                                    Agency_Id = DayList[0].Agency_Id,
                                    MSPIN = DayList[0].MSPIN,
                                    Name = DayList[0].Name,
                                    ProgramCode = DayList[0].ProgramCode,
                                    StartDate = DayList[0].StartDate,
                                    TrainerCode = item.TrainerCode,
                                    DayWiseScore = DayList.Select(x => new DayWiseScore_Vendor
                                    {
                                        DayCount = x.DayCount,
                                        Day = x.DayCount,
                                        IsPresent = x.P_A,
                                    }).ToList()
                                };
                                objList.Add(Tc);
                            }
                        }
                    }
                    foreach (var Row in objList)
                    {
                        if (Row.DayWiseScore.Count < 15)
                        {
                            for (int i = Row.DayWiseScore.Count + 1; i <= 15; i++)
                            {
                                Row.DayWiseScore.Add(new DayWiseScore_Vendor
                                {
                                    Day = null,
                                    DayCount = i,
                                    IsPresent = "",
                                    studentmrks = null,
                                    TotalQuestion = null,
                                    Total_Marks = null,
                                    TypeOfTest = null
                                });
                            }

                        }
                    }

                }
                return objList;
            }
        }
        public static List<DayWiseReportVendor> GetVendorMarksReport(ReportFilter_Vendor Obj)
        {
            using (var context = new CEIDBEntities())
            {
                List<DayWiseReportVendor> objList = new List<DayWiseReportVendor>();

                var SessionList = context.sp_SessionIDsFor_AgencyManagerReport(Obj.ManagerID, Obj.StartDate, Obj.EndDate, Obj.ProgramId, Obj.Trainer_Id, Obj.SessionID).ToList();
                if (SessionList.Count != 0)
                {
                    foreach (var item in SessionList)
                    {
                        var ATDNC = context.sp_GetVendorReport_Attendance(item.SessionID).ToList();

                        var data = context.sp_GetVendorReport_Marks_V2(item.SessionID, item.ProgramType_Id, item.ProgramId).ToList();
                        var MSPINList = data.Select(x => x.MSPIN).Distinct().ToList();
                        foreach (var MSPIN in MSPINList)
                        {
                            var DayList = data.Where(x => x.MSPIN == MSPIN).ToList();
                            DayWiseReportVendor Tc = new DayWiseReportVendor
                            {
                                SessionID = DayList[0].SessionID,
                                Agency_Id = DayList[0].Agency_Id,
                                MSPIN = DayList[0].MSPIN,
                                Name = DayList[0].Name,
                                ProgramCode = DayList[0].ProgramCode,
                                StartDate = DayList[0].StartDate,
                                TrainerCode = item.TrainerCode,
                                DayWiseScore = DayList.Select(x => new DayWiseScore_Vendor
                                {
                                    DayCount = x.DayCount,
                                    Day = x.DayCount,
                                    //IsPresent = x.P_A,
                                    studentmrks = x.Marks
                                }).ToList()
                            };
                            objList.Add(Tc);
                            //var CheckATNDC = ATDNC.Where(x => x.MSPIN == MSPIN && x.SessionID == item.SessionID && x.P_A == "A").FirstOrDefault();
                            //var MSPINDTL = data.Where(x => x.MSPIN == MSPIN).FirstOrDefault();
                            //var PreTestDTL = data.Where(x => x.MSPIN == MSPIN && x.SessionID == item.SessionID && x.TypeOfTest == "1").ToList();
                            //var PostTestDTL = data.Where(x => x.MSPIN == MSPIN && x.SessionID == item.SessionID && x.TypeOfTest == "3").ToList();
                            //if (CheckATNDC == null)
                            //{
                            //    objList.Add(new MarksReport_Vendor
                            //    {
                            //        Co_id = MSPINDTL.Co_id,
                            //        Duration = MSPINDTL.Duration,
                            //        EndDate = MSPINDTL.EndDate,
                            //        FacultyCode = MSPINDTL.FacultyCode,
                            //        MSPIN = MSPINDTL.MSPIN,
                            //        Name = MSPINDTL.Name,
                            //        ProgramCode = MSPINDTL.ProgramCode,
                            //        StartDate = MSPINDTL.StartDate,
                            //        SessionID = MSPINDTL.SessionID,
                            //        IsPresentInPreTest = PreTestDTL.Count == 0 ? 0 : 1,
                            //        IsPresentInPostTest = PostTestDTL.Count == 0 ? 0 : 1,
                            //        //PostTestMaxMarks = PostTestDTL.Count != 0 ? PostTestDTL[0].Total_Marks : 1,
                            //        //PostTestPercentage=0,
                            //        PostTest_MarksObtained = PostTestDTL.Count != 0 ? PostTestDTL.Sum(x => x.Marks) : 0,
                            //        //PostTotalMarks=,
                            //        //PreTestMaxMarks = PreTestDTL.Count != 0 ? PreTestDTL[0].Total_Marks : 1,
                            //        //PreTestPercentage=,
                            //        PreTest_MarksObtained = PreTestDTL.Count != 0 ? PreTestDTL.Sum(x => x.Marks) : 0,
                            //        //PreTotalMarks=
                            //    });
                            //}
                        }

                    }
                    foreach (var Row in objList)
                    {
                        if (Row.DayWiseScore.Count < 15)
                        {
                            for (int i = Row.DayWiseScore.Count + 1; i <= 15; i++)
                            {
                                Row.DayWiseScore.Add(new DayWiseScore_Vendor
                                {
                                    Day = null,
                                    DayCount = i,
                                    IsPresent = "",
                                    studentmrks = null,
                                    TotalQuestion = null,
                                    Total_Marks = null,
                                    TypeOfTest = null
                                });
                            }

                        }
                    }

                }
                return objList;
            }
        }

        public static List<ProgramList_For_Filter_Vendor> GetProgramList(ReportFilter_Vendor Obj)
        {
            using (var context = new CEIDBEntities())
            {
                List<ProgramList_For_Filter_Vendor> objList = new List<ProgramList_For_Filter_Vendor>();

                var PRGList = context.sp_GetProgramListForFilter_Vendor(Obj.StartDate, Obj.EndDate).ToList();
                if (PRGList.Count != 0)
                {
                    objList = PRGList.Select(x => new ProgramList_For_Filter_Vendor
                    {
                        EndDate = x.EndDate,
                        ProgramCode = x.ProgramCode,
                        ProgramID = x.ProgramID,
                        SessionID = x.SessionID,
                        StartDate = x.StartDate,
                        ProgramName = x.ProgramName,
                        ProgramType_Id = x.ProgramType_Id
                    }).ToList();
                }
                return objList;
            }
        }
    }
}
