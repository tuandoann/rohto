﻿using HRM_ROHTO.Models.BUS;
using HRM_ROHTO.Util;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_ROHTO.Controllers
{
    public class ReportSummaryByEmployeeController : BaseController
    {
        //
        // GET: /ReportSummaryByEmployee/
        #region biến
        static DataTable dtToExcel;
        static DateTime FROMDATE = new DateTime();
        static DateTime TODATE = new DateTime();
        static List<long> TONG = new List<long>();
        static List<object> RESULT_LIST = new List<object>();
        static List<object> TONG_ITEM = new List<object>();
        #endregion

        /// <summary>
        /// hàm dùng để load trang báo cáo
        /// </summary>
        /// <param name="FromDate">ngày đến</param>
        /// <param name="ToDate">ngày đi</param>
        /// <returns></returns>
        public ActionResult Index(DateTime? FromDate, DateTime? ToDate)
        {
            if (!CheckPermission("ReportSummaryByEmployee", 1))
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

            //  ViewBag.store = DA_ScheduleMenu.Instance.Exec("pr_ReportMealCardByDay", lstParam.ToArray());
            // DateTime FromDate = getFromDateFromNow();
            // ViewBag.FromDate = FromDate.ToString("yyyy-MM-dd");
            var result_store = DA_ScheduleMenu.Instance.Exec("pr_ReportRegisterByEmployee", lstParam.ToArray());
            dtToExcel = pr_SummarybyEmployee(FromDate, ToDate);

            //lưu trữ dữ liệu datatable
            List<object> result_list = new List<object>();
            //lưu trữ tổng các ca và tổng
            List<long> tong = new List<long>();
            List<object> tong_item = new List<object>();
            List<DataRow> result_current = new List<DataRow>();
            DateTime date_before = new DateTime();
            long sum_ca0 = 0;
            long sum_ca1 = 0;
            long sum_ca2 = 0;
            long sum_ca3 = 0;
            long sum_ca01 = 0;
            long sum_ca11 = 0;
            long sum = 0;
            for (int i = 0; i < result_store.Rows.Count; i++)
            {
                DateTime date_current = Convert.ToDateTime(result_store.Rows[i][0].ToString());
                int result_compare = DateTime.Compare(date_before, date_current);

                //nếu nó là đầu vòng lặp hoặc nó nó là đầu group ngày
                if (i == 0 || result_compare != 0)
                {
                    date_before = date_current;
                    if (result_compare != 0 && i != 0)
                    {
                        result_list.Add(result_current);
                        result_current = new List<DataRow>();
                    }
                    result_current.Add(result_store.Rows[i]);
                }
                //nếu nó nằm ở giữa vòng lặp group
                else
                {
                    result_current.Add(result_store.Rows[i]);
                }

                //tính tổng các ca và tổng 
                sum_ca0 = string.IsNullOrWhiteSpace(result_store.Rows[i]["Ca0"].ToString())? sum_ca0 : ++sum_ca0;
                sum_ca1 = string.IsNullOrWhiteSpace(result_store.Rows[i]["Ca1"].ToString()) ? sum_ca1 : ++sum_ca1;
                sum_ca2 = string.IsNullOrWhiteSpace(result_store.Rows[i]["Ca2"].ToString()) ? sum_ca2 : ++sum_ca2;
                sum_ca3 = string.IsNullOrWhiteSpace(result_store.Rows[i]["Ca3"].ToString()) ? sum_ca3 : ++sum_ca3;
                sum_ca01 = string.IsNullOrWhiteSpace(result_store.Rows[i]["Ca0Plus"].ToString()) ? sum_ca01 : ++sum_ca01;
                sum_ca11 = string.IsNullOrWhiteSpace(result_store.Rows[i]["Ca1Plus"].ToString()) ? sum_ca11 : ++sum_ca11;
                sum += Convert.ToInt64(result_store.Rows[i]["Total"]);

                //nếu nó nằm ở cuối vòng lặp
                if (i == result_store.Rows.Count - 1)
                {
                    result_list.Add(result_current);
                }
            }
            foreach(List<DataRow> element in result_list)
            {
                List<long> sum_item = new List<long>();
                long sum_ca0t = 0;
                long sum_ca1t = 0;
                long sum_ca21 = 0;
                long sum_ca31 = 0;
                long sum_ca011 = 0;
                long sum_ca111 = 0;
                long sum1 = 0;
                foreach(DataRow item in element)
                {
                    sum_ca0t = string.IsNullOrWhiteSpace(item["Ca0"].ToString()) ? sum_ca0t : ++sum_ca0t;
                    sum_ca1t = string.IsNullOrWhiteSpace(item["Ca1"].ToString()) ? sum_ca1t : ++sum_ca1t;
                    sum_ca21 = string.IsNullOrWhiteSpace(item["Ca2"].ToString()) ? sum_ca21 : ++sum_ca21;
                    sum_ca31 = string.IsNullOrWhiteSpace(item["Ca3"].ToString()) ? sum_ca31 : ++sum_ca31;
                    sum_ca011 = string.IsNullOrWhiteSpace(item["Ca0Plus"].ToString()) ? sum_ca011 : ++sum_ca011;
                    sum_ca111 = string.IsNullOrWhiteSpace(item["Ca1Plus"].ToString()) ? sum_ca111 : ++sum_ca111;
                    sum1 += Convert.ToInt64(item["Total"]);
                }
                sum_item.Add(sum_ca0t);
                sum_item.Add(sum_ca1t);
                sum_item.Add(sum_ca21);
                sum_item.Add(sum_ca31);
                sum_item.Add(sum_ca011);
                sum_item.Add(sum_ca111);
                sum_item.Add(sum1);
                tong_item.Add(sum_item);
            }
            //set giá trị cho tổng
            tong.Add(sum_ca0);
            tong.Add(sum_ca1);
            tong.Add(sum_ca2);
            tong.Add(sum_ca3);
            tong.Add(sum_ca01);
            tong.Add(sum_ca11);
            tong.Add(sum);

            //int sum = 0;
            //for (int i = 0; i < dtToExcel.Rows.Count; i++)
            //{
            //    sum = sum + (int)dtToExcel.Rows[i]["Quantity"];
            //}
            //gán giá trị cho viewbag
            ViewBag.store = result_list;
            ViewBag.Tong = tong;
            ViewBag.Tong_item = tong_item;

            //se giá trị cho biến toàn cục
            FROMDATE = Convert.ToDateTime(FromDate);
            TODATE = Convert.ToDateTime(ToDate);
            TONG = tong;
            RESULT_LIST = result_list;
            TONG_ITEM = tong_item;

            return View();

        }
        /// <summary>
        /// hàm đổ dữ liệu cho biến toàn cục
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public DataTable pr_SummarybyEmployee(DateTime? FromDate, DateTime? ToDate)
        {
            Database db1 = new Database();
            db1.fn_GetData_Pro("pr_ReportRegisterByEmployee", new SqlParameter("@fromdate", FromDate),
                                                           new SqlParameter("@todate", ToDate));
            DataTable dt1 = db1.mn_Table;
            return dt1;
        }

        /// <summary>
        /// hàm dùng để xuất excel
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportMealCard(FormCollection fc)
        {

            // dtToExcel = pr_GetListMealToomorrow(day, Month, Year);
            using (ExcelPackage pck = new ExcelPackage())
            {
                var wsList = pck.Workbook.Worksheets.Add("Báo cáo");
                String date = "Từ " + FROMDATE.ToString("dd/MM/yyyy") + " ĐẾN NGÀY " + TODATE.ToString("dd/MM/yyyy");
                int ndong = dtToExcel.Rows.Count;
                int countGroup = RESULT_LIST.Count;
                countGroup *= 2;
                //định danh font cho toàn sheet
                wsList.Cells[1, 1, ndong + 4 + countGroup, 10].Style.Font.Name = "Tahoma";
                wsList.Cells[1, 1, ndong + 4 + countGroup, 10].Style.Font.Size = 11;
                wsList.Cells[3, 1, ndong + 4 + countGroup, 10].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, ndong + 4 + countGroup, 10].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, ndong + 4 + countGroup, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, ndong + 4 + countGroup, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;   

                #region phần cứng
                //đổ dữ liệu
                wsList.Cells["B1"].LoadFromText("THỐNG KÊ SỐ LƯỢNG PHẦN CƠM ĐÃ ĐĂNG KÝ CỦA NHÂN VIÊN");
                wsList.Cells["B2"].Value = date;
                wsList.Cells["B2"].Style.Numberformat.Format = "dd/MM/yyyy";
                wsList.Cells[ndong + 4 + countGroup, 1].LoadFromText("Tổng cộng");
                for (int i = 0; i < TONG.Count; i++)
                {
                    wsList.Cells[ndong + 4 + countGroup, 4 + i, ndong + 4 + countGroup, 4 + i].LoadFromText(TONG.ElementAt(i).ToString());
                }
                //style
                wsList.Cells[1, 2, 1, 10].Merge = true;
                wsList.Cells[2, 2, 2, 10].Merge = true;
                wsList.Cells[ndong + 4 + countGroup, 1, ndong + 4 + countGroup, 3].Merge = true;
                wsList.Cells[1, 2, 1, 10].Style.Font.Bold = true;
                wsList.Cells[2, 2, 2, 10].Style.Font.Bold = true;
                wsList.Cells[1, 2, 1, 10].Style.Font.Size = 14;
                wsList.Cells[ndong + 4 + countGroup, 1, ndong + 4 + countGroup, 10].Style.Font.Bold = true;
                wsList.Cells[1, 2, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[2, 2, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[ndong + 4 + countGroup, 1, ndong + 4 + countGroup, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion

                #region phần động
                #region tiêu đề cột
                //đỗ dữ liệu
                wsList.Cells[3, 1, 3, 1].LoadFromText("STT");
                wsList.Cells[3, 2, 3, 2].LoadFromText("NHÂN VIÊN");
                wsList.Cells[3, 3, 3, 3].LoadFromText("MÃ NHÂN VIÊN");
                wsList.Cells[3, 4, 3, 4].LoadFromText("CA 0");
                wsList.Cells[3, 5, 3, 5].LoadFromText("CA 1");
                wsList.Cells[3, 6, 3, 6].LoadFromText("CA 2");
                wsList.Cells[3, 7, 3, 7].LoadFromText("CA 3");
                wsList.Cells[3, 8, 3, 8].LoadFromText("CA 0+");
                wsList.Cells[3, 9, 3, 9].LoadFromText("CA 1+");
                wsList.Cells[3, 10, 3, 10].LoadFromText("TỔNG");
                //style
                wsList.Cells[3, 1, 3, 10].Style.Font.Bold = true;
                wsList.Cells[3, 1, 3, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion
                #region dữ liệu
                int index = 4;
                int index_tong_item = 0;
                foreach(List<DataRow> lDataRow in RESULT_LIST)
                {
                    int stt = 1;
                    //băt đầu vô group
                    wsList.Cells[index, 1, index, 1].LoadFromText(Convert.ToDateTime(lDataRow.ElementAt(0)["MealDate"]).ToString("dd/MM/yyyy"));
                    wsList.Cells[index, 1, index, 10].Merge = true;
                    wsList.Cells[index, 1, index, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //giữa group
                    index += 1;
                    foreach(DataRow item in lDataRow)
                    {
                        //đỗ dữ liệu
                        wsList.Cells[index, 1, index, 1].LoadFromText(stt.ToString());
                        wsList.Cells[index, 2, index, 2].LoadFromText(item["FullName"].ToString());
                        wsList.Cells[index, 3, index, 3].LoadFromText(item["EmployeeCode"].ToString());
                        wsList.Cells[index, 4, index, 4].LoadFromText(item["Ca0"].ToString());
                        wsList.Cells[index, 5, index, 5].LoadFromText(item["Ca1"].ToString());
                        wsList.Cells[index, 6, index, 6].LoadFromText(item["Ca2"].ToString());
                        wsList.Cells[index, 7, index, 7].LoadFromText(item["Ca3"].ToString());
                        wsList.Cells[index, 8, index, 8].LoadFromText(item["Ca0Plus"].ToString());
                        wsList.Cells[index, 9, index, 9].LoadFromText(item["Ca1Plus"].ToString());
                        wsList.Cells[index, 10, index, 10].LoadFromText(item["Total"].ToString());
                        //style
                        wsList.Cells[index, 3, index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsList.Cells[index, 1, index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //kết thúc một item
                        index += 1;
                        stt += 1;
                    }
                    //kết thúc group
                    List<long> sum_item = ((IEnumerable<long>)TONG_ITEM[index_tong_item]).ToList();
                    wsList.Cells[index, 1, index, 1].LoadFromText("Tổng: ");
                    for (int i = 0; i < sum_item.Count; i++ )
                    {
                        wsList.Cells[index, i + 4, index, i + 4].LoadFromText(sum_item.ElementAt(i).ToString());
                    }
                    wsList.Cells[index, 1, index, 3].Merge = true;
                    wsList.Cells[index, 1, index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[index, 1, index, 10].Style.Font.Bold = true;
                    index += 1;
                }
                #endregion
                #endregion
                //wsList.Cells[wsList.Dimension.Address].AutoFitColumns();

                //modify width column
                wsList.Column(1).Width = 5;
                wsList.Column(2).Width = 23;
                wsList.Column(3).Width = 10;
                wsList.Column(4).Width = 14;
                wsList.Column(5).Width = 14;
                wsList.Column(6).Width = 14;
                wsList.Column(7).Width = 14;
                wsList.Column(8).Width = 14;
                wsList.Column(9).Width = 14;
                wsList.Column(10).Width = 8;

                //định dạnh wrap text cho sheet
                wsList.Cells[1, 1, ndong + 4 + countGroup, 10].Style.WrapText = true;

                string path = Server.MapPath("/TK_SoLuongPhanComTheoNhanVien.xlsx");
                Stream stream = System.IO.File.Create(path);
                pck.SaveAs(stream);
                stream.Close();
                return Json("/TK_SoLuongPhanComTheoNhanVien.xlsx");
            }

        }
    }
}