using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using System.Data;

namespace HRM_ROHTO.Controllers
{
    public class ConfigController : BaseController
    {
        //
        // GET: /Config/
        public ActionResult Index()
        {
            if (!CheckPermission("Config", 1))
                return RedirectToAction("Login", "Login");
            List<object> data = DA_Config.Instance.getAllEntity();
            moveItem(2, 1, ref data);
            moveItem(3, 8, ref data);
            moveItem(7, 3, ref data);
            ViewBag.store = data;
            return View();
        }
        /// <summary>
        /// thay đổi vị trí index của item trong list object
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        /// <param name="ls"></param>
        /// <returns></returns>
        private List<object> moveItem(int oldIndex, int newIndex, ref List<object> ls)
        {
            var item = ls.ElementAt(oldIndex);
            ls.RemoveAt(oldIndex);
            ls.Insert(newIndex, item);
            return ls;
        }

        [HttpPost]
        public JsonResult getListNameKey()
        {
            return Json(DA_Config.KeyEntity);
        }

        [HttpPost]
        public JsonResult actionEdit()
        {
            try
            {
                var id = Request.Form.GetValues("id").FirstOrDefault();
                var value = Request.Form.GetValues("value").FirstOrDefault();
                if(!string.IsNullOrWhiteSpace(id))
                {
                    string ID = id.ToString();
                    string VALUE = string.IsNullOrWhiteSpace(value)? "" : value.ToString();
                    SYS_CONFIG item = DA_Config.Instance.GetById(ID);
                    item.Value = VALUE;
                    return Json(DA_Config.Instance.Update(item) > 0 ? true : false);
                }
                return Json(false);
            }
            catch(Exception ex)
            {
                return Json(false);
            }
        }
    }
}
