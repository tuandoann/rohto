using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public bool CheckPermission(string FunctionName, int ActionType)
        {
            SYS_ROLEPERMISSION item = new SYS_ROLEPERMISSION
            {
                View = false,
                Add = false,
                Edit = false,
                Delete = false,
                Print = false,
                Import = false,
                Export = false,
                Accept = false,
                Cancel = false
            };
            if (Session["RoleID"] != null)
            {
                int RoleID = int.Parse(Session["RoleID"].ToString());
                bool IsAdmin = false;
                bool.TryParse(Session["IsAdmin"].ToString(), out IsAdmin);

                item = DA_RolePermission.Instance.GetPermissionByFunction(RoleID, FunctionName, IsAdmin);

            }
            ViewBag.View = item.View;
            ViewBag.Add = item.Add;
            ViewBag.Edit = item.Edit;
            ViewBag.Delete = item.Delete;
            ViewBag.Print = item.Print;

            switch (ActionType)
            {
                case 1: // view
                    {
                        return item.View;
                    }
                case 2: // add
                    {
                        return item.Add;
                    }
                case 3: // edit
                    {
                        return item.Edit;
                    }
                default:
                    {
                        return item.View;
                    }
            }
        }

        public void WriteLog(string FunctionName, string ActionName, object OldData = null, object NewData = null)
        {
            TBL_AUDIT Audit = new TBL_AUDIT();
            Audit.UserName = Session["UserName"].ToString();
            Audit.AuditDate = DateTime.Now;
            Audit.FunctionName = FunctionName;
            Audit.Action = ActionName;
            Audit.OldData = JsonConvert.SerializeObject(OldData);
            Audit.NewData = JsonConvert.SerializeObject(NewData);
            DA_Audit.Instance.Insert(Audit);
        }
    }
}
