using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.ListExt
{
    public class Shift_Time_Range_Ext
    {
        public int ShiftTimeRangeID { get; set; }
        public int ShiftID { get; set; }
        public string ShiftCode { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
    }
}