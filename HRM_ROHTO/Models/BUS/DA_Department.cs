using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Department : EfRepositoryBase<TBL_DEPARTMENT>
    {
        #region Constructor
        private static volatile DA_Department _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Department Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Department();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public bool CheckDepartmentCodeExist(string departmentCode)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_DEPARTMENT ds = context.TBL_DEPARTMENT.FirstOrDefault(x => x.DepartmentCode == departmentCode);
                if (ds != null)
                    return false;
            }
            return true;
        }

        public int? GetDepartmentIDByCode(string departmentCode)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_DEPARTMENT Department = context.TBL_DEPARTMENT.FirstOrDefault(x => x.DepartmentCode == departmentCode);

                int? DepartmentID = null;

                if (Department != null)
                    DepartmentID = Department.DepartmentID;

                return DepartmentID;
            }
        }

        public List<SelectListItem> GetComboboxDepartment()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                foreach (var item in context.TBL_DEPARTMENT)
                {
                    list.Add(new SelectListItem { Value = item.DepartmentID.ToString(), Text = item.DepartmentName });
                }
            }

            return list;
        }

        public int GetLevelIDOfParent(int? parentID)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                int levelID = context.TBL_DEPARTMENT.FirstOrDefault(x => x.DepartmentID == parentID).LevelID;
                return levelID;
            }
        }

        public bool CheckLevelID_Edit(int departmentID, int parentID)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                int levelID = context.TBL_DEPARTMENT.FirstOrDefault(x => x.DepartmentID == departmentID).LevelID;
                if (levelID < GetLevelIDOfParent(parentID))
                    return false;
            }
            return true;
        }

        public List<SelectListItem> GetListForCombobox()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.TBL_DEPARTMENT
                         orderby b.ParentID, b.DepartmentName
                         select
                         new { b.DepartmentID, b.DepartmentName };
                if (ds.Any())
                {
                    foreach (var item in ds)
                    {
                        SelectListItem b = new SelectListItem { Value = item.DepartmentID.ToString(), Text = item.DepartmentName };
                        lst.Add(b);
                    }
                }

            }
            return lst;
        }

        public List<SelectListItem> GetComboboxByAndParentID(long? ParentID)
        {
            if (ParentID == 0)
            {
                ParentID = null;
            }
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.TBL_DEPARTMENT
                         where b.ParentID == ParentID
                         orderby b.ParentID, b.DepartmentName
                         select
                         new { b.DepartmentID, b.DepartmentName };
                if (ds.Any())
                {
                    foreach (var item in ds)
                    {
                        SelectListItem b = new SelectListItem { Value = item.DepartmentID.ToString(), Text = item.DepartmentName };
                        lst.Add(b);
                    }
                }

            }
            // add blank row
            lst.Insert(0, new SelectListItem { Value = "0", Text = "" });
            return lst;
        }

        public List<SelectListItem> GetListForComboboxParent(long departmentID)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var ds = from b in context.TBL_DEPARTMENT
                         where b.DepartmentID != departmentID
                         orderby b.ParentID, b.DepartmentName
                         select
                         new { b.DepartmentID, b.DepartmentName };
                if (ds.Any())
                {
                    foreach (var item in ds)
                    {
                        SelectListItem b = new SelectListItem { Value = item.DepartmentID.ToString(), Text = item.DepartmentName };
                        lst.Add(b);
                    }
                }

            }
            // add blank row
            lst.Insert(0, new SelectListItem { Value = "0", Text = "" });
            return lst;
        }

        public string GetDepartmentNameFromDepartmentID(long? departmentID)
        {
            if (departmentID == null || departmentID == 0)
            {
                return "";
            }
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                return context.TBL_DEPARTMENT.FirstOrDefault(m => m.DepartmentID == departmentID).DepartmentName;
            }

        }

        public void GetListDepartmentToTreeView(ref List<TBL_DEPARTMENT> lst, long? ParentID)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {

                var ds = from b in context.TBL_DEPARTMENT
                         where b.ParentID == ParentID
                         //&& b.IsActive == true
                         select b.DepartmentID;
                if (ds.Any())
                {
                    foreach (long id in ds)
                    {
                        // lay thong tin node hien tai:
                        TBL_DEPARTMENT child = context.TBL_DEPARTMENT.FirstOrDefault(m => m.DepartmentID == id);

                        if (child != null)
                        {
                            lst.Add(child);
                            GetListDepartmentToTreeView(ref lst, child.DepartmentID);
                        }
                    }
                }
            }
        }

        public string GetDepartmentCodeFromDepartmentID(long departmentID)
        {
            string DepartmentCode = "";
            if (departmentID != 0)
            {
                using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
                {
                    try
                    {
                        DepartmentCode = context.TBL_DEPARTMENT.FirstOrDefault(m => m.DepartmentID == departmentID).DepartmentCode;
                    }
                    catch { }
                }
            }
            return DepartmentCode;
        }

        public int CheckDepartmentCodeNotExist(string departmentID, string departmentCode)
        {
            long id = 0;
            long.TryParse(departmentID, out id);
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_DEPARTMENT addr = context.TBL_DEPARTMENT.FirstOrDefault(m => m.DepartmentID != id && m.DepartmentCode == departmentCode);
                if (addr == null)
                {
                    return 1;
                }
                return 0;
            }
        }

        public void DeleteAdvance(long ID)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                List<TBL_DEPARTMENT> lst = new List<TBL_DEPARTMENT>();
                DA_Department.Instance.GetListDepartmentToTreeView(ref lst, ID);
                foreach (var addr in lst)
                {
                    DA_Department.Instance.Delete(addr.DepartmentID);
                }
                DA_Department.Instance.Delete(ID);
            }
        }
    }
}