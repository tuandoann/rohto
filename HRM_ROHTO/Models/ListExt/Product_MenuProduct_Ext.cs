using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class Product_MenuProduct_Ext
    {
        public int MenuID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public DateTime MealDate { get; set; }
        public int ShiftID { get; set; }
        public int ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
    }
}