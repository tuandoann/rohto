using HRM_ROHTO.Models.ListExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Role : EfRepositoryBase<SYS_ROLE>
    {
        #region Constructor
        private static volatile DA_Role _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Role Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Role();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public bool CheckRoleNameNotExist(int RoleID, string RoleName)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_ROLE ds = context.SYS_ROLE.FirstOrDefault(m => m.RoleID != RoleID && m.RoleName == RoleName);
                if (ds != null) // exists
                {
                    return false;
                }
            }
            return true;
        }

        public List<SelectListItem> GetListForCombobox()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.SYS_ROLE
                         orderby b.RoleName
                         select
                         new { b.RoleID, b.RoleName };
                if (ds.Any())
                {
                    foreach (var item in ds)
                    {
                        SelectListItem b = new SelectListItem { Value = item.RoleID.ToString(), Text = item.RoleName };
                        lst.Add(b);
                    }
                }

            }
            return lst;
        }

        public int GetMaxRoleID()
        {
            int RoleId = 0;
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                try
                {
                    RoleId = context.SYS_ROLE.Max(m => m.RoleID);
                }
                catch (Exception ex)
                {
                }
            }
            return RoleId;
        }

        public List<RolePermissionExt> GetListRolePermission(int RoleID)
        {
            List<RolePermissionExt> lst = new List<RolePermissionExt>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.SYS_FUNCTION
                         //join c in context.SYS_ROLEPERMISSION on b.FunctionName equals c.FunctionName into ad
                         //from c in ad.DefaultIfEmpty()
                         //join d in context.SYS_ROLE on c.RoleID equals d.RoleID
                         //where d.RoleID == RoleID
                         orderby b.Description
                         select new
                         {
                             b.Description,
                             b.FunctionName
                         };
                if (ds.Any())
                {
                    foreach (var item in ds)
                    {
                        SYS_ROLEPERMISSION rolePer = DA_RolePermission.Instance.GetPermissionByFunction(RoleID, item.FunctionName);
                        RolePermissionExt ext = new RolePermissionExt
                        {
                            FunctionName = item.FunctionName,
                            Add = rolePer.Add,
                            Delete = rolePer.Delete,
                            Edit = rolePer.Edit,
                            View = rolePer.View,
                            Print = rolePer.Print,
                            RoleID = rolePer.RoleID,
                            Description = item.Description,
                            Export = rolePer.Export,
                            Import = rolePer.Import
                        };
                        lst.Add(ext);
                    }
                }
            }
            return lst;
        }

        public int CheckBeforeDelete(string ID)
        {
            long id = 0;
            long.TryParse(ID, out id);
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_USER user = context.SYS_USER.FirstOrDefault(m => m.RoleID == id);
                if (user != null)
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}