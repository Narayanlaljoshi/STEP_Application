using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEPDAL.DB;
using ProjectBLL.CustomModel;
using System.Data;
using System.Data.Entity;

namespace STEPDAL.CustomDAL
{
    public class FeedbackDAL
    {
        public static List<FeedbackQuestionsSet> GetFeedbackQuestion(int ProgramId, string SessionID, string MSPIN)
        {
            ProgramId = 16;
            List<FeedbackQuestionsSet> List = null;
            using (var Context = new CEIDBEntities())
            {
                var Data = Context.sp_GetFeedBackQueSet(ProgramId).ToList();
                if (Data.Count != 0) {
                    List = Data.Select(x => new FeedbackQuestionsSet {
                        ProgramId=x.ProgramId,
                        CreatedBy=null,
                        CreationDate=x.CreationDate,
                        FeedbackQuestion_Id=x.FeedbackQuestion_Id,
                        IsActive=x.IsActive,
                        ModifiedBy=x.ModifiedBy,
                        ModifiedDate=x.ModifiedDate,
                        ProgramTestCalenderId=x.ProgramTestCalenderId,
                        Question=x.Question,
                        MSPIN=MSPIN,
                        SessionID=SessionID,
                        Rating=0
                    }).ToList();
                }
                return List;
            }
        }
        public static string CaptureFeedback(List<FeedbackQuestionsSet> Obj)
        {
            string MSG = string.Empty;
            using (var Context = new CEIDBEntities())
            {
                foreach (var Row in Obj)
                {
                    int Status = Context.sp_Insert_Update_TblFeedbackCaptured(Row.FeedbackQuestion_Id, Row.MSPIN, Row.SessionID, Row.Rating, true, 1);
                    if (Status < 1)
                        MSG += "Error";
                }
                return MSG.Contains("Error")?"Error: Data not updated, Pls do submit again":"Success: Data updated successfuuly";
            }
        }

        public static string InsertQuestions(DataTable Tbl, FeedbackModelData Obj)
        {
            using (var context = new CEIDBEntities())
            {
                foreach (DataRow DR in Tbl.Rows)
                {
                    string Questions = string.Empty;
                    Questions = DR["Questions"].ToString();
                    if (Questions != "")
                    {
                        var insertData=context.sp_InsertFeedbackQuestions(Obj.ProgramId, 0, Questions, true, Obj.UserId, DateTime.Now);
                    }
                    //try
                    //{
                    //    if (Questions != "")
                    //    {
                    //        TblFeedbackQuestion TFQ = new TblFeedbackQuestion();
                    //        //     {
                    //        TFQ.ProgramId = Obj.ProgramId;
                    //        TFQ.ProgramTestCalenderId = 0;
                    //        TFQ.Question = Questions;
                    //        TFQ.IsActive = true;
                    //        TFQ.CreatedBy = Obj.UserId;
                    //        TFQ.CreationDate = DateTime.Now;
                    //     //   };
                    //        context.Entry(TFQ).State = EntityState.Added;
                    //        context.SaveChanges();
                    //    }
                    //}
                    //catch(Exception e)
                    //{
                    //    Console.WriteLine(e);
                    //}
                }
                return "Feedback Questions uploaded successfully";
            }
        }
    }
}
