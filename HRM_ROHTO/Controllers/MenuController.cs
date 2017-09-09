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
    public class MenuController : BaseController
    {
        //
        // GET: /Menu/

        public ActionResult Index()
        {
            if (!CheckPermission("Menu", 1))
                return RedirectToAction("Login", "Login", "Login");
            return View();
        }

        public JsonResult GetAllMenu()
        {
            IList<TBL_MENU> Menu = DA_Menu.Instance.GetAll().OrderBy(x => x.MenuDay).ToList();
            return Json(new { data = Menu }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var lstJoin = (from pro in DA_Product.Instance.GetAll()
                                  join type in DA_ProductType.Instance.GetAll() on pro.ProductTypeID equals type.ProductTypeID
                                  orderby pro.ProductName
                                  select new { pro.ProductID, pro.ProductName, type.ProductTypeName }).ToList();

            IList<Product_MenuProduct_Ext> lstProductExt = new List<Product_MenuProduct_Ext>();
            foreach (var item in lstJoin)
            {
                Product_MenuProduct_Ext ext = new Product_MenuProduct_Ext
                {
                    ProductID = item.ProductID,
                    ProductName = item.ProductName,
                    ProductTypeName = item.ProductTypeName
                };
                lstProductExt.Add(ext);
            }

            ViewBag.ListProduct = lstProductExt;
            ViewBag.ListShift = DA_Shift.Instance.GetAll();
            ViewBag.ListMenuDay = DA_MenuProduct.Instance.GetListMenuDay();

            return View();
        }

        public JsonResult CheckMenuDayAndShiftIDExist(int MenuDay, string menuShift)
        {
            string shift = "";
            shift = DA_Menu.Instance.CheckMenuDayAndShiftIDExist(MenuDay, menuShift);
            return Json(new { data = shift }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditCheckMenuDayAndShiftIDExist(int MenuID, int MenuDay, string menuShift)
        {
            string shift = "";
            shift = DA_Menu.Instance.CheckEditMenuDayAndShiftIDExist(MenuID, MenuDay, menuShift);
            return Json(new { data = shift }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveCreateData(TBL_MENU Menu, string menuProduct, string menuShift)
        {
            string flag = "";
            try
            {
                dynamic MenuProduct = JArray.Parse(menuProduct);
                dynamic MenuShift = JArray.Parse(menuShift);

                Menu.UserCreate = int.Parse(Session["UserID"].ToString());
                Menu.ModifiedDate = DateTime.Now;

                DA_Menu.Instance.Insert(Menu);
                int MenuID = DA_Menu.Instance.GetAll().OrderByDescending(x => x.MenuID).FirstOrDefault().MenuID;

                //Ghi log
                WriteLog("Menu", "Create", null, Menu);

                //List New Menu Product
                List<TBL_MENU_PRODUCT> newMenu_Product = new List<TBL_MENU_PRODUCT>();
                for (int i = 0; i < MenuProduct.Count; i++)
                {
                    TBL_MENU_PRODUCT Menu_Product = new TBL_MENU_PRODUCT();
                    Menu_Product.MenuID = MenuID;
                    Menu_Product.ProductID = int.Parse(MenuProduct[i].Value);
                    DA_MenuProduct.Instance.Insert(Menu_Product);

                    //Add vào List New Menu Product
                    newMenu_Product.Add(Menu_Product);
                }
                // Ghi log
                WriteLog("MenuProduct", "Create", null, newMenu_Product);

                //List New Menu Shift
                List<TBL_MENU_SHIFT> newMenu_Shift = new List<TBL_MENU_SHIFT>();
                for (int i = 0; i < MenuShift.Count; i++)
                {
                    TBL_MENU_SHIFT Menu_Shift = new TBL_MENU_SHIFT();
                    Menu_Shift.MenuID = MenuID;
                    Menu_Shift.ShiftID = int.Parse(MenuShift[i].Value);
                    DA_MenuShift.Instance.Insert(Menu_Shift);

                    //Add vào List New Menu Shift
                    newMenu_Shift.Add(Menu_Shift);
                }
                // Ghi log
                WriteLog("MenuShift", "Create", null, newMenu_Shift);

                flag = "1";
            }
            catch (Exception)
            {
                flag = "-1";
                throw;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int menuID)
        {
            ViewBag.ListMenuDay = DA_MenuProduct.Instance.GetListMenuDay();

            var ListMenuProDuct = from a in DA_MenuProduct.Instance.GetAll()
                      join b in DA_Product.Instance.GetAll() on a.ProductID equals b.ProductID
                      join c in DA_ProductType.Instance.GetAll() on b.ProductTypeID equals c.ProductTypeID
                    where a.MenuID==menuID
                      select new { b.ProductID, b.ProductName, a.MenuID, c.ProductTypeName };
                      

            //var ListMenuProDuct = DA_Product.Instance.GetAll().Join(DA_MenuProduct.Instance.GetAll(), x => x.ProductID, y => y.ProductID, (x, y) => new { ProductID = x.ProductID, ProductName = x.ProductName, MenuID = y.MenuID }).Where(x => x.MenuID == menuID).OrderBy(x => x.ProductName).ToList();

            List<Product_MenuProduct_Ext> LstProduct_MenuProduct = new List<Product_MenuProduct_Ext>();

            foreach (var item in ListMenuProDuct)
            {
                Product_MenuProduct_Ext ext = new Product_MenuProduct_Ext
                {
                    MenuID = menuID,
                    ProductID = item.ProductID,
                    ProductName = item.ProductName,
                    ProductTypeName = item.ProductTypeName
                };
                LstProduct_MenuProduct.Add(ext);
            }
            ViewBag.LstMenuProduct = LstProduct_MenuProduct.OrderBy(x => x.MenuID);

            List<ProductExt> lstProExt = new List<ProductExt>();

            foreach (var pro in DA_Product.Instance.GetAll().OrderBy(x => x.ProductName).Join(DA_ProductType.Instance.GetAll(), x => x.ProductTypeID, y => y.ProductTypeID, (x, y) => new { x.ProductID, x.ProductName, y.ProductTypeName }))
            {
                if (ListMenuProDuct.FirstOrDefault(x => x.ProductID == pro.ProductID) == null)
                {
                    ProductExt ext = new ProductExt
                    {
                        ProductID = pro.ProductID,
                        ProductName = pro.ProductName,
                        ProductTypeName = pro.ProductTypeName
                    };
                    lstProExt.Add(ext);
                }
            }
            ViewBag.LstProduct = lstProExt;

            //
            var ListMenuShift = DA_Shift.Instance.GetAll().Join(DA_MenuShift.Instance.GetAll(), x => x.ShiftID, y => y.ShiftID, (x, y) => new { ShiftID = x.ShiftID, ShiftName = x.ShiftName, MenuID = y.MenuID }).Where(x => x.MenuID == menuID).ToList();

            List<Menu_Shift_Ext> LstMenu_Shift = new List<Menu_Shift_Ext>();

            foreach (var item in ListMenuShift)
            {
                Menu_Shift_Ext ext = new Menu_Shift_Ext
                {
                    MenuID = menuID,
                    ShiftID = item.ShiftID,
                    ShiftName = item.ShiftName,
                };
                LstMenu_Shift.Add(ext);
            }
            ViewBag.LstShift = DA_Shift.Instance.GetAll();
            ViewBag.LstMenuShift = LstMenu_Shift;

            return View(DA_Menu.Instance.GetById(menuID));
        }

        [HttpPost]
        public ActionResult SaveEditData(TBL_MENU Menu, string menuProduct, string menuShift)
        {
            string flag = "";
            try
            {
                dynamic MenuProduct = JArray.Parse(menuProduct);
                dynamic MenuShift = JArray.Parse(menuShift);

                Menu.ModifiedUser = int.Parse(Session["UserID"].ToString());
                Menu.ModifiedDate = DateTime.Now;

                DA_Menu.Instance.Update(Menu);

                // Ghi log
                TBL_MENU oldMenu = DA_Menu.Instance.GetById(Menu.MenuID);
                WriteLog("Menu", "Edit", oldMenu, Menu);

                // List Old Menu Product
                List<TBL_MENU_PRODUCT> oldMenu_Product = new List<TBL_MENU_PRODUCT>();
                foreach (var menu_pro in DA_MenuProduct.Instance.GetAll().Where(x => x.MenuID == Menu.MenuID))
                {
                    //Add List Old Menu Product
                    oldMenu_Product.Add(menu_pro);

                    DA_MenuProduct.Instance.Delete(menu_pro);
                }

                // List New Menu Product
                List<TBL_MENU_PRODUCT> newMenu_Product = new List<TBL_MENU_PRODUCT>();
                for (int i = 0; i < MenuProduct.Count; i++)
                {
                    TBL_MENU_PRODUCT Menu_Product = new TBL_MENU_PRODUCT();
                    Menu_Product.MenuID = Menu.MenuID;
                    Menu_Product.ProductID = int.Parse(MenuProduct[i].Value);
                    DA_MenuProduct.Instance.Insert(Menu_Product);

                    //Add List New Menu Product
                    newMenu_Product.Add(Menu_Product);
                }
                // Ghi log
                WriteLog("MenuProduct", "Edit", oldMenu_Product, newMenu_Product);

                // List Old Menu Shift
                List<TBL_MENU_SHIFT> oldMenuShift = new List<TBL_MENU_SHIFT>();
                foreach (var menu_shift in DA_MenuShift.Instance.GetAll().Where(x => x.MenuID == Menu.MenuID))
                {
                    // Add List Old Menu shift
                    oldMenuShift.Add(menu_shift);

                    DA_MenuShift.Instance.Delete(menu_shift);
                }

                // List New Menu Shift
                List<TBL_MENU_SHIFT> newMenuShift = new List<TBL_MENU_SHIFT>();
                for (int i = 0; i < MenuShift.Count; i++)
                {
                    TBL_MENU_SHIFT Menu_Shift = new TBL_MENU_SHIFT();
                    Menu_Shift.MenuID = Menu.MenuID;
                    Menu_Shift.ShiftID = int.Parse(MenuShift[i].Value);
                    DA_MenuShift.Instance.Insert(Menu_Shift);

                    // Add List New Menu shift
                    newMenuShift.Add(Menu_Shift);
                }
                // Ghi log
                WriteLog("MenuShift", "Edit", oldMenuShift, newMenuShift);

                flag = "1";
            }
            catch (Exception)
            {
                flag = "-1";
                throw;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int MenuID)
        {
            string flag = "1";

            if (!DA_Menu.Instance.CheckBeforeDelete(MenuID))
                flag = "-1";
            else
            {
                // List Old Menu Shift
                List<TBL_MENU_SHIFT> oldMenuShift = new List<TBL_MENU_SHIFT>();
                foreach (var shift in DA_MenuShift.Instance.GetAll().Where(x => x.MenuID == MenuID))
                {
                    // Add vào List Old Menu Shift
                    oldMenuShift.Add(shift);

                    DA_MenuShift.Instance.Delete(shift);
                }
                // Ghi log
                WriteLog("MenuShift", "Delete", oldMenuShift, null);

                // List Old Menu Product
                List<TBL_MENU_PRODUCT> oldMenuProduct = new List<TBL_MENU_PRODUCT>();
                foreach (var product in DA_MenuProduct.Instance.GetAll().Where(x => x.MenuID == MenuID))
                {
                    // Add vào List Old Menu Product
                    oldMenuProduct.Add(product);

                    DA_MenuProduct.Instance.Delete(product);
                }
                //Ghi log
                WriteLog("MenuProduct", "Delete", oldMenuProduct, null);

                //Ghi log
                TBL_MENU oldMenu = DA_Menu.Instance.GetById(MenuID);
                WriteLog("Menu", "Delete", oldMenu, null);

                DA_Menu.Instance.Delete(MenuID);
                
                flag = "1";
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}
