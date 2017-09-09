using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_ExceptionMeal_Shift : EfRepositoryBase<TBL_EXCEPTION_MEAL_SHIFT>
    {
        #region Contructor
        public static volatile DA_ExceptionMeal_Shift _Instance;
        public static readonly object RootSync = new Object();
        public static DA_ExceptionMeal_Shift Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (RootSync)
                    {
                        if (_Instance == null)
                            _Instance = new DA_ExceptionMeal_Shift();
                    }
                }
                return _Instance;
            }
        }
        #endregion
    }
}