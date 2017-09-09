using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class Schedule_Menu_Product_Shift_Ext
    {
        public long ScheduleMenuID { get; set; }
        public DateTime MealDate { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeID { get; set; }
        public int ShiftID { get; set; }
        public string ShiftCode { get; set; }
    }
}