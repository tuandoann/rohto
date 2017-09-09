using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Menu : EfRepositoryBase<TBL_MENU>
    {
        #region Constructor
        private static volatile DA_Menu _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Menu Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Menu();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public bool CheckBeforeDelete(int MenuID)
        {
            TBL_SCHEDULE_MENU Schedule_Menu = DA_ScheduleMenu.Instance.GetAll().FirstOrDefault(x => x.MenuID == MenuID);

            if (Schedule_Menu != null)
                return false;

            return true;
        }

        public string CheckMenuDayAndShiftIDExist(int MenuDay, string menuShift)
        {
            var Join = from menu in GetAll()
                       join menu_shi in DA_MenuShift.Instance.GetAll() on menu.MenuID equals menu_shi.MenuID
                       join shi in DA_Shift.Instance.GetAll() on menu_shi.ShiftID equals shi.ShiftID
                       where menu.MenuDay == MenuDay
                       select new { menu.MenuID, menu.MenuDay, shi.ShiftID, shi.ShiftCode };

            dynamic MenuShift = JArray.Parse(menuShift);

            string Shift = "";


            for (int i = 0; i < MenuShift.Count; i++)
            {
                foreach (var item in Join)
                {
                    if (item.ShiftID == int.Parse(MenuShift[i].Value))
                    {
                        Shift += item.ShiftCode + ", ";
                        break;
                    }
                }
            }
            return Shift;
        }

        public string CheckEditMenuDayAndShiftIDExist(int MenuID, int MenuDay, string menuShift)
        {
            // Lấy ShiftID từ Database của Menu_shift có MenuID = MenuID
            var Join = from menu in GetAll()
                       join menu_shi in DA_MenuShift.Instance.GetAll() on menu.MenuID equals menu_shi.MenuID
                       where menu.MenuID == MenuID
                       select new { menu_shi.ShiftID };
            
            // Menu_Shift truyền từ View
            var MenuShift = JArray.Parse(menuShift);

            // Lấy ShiftID truyền từ view không có trong ShiftID lấy từ Database
            List<string> lstShiftID = new List<string>();
            for (int i = 0; i < MenuShift.Count; i++)
            {
                bool Check = true;
                foreach (var item in Join)
                {
                    if (MenuShift[i].ToString() != item.ShiftID.ToString())
                        Check = false;
                    else
                    {
                        Check = true;
                        break;
                    }
                }
                if (Check == false)
                    lstShiftID.Add(MenuShift[i].ToString());
            }

            if (lstShiftID.Count == 0)
            {
                return "";
            }

            // Kiếm tra ShiftID dư tồn tại
            var lstJoin = from menu in GetAll()
                       join menu_shi in DA_MenuShift.Instance.GetAll() on menu.MenuID equals menu_shi.MenuID
                       join shi in DA_Shift.Instance.GetAll() on menu_shi.ShiftID equals shi.ShiftID
                       where menu.MenuDay == MenuDay
                       select new { menu.MenuID, menu.MenuDay, shi.ShiftID, shi.ShiftCode };
            
            string Shift = "";
            
            for (int i = 0; i < lstShiftID.Count; i++)
            {
                foreach (var item in lstJoin)
                {
                    if (item.ShiftID == int.Parse(lstShiftID[i].ToString()))
                    {
                        Shift += item.ShiftCode + ", ";
                        break;
                    }
                }
            }

            return Shift;
        }
    }
}