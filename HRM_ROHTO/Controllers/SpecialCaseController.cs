using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class SpecialCaseController : BaseController
    {
        //
        // GET: /SpecialCase/
        #region load page
        /// <summary>
        /// load page index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!CheckPermission("SpecialCase", 1))
                return RedirectToAction("Login", "Login");
            return View();
        }
        /// <summary>
        /// load page create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// load page edit
        /// </summary>
        /// <param name="broadcastID"></param>
        /// <returns></returns>
        public ActionResult Edit(int specialID)
        {
            TBL_SPECIAL SpecialCase = DA_SpecialCase.Instance.GetAll().FirstOrDefault(x => x.SpecialID == specialID);
            return View(SpecialCase);
        }
        #endregion

        #region action page
        /// <summary>
        /// get list TBL_SPECIAL
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllSpecialCase()
        {
            try
            {
                //jQuery DataTables Param
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                //Find paging info
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();


                #region get para from view

                DateTime dateFrom = Convert.ToDateTime(Request.Form.GetValues("dateFrom").FirstOrDefault() == null ? null : Request.Form.GetValues("dateFrom").ToArray()[0]);
                DateTime dateTo = Convert.ToDateTime(Request.Form.GetValues("dateTo").FirstOrDefault() == null ? null : Request.Form.GetValues("dateTo").ToArray()[0]);

                int orderColumn = Convert.ToInt32(Request.Form.GetValues("order[0][column]").FirstOrDefault());
                //Find order columns info
                var sortColumn = Request.Form.GetValues("columns[" + (orderColumn == 0 ? 1 : orderColumn) + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                //find search columns info
                var search = Request.Form["search[value]"];

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt16(start) : 0;
                #endregion

                long recordsTotal = 0;

                List<TBL_SPECIAL> data = DA_SpecialCase.Instance.getObjectForDatatable(dateFrom, dateTo, search.ToString(), skip, length != null ? Convert.ToInt32(length) : 0, sortColumn, sortColumnDir);
                recordsTotal = DA_SpecialCase.Instance.getCountOBjectForDatatable(dateFrom, dateTo, search.ToString());
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }
        /// <summary>
        /// action create
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult actionCreate()
        {
            TBL_SPECIAL item = new TBL_SPECIAL();

            //get data from view
            item.SpecialDate = Convert.ToDateTime(Request.Form.GetValues("SpecialDate").FirstOrDefault() == null ? null : Request.Form.GetValues("SpecialDate").ToArray()[0]);
            item.Notes = Request.Form.GetValues("Notes").FirstOrDefault() == null ? string.Empty : Request.Form.GetValues("Notes").FirstOrDefault();
            try
            {
                string quantity = Request.Form.GetValues("Quantity").FirstOrDefault() == null ? "" : Request.Form.GetValues("Quantity").FirstOrDefault().ToString();
                item.Quantity = String.IsNullOrWhiteSpace(quantity)? 0:  Convert.ToInt32(quantity);
            }
            catch(Exception ex)
            {
                item.Quantity = 0;
            }

           //check datetime
            if(DA_SpecialCase.Instance.checkExistDateObject(item.SpecialDate,0))
            {
                return Json(-1);
            }

            //excute create item
            int result =  DA_SpecialCase.Instance.Insert(item);
            //write log
            WriteLog("SpecialCase", "Create", null, item);

            //chuyển trang sang trang index
            return Json(result > 0 ? 1 : 0);
        }
        /// <summary>
        /// action edit
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult actionEdit()
        {
            TBL_SPECIAL item = new TBL_SPECIAL();

            //get data from view
            try
            {
                string id = Request.Form.GetValues("SpecialID").FirstOrDefault() == null ? "" : Request.Form.GetValues("SpecialID").FirstOrDefault().ToString();
                item.SpecialID = String.IsNullOrWhiteSpace(id) ? 0 : Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                item.SpecialID = 0;
            }
            item.SpecialDate = Convert.ToDateTime(Request.Form.GetValues("SpecialDate").FirstOrDefault() == null ? null : Request.Form.GetValues("SpecialDate").ToArray()[0]);
            item.Notes = Request.Form.GetValues("Notes").FirstOrDefault() == null ? string.Empty : Request.Form.GetValues("Notes").FirstOrDefault();
            try
            {
                string quantity = Request.Form.GetValues("Quantity").FirstOrDefault() == null ? "" : Request.Form.GetValues("Quantity").FirstOrDefault().ToString();
                item.Quantity = String.IsNullOrWhiteSpace(quantity) ? 0 : Convert.ToInt32(quantity);
            }
            catch (Exception ex)
            {
                item.Quantity = 0;
            }

            //check datetime
            if (DA_SpecialCase.Instance.checkExistDateObject(item.SpecialDate, item.SpecialID))
            {
                return Json(-1);
            }

            //excute edit item
            int result = DA_SpecialCase.Instance.Update(item);

            //Ghi log
            TBL_SPECIAL oldSPECIAL = DA_SpecialCase.Instance.GetById(item.SpecialID);
            WriteLog("SpecialCase", "Edit", oldSPECIAL, item);

            //chuyển trang sang trang index
            return Json(result > 0 ? 1 : 0);
        }
        /// <summary>
        /// action delete
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult DeleteSpecialCase(long ID)
        {
            //excute action delete
            DA_SpecialCase.Instance.Delete(ID);

            //Ghi log
            TBL_SPECIAL oldSPECIAL = DA_SpecialCase.Instance.GetById(ID);
            WriteLog("SpecialCase", "Delete", oldSPECIAL, null);

            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
