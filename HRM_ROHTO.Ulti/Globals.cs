using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

/// <summary>
/// Summary description for Globals
/// </summary>
namespace HRM_ROHTO.Util
{
    public class Globals
    {

        public static string _DateFormat = "dd/MM/yyyy";
        public static string _DateFormatDefault = "dd/MM/yyyy";
        public static int _dateExpiry = 30;
        public static string _format_Money = "#,##0.####";
        public static string _format_int = "#,##0";
        public static string _format_percent = "0.#%";
        public static string _ConnectionString = "";
        public static string linkAPI = "";
        public static string UserNameAPI = "1";
        public static string PasswordAPI = "1";
        public static string SiteRootLangs = "";
        public static double MaxFileSizeUpload = 1048576;
        public static int SaleItemEachPage = 12;
        public static float ScaleImageWidth = 240;

        public static float ScaleImageHeight = 320;

        public static DateTime beginDate = new DateTime(1900, 01, 01);
        public static DateTime endDate = new DateTime(3000, 01, 01);
        public static string fn_ReturndateFromString(string s, out DateTime date)
        {
            date = new DateTime();
            if (s == "")
            {
                return "";
            }
            string[] tmp = s.Split(' ');
            string[] format;
            if (_DateFormatDefault == _DateFormat)
                format = _DateFormat.Split('/');
            else
            {
                format = _DateFormatDefault.Split('/');
                _DateFormatDefault = _DateFormat;
            }
            string[] arr = tmp[0].Split('/');
            DateTime dt = new DateTime();
            try
            {
                if (format[0].ToLower() == "dd" || format[0] == "d")
                {
                    if (format[1].ToLower() == "mm" || format[1].ToLower() == "m")
                        dt = new DateTime((int.Parse(arr[2]) < 99 ? 2000 + int.Parse(arr[2]) : int.Parse(arr[2])), int.Parse(arr[1]), int.Parse(arr[0]), 0, 0, 0);
                    else
                        dt = new DateTime((int.Parse(arr[2]) < 99 ? 2000 + int.Parse(arr[2]) : int.Parse(arr[2])), int.Parse(arr[0]), int.Parse(arr[1]), 0, 0, 0);
                }
                else
                {
                    if (format[0].ToLower() == "mm" || format[0] == "m")
                    {
                        if (format[1].ToLower() == "dd" || format[1].ToLower() == "d")
                            dt = new DateTime((int.Parse(arr[2]) < 99 ? 2000 + int.Parse(arr[2]) : int.Parse(arr[2])), int.Parse(arr[0]), int.Parse(arr[1]), 0, 0, 0);
                        else
                            dt = new DateTime((int.Parse(arr[2]) < 99 ? 2000 + int.Parse(arr[2]) : int.Parse(arr[2])), int.Parse(arr[1]), int.Parse(arr[0]), 0, 0, 0);
                    }
                    if (format[0].ToLower() == "yy" || format[0].ToLower() == "yyyy")
                    {
                        if (format[1].ToLower() == "mm" || format[1].ToLower() == "m")
                            dt = new DateTime((int.Parse(arr[0]) < 99 ? 2000 + int.Parse(arr[0]) : int.Parse(arr[0])), int.Parse(arr[1]), int.Parse(arr[2]), 0, 0, 0);
                        else
                            dt = new DateTime((int.Parse(arr[0]) < 99 ? 2000 + int.Parse(arr[0]) : int.Parse(arr[0])), int.Parse(arr[2]), int.Parse(arr[1]), 0, 0, 0);
                    }
                }
            }
            catch (Exception ex) { return ex.Message; }
            date = dt;
            return "";
        }

        public static string fn_GetSiteRoot()
        {
            string Port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";

            string sOut = Protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return sOut;
        }

        /// <summary>
        /// Caculating distant between fromdate and todate
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        public static int fn_CaculateDate(DateTime fromdate, DateTime todate)
        {
            TimeSpan Time = todate - fromdate;
            return Time.Days;
        }

        /// <summary>
        /// Tạo mã tự động
        /// </summary>
        /// <param name="prefix">Tiếp đầu ngữ</param>
        /// <param name="lengthAutoNum">độ dài chuỗi số tự tăng</param>
        /// <param name="autoNumber">Số tự tăng</param>
        /// <returns></returns>
        public static string fn_GerateAutoCode(string prefix, int lengthAutoNum, int autoNumber)
        {
            if (lengthAutoNum <= 0)
                return "";
            string sAuto = autoNumber.ToString();
            while (sAuto.Length < lengthAutoNum)
            {
                sAuto = "0" + sAuto;
            }

            return prefix + sAuto;
        }


        public static String DownloadString(WebClient webClient, String address, Encoding encoding)
        {


            byte[] buffer = webClient.DownloadData(address);

            byte[] bom = encoding.GetPreamble();

            if ((0 == bom.Length) || (buffer.Length < bom.Length))
            {
                return encoding.GetString(buffer);
            }

            for (int i = 0; i < bom.Length; i++)
            {
                if (buffer[i] != bom[i])
                {
                    return encoding.GetString(buffer);
                }
            }

            return encoding.GetString(buffer, bom.Length, buffer.Length - bom.Length);
        }

        //Minh

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }


        public static System.Drawing.Image ScaleImage(System.Drawing.Image imgPhoto)
        {
            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // force resize, might distort image
            if (ScaleImageWidth != 0 && ScaleImageHeight != 0)
            {
                destWidth = ScaleImageWidth;
                destHeight = ScaleImageHeight;
            }
            // change size proportially depending on width or height
            else if (ScaleImageHeight != 0)
            {
                destWidth = (float)(ScaleImageHeight * sourceWidth) / sourceHeight;
                destHeight = ScaleImageHeight;
            }
            else
            {
                destWidth = ScaleImageWidth;
                destHeight = (float)(sourceHeight * ScaleImageWidth / sourceWidth);
            }

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }


        public static byte[] Resize2Max50Kbytes(byte[] byteImageIn)
        {
            byte[] currentByteImageArray = byteImageIn;
            double scale = 1f;


            MemoryStream inputMemoryStream = new MemoryStream(byteImageIn);
            System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(inputMemoryStream);

            try
            {
                while (currentByteImageArray.Length > 60000)
                {
                    Bitmap fullSizeBitmap = new Bitmap(fullsizeImage, new Size((int)(fullsizeImage.Width * scale), (int)(fullsizeImage.Height * scale)));
                    MemoryStream resultStream = new MemoryStream();

                    fullSizeBitmap.Save(resultStream, fullsizeImage.RawFormat);

                    currentByteImageArray = resultStream.ToArray();
                    resultStream.Dispose();
                    resultStream.Close();

                    scale -= 0.05f;
                }
            }
            catch (Exception ex)
            {

            }

            return currentByteImageArray;
        }
    }
}