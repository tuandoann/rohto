using HRM_ROHTO.Models.BUS;
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
    public class ReportSoLuongPhanComCanThanhToanController  : BaseController
    {
        //
        // GET: /TK_SoLuongPhanComCanThanhToan/
        #region biến 
        static DataTable dtToExcel;
        static List<Int32> LTONG = new List<int>();
        static DateTime FROMDATE = new DateTime();
        static DateTime ToDATE = new DateTime();
        #endregion

        /// <summary>
        /// hàm được dùng để load dữ liệu
        /// </summary>
        /// <param name="FromDate">ngày đến</param>
        /// <param name="ToDate">ngày đi</param>
        /// <returns></returns>
        public ActionResult Index(DateTime? FromDate, DateTime? ToDate)
        {
            //check login
            if (!CheckPermission("ReportSoLuongPhanComCanThanhToan", 1))
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
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("@fromdate", FromDate));
            lstParam.Add(new SqlParameter("@todate", ToDate));

            //hứng dữ liệu từ store
            List<Int32> lTong = new List<int>();
            DataTable result_store = new DataTable();
            result_store = DA_ScheduleMenu.Instance.Exec("pr_ReportSLPhanComCanThanhToan", lstParam.ToArray());

            #region tính biến item tổng
            //khai báo các biến tổng các ca
            int dkCa1, inPhieuCa1, inPhieuDXCa1, payCa1, dkCa0, inPhieuCa0, inPhieuDXCa0, payCa0, dkCa1P, inPhieuCa1P, inPhieuDXCa1P, payCa1P, dkCa2, inPhieuCa2, inPhieuDXCa2, payCa2, dkCa0P, inPhieuCa0P, inPhieuDXCaoP, payCa0P, dkCa3, inPhieuCa3, inPhieuDXCa3, payCa3, dkCaSum, inPhieuCaSum, inPhieuDXCaSum, payCaSum, pay, sl;
            dkCa1 = inPhieuCa1 = inPhieuDXCa1 = inPhieuDXCa1 = dkCa0 = inPhieuCa0 = inPhieuDXCa0 = dkCa1P = inPhieuCa1P = inPhieuDXCa1P = dkCa2 = inPhieuCa2 = inPhieuDXCa2 = dkCa0P = inPhieuCa0P = inPhieuDXCaoP = dkCa3 = inPhieuCa3 = inPhieuDXCa3 = payCa1 = payCa0 = payCa1P = payCa2 = payCa0P = payCa3 = dkCaSum = inPhieuCaSum = inPhieuDXCaSum = payCaSum = pay = sl = 0;
            foreach(DataRow item in result_store.Rows)
            {
                dkCa1 += Convert.ToInt32(item["Ca1Reg"]);
                inPhieuCa1 += Convert.ToInt32(item["Ca1Print"]);
                inPhieuDXCa1 += Convert.ToInt32(item["Ca1Exp"]);
                payCa1 += Convert.ToInt32(item["Ca1Payment"]);

                dkCa0 += Convert.ToInt32(item["Ca0Reg"]);
                inPhieuCa0 += Convert.ToInt32(item["Ca0Print"]);
                inPhieuDXCa0 += Convert.ToInt32(item["Ca0Exp"]);
                payCa0 += Convert.ToInt32(item["Ca0Payment"]);

                dkCa1P += Convert.ToInt32(item["Ca1PlusReg"]);
                inPhieuCa1P += Convert.ToInt32(item["Ca1PlusPrint"]);
                inPhieuDXCa1P += Convert.ToInt32(item["Ca1PlusExp"]);
                payCa1P += Convert.ToInt32(item["Ca1PlusPayment"]);

                dkCa2 += Convert.ToInt32(item["Ca2Reg"]);
                inPhieuCa2 += Convert.ToInt32(item["Ca2Print"]);
                inPhieuDXCa2 += Convert.ToInt32(item["Ca2Exp"]);
                payCa2 += Convert.ToInt32(item["Ca2Payment"]);

                dkCa0P += Convert.ToInt32(item["Ca0PlusReg"]);
                inPhieuCa0P += Convert.ToInt32(item["Ca0PlusPrint"]);
                inPhieuDXCaoP += Convert.ToInt32(item["Ca0PlusExp"]);
                payCa0P += Convert.ToInt32(item["Ca0PlusPayment"]);

                dkCa3 += Convert.ToInt32(item["Ca3Reg"]);
                inPhieuCa3 += Convert.ToInt32(item["Ca3Print"]);
                inPhieuDXCa3 += Convert.ToInt32(item["Ca3Exp"]);
                payCa3 += Convert.ToInt32(item["Ca3Payment"]);

                dkCaSum += Convert.ToInt32(item["SumReg"]);
                inPhieuCaSum += Convert.ToInt32(item["SumPrint"]);
                inPhieuDXCaSum += Convert.ToInt32(item["SumExp"]);
                payCaSum += Convert.ToInt32(item["SumPayment"]);

                pay += Convert.ToInt32(item["DeltaPayment"]);

                sl += Convert.ToInt32(item["Special"]);
            }
            
            //add item tong vào list
            lTong.Add(dkCa1);
            lTong.Add(inPhieuCa1);
            lTong.Add(inPhieuDXCa1);
            lTong.Add(payCa1);

            lTong.Add(dkCa0);
            lTong.Add(inPhieuCa0);
            lTong.Add(inPhieuDXCa0);
            lTong.Add(payCa0);

            lTong.Add(dkCa1P);
            lTong.Add(inPhieuCa1P);
            lTong.Add(inPhieuDXCa1P);
            lTong.Add(payCa1P);

            lTong.Add(dkCa2);
            lTong.Add(inPhieuCa2);
            lTong.Add(inPhieuDXCa2);
            lTong.Add(payCa2);

            lTong.Add(dkCa0P);
            lTong.Add(inPhieuCa0P);
            lTong.Add(inPhieuDXCaoP);
            lTong.Add(payCa0P);

            lTong.Add(dkCa3);
            lTong.Add(inPhieuCa3);
            lTong.Add(inPhieuDXCa3);
            lTong.Add(payCa3);

            lTong.Add(dkCaSum);
            lTong.Add(inPhieuCaSum);
            lTong.Add(inPhieuDXCaSum);
            lTong.Add(payCaSum);

            lTong.Add(pay);
            lTong.Add(sl);

            #endregion

            //set giá trị cho view bag
            ViewBag.store = result_store;
            ViewBag.tong = lTong;

            //set giá trị cho biến cục bộ
            LTONG = lTong;
            dtToExcel = pr_SummarybyShift(FromDate, ToDate);
            FROMDATE = Convert.ToDateTime(FromDate);
            ToDATE = Convert.ToDateTime(ToDate);

            //ViewBag.FromDate = f.ToString("yyyy-MM-dd");
            //ViewBag.ToDate = t.ToString("yyyy-MM-dd");
            return View();
        }
        /// <summary>
        /// hàm dùng để đổ dữ liệu cho dtToExcel
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public DataTable pr_SummarybyShift(DateTime? FromDate, DateTime? ToDate)
        {
            Database db1 = new Database();
            db1.fn_GetData_Pro("pr_ReportSLPhanComCanThanhToan", new SqlParameter("@fromdate", FromDate),
                                                           new SqlParameter("@todate", ToDate));
            DataTable dt1 = db1.mn_Table;
            return dt1;
        }

        /// <summary>
        /// hàm dùng để xuất ra excel
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
                int ncot = dtToExcel.Columns.Count;
                int nrow = dtToExcel.Rows.Count;
                int addRow = 0;
                int lastRow = nrow + 5 + addRow;

                #region dữ liệu
                #region tiêu đề
                try
                {
                    //đỗ dữ liệu
                    wsList.Cells[3, 1].LoadFromText("THÁNG");
                    wsList.Cells[3, 2].LoadFromText("CA 1");
                    wsList.Cells[3, 6].LoadFromText("CA 0");
                    wsList.Cells[3, 10].LoadFromText("CA 1+");
                    wsList.Cells[3, 14].LoadFromText("CA 2");
                    wsList.Cells[3, 18].LoadFromText("CA 0+");
                    wsList.Cells[3, 22].LoadFromText("CA 3");
                    wsList.Cells[3, 26].LoadFromText("TỔNG");
                    wsList.Cells[3, 31].LoadFromText("TRƯỜNG HỢP ĐẶC BIỆT");
                    wsList.Cells[3, 30].LoadFromText("SỐ LƯỢNG CHÊNH LỆCH");
                    wsList.Cells[4, 31].LoadFromText("GHI CHÚ");
                    wsList.Cells[4, 32].LoadFromText("SL");
                    for (int i = 2; i < 30; i++)
                    {
                        wsList.Cells[4, i].LoadFromText("SL ĐĂNG KÝ");
                        wsList.Cells[4, ++i].LoadFromText("SL ĐÃ IN PHIẾU");
                        wsList.Cells[4, ++i].LoadFromText("SL IN PHIẾU ĐỘT XUẤT");
                        wsList.Cells[4, ++i].LoadFromText("PAYMENT");
                    }
                    //style
                    wsList.Cells[3, 1, 4, 1].Merge = true;
                    wsList.Cells[3, 30, 4, 30].Merge = true;
                    for (int i = 2; i < 30; i++)
                    {
                        wsList.Cells[3, i, 3, i = 3 + i].Merge = true;
                    }
                    wsList.Cells[3, 31, 3, 32].Merge = true;
                    wsList.Cells[3, 1, 4, ncot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[lastRow, 2, lastRow, ncot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    wsList.Cells[3, 1, 4, ncot].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[3, 1, 4, ncot].Style.Font.Bold = true;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                }
                #endregion
                #region dữ liệu
                for (int i = 0; i < dtToExcel.Rows.Count; i++)
                {
                    //một dòng excel
                    DateTime mealDate = Convert.ToDateTime(dtToExcel.Rows[i]["MealDate"]);
                    var dayOfWeek = mealDate.DayOfWeek;
                    int index_row = i + 5 + addRow;
                    if ((dayOfWeek == DayOfWeek.Monday) && i != 0)
                    {
                        addRow += 1;
                        ++index_row;
                    }
                    if ((dayOfWeek == DayOfWeek.Sunday) || (dayOfWeek == DayOfWeek.Saturday))
                    {
                        wsList.Cells[index_row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wsList.Cells[index_row, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                    }
                    wsList.Cells[index_row, 1].Value = mealDate;
                    wsList.Cells[index_row, 1].Style.Numberformat.Format = "dd/MM/yyyy";
                    //đổ dữ liệu các ca
                    for (int j = 0; j < 7; j++)
                    {
                        string ca;
                        switch (j)
                        {
                            case 0:
                                ca = "Ca1";
                                break;
                            case 1:
                                ca = "Ca0";
                                break;
                            case 2:
                                ca = "Ca1Plus";
                                break;
                            case 3:
                                ca = "Ca2";
                                break;
                            case 4:
                                ca = "Ca0Plus";
                                break;
                            case 5:
                                ca = "Ca3";
                                break;
                            default:
                                ca = "Sum";
                                break;
                        }
                        wsList.Cells[index_row, (4 * j) + 2].LoadFromText(dtToExcel.Rows[i][ca + "Reg"].ToString());
                        wsList.Cells[index_row, (4 * j) + 2 + 1].LoadFromText(dtToExcel.Rows[i][ca + "Print"].ToString());
                        wsList.Cells[index_row, (4 * j) + 2 + 2].LoadFromText(dtToExcel.Rows[i][ca + "Exp"].ToString());
                        wsList.Cells[index_row, (4 * j) + 2 + 3].LoadFromText(dtToExcel.Rows[i][ca + "Payment"].ToString());
                    }
                    wsList.Cells[index_row, 30].LoadFromText(dtToExcel.Rows[i]["DeltaPayment"].ToString());
                    wsList.Cells[index_row, 31].LoadFromText(dtToExcel.Rows[i]["Notes"].ToString());
                    wsList.Cells[index_row, 32].LoadFromText(dtToExcel.Rows[i]["Special"].ToString());
                }
                #endregion
                #endregion
                lastRow = nrow + 5 + addRow;
                #region phần cứng
                //đỗ dữ liệu
                wsList.Cells["A1"].LoadFromText("THỐNG KÊ SỐ LƯỢNG PHẦN CƠM CẦN THANH TOÁN");
                wsList.Cells["A2"].LoadFromText("TỪ " + FROMDATE.ToString("dd/MM/yyyy") +" ĐẾN "+ ToDATE.ToString("dd/MM/yyyy"));
                wsList.Cells[lastRow, 1, lastRow, 1].LoadFromText("Tổng cộng");
                int index = 2;
                foreach (int item in LTONG)
                {
                    index = index == 31 ? 32 : index;
                    wsList.Cells[lastRow, index,lastRow, index].LoadFromText(item.ToString());
                    wsList.Cells[lastRow, index, lastRow, index].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    wsList.Cells[lastRow, index, lastRow, index].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    index += 1;
                }
                //style
                wsList.Cells[1, 1, 1, ncot].Merge = true;
                wsList.Cells[2, 1, 2, ncot].Merge = true;
                wsList.Cells[1, 1, 1, ncot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[1, 1, 1, ncot].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsList.Cells[2, 1, 2, ncot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsList.Cells[2, 1, 2, ncot].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsList.Cells[lastRow, 1, lastRow, ncot].Style.Font.Bold = true;
                wsList.Cells[1, 1, 1, ncot].Style.Font.Bold = true;
                wsList.Cells[1, 1, 1, ncot].Style.Font.Size = 14;
                #endregion

                //định dạng màu sắc
                wsList.Cells[2, 2, lastRow, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 6, lastRow, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 10, lastRow, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 14, lastRow, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 18, lastRow, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 22, lastRow, 25].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 26, lastRow, 29].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 30, lastRow, 30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsList.Cells[2, 2, lastRow, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(252, 213, 180));
                wsList.Cells[2, 6, lastRow, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(219, 229, 241));
                wsList.Cells[2, 10, lastRow, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(155, 187, 89));
                wsList.Cells[2, 14, lastRow, 17].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(215, 228, 188));
                wsList.Cells[2, 18, lastRow, 21].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(141, 180, 227));
                wsList.Cells[2, 22, lastRow, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(252, 213, 180));
                wsList.Cells[2, 26, lastRow, 29].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(178, 161, 199));
                wsList.Cells[2, 30, lastRow, 30].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#DF0101"));

                //định dạng wraptext
                wsList.Cells[4, 2, 4, 29].Style.WrapText = true;
                wsList.Cells[3, 30, 4, 30].Style.WrapText = true;
                wsList.Cells[4, 31, 4, 31].Style.WrapText = true;


                //định danh font cho toàn sheet
                wsList.Cells[1, 1, lastRow, ncot].Style.Font.Name = "Tahoma";
                wsList.Cells[1, 1, lastRow, ncot].Style.Font.Size = 11;
                wsList.Cells[3, 1, lastRow, ncot].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, lastRow, ncot].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, lastRow, ncot].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                wsList.Cells[3, 1, lastRow, ncot].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //chỉnh width cho sheet
                wsList.Column(1).Width = 13;
                for (int i = 2; i <= 32; i++)
                {
                    wsList.Column(i).Width = 10;
                }

                string path = Server.MapPath("/TK_SoLuongPhanComCanThanhToan.xlsx");
                Stream stream = System.IO.File.Create(path);
                pck.SaveAs(stream);
                stream.Close();
                return Json("/TK_SoLuongPhanComCanThanhToan.xlsx");
            }

        }
    }
}
