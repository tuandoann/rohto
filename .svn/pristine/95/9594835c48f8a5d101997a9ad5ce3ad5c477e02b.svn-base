using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                Session["LoginManager"] = null;
                return RedirectToAction("Login", "Login", "Login");
            }

            return View();
        }

        public ActionResult Menu()
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string strRP1 = "";
            string strRP2 = "";
            if (Session["UserID"] == null || Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Login", "Login");
            }

            if (Session["UserID"] != null && Session["UserName"] != null)
            {
                int RoleID = 0;
                int.TryParse(Session["RoleID"].ToString(), out RoleID);
                bool IsAdmin = false;
                bool.TryParse(Session["IsAdmin"].ToString(), out IsAdmin);
                List<SYS_FUNCTION> lst = DA_Function.Instance.GetListFunctionByRoleToShowMenu(RoleID, IsAdmin);

                str1 += @"<ul class='sidebar-menu'>";
                str1 += "<li class='header'>MENU</li>";
                str2 += @"<li class='treeview'><a href = '#' >
                                            <i class='fa fa-cog'></i>
                                            <span>Thiết lập</span>
                                            <span class='pull-right-container'>
                                              <i class='fa fa-angle-left pull-right'></i>
                                            </span>
                                          </a>
                                          <ul class='treeview-menu'>";

                strRP1 += @"<li class='treeview'><a href = '#' >
                                            <i class='fa fa-th-list'></i>
                                            <span>Báo cáo</span>
                                            <span class='pull-right-container'>
                                              <i class='fa fa-angle-left pull-right'></i>
                                            </span>
                                          </a>
                                          <ul class='treeview-menu'>";


                foreach (SYS_FUNCTION item in lst)
                {
                    switch (item.ModuleCode)
                    {
                        case "SYS":
                            {
                                str3 += @"<li><a href='" + item.Path + "'><i class='" + item.IconURL + "'></i> <span>" + item.Description + "</span></a></li>";
                                break;
                            }

                        case "RP":
                            {
                                strRP2 += @"<li><a href='" + item.Path + "'><i class='" + item.IconURL + "'></i> <span>" + item.Description + "</span></a></li>";
                                break;
                            }
                        default:
                            {
                                str5 += @"<li><a href='" + item.Path + "'><i class='" + item.IconURL + "'></i> <span>" + item.Description + "</span></a></li>";
                                break;
                            }
                    }

                }

                str4 += @"</ul>
                         </li>";
                if (strRP2 != "")
                {
                    strRP1 += strRP2;
                    strRP1 += @"</ul>
                         </li>";
                }
                else
                {
                    strRP1 = "";
                }

                str6 += @"</ul>";

            }
            if (str3 == "")
            {
                str2 = str4 = "";
            }
            ViewBag.menu = str1 + str2 + str3 + str4 + str5 + strRP1 + str6;
            return PartialView();
        }

        public bool CheckSession()
        {
            if (Session["UserID"] != null)
            {
                return true;
            }
            return false;
        }


        [HttpGet]
        public JsonResult KeepSession()
        {
            Session["Heartbeat"] = DateTime.Now;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckPermission(string Path)
        {
            //if (Path.ToLower().Contains("view"))
            //{
            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}

            //if (Path.ToLower().Contains("home"))
            //{
            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}

            bool flag = true;
            //int RoleID = 0;
            //int.TryParse(Session["RoleID"].ToString(), out RoleID);
            //string s = Path;
            //int index = s.LastIndexOf("/");
            //int num = s.Count(f => f == '/');
            //if ((num == 1 || index > 0) && s != "/") // khac trang home
            //{
            //    string _Path = s;
            //    if (num > 1)
            //    {
            //        Path = s.Remove(s.LastIndexOf("/"));
            //        Path = Path.Replace("/Edit", "").Replace("/Add", "");
            //    }
            //    // kiem tra xem user co quyen truy cap vao url nay ko?
            //    flag = DA_Function.Instance.CheckPermission(RoleID, Path);

            //}
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}
