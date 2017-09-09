using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class UserExt
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public int? RoleID { get; set; }
        public string RoleName { get; set; }
        public string ListZoneName { get; set; }
    }
}