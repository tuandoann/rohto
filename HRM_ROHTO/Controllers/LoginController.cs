using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            if (fc["txtUserName"].Trim() == "")
            {
                ViewBag.Error = "Tên đăng nhập không được để trống";
                return View();
            }
            if (fc["txtPassword"].Trim() == "")
            {
                ViewBag.Error = "Mật khẩu không được để trống";
                return View();
            }
            SYS_USER user = DA_User.Instance.Login(fc["txtUserName"].Trim(), fc["txtPassword"].Trim());
            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                Session["FullName"] = user.FullName;
                Session["RoleID"] = user.RoleID == null ? 0 : user.RoleID;
                Session["UserImage"] = null;
                Session["IsAdmin"] = user.IsAdmin;

                //Ghi log
                WriteLog("Login", "Login");
                
                return RedirectToAction("Index", "Home", "Homes");
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            //Ghi log
            WriteLog("Logout", "Logout");

            // set some session to null
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["FullName"] = null;
            Session["RoleID"] = null;
            Session["UserImage"] = null;
            Session["IsAdmin"] = null;

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
    }
}
