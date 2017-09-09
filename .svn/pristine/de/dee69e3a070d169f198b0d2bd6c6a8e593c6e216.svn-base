using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class ShiftController : BaseController
    {
        //
        // GET: /Shift/

        public ActionResult Index()
        {
            if (!CheckPermission("Shift", 1))
                return RedirectToAction("Login", "Login", "Login");
            return View();
        }

        public JsonResult LoadData()
        {
            IEnumerable<SYS_SHIFT> lstShift = DA_Shift.Instance.GetAll();
            return Json(new { data = lstShift }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SYS_SHIFT item)
        {
            DA_Shift.Instance.Insert(item);

            // Ghi log
            WriteLog("Shift", "Create", null, item);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int ShiftID)
        {
            SYS_SHIFT Shift = DA_Shift.Instance.GetAll().FirstOrDefault(x => x.ShiftID == ShiftID);
            return View(Shift);
        }

        [HttpPost]
        public ActionResult Edit(SYS_SHIFT item)
        {
            DA_Shift.Instance.Update(item);

            // Ghi log
            SYS_SHIFT oldShift = DA_Shift.Instance.GetById(item.ShiftID);
            WriteLog("Shift", "Edit", oldShift, item);

            return RedirectToAction("Index");
        }

        public JsonResult Delete(int ShiftID)
        {
            // Ghi log
            SYS_SHIFT oldShift = DA_Shift.Instance.GetById(ShiftID);
            WriteLog("Shift", "Edit", oldShift, null);

            DA_Shift.Instance.Delete(ShiftID);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
