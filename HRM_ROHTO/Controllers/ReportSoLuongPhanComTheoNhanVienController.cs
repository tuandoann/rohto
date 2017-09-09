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
using System.Globalization;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class ReportSoLuongPhanComTheoNhanVienController : BaseController
    {
        #region bien
        private static DataTable DIRECTLABOR = new DataTable();
        private static DataTable OVERHEADSTAFF = new DataTable();
        private static DataTable OUTSIDELUNCHEONCHARGE = new DataTable();
        private static Boolean DK;
        private static int[] SUBTOTALDIRECTLABOR;
        private static int[] SUBTOTALOVERHEADSTAFF;
        private static int[] SUBTOTALOUTSIDELUNCHEONCHARGE;
        private static int[] GRANDTOTAL;
        private static int SOTHANG;
        private static int[] TOTALLUNCHEON;
        private static int YEAR;
        private static int MONTH;

        #endregion
        // GET: /ReportSoLuongPhanComTheoNhanVien/
        public ActionResult Index(int year = 0, int month = 0, bool dk = true)
        {
            if (!CheckPermission("ReportSoLuongPhanComTheoNhanVien", 1))
                return RedirectToAction("Login", "Login", "Login");

            int Year = year == 0 ? DateTime.Now.Year : year;
            int Month = month == 0 ? DateTime.Now.Month : month;
            int lastDay = DateTime.DaysInMonth(Year, Month);

            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("@FromDate", Year + "-" + Month + "-01"));
            lstParam.Add(new SqlParameter("@ToDate", Year + "-" + Month + "-" + lastDay));
            lstParam.Add(new SqlParameter("@IsBegin", dk ? 1 : 0));
            DataTable store = DA_ScheduleMenu.Instance.Exec("pr_ReportSLPhanComTheoNhanVien", lstParam.ToArray());

            //danh sách các nhân viên
            DataTable directLabor = new DataTable();
            DataTable overheadStaff = new DataTable();
            DataTable outsideLuncheonCharge = new DataTable();

            //số tháng được lựa chọn
            int soThang = (dk ? 16 - 1 : DateTime.DaysInMonth(year, month) - 15) + 1;

            //tổng nhân viên
            int[] subTotalDirectLabor = new int[soThang];
            int[] subTotalOverheadStaff = new int[soThang];
            int[] subTotalOutsideLuncheonCharge = new int[soThang];

            //tổng các nhân viên
            int[] totalLuncheon = new int[soThang];             // tổng nhân viên ở bên trong
            int[] grandTotal = new int[soThang];                // tổng nhân viên

            //tạo dữ liệu tất cả dữ liệu mặc định cho danh sách tổng cộng
            for (int i = 0; i < soThang; i++)
            {
                subTotalDirectLabor[i] = 0;
                subTotalOverheadStaff[i] = 0;
                subTotalOutsideLuncheonCharge[i] = 0;
                totalLuncheon[i] = 0;
                grandTotal[i] = 0;
            }

            //add col for datatable
            for (int i = 0; i < store.Columns.Count; i++)
            {
                string nameCol = store.Columns[i].ColumnName;
                directLabor.Columns.Add(nameCol);
                overheadStaff.Columns.Add(nameCol);
                outsideLuncheonCharge.Columns.Add(nameCol);
            }

            //tạo dữ liệu cho những đối tượng là nhân viên ( danh sách nhân viên, tổng)
            for (int i = 0; i < store.Rows.Count; i++)
            {
                int startThang = dk ? 1 : 15;
                switch (Convert.ToInt32(store.Rows[i]["EmployeeType"]))
                {
                    // là nhân viên DIRECT LABOR
                    case 1:
                        //add dữ liệu datatable
                        directLabor.ImportRow(store.Rows[i]);
                        //add dữ liệu danh sách tổng
                        for (int j = 0; j < soThang; j++)
                        {
                            string nameCol = (j == soThang - 1) ? "Total" : "Ngay" + (j + startThang).ToString();
                            subTotalDirectLabor[j] += Convert.ToInt32(store.Rows[i][nameCol]);
                        }
                        break;
                    //là nhân viên OVERHEAD STAFF
                    case 2:
                        //add dữ liệu datatable
                        overheadStaff.ImportRow(store.Rows[i]);
                        //add dữ liệu danh sách tổng
                        for (int j = 0; j < soThang; j++)
                        {
                            string nameCol = (j == soThang - 1) ? "Total" : "Ngay" + (j + startThang).ToString();
                            subTotalOverheadStaff[j] += Convert.ToInt32(store.Rows[i][nameCol]);
                        }
                        break;
                    //là nhân viên Outside Luncheon Charge
                    default:
                        //add dữ liệu datatable
                        outsideLuncheonCharge.ImportRow(store.Rows[i]);
                        //add dữ liệu vào danh sách tổng
                        for (int j = 0; j < soThang; j++)
                        {
                            string nameCol = (j == soThang - 1) ? "Total" : "Ngay" + (j + startThang).ToString();
                            subTotalOutsideLuncheonCharge[j] += Convert.ToInt32(store.Rows[i][nameCol]);
                        }
                        break;
                }
            }

            //add list sum
            for (int i = 0; i < soThang; i++)
            {
                grandTotal[i] += subTotalDirectLabor[i];
                grandTotal[i] += subTotalOverheadStaff[i];
                totalLuncheon[i] += grandTotal[i];
                totalLuncheon[i] += subTotalOutsideLuncheonCharge[i];
            }

            ViewBag.dk = dk;
            ViewBag.soThang = soThang;
            ViewBag.directLabor = directLabor;
            ViewBag.outsideLuncheonCharge = outsideLuncheonCharge;
            ViewBag.overheadStaff = overheadStaff;
            ViewBag.subTotalDirectLabor = subTotalDirectLabor;
            ViewBag.subTotalOverheadStaff = subTotalOverheadStaff;
            ViewBag.subTotalOutsideLuncheonCharge = subTotalOutsideLuncheonCharge;
            ViewBag.grandTotal = grandTotal;
            ViewBag.totalLuncheon = totalLuncheon;

            DK = dk;
            YEAR = Year;
            MONTH = Month;
            DIRECTLABOR = directLabor;
            SOTHANG = soThang;
            OUTSIDELUNCHEONCHARGE = outsideLuncheonCharge;
            OVERHEADSTAFF = overheadStaff;
            SUBTOTALDIRECTLABOR = subTotalDirectLabor;
            SUBTOTALOVERHEADSTAFF = subTotalOverheadStaff;
            GRANDTOTAL = grandTotal;
            SUBTOTALOUTSIDELUNCHEONCHARGE = subTotalOutsideLuncheonCharge;
            TOTALLUNCHEON = totalLuncheon;

            return View();
        }
        
        [HttpPost]
        public JsonResult ExportSoLuongPhanComTheoNhanVien(FormCollection fc)
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    
                    int startThang = DK ? 1 : 15;
                    int lastRow = 2 + (1 + 1 + 1+ ( DIRECTLABOR.Rows.Count == 0 ? 1: DIRECTLABOR.Rows.Count) + (1 + 1 + ( OVERHEADSTAFF.Rows.Count == 0 ? 1 : OVERHEADSTAFF.Rows.Count)) + 1 + 1 + (1 + (OUTSIDELUNCHEONCHARGE.Rows.Count == 0 ? 1 : OUTSIDELUNCHEONCHARGE.Rows.Count) +1 )) + 3 ;
                    int lastCol = SOTHANG + 3;
                    var wsList = pck.Workbook.Worksheets.Add(startThang.ToString() + "~" + (startThang + SOTHANG - 2).ToString()+ CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MONTH));

                    //định danh font cho toàn sheet
                    wsList.Cells[1, 1, lastRow, lastCol].Style.Font.Name = "Tahoma";
                    wsList.Cells[1, 1, lastRow, lastCol].Style.Font.Size = 11;
                    wsList.Cells[3, 1, lastRow, lastCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, lastRow, lastCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, lastRow , lastCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, lastRow, lastCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    #region phần cứng
                    //đỗ dữ liệu
                    wsList.Cells[1, 1].LoadFromText("LUNCHEON");
                    String ngayQuery = "FROM " + startThang.ToString() + "-" + MONTH.ToString() + "-" + YEAR.ToString() + " ~ " + (startThang + SOTHANG -2).ToString() + "-" + MONTH.ToString() + "-" + YEAR.ToString();
                    DateTime now = DateTime.Now;
                    wsList.Cells[2, 1].LoadFromText("Date");
                    wsList.Cells[2, 2].Value = now;
                    wsList.Cells[2, 2].Style.Numberformat.Format = "dd-MM-yyyy";
                    wsList.Cells[1, 3].LoadFromText(ngayQuery);
                    wsList.Cells[3, 1].LoadFromText("NO");
                    wsList.Cells[3, 2].LoadFromText("DATE");
                    for (int i = 0; i < SOTHANG; i++)
                    {
                        wsList.Cells[3, i + 4].LoadFromText((i == SOTHANG - 1) ? "TOTAL" : (i + startThang).ToString());
                    }
                    wsList.Cells[lastRow - 2, 1].LoadFromText("Actual luncheon (2)");
                    wsList.Cells[lastRow - 1, 1].LoadFromText("Balance [(2)-(1)]");
                    wsList.Cells[lastRow, 1].LoadFromText("Amount");
                    //định dạng style
                    wsList.Cells[1, 1, 1, 2].Merge = true;
                    wsList.Cells[1, 3, 1, lastCol].Merge = true;
                    wsList.Cells[3, 2, 3, 3].Merge = true;
                    wsList.Cells[lastRow - 2, 1, lastRow - 2, 3].Merge = true;
                    wsList.Cells[lastRow - 1, 1, lastRow - 1, 3].Merge = true;
                    wsList.Cells[lastRow, 1, lastRow, 3].Merge = true;
                    wsList.Cells[1, 1, 3, lastCol].Style.Font.Bold = true;
                    wsList.Cells[1, 1, 3, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 1, 3, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[lastRow - 2, 1, lastRow - 2, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[lastRow - 2, 1, lastRow - 2, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[lastRow - 1, 1, lastRow - 1, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[lastRow - 1, 1, lastRow - 1, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[lastRow, 1, lastRow, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[lastRow, 1, lastRow, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 1, 1, lastCol].Style.Font.Size = 14;
                    wsList.Cells[lastRow-2, 1, lastRow, lastCol].Style.Font.Bold = true;
                    #endregion

                    #region đỗ dữ liệu các loại nhân viên
                    int indexRow = 4;
                    //DIRECT LABOR
                    drawRowEmployee(indexRow, DIRECTLABOR, wsList, "DIRECT LABOR", SOTHANG, DK, false);
                    indexRow += DIRECTLABOR.Rows.Count + 1;
                    drawRowTotal(indexRow, SUBTOTALDIRECTLABOR, wsList, "SUB TOTAL");

                    //OVERHEAD STAFF
                    indexRow += 1;
                    drawRowEmployee(indexRow, OVERHEADSTAFF, wsList, "OVERHEAD STAFF", SOTHANG, DK, false);
                    indexRow += OVERHEADSTAFF.Rows.Count == 0 ? 1 : OVERHEADSTAFF.Rows.Count + 1;
                    drawRowTotal(indexRow, SUBTOTALOVERHEADSTAFF, wsList, "SUB TOTAL");

                    //tổng GRAND TOTAL và TOTAL LUNCHEON
                    indexRow += 1;
                    drawRowTotal(indexRow, GRANDTOTAL, wsList, "GRAND TOTAL");
                    indexRow += 1;
                    drawRowTotal(indexRow, new int[0], wsList, "TOTAL LUNCHEON");

                    //Outside luncheon charge
                    indexRow += 1;
                    drawRowEmployee(indexRow, OUTSIDELUNCHEONCHARGE, wsList, "Outside luncheon charge", SOTHANG, DK, true);
                    indexRow += OUTSIDELUNCHEONCHARGE.Rows.Count == 0 ? 1 : OUTSIDELUNCHEONCHARGE.Rows.Count +1;
                    wsList.Cells[indexRow, 1].LoadFromText("For emploees who work overtime but cancel late");
                    wsList.Cells[indexRow, 1, indexRow, 3].Merge = true;
                    ++indexRow;
                    drawRowTotal(indexRow, SUBTOTALOUTSIDELUNCHEONCHARGE, wsList, "Sub Total");

                    //Total luncheon (1)
                    ++indexRow;
                    drawRowTotal(indexRow, TOTALLUNCHEON, wsList, "Total luncheon (1)");
                    #endregion

                    wsList.Cells[lastRow + 1, 1].LoadFromText("PREPARED BY");
                    wsList.Cells[lastRow + 1, 1, lastRow + 1, lastCol].Merge = true;
                    wsList.Cells[lastRow + 1, 1, lastRow + 1, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    wsList.Cells[lastRow + 4, 1, lastRow + 4, 1].LoadFromText(DA_Config.Instance.getValueFromKeyConfig("PREPAREDBY").ToString());
                    wsList.Cells[lastRow + 4, 1, lastRow + 4, lastCol].Merge = true;
                    wsList.Cells[lastRow + 4, 1, lastRow + 4, lastCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    wsList.Cells[lastRow + 1, 1, lastRow + 4, lastCol].Style.Font.Name = "Tahoma";

                    //định dạng width
                    wsList.Column(1).Width = 13;
                    wsList.Column(2).Width = 40;
                    wsList.Column(3).Width = 20;
                    for (int i = 4; i < 4 + SOTHANG; i++ )
                    {
                        wsList.Column(i).Width = 10;
                    }

                    string path = Server.MapPath("/LUNCHEON.xlsx");
                    Stream stream = System.IO.File.Create(path);
                    pck.SaveAs(stream);
                    stream.Close();
                    return Json("/LUNCHEON.xlsx");
                }
            }
            catch(Exception ex)
            {
                string e = ex.Message;
                return Json((-1).ToString());
            }
        }
        /// <summary>
        /// xuất dòng tổng cho excel
        /// </summary>
        /// <param name="rowIndex">dòng excel muốn xuất</param>
        /// <param name="array">mảng giá trị</param>
        /// <param name="wsList">wordshit</param>
        /// <param name="tittle">tiêu đề của total</param>
        private void drawRowTotal(int rowIndex, int[] array, ExcelWorksheet wsList, string tittle)
        {
            try
            {
                wsList.Cells[rowIndex, 1].LoadFromText(tittle);
                for (int i = 0; i < array.Count(); i++)
                {
                    wsList.Cells[rowIndex, i + 4].LoadFromText(array[i].ToString());
                }
                wsList.Cells[rowIndex, 1, rowIndex, 3].Merge = true;
                wsList.Cells[rowIndex, 1, rowIndex, array.Count() + 3].Style.Font.Bold = true;
                wsList.Cells[rowIndex, 1, rowIndex, array.Count() + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[rowIndex, 1, rowIndex, array.Count() + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            catch(Exception ex)
            {
                string e = ex.Message;
            }
        }
        /// <summary>
        /// xuất loại nhân viên cho báo cáo
        /// </summary>
        /// <param name="rowIndexStart">dòng đầu tiên trong excel dành cho việc xuất loại nhân viên</param>
        /// <param name="ls">dữ liệu loại nhân viên</param>
        /// <param name="wsList">wordshit</param>
        /// <param name="tittle">tên loại nhân viên</param>
        /// <param name="soThang">số tháng xuất excel</param>
        /// <param name="dk">có phải là 15 ngày đầu,hoặc 15 ngày cuối</param>
        private void drawRowEmployee(int rowIndexStart, DataTable ls, ExcelWorksheet wsList, string tittle, int soThang, bool dk, bool isOutsideLuncheonCharge)
        {
            try
            {
                int nRow = ls.Rows.Count;
                if (!isOutsideLuncheonCharge)
                {
                    wsList.Cells[rowIndexStart, 2].LoadFromText(tittle);
                    wsList.Cells[rowIndexStart, 3].LoadFromText("EMPLOYEE CODE");
                    wsList.Cells[rowIndexStart, 2, rowIndexStart, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[rowIndexStart, 2, rowIndexStart, 3].Style.Font.Bold = true;
                    wsList.Cells[rowIndexStart, 2, rowIndexStart, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rowIndexStart += 1;
                }
                else
                {
                    wsList.Cells[rowIndexStart, 1].LoadFromText(tittle);
                    wsList.Row(rowIndexStart).Height = 60;
                    wsList.Cells[rowIndexStart, 1, rowIndexStart + nRow, 1].Merge = true;
                    wsList.Cells[rowIndexStart, 1, rowIndexStart + nRow, 1].Style.WrapText = true;
                    wsList.Cells[rowIndexStart, 1, rowIndexStart + nRow, 1].Style.Font.Bold = true;
                }

                for (int i = 0; i < nRow; i++)
                {
                    if (!isOutsideLuncheonCharge)
                    {
                        wsList.Cells[rowIndexStart + i, 1].LoadFromText((i + 1).ToString());
                    }
                    wsList.Cells[rowIndexStart + i, 2].LoadFromText(ls.Rows[i]["FullName"].ToString());
                    wsList.Cells[rowIndexStart + i, 3].LoadFromText(ls.Rows[i]["EmployeeCode"].ToString());
                    int startDate = (dk ? 1 : 15);
                    for (int j = startDate; j < (startDate + soThang); j++)
                    {
                        string nameCol = (j == (startDate + soThang) - 1) ? "Total" : "Ngay" + j.ToString();
                        wsList.Cells[rowIndexStart + i, (j - startDate) + 4].LoadFromText(ls.Rows[i][nameCol].ToString());
                    }
                }
                wsList.Cells[rowIndexStart, 3, rowIndexStart + nRow, soThang + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[rowIndexStart, 3, rowIndexStart + nRow, soThang + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsList.Cells[rowIndexStart, 1, rowIndexStart + nRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[rowIndexStart, 1, rowIndexStart + nRow, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsList.Cells[rowIndexStart, 2, rowIndexStart + nRow, 2].Style.WrapText = true;
            }
            catch(Exception ex)
            {
                string e = ex.Message;
            }
        }
    }
}
