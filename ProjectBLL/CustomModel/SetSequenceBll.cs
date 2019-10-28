using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class SetSequenceBll
    {
        public int Set_Id { get; set; }
        public string SetNumber { get; set; }
    }
    public class SetSequenceForProgramTestCalenderId
    {
        public Nullable<int> Set_Id { get; set; }
        public string Set_Title { get; set; }
        public int ProgramTestCalenderId { get; set; }
        public Nullable<int> ProgramId { get; set; }
    }
}
