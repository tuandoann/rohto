using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_ScheduleMenuShift : EfRepositoryBase<TBL_SCHEDULE_MENU_SHIFT>
    {
        #region Constructor
        private static volatile DA_ScheduleMenuShift _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_ScheduleMenuShift Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_ScheduleMenuShift();
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}