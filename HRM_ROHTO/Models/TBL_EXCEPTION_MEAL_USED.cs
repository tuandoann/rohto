//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM_ROHTO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_EXCEPTION_MEAL_USED
    {
        public long ExceptionMealUsedID { get; set; }
        public System.DateTime MealDate { get; set; }
        public System.DateTime PrintTime { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public int MenuID { get; set; }
        public Nullable<int> UserCreate { get; set; }
        public Nullable<int> UserPrint { get; set; }
        public Nullable<int> POSCreate { get; set; }
        public Nullable<int> POSModified { get; set; }
        public Nullable<int> POSPrint { get; set; }
    }
}
