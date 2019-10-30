﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBLL.CustomModel
{
    public class FacultyDetailsBLL
    {
        public int Faculty_Id { get; set; }
        public int Agency_Id { get; set; }
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public int ParentFaculty_Id { get; set; }
        public string ParentFaculty_Code { get; set; }
        public string Email { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}