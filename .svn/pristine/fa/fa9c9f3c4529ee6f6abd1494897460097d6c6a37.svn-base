using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /Role/

        public ActionResult Index(FormCollection fc, int page = 1)
        {
            ViewBag.List = DA_Role.Instance.GetAll();
            bool CanView = CheckPermission("Role", 1);
            if (!CanView)
            {
                return RedirectToAction("Login", "Login", "Login");
            }
            return View();

        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            // combobox Role
            RepairToShow();
            return View();
        }

        public ActionResult Edit(string id)
        {
            int ID = int.Parse(id);
            SYS_ROLE Role = DA_Role.Instance.GetById(ID);
            if (Role == null)
            {
                return HttpNotFound();
            }
            RepairToShow(ID);
            return View(Role);
        }

        public ActionResult Delete(int id)
        {
            DA_RolePermission.Instance.DeleteByRoleID(id);
            DA_Role.Instance.Delete(id);
            return RedirectToAction("Index");
        }

        public JsonResult CheckBeforeDelete(string ID)
        {
            int flag = DA_Role.Instance.CheckBeforeDelete(ID);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public void RepairToShow(int id = 0)
        {
            ViewBag.List = DA_Role.Instance.GetListRolePermission(id);
        }

        [HttpPost]
        public string CheckRoleNameIsNotExist(string RoleID, string RoleName)
        {
            int ID = 0;
            int.TryParse(RoleID, out ID);
            if (DA_Role.Instance.CheckRoleNameNotExist(ID, RoleName.Trim()))
            {
                return "1";
            }
            else
            {
                return "-1";
            }
        }

        [HttpPost]
        public JsonResult SaveData(string _Json)
        {
            string flag = "";
            try
            {
                dynamic json = JArray.Parse(_Json);
                foreach (var item in json)
                {
                    int _id = 0;
                    int.TryParse(item["RoleID"].ToString(), out _id);
                    SYS_ROLE obj = new SYS_ROLE
                    {
                        RoleID = _id,
                        RoleName = item["RoleName"].ToString(),
                        Note = item["Note"].ToString()
                    };


                    string listDetail = item["ListDetail"].ToString();
                    dynamic jsonDetail = JArray.Parse(listDetail);
                    List<SYS_ROLEPERMISSION> lstModule = new List<SYS_ROLEPERMISSION>();
                    foreach (var iDetail in jsonDetail)
                    {
                        if (iDetail["FunctionName"] != null)
                        {
                            bool ischeck = false;
                            ischeck = bool.Parse(iDetail["View"].ToString());

                            SYS_ROLEPERMISSION dObj = new SYS_ROLEPERMISSION
                            {
                                FunctionName = iDetail["FunctionName"].ToString(),
                                View = bool.Parse(iDetail["View"].ToString()),
                                Add = bool.Parse(iDetail["Add"].ToString()),
                                Edit = bool.Parse(iDetail["Edit"].ToString()),
                                Delete = bool.Parse(iDetail["Delete"].ToString()),
                                Print = bool.Parse(iDetail["Print"].ToString())
                            };
                            lstModule.Add(dObj);
                        }
                    }
                    if (obj.RoleID == 0)
                    {
                        DA_Role.Instance.Insert(obj);

                        // Ghi log
                        WriteLog("Role", "Create", null, obj);
                        
                        obj.RoleID = DA_Role.Instance.GetMaxRoleID();
                    }
                    else
                    {
                        DA_Role.Instance.Update(obj);

                        // Ghi log
                        SYS_ROLE oldRole = DA_Role.Instance.GetById(obj.RoleID);
                        WriteLog("Role", "Edit", oldRole, obj);
                    }
                    DA_RolePermission.Instance.UpdateListRolePermission(lstModule, obj.RoleID);
                }
            }
            catch (Exception ex)
            {
                flag = "-1";
            }
            return new JsonResult() { Data = flag, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
