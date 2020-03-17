using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class ReportFilterBLL
    {
        public List<AgencyListForreportFilterBLL> AgencyList { get; set; }
        public List<ProgramListForReportInput> ProgramList { get; set; }
        public List<FacultyList> FacultyList { get; set; }
        public List<SessionListForReportFilter> SessionList { get; set; }
    }
    public class ProgramListForReportInput
    {
        public int ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public int? ProgramType_Id { get; set; }

    }
    public class SessionListForReportFilter
    {
        public string SessionID { get; set; }

    }
    public class MSPIN_SessionList
    {
        public string SessionID { get; set; }
        public string Mspin  { get; set; }
    }
    public class Filter_STEP_Agency
    {
        public string UserName { get; set; }
        public int Month { get; set; }
        public string ProgramCode { get; set; }
    }
}
