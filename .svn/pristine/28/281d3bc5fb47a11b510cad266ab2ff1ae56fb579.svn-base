
using System;
using System.Data;
using System.Data.SqlClient;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Text;
using HRM_ROHTO.Util.DTO;
namespace HRM_ROHTO.Util.BUS
{
    public class BUS_User : Base
    {
        public bool Login(string UserName, string Password)
        {

            base.fn_Get("pr_User_Login", new SqlParameter("@UserName", UserName)
                                                , new SqlParameter("@Password", Password));
            if (base.mn_Table != null && base.mn_Table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkEmailExist(string Email, Guid StoreID)
        {

            base.fn_Get("pr_User_CheckEmailExist", new SqlParameter("@Email", Email));
            if (base.mn_Table != null && base.mn_Table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool fn_Get(string UserID, string Search, int page, int row)
        {
            SqlParameter p = new SqlParameter("@UserID", DBNull.Value);
            if (UserID != "")
                p = new SqlParameter("@UserID", UserID);
          
            return base.fn_Get("pr_User_sel", p, new SqlParameter("@search", Search)
                                                , new SqlParameter("@page", page)
                                                , new SqlParameter("@row", row));
        }

        public bool fn_GetByUserID(string UserID)
        {
            SqlParameter p = new SqlParameter("@UserID", DBNull.Value);
            if (UserID != "")
                p = new SqlParameter("@UserID", UserID);

            return base.fn_Get("pr_GetUserBy_UserID", p);
        }

        

        //public bool fn_Save(User dto, bool IsInsert)
        //{
        //    SqlParameter Image = new SqlParameter("@UserImage", SqlDbType.VarBinary, -1);
        //    if (dto.UserImage != null)
        //        Image.Value = dto.UserImage;
        //    else
        //    {
        //        Image.Value = DBNull.Value;
        //    }

        //    SqlParameter r = new SqlParameter("@RoleID", DBNull.Value);
        //    if (dto.RoleID != null)
        //    {
        //        r = new SqlParameter("@RoleID", dto.RoleID);
        //    }
        //    return base.fn_Execute("pr_User_save", new SqlParameter("@UserID", dto.UserID)
        //                                        , new SqlParameter("@UserName", dto.UserName)
        //                                        , new SqlParameter("@FullName", dto.FullName)
        //                                          , new SqlParameter("@Password", dto.Password)
        //                                        , new SqlParameter("@Email", dto.Email)
        //                                        , Image
        //                                        , new SqlParameter("@Birthday", dto.Birthday)
        //                                        , new SqlParameter("@Gender", dto.Gender)
        //                                        , r
        //                                        , new SqlParameter("@Insert", IsInsert));
        //}

        public bool fn_Delete(Guid UserID)
        {
            return base.fn_Execute("pr_User_del", new SqlParameter("@UserID", UserID));
        }

        public bool fn_CheckUserName(string UserID, string UserName)
        {
            return base.fn_Get("pr_User_CheckUserName", new SqlParameter("@UserID", UserID), new SqlParameter("@UserName", UserName));
        }

        public bool fn_CheckEmail(string UserID, string Email)
        {
            return base.fn_Get("pr_User_CheckEmail", new SqlParameter("@UserID", UserID), new SqlParameter("@Email", Email));
        }

    }
}
