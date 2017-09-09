﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using HRM_ROHTO.Models.ListExt;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using HRM_ROHTO.Util;

namespace HRM_ROHTO.Controllers
{
    public class ReportMealCardTomorrowController : BaseController
    {
        //
        // GET: /ScheduleMenu/
        #region biến
        static DataTable dtToExcel;
        static string c = string.Empty;
        static string date;
        static List<int> LSCOUNT = new List<int>();
        static List<long> LSSUM = new List<long>();
        static int SUM;
        #endregion

        public ActionResult Index(int year = 0, int month = 0, int day = 0)
        {
            if (!CheckPermission("ReportMealCardTomorrow", 1))
                return RedirectToAction("Login", "Login", "Login");

            int Year = year == 0 ? DateTime.Now.Year : year;
            int Month = month == 0 ? DateTime.Now.Month : month;
            int Day = day == 0 ? DateTime.Now.Month : day;

            //load dữ liệu
            getDataReport(Year, Month, Day);

            //gán giá trị cho các viewBag
            ViewBag.FromDate = new DateTime(Year, Month, Day).ToString("dd-MM-yyyy");
            ViewBag.Tong = SUM;
            ViewBag.lsCount = LSCOUNT;
            ViewBag.lsSum = LSSUM;

            return View();
        }
       
        // Gọi khi click search
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            if (!CheckPermission("ReportMealCardTomorrow", 1))
                return RedirectToAction("Login", "Login", "Login");

            DateTime FromDate = getFromDateFromNow();

            List<SqlParameter> lstParam = new List<SqlParameter>();

            lstParam.Add(new SqlParameter("@day", FromDate.Day));
            lstParam.Add(new SqlParameter("@month", FromDate.Month));
            lstParam.Add(new SqlParameter("@year", FromDate.Year));
            //ViewBag.FromDate = FromDate;
            BeginLoad(fc, ref FromDate);
            ViewBag.ListData = DA_ScheduleMenu.Instance.Exec("pr_ReportMealCardByDay", lstParam.ToArray());
            return PartialView("CustomerDebit_Table", ViewBag.ListData);
        }

        public void BeginLoad(FormCollection fc, ref DateTime date_year)
        {
            if (fc["FromDate"] != null && fc["FromDate"].Trim() != "")
            {
                DateTime.TryParseExact(fc["FromDate"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date_year);
            }
            ViewBag.FromDate = date_year.ToString("yyyy-MM-dd");
        }
        public DateTime getFromDateFromNow()
        {
            DateTime oldTime = DateTime.Now.AddMonths(-3);
            DateTime dtime = new DateTime(oldTime.Year, oldTime.Month, 1, 0, 0, 0);
            return dtime;
        }

        /// <summary>
        /// hàm dùng để xuất ra excel
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportMealCardTomorrow()
        {
            string refer = "";
            return Json((!exportExcel(ref refer)) ? "0!" + refer : "1!/TK_SoLuongPhieuComDangKy.xlsx");
        }

        #region function
        public DataTable pr_GetListMealToomorrow(int day, int month, int year)
        {
            Database db1 = new Database();
            db1.fn_GetData_Pro("pr_ReportMealCardByDay", new SqlParameter("@day", day),
                                                           new SqlParameter("@month", month),
                                                            new SqlParameter("@year", year));
            DataTable dt1 = db1.mn_Table;
            return dt1;
        }
        public Boolean exportExcel(ref string refer)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                try
                {
                    //tạo dữ liệu khi các biến toàn cục không có dữ liệu
                    if ( (LSCOUNT.Count == 0 || LSSUM.Count == 0 || dtToExcel.Rows.Count == 0) && string.IsNullOrWhiteSpace(date))
                    {
                        DateTime now = DateTime.Now;
                        getDataReport(now.Year, now.Month, now.Day + 1);
                    }

                    var wsList = pck.Workbook.Worksheets.Add("Báo cáo");
                    int nCot = dtToExcel.Columns.Count;
                    int nDong = dtToExcel.Rows.Count;

                    if (LSCOUNT.Count != 0 && LSSUM.Count != 0 && dtToExcel.Rows.Count != 0)
                    {
                        //định dạng chung cho cả sheet
                        wsList.Cells[1, 1, nDong + 3, 4].Style.Font.Name = "Tahoma";
                        wsList.Cells[1, 1, nDong + 3, 4].Style.Font.Size = 11;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        #region phần cứng
                        //đỗ dữ liệu
                        wsList.Cells["A1"].LoadFromText("SỐ LƯỢNG CƠM LÝ THUYẾT");
                        wsList.Cells[2, 1].LoadFromText("NGÀY");
                        wsList.Cells["B2"].LoadFromText(date);
                        wsList.Cells[nDong + 3, 1].LoadFromText("Tổng cộng");
                        wsList.Cells[nDong + 3, 4].LoadFromText(SUM.ToString());
                        //style
                        wsList.Cells[1, 1, 1, 4].Merge = true;
                        wsList.Cells[1, 1, 1, nCot - 1].Style.Font.Bold = true;
                        wsList.Cells[1, 1, 1, nCot - 1].Style.Font.Size = 14;
                        wsList.Cells[1, 1, 1, nCot - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        wsList.Cells[2, 2, 2, 4].Merge = true;
                        wsList.Cells[2, 2, 2, nCot - 1].Style.Font.Bold = true;
                        wsList.Cells["A2"].Style.Font.Size = 14;
                        wsList.Cells[2, 2, 2, nCot - 1].Style.Font.Size = 12;
                        wsList.Cells[2, 1, 2, nCot - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        wsList.Cells[nDong + 3, 1, nDong + 3, 4].Style.Font.Bold = true;
                        wsList.Cells[nDong + 3, 1, nDong + 3, 3].Merge = true;
                        wsList.Cells[nDong + 3, 1, nDong + 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        #endregion

                        #region dữ liệu
                        //tiêu đề
                        wsList.Cells[3, 1, 3, 1].LoadFromText("TÊN CA");
                        wsList.Cells[3, 2, 3, 2].LoadFromText("TÊN MÓN ĂN");
                        wsList.Cells[3, 3, 3, 3].LoadFromText("SỐ LƯỢNG PHẦN");
                        wsList.Cells[3, 4, 3, 4].LoadFromText("TỔNG");
                        wsList.Cells[3, 1, 3, 4].Style.Font.Bold = true;
                        wsList.Cells[3, 1, 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //data
                        int index_result_store = 0;
                        int index_row_excel = 4;
                        try
                        {
                            for (int i = 0; i < LSCOUNT.Count; i++)
                            {
                                int count_item = LSCOUNT.ElementAt(i);
                                for (int j = 0; j < count_item; j++)
                                {
                                    if (j == 0)
                                    {
                                        wsList.Cells[index_row_excel, 1, index_row_excel, 1].LoadFromText(dtToExcel.Rows[index_result_store]["ShiftName"].ToString());
                                        wsList.Cells[index_row_excel, 4, index_row_excel, 4].LoadFromText(LSSUM.ElementAt(i).ToString());
                                        wsList.Cells[index_row_excel, 1, index_row_excel + count_item - 1, 1].Merge = true;
                                        wsList.Cells[index_row_excel, 4, index_row_excel + count_item - 1, 4].Merge = true;
                                    }
                                    wsList.Cells[index_row_excel, 2, index_row_excel, 2].LoadFromText(dtToExcel.Rows[index_result_store]["ProductName"].ToString());
                                    wsList.Cells[index_row_excel, 3, index_row_excel, 3].LoadFromText(dtToExcel.Rows[index_result_store]["Quantity"].ToString());
                                    index_row_excel += 1;
                                    index_result_store += 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string e = ex.Message;
                        }
                        #endregion
                        //định dạng wrap text
                        wsList.Cells[4, 2, nDong + 2, 2].Style.WrapText = true;
                    }
                    else
                    {
                        ++nDong;
                        //định dạng chung cho cả sheet
                        wsList.Cells[1, 1, nDong + 3, 4].Style.Font.Name = "Tahoma";
                        wsList.Cells[1, 1, nDong + 3, 4].Style.Font.Size = 11;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        wsList.Cells[3, 1, nDong + 3, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        wsList.Cells["A1"].LoadFromText("SỐ LƯỢNG CƠM LÝ THUYẾT");
                        wsList.Cells[2, 1].LoadFromText("NGÀY");
                        wsList.Cells["B2"].Value = date;
                        wsList.Cells["B2"].Style.Numberformat.Format = "dd/MM/yyyy";
                        wsList.Cells[nDong + 3, 1].LoadFromText("Tổng cộng");
                        wsList.Cells[nDong + 3, 4].LoadFromText(SUM.ToString());
                        //style
                        wsList.Cells[1, 1, 1, 4].Merge = true;
                        wsList.Cells[1, 1, 1, nCot - 1].Style.Font.Bold = true;
                        wsList.Cells[1, 1, 1, nCot - 1].Style.Font.Size = 14;
                        wsList.Cells[1, 1, 1, nCot - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        wsList.Cells[2, 2, 2, 4].Merge = true;
                        wsList.Cells[2, 2, 2, nCot - 1].Style.Font.Bold = true;
                        wsList.Cells["A2"].Style.Font.Size = 14;
                        wsList.Cells[2, 2, 2, nCot - 1].Style.Font.Size = 12;
                        wsList.Cells[2, 1, 2, nCot - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        wsList.Cells[nDong + 3, 1, nDong + 3, 4].Style.Font.Bold = true;
                        wsList.Cells[nDong + 3, 1, nDong + 3, 3].Merge = true;
                        wsList.Cells[nDong + 3, 1, nDong + 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        wsList.Cells[3, 1, 3, 1].LoadFromText("TÊN CA");
                        wsList.Cells[3, 2, 3, 2].LoadFromText("TÊN MÓN ĂN");
                        wsList.Cells[3, 3, 3, 3].LoadFromText("SỐ LƯỢNG PHẦN");
                        wsList.Cells[3, 4, 3, 4].LoadFromText("TỔNG");
                        wsList.Cells[3, 1, 3, 4].Style.Font.Bold = true;
                        wsList.Cells[3, 1, 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    //định dạng width cho sheet
                    wsList.Column(1).Width = 10;
                    wsList.Column(2).Width = 30;
                    wsList.Column(3).Width = 20;
                    wsList.Column(4).Width = 10;

                    string fileName = (Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)) + "/TK_SoLuongPhieuComDangKy.xlsx").Remove(0, 6);
                    Stream stream = System.IO.File.Create(fileName);
                    pck.SaveAs(stream);
                    stream.Close();
                    pck.Dispose();
                    return true;
                }

                catch (Exception ex)
                {
                    refer = ex.Message;
                    return false;
                }
            }
        }
        /// <summary>
        /// get data from store and fill biến toàn cục
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        private void getDataReport(int year, int month, int day)
        {
            #region hứng dữ liệu từ store
            int Year = year == 0 ? DateTime.Now.Year : year;
            int Month = month == 0 ? DateTime.Now.Month : month;
            int Day = day == 0 ? DateTime.Now.Month : day;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("@day", day));
            lstParam.Add(new SqlParameter("@month", Month));
            lstParam.Add(new SqlParameter("@year", Year));
            ViewBag.store = DA_ScheduleMenu.Instance.Exec("pr_ReportMealCardByDay", lstParam.ToArray());
            dtToExcel = pr_GetListMealToomorrow(day, Month, Year);
            #endregion

            #region Merge cho col ca và col tổng
            List<int> lsCount = new List<int>();
            List<long> lsSum = new List<long>();
            long sum_item = 0;
            int count_item = 0;
            int sum = 0;
            for (int i = 0; i < dtToExcel.Rows.Count; i++)
            {
                //bắt đầu vô vòng lặp
                if (i == 0)
                {
                    sum_item += Convert.ToInt64(dtToExcel.Rows[i]["Quantity"]);
                    count_item += 1;
                }
                //giữa vòng lặp
                else if (i != 0 && i != dtToExcel.Rows.Count - 1)
                {
                    string shiftID_before = dtToExcel.Rows[i - 1]["ShiftID"].ToString();
                    string shiftID_current = dtToExcel.Rows[i]["ShiftID"].ToString();
                    //đầu mỗi group
                    if (String.Compare(shiftID_before, shiftID_current) != 0)
                    {
                        lsSum.Add(sum_item);
                        lsCount.Add(count_item);
                        sum_item = 0;
                        count_item = 0;
                    }
                    count_item += 1;
                    sum_item += Convert.ToInt64(dtToExcel.Rows[i]["Quantity"]);
                }
                //kết thúc vòng lặp
                else if (i == dtToExcel.Rows.Count - 1)
                {
                    lsSum.Add(sum_item);
                    lsCount.Add(count_item);
                }

                sum = sum + (int)dtToExcel.Rows[i]["Quantity"];
            }
            #endregion

            #region gán dữ liệu

            //gán giá trị cho các biến toàn cục
            date = day.ToString() + '-' + month.ToString() + '-' + year.ToString();
            LSCOUNT = lsCount;
            LSSUM = lsSum;
            SUM = sum;

            #endregion
        }
        #endregion
    }
}
