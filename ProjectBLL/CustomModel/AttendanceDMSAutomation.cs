using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class AttendanceDMSAutomation
    {
        public string LV_FACULTY { get; set; }
        public string LV_PROGRAM { get; set; }
        public string LV_SESSION_ID { get; set; }
        public DateTime? LV_FROM_DATE { get; set; }
        public string LV_MSPIN { get; set; }
        public int? LV_DAYS { get; set; }
        public string LV_ATTENDANCE { get; set; }
    }
    public class ScorreDMSAutomation
    {
        public string LV_FACULTY { get; set; }
        public string LV_PROGRAM { get; set; }
        public string LV_SESSION_ID { get; set; }
        public DateTime? LV_FROM_DATE { get; set; }
        public int? LV_FIN_YEAR { get; set; }
        public string LV_MSPIN { get; set; }
        public string PN_NAME{ get; set; }
        public int? LV_PRE_SCORE { get; set; }
        public int? LV_POST_SCORE { get; set; }
    }
    public class PO_ERR_MSGS_Score
    {
        public string COL_VAR1 { get; set; }
        public string COL_VAR2 { get; set; }
        public string COL_VAR3 { get; set; }
        public string COL_DATE1 { get; set; }
        public string COL_VAR4 { get; set; }
        public string COL_VAR5 { get; set; }
        public int COL_NUM1 { get; set; }
        public int COL_NUM2 { get; set; }
    }
    public class PO_ERR_MSGS_Attdn
    {
        public string COL_VAR1 { get; set; }
        public string COL_VAR2 { get; set; }
        public string COL_VAR3 { get; set; }
        public string COL_DATE1 { get; set; }
        public string COL_VAR4 { get; set; }
        public int COL_NUM1 { get; set; }
        public string COL_VAR5 { get; set; }
    }
}
