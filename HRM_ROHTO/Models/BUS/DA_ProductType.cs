using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_ProductType : EfRepositoryBase<SYS_PRODUCT_TYPE>
    {
        #region Constructor
        private static volatile DA_ProductType _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_ProductType Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_ProductType();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public List<SelectListItem> GetComboboxProductType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                foreach (var item in context.SYS_PRODUCT_TYPE)
                {
                    list.Add(new SelectListItem { Value = item.ProductTypeID.ToString(), Text = item.ProductTypeName });
                }
            }
            return list;
        }

        public List<SelectListItem> GetComboboxProductTypeCode()
        {
            List<SelectListItem> List = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                foreach (var item in context.SYS_PRODUCT_TYPE)
                {
                    List.Add(new SelectListItem { Value = item.ProductTypeID.ToString(), Text = item.ProductTypeName });
                }
            }
            return List;
        }

        /// <summary>
        /// get name product type base Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string getNameBaseId(int id)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                return context.SYS_PRODUCT_TYPE.SingleOrDefault(n => n.ProductTypeID == id).ProductTypeName;
            }
        }
    }
}