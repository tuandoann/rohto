using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_ROHTO.Util.BUS
{
    public class Base
    {
        public string mn_Error { get; set; }
        public DataTable mn_Table { get; set; }
        protected Database da;

        public Base()
        {
            mn_Error = string.Empty;
            mn_Table = new DataTable();
            da = new Database();
        }

        protected bool fn_Get(string ProName, params SqlParameter[] parameter)
        {
            if (!da.fn_GetData_Pro(ProName, parameter))
            {
                this.mn_Error = da.mn_Error;
                return false;
            }
            this.mn_Table = da.mn_Table;
            return true;
        }

        protected bool fn_Execute(string ProName, params SqlParameter[] parameter)
        {
            if (!da.fn_Execute_Pro(ProName, parameter))
            {
                this.mn_Error = da.mn_Error;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Value"></param>
        /// <param name="type">0: int, 1: string, uniqueidentify</param>
        /// <returns></returns>
        protected bool fn_CheckDelete(string TableName, string Value, int type)
        {
            if (!da.fn_GetData_Pro("pr_CheckDELETE", new SqlParameter("@TableNameSource", TableName)
                                                      , new SqlParameter("@Value", Value)
                                                      , new SqlParameter("@type", type)))
            {
                this.mn_Error = da.mn_Error;
                return false;
            }
            if (da.mn_Table.Rows.Count == 0)
                return true;
            if (da.mn_Table.Rows[0][0].ToString() == "")
                return true;
            this.mn_Error = "Thông tin đã được lưu trong bảng " + da.mn_Table.Rows[0]["TableName"].ToString();
            return false;
        }


        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Column"></param>
        /// <param name="ID"></param>
        /// <param name="Value"></param>
        /// <param name="type"></param>
        /// <param name="Insert"></param>
        /// <param name="textResult"></param>
        /// <returns></returns>
        /// 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ID">ID cua dòng sử dụng trong trường hợp cập nhật</param>
        /// <param name="IDType">Kiểu dữ liệu của cột ID, 0:Int, 1: String, UniqueIdentifier</param>
        /// <param name="IDColumn">Tên cột ID</param>
        /// <param name="Value">Giá trị cần kiểm tra</param>
        /// <param name="Valuetype">Kiểu dữ liệu của cột cần kiểm tra, 0:Int, 1: String, UniqueIdentifier</param>
        /// <param name="ValueColumn">Tên cột cần kiểm tra</param>
        /// <param name="Insert">1: Insert, 0:Update</param>
        /// <param name="textResult">Nếu đã tồn tại sẽ dùng đoạn này hiện thông báo</param>
        /// <returns></returns>
        protected bool fn_CheckUnique(string TableName, string ID, int IDType, string IDColumn, string Value, int Valuetype, string ValueColumn, int Insert, string textResult)
        {
            if (!da.fn_GetData_Pro("pr_CheckUnique", new SqlParameter("@TableName", TableName)
                                                         , new SqlParameter("@ID", ID)
                                                         , new SqlParameter("@IDType", IDType)
                                                         , new SqlParameter("@IDColumn", IDColumn)
                                                         , new SqlParameter("@Value", Value)
                                                         , new SqlParameter("@Valuetype", Valuetype)
                                                         , new SqlParameter("@ValueColumn", ValueColumn)
                                                         , new SqlParameter("@Insert", Insert)))
            {
                this.mn_Error = da.mn_Error;
                return false;
            }
            if (da.mn_Table.Rows.Count == 0)
                return true;
            if (da.mn_Table.Rows[0][0].ToString() == "0")
                return true;
            this.mn_Error = textResult;
            return false;
        }
    
    }
}
