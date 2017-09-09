using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Broadcast : EfRepositoryBase<TBL_BROADCAST>
    {
        #region Constructor
        private static volatile DA_Broadcast _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Broadcast Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Broadcast();
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}