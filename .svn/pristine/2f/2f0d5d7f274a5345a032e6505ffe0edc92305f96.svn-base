using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models.BUS;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.ListExt;

namespace HRM_ROHTO.Controllers
{
    public class ShiftTimeRangeController : BaseController
    {
        //
        // GET: /ShiftTimeRange/

        public ActionResult Index()
        {
            if (!CheckPermission("ShiftTimeRange", 1))
                return RedirectToAction("Login", "Login", "Login");

            List<Shift_Time_Range_Ext> ShiftTimeRangeExt = new List<Shift_Time_Range_Ext>();

            foreach (var item in DA_Shift.Instance.GetAll().Join(DA_ShiftTimeRange.Instance.GetAll(), x => x.ShiftID, y => y.ShiftID, (x, y) => new { x.ShiftID, ShiftCode = x.ShiftCode, y.ShiftTimeRangeID, y.BeginTime, y.EndTime }).ToList())
            {
                Shift_Time_Range_Ext ext = new Shift_Time_Range_Ext
                {
                    ShiftTimeRangeID = item.ShiftTimeRangeID,
                    ShiftID = item.ShiftID,
                    ShiftCode = item.ShiftCode,
                    BeginTime = item.BeginTime.ToShortTimeString(),
                    EndTime = item.EndTime.ToShortTimeString()
                };
                ShiftTimeRangeExt.Add(ext);
            }
            ViewBag.lstShiftTimeRange = ShiftTimeRangeExt;
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.lstShift = DA_Shift.Instance.GetComboboxShift();
            return View();
        }

        [HttpPost]
        public ActionResult Create(TBL_SHIFT_TIME_RANGE item)
        {
            DateTime beginDate = new DateTime(2000, 01, 01);
            string ngayBegin = beginDate.ToString("dd/MM/yyyy").Substring(0, 10);
            string gioBegin = item.BeginTime.ToString("dd/MM/yyyy HH:mm:ss").Substring(11,8);
            item.BeginTime = Convert.ToDateTime(ngayBegin + " "+ gioBegin);

            string gioEnd = item.EndTime.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 8);
            item.EndTime = Convert.ToDateTime(ngayBegin + " " + gioEnd);


            //item
            DA_ShiftTimeRange.Instance.Insert(item);

            //Ghi log
            WriteLog("ShiftTimeRange", "Create", null, item);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int ShiftTimeRangeID)
        {
            ViewBag.lstShift = DA_Shift.Instance.GetComboboxShift();

            TBL_SHIFT_TIME_RANGE ShiftTimeRange = DA_ShiftTimeRange.Instance.GetAll().FirstOrDefault(x => x.ShiftTimeRangeID == ShiftTimeRangeID);

            if (ShiftTimeRange == null)
                return RedirectToAction("Index");
            return View(ShiftTimeRange);
        }

        [HttpPost]
        public ActionResult Edit(TBL_SHIFT_TIME_RANGE item)
        {
            DateTime beginDate = new DateTime(2000, 01, 01);
            string ngayBegin = beginDate.ToString("dd/MM/yyyy");
            string gioBegin = item.BeginTime.ToString("HH:mm:ss");
            item.BeginTime = Convert.ToDateTime(ngayBegin + " " + gioBegin);

            string gioEnd = item.EndTime.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 8);
            item.EndTime = Convert.ToDateTime(ngayBegin + " " + gioEnd);

            DA_ShiftTimeRange.Instance.Update(item);

            // Ghi log
            TBL_SHIFT_TIME_RANGE oldShiftTimeRange = DA_ShiftTimeRange.Instance.GetById(item.ShiftTimeRangeID);
            WriteLog("ShiftTimeRange", "Edit", oldShiftTimeRange, item);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int ShiftTimeRangeID)
        {
            TBL_SHIFT_TIME_RANGE ShiftTimeRange = DA_ShiftTimeRange.Instance.GetAll().FirstOrDefault(x => x.ShiftTimeRangeID == ShiftTimeRangeID);

            if (ShiftTimeRange == null)
                return RedirectToAction("Index");

            // Ghi log
            WriteLog("ShiftTimeRange", "Delete", ShiftTimeRange, null);

            DA_ShiftTimeRange.Instance.Delete(ShiftTimeRange);
            return RedirectToAction("Index");
        }
    }
}
