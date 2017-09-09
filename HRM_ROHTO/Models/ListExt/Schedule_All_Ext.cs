using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class Schedule_All_Ext
    {
        public IList<TBL_SCHEDULE_MENU> Schedule_Menu_Ext { get; set; }
        //public IList<Schedule_Menu_Product_Shift_Ext> Schedule_Menu_Product_Shift_Ext { get; set; }
        public IList<Schedule_Menu_Shift_Ext> Schedule_Menu_Shift_Ext { get; set; }
        //public IList<TBL_SCHEDULE_MENU> ScheduleMenuID { get; set; }
        public IList<Product_MenuProduct_Ext> Schedule_Menu_Product_Ext { get; set; }
    }
}