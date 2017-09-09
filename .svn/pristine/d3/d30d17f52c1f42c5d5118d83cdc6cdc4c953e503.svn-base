using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Exception_Meal_Used_Product : EfRepositoryBase<TBL_EXCEPTION_MEAL_USED_PRODUCT>
    {
        #region Constructor
        private static volatile DA_Exception_Meal_Used_Product _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Exception_Meal_Used_Product Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Exception_Meal_Used_Product();
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}