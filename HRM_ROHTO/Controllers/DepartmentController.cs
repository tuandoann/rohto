using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class DepartmentController : BaseController
    {
        //
        // GET: /Department/

        public ActionResult Index(FormCollection fc)
        {
            List<TBL_DEPARTMENT> lst = new List<TBL_DEPARTMENT>();
            DA_Department.Instance.GetListDepartmentToTreeView(ref lst, null);
            ViewBag.List = lst;

            bool CanView = CheckPermission("Department", 1);
            if (!CanView)
            {
                return RedirectToAction("Login", "Login", "Login");
            }

            return View();
        }

        public ActionResult Create()
        {
            TBL_DEPARTMENT Item = new TBL_DEPARTMENT();
            if (Request.QueryString["p"] != null)
            {
                int parent = 0;
                int.TryParse(Request.QueryString["p"].ToString(), out parent);
                Item.ParentID = parent;
            }

            RepairToShow();
            return View(Item);
        }

        [HttpPost]
        public ActionResult Create(TBL_DEPARTMENT Item)
        {
            try
            {
                RepairToShow();
                if (Item.ParentID == 0)
                {
                    Item.ParentID = null;
                    Item.LevelID = 1;
                }
                else
                {
                    int parentLevelID = DA_Department.Instance.GetLevelIDOfParent(Item.ParentID);
                    Item.LevelID = parentLevelID + 1;
                }
                DA_Department.Instance.Insert(Item);

                //Ghi log
                WriteLog("Department", "Create", null, Item);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            int _id = int.Parse(id);
            TBL_DEPARTMENT obj = DA_Department.Instance.GetById(_id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            RepairToShow();
            return View(obj);
        }

        [HttpPost]
        public string CheckLevelID_Edit(int deparmentID, int parentID)
        {
            return DA_Department.Instance.CheckLevelID_Edit(deparmentID, parentID) == true ? "1" : "-1";
        }

        [HttpPost]
        public ActionResult Edit(TBL_DEPARTMENT Item)
        {
            try
            {
                RepairToShow(Item.DepartmentID);
                if (Item.ParentID == 0)
                {
                    Item.ParentID = null;
                    Item.LevelID = 1;
                }
                else
                {
                    int parentLevelID = DA_Department.Instance.GetLevelIDOfParent(Item.ParentID);
                    Item.LevelID = parentLevelID + 1;
                }

                DA_Department.Instance.Update(Item);

                //Ghi log
                TBL_DEPARTMENT oldDepartment = DA_Department.Instance.GetById(Item.DepartmentID);
                WriteLog("Department", "Edit", oldDepartment, Item);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //public int GetLevelID(long? ParentID)
        //{
        //    int lv = 0;
        //    if (ParentID != null && ParentID > 0)
        //    {
        //        lv = DA_Department.Instance.GetLevelIDOfDepartment((long)ParentID);
        //    }
        //    return (lv + 1);
        //}

        public ActionResult Delete(string id)
        {
            int _id = int.Parse(id);
            DA_Department.Instance.DeleteAdvance(_id);

            //Ghi log
            TBL_DEPARTMENT oldDepartment = DA_Department.Instance.GetById(id);
            WriteLog("Department", "Delete", oldDepartment, null);

            return RedirectToAction("Index");
        }

        public void RepairToShow(long DepartmentID = 0)
        {
            ViewBag.lstDepartmentParent = DA_Department.Instance.GetListForComboboxParent(DepartmentID);
        }

        //public JsonResult CheckBeforeDelete(string ID)
        //{
        //    int flag = DA_Department.Instance.CheckBeforeDelete(ID);
        //    return Json(flag, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult CheckDepartmentCodeNotExist(string DepartmentID, string DepartmentCode)
        {
            int flag = DA_Department.Instance.CheckDepartmentCodeNotExist(DepartmentID, DepartmentCode);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}
