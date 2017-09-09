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
    public class ReportSummaryByShiftController : BaseController
    {
        //
        // GET: /ReportSummaryByShift/
        static DataTable dtToExcel;
        public ActionResult Index(DateTime? FromDate, DateTime? ToDate)
        {
            if (!CheckPermission("ReportSummaryByShift", 1))
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
            ViewBag.store = DA_ScheduleMenu.Instance.Exec("pr_ReportSummaryByShift", lstParam.ToArray());
            dtToExcel = pr_SummarybyShift(FromDate, ToDate);
            int sum = 0;
            for (int i = 0; i < dtToExcel.Rows.Count; i++)
            {
                sum = sum + (int)dtToExcel.Rows[i]["Quantity"];
            }

            ViewBag.Tong = sum;

            return View();
        }

        public DataTable pr_SummarybyShift(DateTime? FromDate, DateTime? ToDate)
        {
            Database db1 = new Database();
            db1.fn_GetData_Pro("pr_ReportSummaryByShift", new SqlParameter("@fromdate", FromDate),
                                                           new SqlParameter("@todate", ToDate));
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

                wsList.Cells["A1"].LoadFromText("Thống kê phần cơm theo ca");

                // string strngay = "Từ : " + FromDate.ToString("dd/MM/yyyy HH:mm:ss") + " - Đến : " + ToDate.ToString("dd/MM/yyyy HH:mm:ss");
                Random rand = new Random();
                // int randomvalue = rand.Next(1000, 9999);
                string namefile = "Bao_Cao_Theo_Ca";
                // wsList.Cells["A2"].LoadFromText(strngay);
                int ncot = dtToExcel.Columns.Count;
                int ndong = dtToExcel.Rows.Count;

                int sum = 0;
                for (int i = 0; i < dtToExcel.Rows.Count; i++)
                {
                    sum = sum + (int)dtToExcel.Rows[i]["Quantity"];
                }

               
                DataTable dt2 = new DataTable();
                System.Data.DataView view = new System.Data.DataView(dtToExcel);
                dt2 = view.ToTable(false, "ShiftName", "Quantity");
                

                wsList.Cells[ndong + 4, 1].LoadFromText("Tổng cộng");
                wsList.Cells[ndong + 4, 1].Style.Font.Bold = true;
                //wsList.Cells[ncot, 1, ncot, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //wsList.Cells[ncot, 1, ncot, 1].Style.Font.Size = 11;

                wsList.Cells[ndong+4, 2].LoadFromText(sum.ToString());
                wsList.Cells[ndong+4, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                wsList.Cells[ndong + 4, 2].Style.Font.Bold = true;
                wsList.Cells[ndong+4, 2].Style.Font.Size = 11;

                dt2.Columns["ShiftName"].ColumnName = "Ca";
                dt2.Columns["Quantity"].ColumnName = "Số lượng";
                wsList.Cells["A3"].LoadFromDataTable(dt2, true, TableStyles.Light20);
                wsList.Cells[wsList.Dimension.Address].AutoFitColumns();

                //wsList.Cells["A" + ndong + ""].LoadFromText("Tổng cộng");
                wsList.Cells[ndong, 1, ndong, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[ndong, 1, ndong, 1].Style.Font.Size = 11;

                wsList.Cells[ndong, 2].LoadFromText(sum.ToString());
                wsList.Cells[ndong, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[ndong, 2].Style.Font.Size = 11;

                wsList.Cells[1, 1, 1, ncot - 1].Merge = true;
                wsList.Cells[1, 1, 1, ncot - 1].Style.Font.Bold = true;
                wsList.Cells[1, 1, 1, ncot - 1].Style.Font.Size = 16;
                wsList.Cells[1, 1, 1, ncot - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                wsList.Cells[2, 1, 2, ncot - 1].Merge = true;
                wsList.Cells[2, 1, 2, ncot - 1].Style.Font.Size = 12;
                wsList.Cells[2, 1, 2, ncot - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                wsList.Column(1).Width = 20;
                wsList.Column(2).Width = 50;
                wsList.Column(3).Width = 50;
                wsList.Column(4).Width = 20;
                wsList.Column(5).Width = 20;

                wsList.Column(2).Style.WrapText = true;
                wsList.Column(3).Style.WrapText = true;
                wsList.Column(5).Style.Numberformat.Format = "#,##0";
                wsList.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                wsList.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                wsList.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                string path = Server.MapPath("/TK_SoLuongPhanComTheoCa.xlsx");
                Stream stream = System.IO.File.Create(path);
                pck.SaveAs(stream);
                stream.Close();
                return Json("/TK_SoLuongPhanComTheoCa.xlsx");
            }

        }
    }
}
