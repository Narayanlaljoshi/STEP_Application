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
        public int Status_Id { get; set; }
        public string Remarks { get; set; }
        public int User_Id { get; set; }

    }
    public class SummaryReport
    {
        public string AgencyCode { get; set; }
        public int TotalQueries { get; set; }
        public int Closed { get; set; }
        public int Pending { get; set; }

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
        public string Name { get; set; }
        public string Remarks { get; set; }
        public string AgencyCode { get; set; }
        public string Email { get; set; }
        public int Status_Id { get; set; }
        public string StatusName { get; set; }

        public List<Attachments> AttachmentsList { get; set; }
        //public string AttachmentName { get; set; }
        //public string AttachmentPath { get; set; }
        //public string AttachmentType { get; set; }
    }
    public class Attachments {
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
    public class EmployeeDetails {
        public string QuerySubject { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmployeeCode { get; set; }
    }
    public class StatusdetailInfo
    {
        public long Query_Id { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string Added_By { get; set; }
        public System.DateTime Added_Date { get; set; }
        //public string EMP_CODE { get; set; }
        //public string Emp_First_name { get; set; }
        //public string FutureStatus { get; set; }
    }
    public class FutureStatus
    {
        public string Status { get; set; }
        public string EmployeeCode { get; set; }
    }
}

