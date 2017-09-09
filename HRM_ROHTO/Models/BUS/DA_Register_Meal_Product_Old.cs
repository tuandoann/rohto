using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Register_Meal_Product_Old : EfRepositoryBase<TBL_REGISTER_MEAL_PRODUCT_OLD>
    {
        #region Constructor
        private static volatile DA_Register_Meal_Product_Old _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Register_Meal_Product_Old Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Register_Meal_Product_Old();
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}