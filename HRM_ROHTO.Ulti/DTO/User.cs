//using Maple.Lib.Database.ADOProvider.Attributes;
using System;
using System.Drawing;

namespace HRM_ROHTO.Util.DTO
{
    public class User
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Guid StoreID { get; set; }
        public Guid? RoleID { get; set; }
     
    }
}