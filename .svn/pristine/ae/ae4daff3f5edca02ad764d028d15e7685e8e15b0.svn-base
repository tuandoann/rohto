using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models.BUS;
using HRM_ROHTO.Models;
using Newtonsoft.Json.Linq;
using HRM_ROHTO.Models.ListExt;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class ExceptionMealController : BaseController
    {
        //
        // GET: /ExceptionMeal/

        public ActionResult Index(int year = 0, int month = 0)
        {
            if (!CheckPermission("ExceptionMeal", 1))
                return RedirectToAction("Login", "Login", "Login");
            ViewBag.LstExceptionMeal = "";
            int Year = year == 0 ? DateTime.Now.Year : year;
            int Month = month == 0 ? DateTime.Now.Month : month;
            return View(DA_ExceptionMeal.Instance.GetAll().Where(x => x.ExceptionDate.Year == Year && x.ExceptionDate.Month == Month).ToList());
        }

        public ActionResult Create()
        {
            ViewBag.LstShift = DA_Shift.Instance.GetAll().ToList();
            return View();
        }

        [HttpPost]
        public JsonResult SaveData(TBL_EXCEPTION_MEAL exceptionMeal, string lstShiftID, string lstExceptionQty)
        {
            string flag = "";
            try
            {
                JArray LstShiftID = JArray.Parse(lstShiftID);
                JArray LstExceptionQty = JArray.Parse(lstExceptionQty);

                DA_ExceptionMeal.Instance.Insert(exceptionMeal);
                long ExceptionMealID = DA_ExceptionMeal.Instance.GetAll().OrderByDescending(x => x.ExceptionMealID).FirstOrDefault().ExceptionMealID;

                //Ghi log
                WriteLog("ExceptionMeal", "Create", null, exceptionMeal);

                List<TBL_EXCEPTION_MEAL_SHIFT> lstException_Meal_Shift = new List<TBL_EXCEPTION_MEAL_SHIFT>();
                for (int i = 0; i < LstExceptionQty.Count; i++)
                {
                    string Qty = LstExceptionQty[i].ToString();
                    TBL_EXCEPTION_MEAL_SHIFT MealShift = new TBL_EXCEPTION_MEAL_SHIFT();
                    MealShift.ExceptionMealID = ExceptionMealID;
                    MealShift.ShiftID = int.Parse(LstShiftID[i].ToString());
                    MealShift.ExceptionQty = Qty == "" ? 0 : int.Parse(Qty);
                    MealShift.ExceptionQtyUsed = 0;
                    DA_ExceptionMeal_Shift.Instance.Insert(MealShift);

                    //---
                    lstException_Meal_Shift.Add(MealShift);
                }
                //Ghi log
                WriteLog("ExceptionMealShift", "Create", null, lstException_Meal_Shift);

                flag = "1";

                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                flag = "-1";
                throw;
            }
        }

        public ActionResult Edit(long ExceptionMealID)
        {
            TBL_EXCEPTION_MEAL ExceptionMeal = DA_ExceptionMeal.Instance.GetAll().FirstOrDefault(x => x.ExceptionMealID == ExceptionMealID);
            if (ExceptionMeal == null)
                return RedirectToAction("Index");

            var lstExceptionMeal_Shift = DA_Shift.Instance.GetAll().Join(DA_ExceptionMeal_Shift.Instance.GetAll(), x => x.ShiftID, y => y.ShiftID, (x, y) => new { x.ShiftID, x.ShiftName, y.ExceptionMealID, y.ExceptionQty, y.ExceptionQtyUsed }).Where(x => x.ExceptionMealID == ExceptionMealID).ToList();
            List<ExceptionMeal_Shift_Ext> lstMealShiftExt = new List<ExceptionMeal_Shift_Ext>();

            foreach (var item in lstExceptionMeal_Shift)
            {
                ExceptionMeal_Shift_Ext ext = new ExceptionMeal_Shift_Ext
                {
                    ExceptionMealID = item.ExceptionMealID,
                    ShiftID = item.ShiftID,
                    ExceptionQty = item.ExceptionQty,
                    ExceptionQtyUsed = item.ExceptionQtyUsed,
                    ShiftName = item.ShiftName,
                };
                lstMealShiftExt.Add(ext);
            }

            ViewBag.lstMealShift = lstMealShiftExt;
            ViewBag.lstShift = DA_Shift.Instance.GetAll().ToList();

            return View(ExceptionMeal);
        }

        [HttpPost]
        public JsonResult SaveEditData(TBL_EXCEPTION_MEAL exceptionMeal, string lstShiftID, string lstExceptionQty, string lstExceptionQtyUsed)
        {
            string flag = "";
            try
            {
                JArray LstShiftID = JArray.Parse(lstShiftID);
                JArray LstExceptionQty = JArray.Parse(lstExceptionQty);
                JArray LstExceptionQtyUsed = JArray.Parse(lstExceptionQtyUsed);

                DA_ExceptionMeal.Instance.Update(exceptionMeal);

                // Ghi log
                TBL_EXCEPTION_MEAL oldException_Meal = DA_ExceptionMeal.Instance.GetById(exceptionMeal.ExceptionMealID);
                WriteLog("ExceptionMeal", "Edit", oldException_Meal, exceptionMeal);

                // List Old Exception Meal Shift
                List<TBL_EXCEPTION_MEAL_SHIFT> oldException_Meal_Shift = new List<TBL_EXCEPTION_MEAL_SHIFT>();
                foreach (var item in DA_ExceptionMeal_Shift.Instance.GetAll().Where(x => x.ExceptionMealID == exceptionMeal.ExceptionMealID))
                {
                    // Add vào List Old Exception Meal Shift
                    TBL_EXCEPTION_MEAL_SHIFT Exception_Meal_Shift = DA_ExceptionMeal_Shift.Instance.GetById(item.ExceptionMealShiftID);
                    oldException_Meal_Shift.Add(Exception_Meal_Shift);

                    DA_ExceptionMeal_Shift.Instance.Delete(item);
                }

                // List New Exception Meal Shift
                List<TBL_EXCEPTION_MEAL_SHIFT> newException_Meal_Shift = new List<TBL_EXCEPTION_MEAL_SHIFT>();
                for (int i = 0; i < LstExceptionQty.Count; i++)
                {
                    string Qty = LstExceptionQty[i].ToString();
                    string QtyUsed = LstExceptionQtyUsed[i].ToString();
                    TBL_EXCEPTION_MEAL_SHIFT MealShift = new TBL_EXCEPTION_MEAL_SHIFT();
                    MealShift.ExceptionMealID = exceptionMeal.ExceptionMealID;
                    MealShift.ShiftID = int.Parse(LstShiftID[i].ToString());
                    MealShift.ExceptionQty = Qty == "" ? 0 : int.Parse(Qty);
                    MealShift.ExceptionQtyUsed = QtyUsed == null ? 0 : int.Parse(QtyUsed);
                    DA_ExceptionMeal_Shift.Instance.Insert(MealShift);

                    // Add vào List New Exception Meal Shift
                    newException_Meal_Shift.Add(MealShift);
                }

                // Ghi log
                WriteLog("ExceptionMealShift", "Edit", oldException_Meal_Shift, newException_Meal_Shift);

                flag = "1";

                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                flag = "-1";
                throw;
            }
        }

        [HttpPost]
        public JsonResult CheckDateExist(DateTime Date)
        {
            string flag = "1";
            if (!DA_ExceptionMeal.Instance.CheckDateExist(Date))
                flag = "-1";
            else
                flag = "1";
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEditDateExist(long ID, DateTime Date)
        {
            string flag = "1";
            if (!DA_ExceptionMeal.Instance.CheckEditDateExist(ID, Date))
                flag = "-1";
            else
                flag = "1";
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long ExceptionMealID)
        {
            string flag = "";
            try
            {
                //Ghi log
                TBL_EXCEPTION_MEAL Exception_Meal = DA_ExceptionMeal.Instance.GetById(ExceptionMealID);
                WriteLog("ExceptionMeal", "Delete", Exception_Meal, null);

                DA_ExceptionMeal.Instance.Delete(ExceptionMealID);
                

                // List Old Exception Meal Shift
                List<TBL_EXCEPTION_MEAL_SHIFT> oldException_Meal_Shift = new List<TBL_EXCEPTION_MEAL_SHIFT>();
                foreach (var item in DA_ExceptionMeal_Shift.Instance.GetAll().Where(x => x.ExceptionMealID == ExceptionMealID))
                {
                    // Add vào List Old Exception Meal Shift
                    TBL_EXCEPTION_MEAL_SHIFT Exception_Meal_Shift = DA_ExceptionMeal_Shift.Instance.GetById(item.ExceptionMealShiftID);
                    oldException_Meal_Shift.Add(Exception_Meal_Shift);

                    DA_ExceptionMeal_Shift.Instance.Delete(item);
                }

                // Ghi log
                WriteLog("ExceptionMealShift", "Delete", oldException_Meal_Shift, null);

                flag = "1";
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "-1";
                throw;
            }
        }
    }
}
