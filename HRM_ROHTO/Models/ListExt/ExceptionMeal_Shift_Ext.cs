using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class ExceptionMeal_Shift_Ext
    {
        public long ExceptionMealID { get; set; }
        public int ShiftID { get; set; }
        public int ExceptionQty { get; set; }
        public int ExceptionQtyUsed { get; set; }
        public string ShiftName { get; set; }
    }
}