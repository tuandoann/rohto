using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_SpecialCase : EfRepositoryBase<TBL_SPECIAL>
    {
        #region Constructor
        private static volatile DA_SpecialCase _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_SpecialCase Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_SpecialCase();
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// get list object for datatable
        /// </summary>
        /// <param name="DateFrom">Date From</param>
        /// <param name="DateTo">Date To</param>
        /// <param name="search">Content search</param>
        /// <param name="start">index start pagging</param>
        /// <param name="length">length pagging</param>
        /// <returns></returns>
        public List<TBL_SPECIAL> getObjectForDatatable(DateTime DateFrom, DateTime DateTo, string search, int start, int length, string sortColumn, string sortColumnDir)
        {
            try
            {
                using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
                {
                    List<TBL_SPECIAL> getData = new List<TBL_SPECIAL>();
                    if(DateFrom!=null && DateTo !=null)
                    {
                        getData = context.TBL_SPECIAL.Where(n => (DateTime.Compare(n.SpecialDate, DateFrom) >= 0 && DateTime.Compare(n.SpecialDate, DateTo) <= 0)).ToList<TBL_SPECIAL>();
                    }
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        getData = getData.Where(n => n.Notes.Contains(search)).ToList<TBL_SPECIAL>();
                    }
                    getData = getData.Skip(start).Take(length).ToList<TBL_SPECIAL>();
                    if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortColumnDir))
                    {
                        getData = getData.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                    }
                    return getData;
                }
            }
            catch(Exception ex)
            {
                return new List<TBL_SPECIAL>();
            }
            
        }
        /// <summary>
        /// count all object for datatable
        /// </summary>
        /// <param name="DateFrom">Date From</param>
        /// <param name="DateTo">Date To</param>
        /// <param name="search">Content search</param>
        /// <returns></returns>
        public long getCountOBjectForDatatable(DateTime DateFrom, DateTime DateTo, string search)
        {
            long result = 0;
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                List<TBL_SPECIAL> getData = new List<TBL_SPECIAL>();
                if (DateFrom != null && DateTo != null)
                {
                    getData = context.TBL_SPECIAL.Where(n => (DateTime.Compare(n.SpecialDate, DateFrom) > 0 && DateTime.Compare(n.SpecialDate, DateTo) < 0)).ToList<TBL_SPECIAL>();
                }
                if (!string.IsNullOrWhiteSpace(search))
                {
                    getData = getData.Where(n => n.Notes.Contains(search)).ToList<TBL_SPECIAL>();
                }
                result = getData.Count();
            }
            return result;
        }
        public Boolean checkExistDateObject(DateTime date, int idObject)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_SPECIAL item = context.TBL_SPECIAL.SingleOrDefault(n => DateTime.Compare(n.SpecialDate, date) == 0);
                if(item != null)
                {
                    if(idObject !=0)
                    {
                        return (item.SpecialID == idObject) ? false : true;
                    }
                    return true;
                }
            }
            return false;
        }

    }
}