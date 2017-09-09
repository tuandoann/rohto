using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class ReportAuditController : BaseController
    {
        //
        // GET: /ReportAudit/

        public ActionResult Index()
        {
            if (!CheckPermission("ReportAudit", 1))
                return RedirectToAction("Login", "Login");

            return View();
        }

        public JsonResult GetAllAudit()
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

                List<TBL_AUDIT> data = DA_Audit.Instance.getObjectForDatatable(dateFrom, dateTo, search.ToString(), skip, length != null ? Convert.ToInt32(length) : 0, sortColumn, sortColumnDir);
                recordsTotal = DA_Audit.Instance.getCountOBjectForDatatable(dateFrom, dateTo, search.ToString());
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }
        /// <summary>
        /// event click viewHistory
        /// </summary>
        /// <returns></returns>
        public JsonResult ViewHistory()
        {
            try
            {
                var id = Request.Form.GetValues("id_Item").FirstOrDefault();
                long idItem = id == null ? 0 : Convert.ToInt64(id);
                TBL_AUDIT element = DA_Audit.Instance.getItemBaseId(idItem);
                return Json(new { oldData = element.OldData, newData = element.NewData, error = "" });
            }
            catch(Exception ex)
            {
                return Json(new { oldData = "null", newData = "null", error = ex.Message });
            }
        }
    }
}
