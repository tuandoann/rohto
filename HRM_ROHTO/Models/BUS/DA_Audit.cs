using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Audit : EfRepositoryBase<TBL_AUDIT>
    {
        #region Constructor
        private static volatile DA_Audit _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Audit Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Audit();
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
        public List<TBL_AUDIT> getObjectForDatatable(DateTime DateFrom, DateTime DateTo, string search, int start, int length, string sortColumn, string sortColumnDir)
        {
            try
            {
                using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
                {
                    List<TBL_AUDIT> getData = new List<TBL_AUDIT>();
                    //if (DateFrom != null && DateTo != null)
                    //{
                    //    getData = context.TBL_AUDIT.Where(n => (DateTime.Compare(n.AuditDate, DateFrom) >= 0 && DateTime.Compare(n.AuditDate, DateTo) <= 0)).ToList<TBL_AUDIT>();
                    //}
                    //if (!string.IsNullOrWhiteSpace(search))
                    //{
                    //    getData = getData.Where(n => n.UserName.Contains(search) || n.FunctionName.Contains(search) || n.Action.Contains(search) || n.OldData.Contains(search) || n.NewData.Contains(search)).ToList<TBL_AUDIT>();
                    //}
                    //getData = getData.Skip(start).Take(length).ToList<TBL_AUDIT>();
                    //if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortColumnDir))
                    //{
                    //    getData = getData.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                    //}
                    //check data
                    if (DateFrom == null || DateTo == null)
                        DateFrom = DateTo = DateTime.Now;
                    search = String.IsNullOrWhiteSpace(search) ? "" : search;
                    sortColumn = String.IsNullOrWhiteSpace(sortColumn) ? "" : sortColumn;
                    sortColumnDir = String.IsNullOrWhiteSpace(sortColumnDir) ? "" : sortColumnDir;
                    //get data
                    getData = (from audit in context.TBL_AUDIT
                              where ( (DateFrom == null && DateTo == null) || ((DateTime.Compare(audit.AuditDate, DateFrom) >= 0 && DateTime.Compare(audit.AuditDate, DateTo) <= 0)) ) &&
                                ( (search == "") || audit.UserName.Contains(search) || audit.FunctionName.Contains(search) || audit.Action.Contains(search) || audit.OldData.Contains(search) || audit.NewData.Contains(search))
                               select audit).OrderBy((sortColumn == "" && sortColumnDir == "") ? "AuditDate asc" : sortColumn + " " + sortColumnDir).Skip(start).Take(length).ToList<TBL_AUDIT>();
                    return getData;
                }
            }
            catch (Exception ex)
            {
                return new List<TBL_AUDIT>();
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
                List<TBL_AUDIT> getData = new List<TBL_AUDIT>();
                //check data
                if (DateFrom == null || DateTo == null)
                    DateFrom = DateTo = DateTime.Now;
                search = String.IsNullOrWhiteSpace(search) ? "" : search;
                //get data
                getData = (from audit in context.TBL_AUDIT
                           where ((DateFrom == null && DateTo == null) || ((DateTime.Compare(audit.AuditDate, DateFrom) >= 0 && DateTime.Compare(audit.AuditDate, DateTo) <= 0))) &&
                             ((search == "") || audit.UserName.Contains(search) || audit.FunctionName.Contains(search) || audit.Action.Contains(search) || audit.OldData.Contains(search) || audit.NewData.Contains(search))
                           select audit).ToList<TBL_AUDIT>();
                result = getData.Count();
            }
            return result;
        }

        /// <summary>
        /// get item base id item
        /// </summary>
        /// <param name="id">id item</param>
        /// <returns>item</returns>
        public TBL_AUDIT getItemBaseId(long id)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_AUDIT element = id == null ? null : context.TBL_AUDIT.SingleOrDefault(n => n.AuditID == id);
                return element;
            }
        }
    }
}