using HRM_ROHTO.Models.ListExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Product : EfRepositoryBase<TBL_PRODUCT>
    {
        #region Constructor
        private static volatile DA_Product _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Product Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Product();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public IList<ProductExt> GetProductExt()
        {
            List<ProductExt> lst = new List<ProductExt>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                foreach (var item in context.TBL_PRODUCT)
                {
                    ProductExt ext = new ProductExt
                    {
                        ProductID = item.ProductID,
                        ProductName = item.ProductName,
                        ProductTypeName = context.SYS_PRODUCT_TYPE.FirstOrDefault(x => x.ProductTypeID == item.ProductTypeID).ProductTypeName,
                        IsActive = item.IsActive == true ? "Đang sử dụng" : "Ngưng sử dụng"
                    };
                    lst.Add(ext);

                }
            }
            return lst;
        }
        public List<Object> getPrductForExport()
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                List<object> result = new List<object>();
                result = (from p in context.TBL_PRODUCT
                          join t in context.SYS_PRODUCT_TYPE on p.ProductTypeID equals t.ProductTypeID
                          select  new {STT = 1, ProductCode = p.ProductCode, ProductName = p.ProductName, ProductTypeName = t.ProductTypeName, IsActive = (p.IsActive == true ? "Hoạt động" :"Không hoạt động"), Description = p.Description }).ToList<object>();
                return result;
            }
        }

        public bool CheckProductCodeNotExist(string ProductCode)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_PRODUCT ds = context.TBL_PRODUCT.FirstOrDefault(x => x.ProductCode == ProductCode);
                if (ds != null)
                    return false;
            }
            return true;
        }

        public bool CheckEditProductCodeHasExist(int ProductID, string ProductCode)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_PRODUCT ds = context.TBL_PRODUCT.FirstOrDefault(x => x.ProductID == ProductID);
                if (ds == null)
                    return false;
                if (ds.ProductCode != ProductCode && !CheckProductCodeNotExist(ProductCode))
                    return false;
            }
            return true;
        }

        public bool CheckBeforeDelete(int ProductID)
        {
            TBL_MENU_PRODUCT Menu_Product = DA_MenuProduct.Instance.GetAll().FirstOrDefault(x => x.ProductID == ProductID);

            TBL_REGISTER_MEAL_PRODUCT Register_Meal_Product = DA_Register_Meal_Product.Instance.GetAll().FirstOrDefault(x => x.ProductID == ProductID);

            TBL_REGISTER_MEAL_PRODUCT_OLD Register_Meal_Product_Old = DA_Register_Meal_Product_Old.Instance.GetAll().FirstOrDefault(x => x.ProductID == ProductID);

            TBL_SCHEDULE_MENU_PRODUCT Schedule_Menu_Product = DA_ScheduleMenuProduct.Instance.GetAll().FirstOrDefault(x => x.ProductID == ProductID);

            TBL_EXCEPTION_MEAL_USED_PRODUCT Exception_Meal_Used_Product = DA_Exception_Meal_Used_Product.Instance.GetAll().FirstOrDefault(x => x.ProductID == ProductID);

            if (Menu_Product != null || Register_Meal_Product != null || Register_Meal_Product_Old != null || Schedule_Menu_Product != null || Exception_Meal_Used_Product != null)
                return false;
            
            return true;
        }
    }
}