using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Shift : EfRepositoryBase<SYS_SHIFT>
    {
        #region Constructor
        private static volatile DA_Shift _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Shift Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Shift();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public IList<SelectListItem> GetComboboxShift()
        {
            IList<SelectListItem> lstShift = new List<SelectListItem>();
            foreach (var shift in Instance.GetAll())
            {
                lstShift.Add(new SelectListItem { Value = shift.ShiftID.ToString(), Text = shift.ShiftCode });
            }
            return lstShift;
        }
    }
}