using System;
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

namespace HRM_ROHTO.Controllers
{
    public class ScheduleMenuController : BaseController
    {
        //
        // GET: /ScheduleMenu/
        private static DataTable dtToExcel = new DataTable();
        private static DateTime FROMDATE = new DateTime();
        public ActionResult Index(int year = 0, int month = 0)
        {
            if (!CheckPermission("ScheduleMenu", 1))
                return RedirectToAction("Login", "Login", "Login");

            int Year = year == 0 ? DateTime.Now.Year : year;
            int Month = month == 0 ? DateTime.Now.Month : month;

            //Schedule_All_Ext Schedule_All_Ext = new Schedule_All_Ext
            //{
            //    Schedule_Menu_Ext = DA_ScheduleMenu.Instance.GetDateMealScheduleMenu(Year, Month),
            //    Schedule_Menu_Product_Shift_Ext = DA_ScheduleMenu.Instance.GetDataScheduleMenu(),
            //    ScheduleMenuID = DA_ScheduleMenu.Instance.GetAll().ToList()
            //};

            int lastDay = DateTime.DaysInMonth(Year, Month);

            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("@FromDate", Year + "-" + Month + "-01"));
            lstParam.Add(new SqlParameter("@ToDate", Year + "-" + Month + "-" + lastDay));
            ViewBag.store = dtToExcel = DA_ScheduleMenu.Instance.Exec("pr_getScheduleMenu", lstParam.ToArray());
            FROMDATE = new DateTime(Year, Month, 1);
            //return View(Schedule_All_Ext);

            Schedule_All_Ext Schedule_All_Ext = new Schedule_All_Ext
            {
                Schedule_Menu_Ext = DA_ScheduleMenu.Instance.GetScheduleMenu(Year, Month),
                Schedule_Menu_Product_Ext = DA_ScheduleMenu.Instance.GetScheduleProduct(),
                Schedule_Menu_Shift_Ext = DA_ScheduleMenu.Instance.GetScheduleMenuShift()
            };

            return View(Schedule_All_Ext);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveData(FormCollection fc, string update = null, string cancel = null)
        {
            DateTime From = DateTime.Parse(fc["From"]);
            DateTime To = DateTime.Parse(fc["To"]);

            for (DateTime date = From; date <= To; date = date.AddDays(1.0))
            {
                if (update != null)
                {
                    //delete Schedule_Menu
                    List<TBL_SCHEDULE_MENU> lstSchedule_Menu = DA_ScheduleMenu.Instance.GetAll().Where(x => x.MealDate == date).ToList();
                    foreach (var menu in lstSchedule_Menu)
                    {
                        //Ghi log
                        TBL_SCHEDULE_MENU oldSchedule_Menu = DA_ScheduleMenu.Instance.GetById(menu.ScheduleMenuID);
                        WriteLog("ScheduleMenu", "Delete", oldSchedule_Menu);

                        //delete Schedule_Menu
                        DA_ScheduleMenu.Instance.Delete(menu);

                        //delete Schedule_Menu_Product
                        List<TBL_SCHEDULE_MENU_PRODUCT> lstSchedule_Menu_Product = DA_ScheduleMenuProduct.Instance.GetAll().Where(x => x.ScheduleMenuID == menu.ScheduleMenuID).ToList();
                        // List Old Schedule Menu Product
                        List<TBL_SCHEDULE_MENU_PRODUCT> lstOldSchedule_Menu_Product = new List<TBL_SCHEDULE_MENU_PRODUCT>();
                        foreach (var item in lstSchedule_Menu_Product)
                        {
                            //Add Vào List Old Schedule Menu Product
                            TBL_SCHEDULE_MENU_PRODUCT oldSchedule_Menu_Product = DA_ScheduleMenuProduct.Instance.GetById(item.ScheduleMenuID);
                            lstOldSchedule_Menu_Product.Add(oldSchedule_Menu_Product);

                            DA_ScheduleMenuProduct.Instance.Delete(item);
                        }
                        // Ghi log
                        WriteLog("ScheduleMenuProduct", "Delete", lstOldSchedule_Menu_Product, null);

                        //delete Schedule_Menu_Shift
                        List<TBL_SCHEDULE_MENU_SHIFT> lstSchedule_Menu_Shift = DA_ScheduleMenuShift.Instance.GetAll().Where(x => x.ScheduleMenuID == menu.ScheduleMenuID).ToList();
                        // List Old Schedule Menu Shift
                        List<TBL_SCHEDULE_MENU_SHIFT> lstOldSchedule_Menu_Shift = new List<TBL_SCHEDULE_MENU_SHIFT>();
                        foreach (var item in lstSchedule_Menu_Shift)
                        {
                            //Add Vào List Old Schedule Menu Shift
                            TBL_SCHEDULE_MENU_SHIFT oldSchedule_Menu_Shift = DA_ScheduleMenuShift.Instance.GetById(item.ScheduleMenuID);
                            lstOldSchedule_Menu_Shift.Add(oldSchedule_Menu_Shift);

                            DA_ScheduleMenuShift.Instance.Delete(item);
                        }
                        // Ghi log
                        WriteLog("ScheduleMenuShift", "Delete", lstOldSchedule_Menu_Shift, null);
                    }
                }

                if (cancel != null)
                {
                    TBL_SCHEDULE_MENU Schedule_Menu = DA_ScheduleMenu.Instance.GetAll().FirstOrDefault(x => x.MealDate == date);
                    if (Schedule_Menu != null)
                        continue;
                }

                List<TBL_MENU> tbl_Menu = DA_Menu.Instance.GetAll().Where(x => x.MenuDay == date.Day).ToList();
                if (tbl_Menu != null)
                {
                    foreach (var menu in tbl_Menu)
                    {
                        //insert Schedule_Menu
                        TBL_SCHEDULE_MENU Schedule_Menu = new TBL_SCHEDULE_MENU();
                        Schedule_Menu.MealDate = date;
                        Schedule_Menu.MenuID = menu.MenuID;
                        Schedule_Menu.UserModified = int.Parse(Session["UserID"].ToString());
                        Schedule_Menu.DateModified = DateTime.Now;
                        DA_ScheduleMenu.Instance.Insert(Schedule_Menu);

                        //Ghi log
                        WriteLog("ScheduleMenu", "Create", null, Schedule_Menu);

                        //Get Schedule_Menu_ID
                        long Schedule_Menu_ID = DA_ScheduleMenu.Instance.GetAll().OrderByDescending(x => x.ScheduleMenuID).FirstOrDefault().ScheduleMenuID;

                        //insert Schedule_Menu_Product
                        List<TBL_MENU_PRODUCT> lstMenuProduct = DA_MenuProduct.Instance.GetAll().Where(x => x.MenuID == menu.MenuID).ToList();
                        // List New Schedule Menu Product
                        List<TBL_SCHEDULE_MENU_PRODUCT> lstNewSchedule_Menu_Product = new List<TBL_SCHEDULE_MENU_PRODUCT>();
                        foreach (var item in lstMenuProduct)
                        {
                            TBL_SCHEDULE_MENU_PRODUCT Schedule_Menu_Product = new TBL_SCHEDULE_MENU_PRODUCT();
                            Schedule_Menu_Product.ScheduleMenuID = Schedule_Menu_ID;
                            Schedule_Menu_Product.MenuID = menu.MenuID;
                            Schedule_Menu_Product.ProductID = item.ProductID;
                            DA_ScheduleMenuProduct.Instance.Insert(Schedule_Menu_Product);

                            // Add vào List New Schedule Menu Product
                            lstNewSchedule_Menu_Product.Add(Schedule_Menu_Product);
                        }
                        // Ghi log
                        WriteLog("ScheduleMenuProduct", "Create", null, lstNewSchedule_Menu_Product);

                        //insert Schedule_Menu_Shift
                        List<TBL_MENU_SHIFT> lstMenuShift = DA_MenuShift.Instance.GetAll().Where(x => x.MenuID == menu.MenuID).ToList();
                        // List New Schedule Menu Product
                        List<TBL_SCHEDULE_MENU_SHIFT> lstNewSchedule_Menu_Shift = new List<TBL_SCHEDULE_MENU_SHIFT>();
                        foreach (var item in lstMenuShift)
                        {
                            TBL_SCHEDULE_MENU_SHIFT Schedule_Menu_Shift = new TBL_SCHEDULE_MENU_SHIFT();
                            Schedule_Menu_Shift.ScheduleMenuID = Schedule_Menu_ID;
                            Schedule_Menu_Shift.MenuID = menu.MenuID;
                            Schedule_Menu_Shift.ShiftID = item.ShiftID;
                            DA_ScheduleMenuShift.Instance.Insert(Schedule_Menu_Shift);

                            // Add vào List New Schedule Menu Shift
                            lstNewSchedule_Menu_Shift.Add(Schedule_Menu_Shift);
                        }
                        // Ghi log
                        WriteLog("ScheduleMenuShift", "Create", null, lstNewSchedule_Menu_Shift);
                    }
                }
            }
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckBeforeSave(DateTime From, DateTime To)
        {
            List<DateTime> lstDateTime = new List<DateTime>();
            List<TBL_SCHEDULE_MENU> lst = DA_ScheduleMenu.Instance.GetAll().Where(x => x.MealDate >= From && x.MealDate <= To).ToList();
            int i = 1;
            foreach (var schedule in lst)
            {
                if (i == 1 || i == lst.Count)
                    lstDateTime.Add(schedule.MealDate);
                i++;
            }

            return Json(new { data = lstDateTime }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int ScheduleMenuID)
        {
            // Ghi log
            TBL_SCHEDULE_MENU oldSchedule_Menu = DA_ScheduleMenu.Instance.GetById(ScheduleMenuID);
            WriteLog("ScheduleMenu", "Delete", oldSchedule_Menu, null);

            DA_ScheduleMenu.Instance.Delete(ScheduleMenuID);

            // List Old Schedule Menu Product
            List<TBL_SCHEDULE_MENU_PRODUCT> lstOldSchedule_Menu_Product = new List<TBL_SCHEDULE_MENU_PRODUCT>();
            foreach (var item in DA_ScheduleMenuProduct.Instance.GetAll().Where(x => x.ScheduleMenuID == ScheduleMenuID))
            {
                // Add vào List Old Schedule Menu Product
                TBL_SCHEDULE_MENU_PRODUCT oldScheduleProductMenu = DA_ScheduleMenuProduct.Instance.GetById(item.ScheduleMenuProductID);
                lstOldSchedule_Menu_Product.Add(oldScheduleProductMenu);

                DA_ScheduleMenuProduct.Instance.Delete(item);
            }
            // Ghi log
            WriteLog("ScheduleMenuProduct", "Delete", lstOldSchedule_Menu_Product, null);

            // List Old Schedule Menu Product
            List<TBL_SCHEDULE_MENU_SHIFT> lstOldSchedule_Menu_Shift = new List<TBL_SCHEDULE_MENU_SHIFT>();
            foreach (var item in DA_ScheduleMenuShift.Instance.GetAll().Where(x => x.ScheduleMenuID == ScheduleMenuID))
            {
                // Add vào List Old Schedule Menu Product
                TBL_SCHEDULE_MENU_SHIFT oldScheduleShiftMenu = DA_ScheduleMenuShift.Instance.GetById(item.ScheduleMenuShiftID);
                lstOldSchedule_Menu_Shift.Add(oldScheduleShiftMenu);

                DA_ScheduleMenuShift.Instance.Delete(item);
            }
            // Ghi log
            WriteLog("ScheduleMenuShift", "Delete", lstOldSchedule_Menu_Shift, null);

            return RedirectToAction("Index");
        }
        /// <summary>
        /// lấy number ở vị trí cuối cùng của chuỗi
        /// </summary>
        /// <returns></returns>
        private int getNumberProductName(string productName)
        {
            char[] charArray = productName.ToCharArray();
            Array.Reverse(charArray);
            string result ="";
            foreach(char item in charArray)
            {
                if(Char.IsDigit(item))
                {
                    result += item;
                }
                else { break; }
            }
            return Convert.ToInt32(result == "" ? "0" : result);
        }
        /// <summary>
        /// xuất báo cáo
        /// </summary>
        /// <returns></returns>
         [HttpPost]
        public JsonResult ExportExcel()
        {
            // dtToExcel = pr_GetListMealToomorrow(day, Month, Year);
            using (ExcelPackage pck = new ExcelPackage())
            {
                try
                {
                    var wsList = pck.Workbook.Worksheets.Add("DanhSachMonTheoNgay");
                    int ndong = dtToExcel.Rows.Count;
                    int nCot = dtToExcel.Columns.Count - 2; //do có hai cột không cần nên phải trừ ra

                    #region phần cứng (các cell ngoài data)
                    //Đỗ dữ liệu
                    wsList.Cells["A1"].LoadFromText("DANH SÁCH THỰC ĐƠN THEO NGÀY");
                    wsList.Cells["A2"].LoadFromText("NGÀY / THÁNG");
                    wsList.Cells["B2"].Value = FROMDATE;
                    wsList.Cells["B2"].Style.Numberformat.Format = "MM/yyyy";
                    //định dạnh style
                    wsList.Cells["A1"].Style.Font.Size = 14;
                    wsList.Cells[1, 1, 1, nCot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells["A1"].Style.Font.Bold = true;
                    wsList.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 1, 1, nCot].Merge = true;
                    wsList.Cells[2, 2, 2, nCot].Merge = true;
                    wsList.Cells[2, 2, 2, nCot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    #region Data
                    //đỗ dữ liệu vào datatable
                    wsList.Cells["A3"].LoadFromText("NGÀY");
                    wsList.Cells["B3"].LoadFromText("CA");

                    for (int i = 3; i < dtToExcel.Columns.Count -1 ; i++)
                    {
                        wsList.Cells[3,i].LoadFromText(DA_ProductType.Instance.getNameBaseId(getNumberProductName(dtToExcel.Columns[i+1].ColumnName)).ToUpper());
                    }
                    int index = 4;
                    for (int i = 0; i < dtToExcel.Rows.Count; i++)
                    {
                        //số dòng merger của cột ngày
                        int rowSpan = 0;
                        //đỗ dữ liệu cho các cột từ vị trí thứ 2
                        for (int j = 1; j < nCot; j++)
                        {
                            //do chỉ cần lấy col từ vị trí thứ 4 
                            string value = dtToExcel.Rows[i][j + 2].ToString();
                            String[] arrayValue = value.Split(',');
                            rowSpan = rowSpan > arrayValue.Length ? rowSpan : arrayValue.Length;
                            if (arrayValue.Length > 0)
                            {
                                for (int n = 0; n < arrayValue.Length; n++)
                                {
                                    wsList.Cells[index + i + n, j + 1].LoadFromText(arrayValue[n]);
                                }
                            }
                            else
                            {
                                wsList.Cells[index + i, j + 1].LoadFromText(value);
                            }
                        }
                        wsList.Cells[index + i, 1].Style.Numberformat.Format = "dd/MM/yyyy";
                        wsList.Cells[index + i, 1].Value = Convert.ToDateTime(dtToExcel.Rows[i]["MealDate"].ToString());
                        //index chỉ là số dòng được độn lên bởi merger 
                        wsList.Cells[index + i, 1, (index += rowSpan - 1) + i, 1].Merge = true;
                        wsList.Cells[(index-(rowSpan - 1)) + i, 1, (index) + i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsList.Cells[(index - (rowSpan - 1)) + i, 1, (index) + i, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                    wsList.Cells[wsList.Dimension.Address].AutoFitColumns();
                    //style excel
                    wsList.Cells[3, 1, 3, nCot].Style.Font.Bold = true;
                    wsList.Cells[3, 1, 3, nCot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    //định dạnh width cho sheet
                    wsList.Column(1).Width = 20;
                    wsList.Column(2).Width = 12;
                    wsList.Column(3).Width = 14;
                    wsList.Column(4).Width = 14;
                    wsList.Column(5).Width = 14;
                    wsList.Column(6).Width = 14;
                    wsList.Column(7).Width = 14;
                    wsList.Column(8).Width = 14;
                    wsList.Column(9).Width = 14;

                    //định danh font cho toàn sheet
                    //index là vị trí bắt đầu đổ dữ liệu table vào excel nhưng do có merger nên nó đã cộng những dòng phát sinh do merger 
                    //số dòng cuối cùng phải cộng cho số phần tử của table và trừ 1 
                    wsList.Cells[1, 1, index + ndong - 1, nCot].Style.Font.Name = "Tahoma";
                    wsList.Cells[1, 1, index + ndong - 1, nCot].Style.Font.Size = 11;
                    wsList.Cells[3, 1, index + ndong - 1, nCot].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, index + ndong - 1, nCot].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, index + ndong - 1, nCot].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 1, index + ndong - 1, nCot].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[3, 2, index + ndong - 1, nCot].Style.WrapText = true;

                    string path = Server.MapPath("/DanhSachMonTheoNgay.xlsx");
                    Stream stream = System.IO.File.Create(path);
                    pck.SaveAs(stream);
                    stream.Close();
                    return Json("true!/DanhSachMonTheoNgay.xlsx");
                }
                catch (Exception ex)
                {
                    return Json("false!" + ex.Message);
                }
            }
            
        }
    }
}
