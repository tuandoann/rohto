using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_MenuProduct : EfRepositoryBase<TBL_MENU_PRODUCT>
    {
        #region Constructor
        private static volatile DA_MenuProduct _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_MenuProduct Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_MenuProduct();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public IList<SelectListItem> GetListMenuDay()
        {
            IList<SelectListItem> List = new List<SelectListItem>();
            for (int i = 1; i <= 31; i++)
            {
                List.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return List;
        }
    }
}