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
    public class ReportPrintMealToDayController : BaseController
    {
        //
        // GET: /ReportPrintMealToDay/
        static DataTable dtToExcel;
        static List<Int32> SUMARRAY = new List<int>();
        static List<Int32> COUNTARRAY = new List<int>();
        static long SUM;
        static DateTime FROMDATE = new DateTime();

        public ActionResult Index(int year = 0, int month = 0, int day = 0)
        {
            if (!CheckPermission("ReportPrintMealToDay", 1))
                return RedirectToAction("Login", "Login", "Login");

            DateTime FromDate;
            try{
                FromDate = new DateTime(year,month,day);
            }
            catch
            {
                FromDate = DateTime.Now;
            }

            List<SqlParameter> lstParam = new List<SqlParameter>();

            lstParam.Add(new SqlParameter("@date", FromDate));

            //DateTime FromDate = getFromDateFromNow();
            //ViewBag.FromDate = FromDate.ToString("yyyy-MM-dd");
            var result_store = DA_ScheduleMenu.Instance.Exec("pr_ReportPrintMealToDay", lstParam.ToArray());
            List<Int32> sum_array = new List<int>();
            List<Int32> count_array = new List<int>();
            int sum_child = 0;
            int count_child = 1;
            int ShiftID = 0;
            for (int i = 0; i < result_store.Rows.Count; i++)
            {
                int shiftId_current = Convert.ToInt32(result_store.Rows[i][0].ToString());
                int quantity_current = Convert.ToInt32(result_store.Rows[i][3]);
                if (ShiftID != shiftId_current)
                {
                    //add dữ liệu vào list
                    if(i!=0)
                    {
                        sum_array.Add(sum_child);
                        count_array.Add(count_child);
                    }
                    //khai báo lại cho sự khởi đầu
                    ShiftID = shiftId_current;
                    count_child = 1;
                    sum_child = quantity_current;
                }
                else
                {
                    //tiếp tục
                    count_child += 1;
                    sum_child += quantity_current;
                }
                if (i == result_store.Rows.Count - 1)
                {
                    count_array.Add(count_child);
                    sum_array.Add(sum_child);
                }
            }
            dtToExcel = pr_SummarybyShift(FromDate);

            long sum = 0;
            for (int i = 0; i < dtToExcel.Rows.Count; i++)
            {
                sum = sum + Convert.ToInt64(dtToExcel.Rows[i]["Quantity"]);
            }

            //set value viewbag
            ViewBag.store = result_store;
            ViewBag.sum_array = sum_array.ToArray();
            ViewBag.count_array = count_array.ToArray();
            ViewBag.tong = sum;

            //gán giá trị biến cục bộ
            SUMARRAY = sum_array;
            COUNTARRAY = count_array;
            SUM = sum;
            FROMDATE = FromDate;

            return View();
        }

        public DataTable pr_SummarybyShift(DateTime? date)
        {
            Database db1 = new Database();
            db1.fn_GetData_Pro("pr_ReportPrintMealToDay", new SqlParameter("@date", date));
            DataTable dt1 = db1.mn_Table;
            return dt1;
        }

        [HttpPost]
        public JsonResult ExportMealCard(FormCollection fc)
        {
            // dtToExcel = pr_GetListMealToomorrow(day, Month, Year);
            using (ExcelPackage pck = new ExcelPackage())
            {
                var wsList = pck.Workbook.Worksheets.Add("Báo cáo");
                int ndong = dtToExcel.Rows.Count;

                //định danh font cho toàn sheet
                wsList.Cells[1, 1, dtToExcel.Rows.Count + 4, 4].Style.Font.Name = "Tahoma";
                wsList.Cells[1, 1, dtToExcel.Rows.Count + 4, 4].Style.Font.Size = 11;
                wsList.Cells[3, 1, dtToExcel.Rows.Count + 4, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, dtToExcel.Rows.Count + 4, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, dtToExcel.Rows.Count + 4, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, dtToExcel.Rows.Count + 4, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;               

                #region phần cứng (các cell ngoài data)
                //Đỗ dữ liệu
                wsList.Cells["A1"].LoadFromText("THỐNG KÊ SỐ LƯỢNG ĐÃ IN CHO TỪNG MÓN");
                wsList.Cells["A2"].LoadFromText("NGÀY");
                wsList.Cells["B2"].Value = FROMDATE;
                wsList.Cells["B2"].Style.Numberformat.Format = "dd/MM/yyyy";
                wsList.Cells[ndong + 4, 1].LoadFromText("Tổng cộng");
                wsList.Cells[ndong + 4, 4].LoadFromText(SUM.ToString());
                //định dạnh style
                wsList.Cells["A1"].Style.Font.Size = 14;
                wsList.Cells[1, 1, 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells["A1"].Style.Font.Bold = true;
                wsList.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[1, 1, 1, 4].Merge = true;
                wsList.Cells[2, 2, 2, 4].Merge = true;
                wsList.Cells[2, 2, 2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[ndong + 4, 1, ndong + 4, 3].Merge = true;
                wsList.Cells[ndong + 4, 1, ndong + 4, 3].Style.Font.Bold = true;
                wsList.Cells[ndong + 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[ndong + 4, 4].Style.Font.Bold = true;
                #endregion
                
                #region Data
                //đỗ dữ liệu vào datatable
                wsList.Cells["A3"].LoadFromText("TÊN CA");
                wsList.Cells["B3"].LoadFromText("TÊN MÓN ĂN");
                wsList.Cells["C3"].LoadFromText("SỐ LƯỢNG [PHẦN]");
                wsList.Cells["D3"].LoadFromText("TỔNG");
                int index = 4;
                for (int i = 0; i < dtToExcel.Rows.Count;i++ )
                {
                    wsList.Cells[index + i, 2, index + i, 2].LoadFromText(dtToExcel.Rows[i]["ProductName"].ToString());
                    wsList.Cells[index + i, 3, index + i, 3].LoadFromText(dtToExcel.Rows[i]["Quantity"].ToString());
                }
                wsList.Cells[wsList.Dimension.Address].AutoFitColumns();
                index = 4;
                for (int i = 0; i < SUMARRAY.Count; i++)
                {
                    //Tên ca
                    wsList.Cells[index, 1, index + COUNTARRAY.ElementAt(i) - 1, 1].Merge = true;
                    wsList.Cells[index, 1, index + COUNTARRAY.ElementAt(i) - 1, 1].LoadFromText(dtToExcel.Rows[index + COUNTARRAY.ElementAt(i) - 5]["ShiftName"].ToString());
                    wsList.Cells[index, 1, index + COUNTARRAY.ElementAt(i) - 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    wsList.Cells[index, 1, index + COUNTARRAY.ElementAt(i) - 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //Tổng
                    wsList.Cells[index, 4, index + COUNTARRAY.ElementAt(i) - 1, 4].Merge = true;
                    wsList.Cells[index, 4, index + COUNTARRAY.ElementAt(i) - 1, 4].LoadFromText(SUMARRAY.ElementAt(i).ToString());
                    wsList.Cells[index, 4, index + COUNTARRAY.ElementAt(i) - 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    wsList.Cells[index, 4, index + COUNTARRAY.ElementAt(i) - 1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    index += COUNTARRAY.ElementAt(i);
                }
                //style excel
                wsList.Cells[3, 1, 3, 4].Style.Font.Bold = true;
                wsList.Cells[3, 1, 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion

                //định dạnh width cho sheet
                wsList.Column(1).Width = 10;
                wsList.Column(2).Width = 40;
                wsList.Column(3).Width = 20;
                wsList.Column(4).Width = 10;

                string path = Server.MapPath("/TK_SoLuongPhieuComDaIn.xlsx");
                Stream stream = System.IO.File.Create(path);
                pck.SaveAs(stream);
                stream.Close();
                return Json("/TK_SoLuongPhieuComDaIn.xlsx");
            }

        }

    }
}