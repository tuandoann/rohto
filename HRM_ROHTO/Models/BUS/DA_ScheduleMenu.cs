using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.ListExt;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_ScheduleMenu : EfRepositoryBase<TBL_SCHEDULE_MENU>
    {
        #region Constructor
        private static volatile DA_ScheduleMenu _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_ScheduleMenu Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_ScheduleMenu();
                    }
                }
                return _instance;
            }
        }
        #endregion

        //public IList<TBL_SCHEDULE_MENU> GetDateMealScheduleMenu(int year, int month)
        //{
        //    IList<TBL_SCHEDULE_MENU> lstMenu = new List<TBL_SCHEDULE_MENU>();
        //    using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
        //    {
        //        lstMenu = (from menu in context.TBL_SCHEDULE_MENU
        //                  where menu.MealDate.Year == year && menu.MealDate.Month == month
        //                  group menu by menu.MealDate into g
        //                  select g.OrderBy(x => x.MealDate.Day).FirstOrDefault()).ToList();
        //    }
        //    return lstMenu;
        //}

        //public IList<Schedule_Menu_Product_Shift_Ext> GetDataScheduleMenu()
        //{
        //    IList<Schedule_Menu_Product_Shift_Ext> lstMenuProExt = new List<Schedule_Menu_Product_Shift_Ext>();
        //    using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
        //    {
        //        var lst = (from menu in context.TBL_SCHEDULE_MENU
        //                   join menu_pro in context.TBL_SCHEDULE_MENU_PRODUCT on menu.ScheduleMenuID equals menu_pro.ScheduleMenuID
        //                   join menu_shift in context.TBL_SCHEDULE_MENU_SHIFT on menu.ScheduleMenuID equals menu_shift.ScheduleMenuID
        //                   join shift in context.SYS_SHIFT on menu_shift.ShiftID equals shift.ShiftID
        //                   join pro in context.TBL_PRODUCT on menu_pro.ProductID equals pro.ProductID
        //                   join type in context.SYS_PRODUCT_TYPE on pro.ProductTypeID equals type.ProductTypeID
        //                   select new { menu.ScheduleMenuID, menu_pro.ProductID, pro.ProductName, pro.ProductTypeID, menu.MealDate, menu.MenuID , shift.ShiftID, shift.ShiftCode}).ToList();

        //        foreach (var item in lst)
        //        {
        //            Schedule_Menu_Product_Shift_Ext ext = new Schedule_Menu_Product_Shift_Ext
        //            {
        //                ScheduleMenuID = item.ScheduleMenuID,
        //                MealDate = item.MealDate,
        //                ProductID = item.ProductID,
        //                ProductName = item.ProductName,
        //                ProductTypeID = item.ProductTypeID,
        //                ShiftID = item.ShiftID,
        //                ShiftCode = item.ShiftCode
        //            };
        //            lstMenuProExt.Add(ext);
        //        }
        //    }
        //    return lstMenuProExt;
        //}

        public string GetDayOfWeek(string dayOfWeek)
        {
            if (dayOfWeek == "Monday")
                return "Thứ 2";
            if (dayOfWeek == "Tuesday")
                return "Thứ 3";
            if (dayOfWeek == "Wednesday")
                return "Thứ 4";
            if (dayOfWeek == "Thursday")
                return "Thứ 5";
            if (dayOfWeek == "Friday")
                return "Thứ 6";
            if (dayOfWeek == "Saturday")
                return "Thứ 7";
            if (dayOfWeek == "Sunday")
                return "Chủ nhật";
            return "";
        }

        public IList<TBL_SCHEDULE_MENU> GetScheduleMenu(int year, int month)
        {
            IList<TBL_SCHEDULE_MENU> lstSchedule_Menu = new List<TBL_SCHEDULE_MENU>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                lstSchedule_Menu = (from menu in context.TBL_SCHEDULE_MENU
                                    where menu.MealDate.Year == year && menu.MealDate.Month == month
                                    group menu by menu.MealDate into g
                                    select g.OrderBy(x => x.MealDate).FirstOrDefault()).ToList();
            }
            return lstSchedule_Menu;
        }

        public IList<Schedule_Menu_Shift_Ext> GetScheduleMenuShift()
        {
            IList<Schedule_Menu_Shift_Ext> lstSchedule_Menu_Shift = new List<Schedule_Menu_Shift_Ext>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var lst = (from menu in context.TBL_SCHEDULE_MENU
                           join menu_shift in context.TBL_SCHEDULE_MENU_SHIFT on menu.ScheduleMenuID equals menu_shift.ScheduleMenuID
                           join shift in context.SYS_SHIFT on menu_shift.ShiftID equals shift.ShiftID
                           select new { menu_shift.ShiftID, shift.ShiftCode, menu.MealDate }).ToList();

                foreach (var item in lst)
                {
                    Schedule_Menu_Shift_Ext ext = new Schedule_Menu_Shift_Ext
                    {
                        ShiftID = item.ShiftID,
                        MealDate = item.MealDate,
                        ShiftCode = item.ShiftCode
                    };
                    lstSchedule_Menu_Shift.Add(ext);
                }
            }
            return lstSchedule_Menu_Shift;
        }

        public IList<Product_MenuProduct_Ext> GetScheduleProduct()
        {
            IList<Product_MenuProduct_Ext> lstSchedule_Menu_Product = new List<Product_MenuProduct_Ext>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var lst = from menu in context.TBL_SCHEDULE_MENU
                          join menu_pro in context.TBL_SCHEDULE_MENU_PRODUCT on menu.ScheduleMenuID equals menu_pro.ScheduleMenuID
                          join menu_shi in context.TBL_SCHEDULE_MENU_SHIFT on menu.ScheduleMenuID equals menu_shi.ScheduleMenuID
                          join pro in context.TBL_PRODUCT on menu_pro.ProductID equals pro.ProductID
                          select new { pro.ProductName, menu.MealDate, menu_shi.ShiftID, pro.ProductTypeID };

                foreach (var item in lst)
                {
                    Product_MenuProduct_Ext ext = new Product_MenuProduct_Ext
                    {
                        ProductName = item.ProductName,
                        MealDate = item.MealDate,
                        ShiftID = item.ShiftID,
                        ProductTypeID = item.ProductTypeID
                    };
                    lstSchedule_Menu_Product.Add(ext);
                }
            }
            return lstSchedule_Menu_Product;
        }

        public int GetProductTypeIDFromName(string productName)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                var type = DA_Product.Instance.GetAll().FirstOrDefault(x => x.ProductName == productName);

                if (type != null)
                    return type.ProductTypeID;
            }
            return -1;
        }
    }
}