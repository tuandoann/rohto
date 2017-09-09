using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM_ROHTO.Models.BUS
{
    public class DA_Config : EfRepositoryBase<SYS_CONFIG>
    {
        #region Constructor
        private static volatile DA_Config _instance;
        private static readonly object SyncRoot = new Object();
        //1: numberic, 2: boolean, 3: textbox, 4: email, 5: pass
        public static readonly Dictionary<string, int> KeyEntity = new Dictionary<string, int>
        {
            { "ChangeMealBeforeHour", 1 },
            { "Email", 3},
            { "EmailServer", 4},
            { "ExceptionMeal", 1 },
            { "MaxDayRegister", 1 },
            { "Password", 5 },
            { "PREPAREDBY", 3 },
            { "ServerName", 3 },
            { "SMTP", 1},
            { "UseSSLAdmin", 2 }
        };

        public static DA_Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DA_Config();
                    }
                }
                return _instance;
            }
        }
        #endregion   
        /// <summary>
        /// get all entity
        /// </summary>
        /// <returns></returns>
        public List<object> getAllEntity()
        {
            try
            {
                using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
                {
                    List<SYS_CONFIG> t = (from i in context.SYS_CONFIG select i ).ToList();
                    List<object> result = t.Select(n => new { Id = n.ConfigID, Value = n.Value, Des = n.Description, type = KeyEntity[n.ConfigID] }).ToList<object>();
                    return result;
                }
            }
            catch(Exception ex){return null;}
        }

        /// <summary>
        /// get value base key
        /// </summary>
        /// <param name="nameKey">key</param>
        /// <returns>value</returns>
        public string getValueFromKeyConfig(string nameKey)
        {
            try
            {
                using (var context = (HRM_ROHTOEntities)Activator.CreateInstance(typeof(HRM_ROHTOEntities), _connectionStr))
                {
                    SYS_CONFIG item = context.SYS_CONFIG.FirstOrDefault(n => String.Compare(n.ConfigID, nameKey) == 0);
                    return item.Value;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}