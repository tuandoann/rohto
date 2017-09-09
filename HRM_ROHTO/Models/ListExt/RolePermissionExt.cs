using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class RolePermissionExt
    {
        public int RoleID { get; set; }
        public string FunctionName { get; set; }
        public string Description { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Print { get; set; }
        public bool Export { get; set; }
        public bool Import { get; set; }
    }
}