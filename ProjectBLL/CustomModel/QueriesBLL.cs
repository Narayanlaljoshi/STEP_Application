using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class CloseQueryBLL
    {
        public int Query_Id { get; set; }
        public string Remarks { get; set; }
        public int User_Id { get; set; }

    }
    public class QueriesBLL
    {

        public int Id { get; set; }
        public string QueryBody { get; set; }
        public string QuerySubject { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public Nullable<int> IsClosed { get; set; }
        public int User_Id { get; set; }
        public string RoleName { get; set; }
        public string Remarks { get; set; }
        public string AgencyType { get; set; }
        public string Email { get; set; }

        public string AttachmentName { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentType { get; set; }
    }
    public class SaveQueryBLL
    {
        public int Id { get; set; }
        public string QuerySubject { get; set; }
        public string QueryBody { get; set; }
        public Nullable<int> CurrentStatus_Id { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public int User_Id { get; set; }

        public string AttachmentType { get; set; }

    }

    public class QueryAttachementBLL
    {
        public string Path { get; set; }
        public int User_Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int Query_Id { get; set; }
        public string AttachmentType { get; set; }

    }
}

