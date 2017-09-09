using System;
using System.Collections.Generic;
using System.Linq;
using HRM_ROHTO.Models.BUS;
using HRM_ROHTO.Util;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Controllers
{
    public class ReportSoLuongPhanComHangNgayController : BaseController
    {
        //
        // GET: /TK_SoLuongPhanComHangNgay/
        #region biến
        static DataTable dtToExcel;
        static List<long> lsSum = new System.Collections.Generic.List<long>();
        static DateTime FROMDATE = new System.DateTime();
        static DateTime TODATE = new System.DateTime();
        #endregion

        public ActionResult Index(DateTime? FromDate, DateTime? ToDate)
        {
            if (!CheckPermission("ReportSoLuongPhanComHangNgay", 1))
                return RedirectToAction("Login", "Login", "Login");
            DateTime f = DateTime.Now;
            DateTime t = DateTime.Now;
            if (!FromDate.HasValue)
            {
                FromDate = f;
            }

            if (!ToDate.HasValue)
            {
                ToDate = t;
            }
            ViewBag.FromDate = f.ToString("yyyy-MM-dd");
            ViewBag.ToDate = t.ToString("yyyy-MM-dd");
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("@fromdate", FromDate));
            lstParam.Add(new SqlParameter("@todate", ToDate));
            dtToExcel = DA_ScheduleMenu.Instance.Exec("pr_ReportSLPhanComHangNgay", lstParam.ToArray());
            List<long> lssum = new List<long>();
            long slDK = 0;
            long slDI = 0;
            long slIDKDX = 0;
            long sum = 0;
            for (int i = 0; i < dtToExcel.Rows.Count;i++ )
            {
                slDK += Convert.ToInt64(dtToExcel.Rows[i]["TotalReg"]);
                slDI += Convert.ToInt64(dtToExcel.Rows[i]["TotalPrint"]);
                slIDKDX += Convert.ToInt64(dtToExcel.Rows[i]["TotalExp"]);
                sum += Convert.ToInt64(dtToExcel.Rows[i]["Total"]);
            }
            lssum.Add(slDK);
            lssum.Add(slDI);
            lssum.Add(slIDKDX);
            lssum.Add(sum);
            ViewBag.store = dtToExcel;
            ViewBag.lsSum = lssum;
            lsSum = lssum;
            FROMDATE = Convert.ToDateTime(FromDate);
            TODATE = Convert.ToDateTime(ToDate);
            return View();
        }

        /// <summary>
        /// hàm dùng để xuất ra excel
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReportSoLuongPhanComHangNgay(FormCollection fc)
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var wsList = pck.Workbook.Worksheets.Add("Báo cáo");
                    int nCot = dtToExcel.Columns.Count;
                    int nDong = dtToExcel.Rows.Count;
                    int lastRow = nDong + 4;
                    int lastCol = nCot + 1;

                    //định dạng chung cho cả sheet
                    wsList.Cells[1, 1, lastRow, lastCol].Style.Font.Name = "Tahoma";
                    wsList.Cells[1, 1, lastRow, lastCol].Style.Font.Size = 11;
                    wsList.Cells[3, 1, lastRow, lastCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, lastRow, lastCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, lastRow, lastCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, lastRow, lastCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    #region phần cứng
                    //Đổ dữ liệu
                    wsList.Cells["A1"].LoadFromText("THỐNG KÊ SỐ LƯỢNG PHẦN CƠM HẰNG NGÀY ");
                    wsList.Cells["A2"].LoadFromText("TỪ " + FROMDATE.ToString("dd/MM/yyyy") + " ĐẾN " + TODATE.ToString("dd/MM/yyyy"));
                    wsList.Cells[lastRow, 1].LoadFromText("TỔNG CỘNG");
                    for (int i = 0; i < lsSum.Count; i++)
                    {
                        wsList.Cells[lastRow, i + 3].LoadFromText(lsSum.ElementAt(i).ToString());
                    }
                    //style
                    wsList.Cells[1, 1, 1, lastCol].Merge = true;
                    wsList.Cells[2, 1, 2, lastCol].Merge = true;
                    wsList.Cells[lastRow, 1, lastRow, 2].Merge = true;
                    wsList.Cells[1, 1, 1, lastCol].Style.Font.Bold = true;
                    wsList.Cells[lastRow, 1, lastRow, lastCol].Style.Font.Bold = true;
                    wsList.Cells[1, 1, 1, lastCol].Style.Font.Size = 14;
                    wsList.Cells[1, 1, 2, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[lastRow, 1, lastRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    #region dữ liệu
                    #region tiêu đề
                    wsList.Cells[3, 1].LoadFromText("STT");
                    wsList.Cells[3, 2].LoadFromText("NGÀY");
                    wsList.Cells[3, 3].LoadFromText("SL PHẦN ĐĂNG KÝ ");
                    wsList.Cells[3, 4].LoadFromText("SL PHẦN ĐÃ IN");
                    wsList.Cells[3, 5].LoadFromText("SL PHẦN IN ĐỘT XUẤT");
                    wsList.Cells[3, 6].LoadFromText("TỔNG");
                    wsList.Cells[3, 1, 3, lastCol].Style.Font.Bold = true;
                    wsList.Cells[3, 1, 3, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[3, 1, 3, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    #endregion
                    #region data
                    for (int i = 0; i < dtToExcel.Rows.Count; i++)
                    {
                        int index_row = i + 4;
                        wsList.Cells[index_row, 1].LoadFromText((i + 1).ToString());
                        wsList.Cells[index_row, 2].Value = Convert.ToDateTime(dtToExcel.Rows[i]["MealDate"]);
                        wsList.Cells[index_row, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                        wsList.Cells[index_row, 3].LoadFromText(dtToExcel.Rows[i]["TotalReg"].ToString());
                        wsList.Cells[index_row, 4].LoadFromText(dtToExcel.Rows[i]["TotalPrint"].ToString());
                        wsList.Cells[index_row, 5].LoadFromText(dtToExcel.Rows[i]["TotalExp"].ToString());
                        wsList.Cells[index_row, 6].LoadFromText(dtToExcel.Rows[i]["Total"].ToString());
                    }
                    wsList.Cells[4, 1, lastRow - 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion
                    #endregion

                    //định dạng width cho sheet
                    wsList.Column(1).Width = 7;
                    wsList.Column(2).Width = 13;
                    wsList.Column(3).Width = 19;
                    wsList.Column(4).Width = 19;
                    wsList.Column(5).Width = 19;
                    wsList.Column(6).Width = 10;

                    //wraptext cho sheet
                    wsList.Cells[3, 1, 3, lastCol].Style.WrapText = true;

                    string path = Server.MapPath("/TK_SoLuongPhanComHangNgay.xlsx");
                    Stream stream = System.IO.File.Create(path);
                    pck.SaveAs(stream);
                    stream.Close();

                    return Json("/TK_SoLuongPhanComHangNgay.xlsx");
                }
            }
            catch(Exception ex)
            {
                string e = ex.Message;
                return Json("");
            }
        }
    }
}
