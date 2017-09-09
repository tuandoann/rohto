using System;
using System.Text.RegularExpressions;
using System.IO;

namespace HRM_ROHTO.Util
{
    /// <summary>
    /// Summary description for Validation
    /// </summary>
    public class Validation
    {
        public static bool fn_ValidCharacter(string name)
        {
            bool bolReg = false;
            string strPattern = @"^([a-zA-Z]+\s{0,2})+$";

            Regex theRegEx = new Regex(strPattern);

            if (theRegEx.IsMatch(name))
            {
                bolReg = true;
            }
            return bolReg;
        }

        public static bool fn_ValidPhone(string phone)
        {
            bool bolReg = false;
            string strPattern = @"^[0-9\ ]+$";

            Regex theRegEx = new Regex(strPattern);

            if (theRegEx.IsMatch(phone))
            {
                bolReg = true;
            }
            return bolReg;
        }

        public static bool fn_ValidEmail(string email)
        {
            bool bolReg = false;
            string strPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            Regex theRegEx = new Regex(strPattern);

            if (theRegEx.IsMatch(email))
            {
                bolReg = true;
            }
            return bolReg;
        }

        public static bool fn_ValidInt(string numInt)
        {
            try
            {
                Convert.ToInt32(numInt);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public static bool fn_ValidFloat(string numFloat)
        {
            try
            {
                Convert.ToDouble(numFloat);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checking Double Number
        /// </summary>
        /// <param name="value"></param>
        /// <param name="largerthen0">Require value larger than 0 or not, true: required.</param>
        /// <returns></returns>
        public static Boolean fn_ValidFloat(object value, bool largerthen0)
        {
            try
            {
                Convert.ToDouble(value);
                if (largerthen0)
                    if (Convert.ToDouble(value) <= 0)
                        return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool fn_ValidWhiteSpace(string name)
        {
            bool bolReg = false;
            string strPattern = @"^[^\s]+$";

            Regex theRegEx = new Regex(strPattern);

            if (theRegEx.IsMatch(name))
            {
                bolReg = true;
            }
            return bolReg;
        }

        public static bool fn_ValidImageFile(string FileName)
        {
            string ext = Path.GetExtension(FileName);
            if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".gif"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool fn_Validate_UserName(string s)
        {
            string reg = @"^[a-zA-Z0-9_.-]*$"; reg = @"^[\w\d_.-]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(s, reg);
        }

    }
}
