using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_ExceptionMeal : EfRepositoryBase<TBL_EXCEPTION_MEAL>
    {
        #region Constructor
        private static volatile DA_ExceptionMeal _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_ExceptionMeal Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_ExceptionMeal();
                    }
                }
                return _instance;
            }
        }
        #endregion
        
        public bool CheckDateExist(DateTime Date)
        {
            TBL_EXCEPTION_MEAL Meal = GetAll().FirstOrDefault(x => x.ExceptionDate == Date);
            if (Meal != null)
                return false;
            return true;
        }

        public bool CheckEditDateExist(long ID, DateTime Date)
        {
            TBL_EXCEPTION_MEAL Meal = GetAll().FirstOrDefault(x => x.ExceptionMealID == ID);
            if (Date != Meal.ExceptionDate && (!CheckDateExist(Date)))
                return false;
            return true;
        }
    }
}