using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Register_Meal_Product : EfRepositoryBase<TBL_REGISTER_MEAL_PRODUCT>
    {
        #region Constructor
        private static volatile DA_Register_Meal_Product _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Register_Meal_Product Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Register_Meal_Product();
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}