using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Function : EfRepositoryBase<SYS_FUNCTION>
    {
        #region Constructor
        private static volatile DA_Function _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Function Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Function();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public List<SYS_FUNCTION> GetListFunctionByRole(int RoleID)
        {
            List<SYS_FUNCTION> lst = new List<SYS_FUNCTION>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.SYS_ROLEPERMISSION
                         join c in context.SYS_FUNCTION on b.FunctionName equals c.FunctionName
                         where b.RoleID == RoleID
                         orderby c.SortOrder
                         select c;
                if (ds.Any())
                {
                    return ds.ToList();
                }

            }
            return lst;
        }

        public List<SYS_FUNCTION> GetListFunctionByRoleToShowMenu(int RoleID, bool IsAdmin = false)
        {
            List<SYS_FUNCTION> lst = new List<SYS_FUNCTION>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                if (IsAdmin)
                {
                    var ds = from b in context.SYS_FUNCTION
                             orderby b.SortOrder
                             select b;
                    if (ds.Any())
                    {
                        return ds.ToList();
                    }
                }
                else
                {
                    var ds = from b in context.SYS_ROLEPERMISSION
                             join c in context.SYS_FUNCTION on b.FunctionName equals c.FunctionName
                             where b.RoleID == RoleID && b.View == true
                             orderby c.SortOrder
                             select c;
                    if (ds.Any())
                    {
                        return ds.ToList();
                    }
                }

            }
            return lst;
        }

        public bool CheckPermission(int RoleID, string Path)
        {
            if (Path.ToLower().Contains("profile") || Path.ToLower().Contains("user"))
            {
                return true;
            }
            if (RoleID == 5 && Path.ToLower().Contains("student"))
            {
                return true;
            }
            if (Path.ToLower().Contains("coachingstudent") && RoleID == 4)
            {
                return true;
            }
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                SYS_FUNCTION function = context.SYS_FUNCTION.FirstOrDefault(m => m.RoleID == RoleID && m.Path.Contains(Path.Trim()));
                if (function == null)
                {
                    return false;
                }
                return true;
            }
        }

    }
}