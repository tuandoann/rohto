using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_RolePermission : EfRepositoryBase<SYS_ROLEPERMISSION>
    {
        #region Constructor
        private static volatile DA_RolePermission _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_RolePermission Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_RolePermission();
                    }
                }
                return _instance;
            }
        }
        #endregion


        public int DeleteByRoleID(int RoleID)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.SYS_ROLEPERMISSION
                         where b.RoleID == RoleID
                         select b;
                if (ds.Any())
                {
                    foreach (SYS_ROLEPERMISSION item in ds)
                    {
                        Instance.Delete(item);
                    }
                }
            }
            return 1;
        }

        public void UpdateListRolePermission(List<SYS_ROLEPERMISSION> lstDetail, int RoleID)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                // Get List old
                var dsOld = from b in context.SYS_ROLEPERMISSION
                            where b.RoleID == RoleID
                            select b;
                if (dsOld.Any()) // delete old data
                {
                    foreach (SYS_ROLEPERMISSION old in dsOld)
                    {
                        if (lstDetail.FirstOrDefault(m => m.RoleID == old.RoleID && m.FunctionName == old.FunctionName) == null) // item da bi xoa
                        {
                            Instance.Delete(old);
                        }
                    }
                }

                // insert new item
                foreach (SYS_ROLEPERMISSION inew in lstDetail)
                {
                    SYS_ROLEPERMISSION item = dsOld.FirstOrDefault(m => m.RoleID == inew.RoleID && m.FunctionName == inew.FunctionName);
                    if (item == null) // item chua ton tai
                    {
                        inew.RoleID = RoleID;
                        Instance.Insert(inew);
                    }
                    else
                    {
                        item.RoleID = inew.RoleID;
                        item.View = inew.View;
                        item.Add = inew.Add;
                        item.Edit = inew.Edit;
                        item.Delete = inew.Delete;
                        item.Print = inew.Print;
                        Instance.Update(item);
                    }
                }
            }
        }

        public SYS_ROLEPERMISSION GetPermissionByFunction(int RoleID, string functionName, bool IsAdmin = false)
        {
            if (IsAdmin)
            {
                return new SYS_ROLEPERMISSION
                {
                    View = true,
                    Add = true,
                    Edit = true,
                    Delete = true,
                    Print = true,
                    Import = true,
                    Export = true,
                    Accept = true,
                    Cancel = true
                };
            }

            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_ROLEPERMISSION item = context.SYS_ROLEPERMISSION.FirstOrDefault(m => m.FunctionName == functionName && m.RoleID == RoleID);
                if (item != null)
                {
                    return item;
                }
            }
            return new SYS_ROLEPERMISSION
            {
                View = false,
                Add = false,
                Edit = false,
                Delete = false,
                Print = false,
                Import = false,
                Export = false,
                Accept = false,
                Cancel = false
            };
        }
    }
}