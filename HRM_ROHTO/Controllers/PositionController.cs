using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class PositionController : BaseController
    {
        //
        // GET: /Position/
        public ActionResult Index()
        {
            ViewBag.List = DA_Position.Instance.GetAll();
            bool CanView = CheckPermission("Position", 1);
            if (!CanView)
                return RedirectToAction("Login", "Login", "Login");
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TBL_POSITION item)
        {
            try
            {
                DA_Position.Instance.Insert(item);

                // Ghi log
                WriteLog("Position", "Create", null, item);

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }

        [HttpPost]
        public string CheckPositionCodeIsNotExist(string PositionCode)
        {
            return DA_Position.Instance.CheckPositionCodeNotExist(PositionCode) == true ? "1" : "-1";
        }

        [HttpPost]
        public string CheckEditPositionCodeHasExist(int PositionID, string PositionCode)
        {
            return DA_Position.Instance.CheckEditPositionCodeHasExist(PositionID, PositionCode) == true ? "1" : "-1";
        }

        [HttpGet]
        public ActionResult Edit(int PositionID)
        {
            TBL_POSITION cus = DA_Position.Instance.GetById(PositionID);
            if (cus == null)
                return HttpNotFound();
            return View(cus);
        }

        [HttpPost]
        public ActionResult Edit(TBL_POSITION item)
        {
            try
            {
                DA_Position.Instance.Update(item);

                // Ghi log
                TBL_POSITION oldPosition = DA_Position.Instance.GetById(item.PositionID);
                WriteLog("Position", "Edit", oldPosition, item);

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }
        
        public ActionResult Delete(int PositionID)
        {
            // Ghi log
            TBL_POSITION oldPosition = DA_Position.Instance.GetById(PositionID);
            WriteLog("Position", "Edit", oldPosition, null);

            DA_Position.Instance.Delete(PositionID);
            return RedirectToAction("Index");
        }

        public JsonResult CheckBeforeDelete(int PositionID)
        {
            string flag = "1";
            if (!DA_Position.Instance.CheckBeforeDelete(PositionID))
                flag = "-1";
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}
