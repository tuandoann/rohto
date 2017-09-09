using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class BroadcastController : BaseController
    {
        //
        // GET: /Broadcast/
        public ActionResult Index()
        {
            if (!CheckPermission("Broadcast", 1))
                return RedirectToAction("Login", "Login");
            return View();
        }

        public JsonResult GetAllBroadCast()
        {
            IList<TBL_BROADCAST> data = DA_Broadcast.Instance.GetAll().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TBL_BROADCAST item)
        {
            item.DateCreate = DateTime.Now;
            item.UserCreate = int.Parse(Session["UserID"].ToString());
            DA_Broadcast.Instance.Insert(item);

            //Ghi log
            WriteLog("Broadcast", "Create", null, item);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int broadcastID)
        {
            TBL_BROADCAST Broadcast = DA_Broadcast.Instance.GetAll().FirstOrDefault(x => x.BroadcastID == broadcastID);
            return View(Broadcast);
        }

        [HttpPost]
        public ActionResult Edit(TBL_BROADCAST item)
        {
            item.UserModified = int.Parse(Session["UserID"].ToString());
            item.DateModified = DateTime.Now;
            DA_Broadcast.Instance.Update(item);

            //Ghi log
            TBL_BROADCAST oldBroadCast = DA_Broadcast.Instance.GetById(item.BroadcastID);
            WriteLog("Broadcast", "Edit", oldBroadCast, item);
            return RedirectToAction("Index");
        }

        public JsonResult DeleteBroadcast(long ID)
        {
            DA_Broadcast.Instance.Delete(ID);

            //Ghi log
            TBL_BROADCAST oldBroadCast = DA_Broadcast.Instance.GetById(ID);
            WriteLog("Broadcast", "Delete", oldBroadCast, null);

            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
