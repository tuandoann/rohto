using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Position : EfRepositoryBase<TBL_POSITION>
    {
        #region Constructor
        private static volatile DA_Position _instance;
        private static readonly object SyncRoot = new Object();

        public static DA_Position Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Position();
                    }
                }
                return _instance;
            }
        }
        #endregion

        public int? GetPositionIDByCode(string positionCode)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_POSITION Position = context.TBL_POSITION.FirstOrDefault(x => x.PositionCode == positionCode);

                int? PositionID = null;

                if (Position != null)
                    PositionID = Position.PositionID;

                return PositionID;
            }
        }

        public List<SelectListItem> GetComboboxPosition()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                foreach (var item in context.TBL_POSITION)
                {
                    list.Add(new SelectListItem { Value = item.PositionID.ToString(), Text = item.PositionName });
                }
            }
            return list;
        }

        public List<SelectListItem> GetComboboxPosition2()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                foreach (var item in context.TBL_POSITION)
                {
                    list.Add(new SelectListItem { Value = item.PositionID.ToString(), Text = item.PositionName });
                }
            }
            return list;
        }

        public List<SelectListItem> GetComboboxPosition3()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                foreach (var item in context.TBL_POSITION)
                {
                    list.Add(new SelectListItem { Value = item.PositionID.ToString(), Text = item.PositionName });
                }
            }
            return list;
        }

        public bool CheckPositionCodeNotExist(string PositionCode)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_POSITION ds = context.TBL_POSITION.FirstOrDefault(x => x.PositionCode == PositionCode);
                if (ds != null)
                    return false;
            }
            return true;
        }

        public bool CheckEditPositionCodeHasExist(int PositionID, string PositionCode)
        {
            using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
            {
                TBL_POSITION ds = context.TBL_POSITION.FirstOrDefault(x => x.PositionID == PositionID);
                if (ds == null)
                    return false;
                if (ds.PositionCode != PositionCode && !CheckPositionCodeNotExist(PositionCode))
                    return false;
            }
            return true;
        }

        public bool CheckBeforeDelete(int PositionID)
        {
            TBL_EMPLOYEE Employee = DA_Employee.Instance.GetAll().FirstOrDefault(x => x.PositionID1 == PositionID || x.PositionID2 == PositionID || x.PositionID3 == PositionID);

            if (Employee != null)
                return false;

            return true;
        }
    }
}