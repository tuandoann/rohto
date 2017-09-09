using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_ScheduleMenuProduct : EfRepositoryBase<TBL_SCHEDULE_MENU_PRODUCT>
    {
        #region Constructor
        private static volatile DA_ScheduleMenuProduct _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_ScheduleMenuProduct Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_ScheduleMenuProduct();
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}