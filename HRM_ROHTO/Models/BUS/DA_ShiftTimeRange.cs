using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_ShiftTimeRange : EfRepositoryBase<TBL_SHIFT_TIME_RANGE>
    {
        #region Constructor
        private static volatile DA_ShiftTimeRange _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_ShiftTimeRange Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_ShiftTimeRange();
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}