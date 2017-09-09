using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class EmployeeExt
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string ContractNo { get; set; }
        public string CardNumber { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string IsLeaveOut { get; set; }
    }
}