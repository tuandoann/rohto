using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;
using HRM_ROHTO.Models.ListExt;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Globalization;
using System.Data.OleDb;
using System.Data;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HRM_ROHTO.Controllers
{
    public class EmployeeController : BaseController
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            if (!CheckPermission("Employee", 1))
                return RedirectToAction("Login", "Login", "Login");
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileUpload)
        {
            if (!CheckPermission("Employee", 1))
                return RedirectToAction("Login", "Login", "Login");

            string path = "";
            OleDbConnection connection = new OleDbConnection();
            DataTable dt;
            DataTable dt_2;

            try
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        path = Path.Combine(Server.MapPath(@"~/Content/Excel/"), fileUpload.FileName);
                        fileUpload.SaveAs(path);

                        string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                        connection.ConnectionString = excelConnectionString;
                        OleDbCommand command = new OleDbCommand("select * from [Employee$]", connection);
                        OleDbCommand command_2 = new OleDbCommand("select Distinct(EmployeeCode) from [Employee$]", connection);
                        connection.Open();
                        OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
                        oleAdapter.SelectCommand = command;

                        OleDbDataAdapter oleAdapter_2 = new OleDbDataAdapter();
                        oleAdapter_2.SelectCommand = command_2;

                        // Sao chép các dòng dữ liệu từ file excel vào Datatable
                        dt = new DataTable();
                        oleAdapter.FillSchema(dt, SchemaType.Source);
                        oleAdapter.Fill(dt);

                        dt_2 = new DataTable();
                        oleAdapter_2.FillSchema(dt_2, SchemaType.Source);
                        oleAdapter_2.Fill(dt_2);

                        // Đóng các kết nối lại
                        command.Dispose();
                        command = null;
                        oleAdapter.Dispose();
                        oleAdapter = null;

                        command_2.Dispose();
                        command_2 = null;
                        oleAdapter_2.Dispose();
                        oleAdapter_2 = null;

                        connection.Dispose();
                        connection = null;
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        bool Error = false;

                        #region Khai báo chuỗi gán lỗi
                        string CheckFullNameNull = "", CheckFullNameLenght = "";
                        string CheckEmployeeCodeExist = "", CheckEmployeeCodeLenght = "";
                        string CheckContractNoExist = "", CheckContractNoLenght = "";
                        string CheckCardNumberExist = "", CheckCardNumberLenght = "";
                        string CheckDepartmentCodeExist = "", CheckDepartmentCodeLenght = "";
                        string CheckPositionCodeExist_1 = "", CheckPositionCodeLenght_1 = "";
                        string CheckPositionCodeExist_2 = "", CheckPositionCodeLenght_2 = "";
                        string CheckPositionCodeExist_3 = "", CheckPositionCodeLenght_3 = "";
                        #endregion

                        #region Kiểm tra rỗng và độ dài chuỗi
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string FullName = dt.Rows[i][0].ToString();
                            string EmployeeCode = dt.Rows[i][1].ToString();
                            string ContractNo = dt.Rows[i][2].ToString();
                            string CardNumber = dt.Rows[i][3].ToString();
                            string DepartmentCode = dt.Rows[i][4].ToString();
                            string PositionCode1 = dt.Rows[i][5].ToString();
                            string PositionCode2 = dt.Rows[i][6].ToString();
                            string PositionCode3 = dt.Rows[i][7].ToString();

                            if (FullName != "" || EmployeeCode != "" || ContractNo != "" || CardNumber != "" || DepartmentCode != "" || PositionCode1 != "" || PositionCode2 != "" || PositionCode3 != "")
                            {
                                // Check FullName
                                if (string.IsNullOrEmpty(FullName))
                                {
                                    CheckFullNameNull += (i + 2) + ", ";
                                    Error = true;
                                }
                                if (FullName.Length > 200)
                                {
                                    CheckFullNameLenght += (i + 2) + ", ";
                                    Error = true;
                                }

                                //Check Employee Code
                                if (EmployeeCode != "")
                                {
                                    if (EmployeeCode.Length > 20)
                                    {
                                        CheckEmployeeCodeLenght += (i + 2) + ", ";
                                        Error = true;
                                    }
                                    if (!DA_Employee.Instance.CheckEmployeeCodeNotExist(EmployeeCode))
                                    {
                                        CheckEmployeeCodeExist += (i + 2) + ", ";
                                        Error = true;
                                    }
                                }

                                //Check ContractNo
                                if (ContractNo != "")
                                {
                                    if (ContractNo.Length > 20)
                                    {
                                        CheckContractNoLenght += (i + 2) + ", ";
                                        Error = true;
                                    }
                                    if (!DA_Employee.Instance.CheckEmployeeContractNoExist(ContractNo))
                                    {
                                        CheckContractNoExist += (i + 2) + ", ";
                                        Error = true;
                                    }
                                }

                                //Card Number
                                if (CardNumber != "")
                                {
                                    if (CardNumber.Length > 20)
                                    {
                                        CheckCardNumberLenght += (i + 2) + ", ";
                                        Error = true;
                                    }
                                }

                                //Check Department Code
                                if (DepartmentCode != "")
                                {
                                    if (DepartmentCode.Length > 20)
                                    {
                                        CheckDepartmentCodeLenght += (i + 2) + ", ";
                                        Error = true;
                                    }
                                    if (DA_Department.Instance.CheckDepartmentCodeExist(DepartmentCode))
                                    {
                                        CheckDepartmentCodeExist += (i + 2) + ", ";
                                        Error = true;
                                    }
                                }

                                //Check Position Code 1
                                if (PositionCode1 != "")
                                {
                                    if (PositionCode1.Length > 20)
                                    {
                                        CheckPositionCodeLenght_1 += (i + 2) + ", ";
                                        Error = true;
                                    }
                                    if (DA_Position.Instance.CheckPositionCodeNotExist(PositionCode1))
                                    {
                                        CheckPositionCodeExist_1 += (i + 2) + ", ";
                                        Error = true;
                                    }
                                }

                                //Check Position Code 2
                                if (PositionCode2 != "")
                                {
                                    if (PositionCode2.Length > 20)
                                    {
                                        CheckPositionCodeLenght_2 += (i + 2) + ", ";
                                        Error = true;
                                    }
                                    if (DA_Position.Instance.CheckPositionCodeNotExist(PositionCode2))
                                    {
                                        CheckPositionCodeExist_2 += (i + 2) + ", ";
                                        Error = true;
                                    }
                                }

                                //Check Position Code 3
                                if (PositionCode3 != "")
                                {
                                    if (PositionCode3.Length > 20)
                                    {
                                        CheckPositionCodeLenght_3 += (i + 2) + ", ";
                                        Error = true;
                                    }
                                    if (DA_Position.Instance.CheckPositionCodeNotExist(PositionCode3))
                                    {
                                        CheckPositionCodeExist_3 += (i + 2) + ", ";
                                        Error = true;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Kiểm tra Card Number trùng trong excel
                        if (dt.Rows.Count > dt_2.Rows.Count)
                        {
                            string lineSameCardNumber = "";
                            int count = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (dt.Rows[i]["CardNumber"].ToString() == dt.Rows[j]["CardNumber"].ToString() && (dt.Rows[i]["CardNumber"].ToString() != ""))
                                    {
                                        if (i != j)
                                        {
                                            lineSameCardNumber += (i + 2) + ", " + (j + 2) + ", ";
                                            count++;
                                        }
                                    }
                                }
                            }

                            if (count > 1)
                            {
                                CheckCardNumberExist += lineSameCardNumber;
                                Error = true;
                            }

                        }

                        #endregion

                        #region Gán Error vào Viewbag
                        if (Error == true)
                        {
                            ViewBag.errorDetails = "<div class='alert alert-danger alert-dismissable'>" +
 //Full Name
 (CheckFullNameNull != "" ? "<p>Vui lòng nhập Full Name dòng thứ: " + CheckFullNameNull + "</p>" : "") +
 (CheckFullNameLenght != "" ? "<p>Vui lòng nhập Full Name không vượt quá 200 ký tự dòng thứ: " + CheckFullNameLenght + "</p>" : "") +

 //Employee
 (CheckEmployeeCodeLenght != "" ? "<p>Vui lòng nhập Employee Code không vượt quá 20 ký tự dòng thứ: " + CheckEmployeeCodeLenght + "</p>" : "") +
 (CheckEmployeeCodeExist != "" ? "<p>Employee Code đã được sử dụng, vui lòng nhập lại dòng thứ: " + CheckEmployeeCodeExist + "</p>" : "") +

 //Contract
 (CheckContractNoLenght != "" ? "<p>Vui lòng nhập ContractNo không vượt quá 20 ký tự dòng thứ: " + CheckContractNoLenght + "</p>" : "") +
 (CheckContractNoExist != "" ? "<p>ContractNo đã được sử dụng, vui lòng nhập lại dòng thứ: " + CheckContractNoExist + "</p>" : "") +

 //Card Number
 (CheckCardNumberLenght != "" ? "<p>Vui lòng nhập Card Number không vượt quá 20 ký tự dòng thứ: " + CheckCardNumberLenght + "</p>" : "") +
 (CheckCardNumberExist != "" ? "<p>Card Number bị trùng nhau, vui lòng nhập lại dòng thứ: " + CheckCardNumberExist + "</p>" : "") +

 //Department
 (CheckDepartmentCodeLenght != "" ? "<p>Vui lòng nhập Department Code không vượt quá 20 ký tự dòng thứ: " + CheckDepartmentCodeLenght + "</p>" : "") +
 (CheckDepartmentCodeExist != "" ? "<p>Không tìm thấy Department Code, vui lòng nhập lại dòng thứ: " + CheckDepartmentCodeExist + "</p>" : "") +

  //Position 1
  (CheckPositionCodeLenght_1 != "" ? "<p>Vui lòng nhập Position Code 1 không vượt quá 20 ký tự dòng thứ: " + CheckPositionCodeLenght_1 + "</p>" : "") +
 (CheckPositionCodeExist_1 != "" ? "<p>Không tìm thấy Position Code 1, vui lòng nhập lại dòng thứ: " + CheckPositionCodeExist_1 + "</p>" : "") +

 //Position 2
 (CheckPositionCodeLenght_2 != "" ? "<p>Vui lòng nhập Position Code 2 không vượt quá 20 ký tự dòng thứ: " + CheckPositionCodeLenght_2 + "</p>" : "") +
 (CheckPositionCodeExist_2 != "" ? "<p>Không tìm thấy Position Code 2, vui lòng nhập lại dòng thứ: " + CheckPositionCodeExist_2 + "</p>" : "") +

 //Position 3
 (CheckPositionCodeLenght_3 != "" ? "<p>Vui lòng nhập Position Code 3 không vượt quá 20 ký tự dòng thứ: " + CheckPositionCodeLenght_3 + "</p>" : "") +
 (CheckPositionCodeExist_3 != "" ? "<p>Không tìm thấy Position Code 3, vui lòng nhập lại dòng thứ: " + CheckPositionCodeExist_3 + "</p>" : "") +

                                "</div>";
                        }
                        #endregion

                        #region Nhập data
                        if (Error == false)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string FullName = dt.Rows[i][0].ToString();
                                string EmployeeCode = dt.Rows[i][1].ToString();
                                string ContractNo = dt.Rows[i][2].ToString();
                                string CardNumber = dt.Rows[i][3].ToString();
                                string DepartmentCode = dt.Rows[i][4].ToString();
                                string PositionCode1 = dt.Rows[i][5].ToString();
                                string PositionCode2 = dt.Rows[i][6].ToString();
                                string PositionCode3 = dt.Rows[i][7].ToString();

                                if (FullName != "" || EmployeeCode != "" || ContractNo != "" || CardNumber != "" || DepartmentCode != "" || PositionCode1 != "" || PositionCode2 != "" || PositionCode3 != "")
                                {
                                    TBL_EMPLOYEE Employee = new TBL_EMPLOYEE();
                                    Employee.FullName = FullName == "" ? null : FullName;
                                    Employee.EmployeeCode = EmployeeCode == "" ? null : EmployeeCode;
                                    Employee.ContractNo = ContractNo == "" ? null : ContractNo;
                                    Employee.DepartmentID = DA_Department.Instance.GetDepartmentIDByCode(DepartmentCode);
                                    Employee.PositionID1 = DA_Position.Instance.GetPositionIDByCode(PositionCode1);
                                    Employee.PositionID2 = DA_Position.Instance.GetPositionIDByCode(PositionCode2);
                                    Employee.PositionID3 = DA_Position.Instance.GetPositionIDByCode(PositionCode3);
                                    Employee.IsLeaveOut = false;

                                    if (CardNumber == "" || DA_Employee.Instance.CheckEmployeeCardNumbertExist(CardNumber))
                                    {
                                        Employee.CardNumber = CardNumber == "" ? null : CardNumber;
                                        DA_Employee.Instance.Insert(Employee);
                                    }
                                    else
                                    {
                                        TBL_EMPLOYEE tbl_Employee = DA_Employee.Instance.GetAll().FirstOrDefault(x => x.CardNumber == CardNumber);
                                        Employee.CardNumber = CardNumber;
                                        Employee.EmployeeID = tbl_Employee.EmployeeID;
                                        tbl_Employee = Employee;

                                        DA_Employee.Instance.Update(tbl_Employee);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                return View();
            }
            catch
            {
                ViewBag.errorDetails = "<div class='alert alert-danger alert-dismissable'>File excel không hợp lệ, vui lòng kiểm tra lại hoặc có thể tại file mẫu tại đây <a href='~/Content/Excel/FileMau/01_Import_Product.xlsx'><span class='fa fa-cloud-download'></span></a></div>";
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
                if (path != "")
                {
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetAllEmployee()
        {
            IList<EmployeeExt> List = DA_Employee.Instance.GetEmployeeExt();
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CheckEmployeeCodeIsNotExist(string EmployeeCode)
        {
            return DA_Employee.Instance.CheckEmployeeCodeNotExist(EmployeeCode) == true ? "1" : "-1";
        }

        [HttpPost]
        public string CheckEditEmployeeCodeHasExist(int EmployeeID, string EmployeeCode)
        {
            return DA_Employee.Instance.CheckEditEmployeeCodeHasExist(EmployeeID, EmployeeCode) == true ? "1" : "-1";
        }

        public ActionResult Create()
        {
            GetComboBox();
            return View();
        }

        [HttpPost]
        public JsonResult SaveData(TBL_EMPLOYEE item)
        {
            string flag = "";
            try
            {
                item.IsLeaveOut = item.IsLeaveOut == true ? false : true;
                item.PositionID1 = item.PositionID1.ToString() == "-1" ? null : item.PositionID1;
                item.PositionID2 = item.PositionID2.ToString() == "-1" ? null : item.PositionID2;
                item.PositionID3 = item.PositionID3.ToString() == "-1" ? null : item.PositionID3;

                DA_Employee.Instance.Insert(item);

                //Ghi log
                WriteLog("Employee", "Create", null, item);

                flag = "1";
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                flag = "-1";
                throw;
            }
        }

        public void GetComboBox()
        {
            List<SelectListItem> listCboDepartment = DA_Department.Instance.GetComboboxDepartment();
            listCboDepartment.Add(new SelectListItem { Text = "Không chọn", Value = "-1", Selected = true });

            List<SelectListItem> listCboPosition = DA_Position.Instance.GetComboboxPosition();
            listCboPosition.Add(new SelectListItem { Text = "Không chọn", Value = "-1", Selected = true });

            List<SelectListItem> listCboPosition2 = DA_Position.Instance.GetComboboxPosition2();
            listCboPosition2.Add(new SelectListItem { Text = "Không chọn", Value = "-1", Selected = true });

            List<SelectListItem> listCboPosition3 = DA_Position.Instance.GetComboboxPosition3();
            listCboPosition3.Add(new SelectListItem { Text = "Không chọn", Value = "-1", Selected = true });

            ViewBag.ListDepartment = listCboDepartment;
            ViewBag.ListPosition = listCboPosition;
            ViewBag.ListPosition2 = listCboPosition2;
            ViewBag.ListPosition3 = listCboPosition3;
        }

        [HttpGet]
        public ActionResult Edit(int EmployeeID)
        {
            GetComboBox();
            TBL_EMPLOYEE cus = DA_Employee.Instance.GetById(EmployeeID);

            if (cus == null)
                return HttpNotFound();
            return View(cus);
        }

        [HttpPost]
        public ActionResult Edit(TBL_EMPLOYEE item)
        {
            try
            {
                DA_Employee.Instance.Update(item);

                //Ghi log
                TBL_EMPLOYEE oldEmployee = DA_Employee.Instance.GetById(item.EmployeeID);
                WriteLog("Employee", "Edit", oldEmployee, item);

                return RedirectToAction("Index");
            }
            catch
            {
                GetComboBox(); return View();
            }
        }


        [HttpPost]
        public JsonResult exportExcel(FormCollection fc)
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var wsList = pck.Workbook.Worksheets.Add("Danh sach nhan vien");

                    DataTable result_store = new DataTable();
                    result_store = DA_ScheduleMenu.Instance.Exec("pr_ExportEmployeeList");
                    int nCot = result_store.Columns.Count;
                    int nRow = result_store.Rows.Count;
                    int lastRow = nRow + 2;

                    //định dạng chung
                    //định danh font cho toàn sheet
                    wsList.Cells[1, 1, lastRow, nCot].Style.Font.Name = "Tahoma";
                    wsList.Cells[1, 1, lastRow, nCot].Style.Font.Size = 11;
                    wsList.Cells[2, 1, lastRow, nCot].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[2, 1, lastRow, nCot].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[2, 1, lastRow, nCot].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[2, 1, lastRow, nCot].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    #region phần cứng
                    //đỗ dữ liệu
                    wsList.Cells[1, 1].LoadFromText("DANH SÁCH NHÂN VIÊN");
                    //style
                    wsList.Cells[1, 1, 1, nCot].Merge = true;
                    wsList.Cells[1, 1, 1, nCot].Style.Font.Bold = true;
                    wsList.Cells[1, 1, 1, nCot].Style.Font.Size = 14;
                    wsList.Cells[1, 1, 1, nCot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[2, 1, 2, nCot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[2, 1, 2, nCot].Style.Font.Bold = true;
                    #endregion

                    #region dữ liệu
                    wsList.Cells[2, 1].LoadFromText("HỌ TÊN");
                    wsList.Cells[2, 2].LoadFromText("MÃ SỐ");
                    wsList.Cells[2, 3].LoadFromText("SỐ HĐ");
                    wsList.Cells[2, 4].LoadFromText("MÃ SỐ THẺ");
                    wsList.Cells[2, 5].LoadFromText("MÃ PHÒNG");
                    wsList.Cells[2, 6].LoadFromText("PHÒNG BAN");
                    wsList.Cells[2, 7].LoadFromText("VỊ TRÍ 1");
                    wsList.Cells[2, 8].LoadFromText("VỊ TRÍ 2");
                    wsList.Cells[2, 9].LoadFromText("VỊ TRÍ 3");
                    wsList.Cells[2, 1, 2, nCot].Style.Font.Bold = true;
                    wsList.Cells[2, 1, 2, nCot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[2, 1, 2, nCot].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    for (int i = 0; i < nRow;i++ )
                    {
                        int index = i+3;
                        wsList.Cells[index, 1].LoadFromText(result_store.Rows[i]["FullName"] != null ? result_store.Rows[i]["FullName"].ToString() : "");
                        wsList.Cells[index, 2].LoadFromText(result_store.Rows[i]["EmployeeCode"] != null ? result_store.Rows[i]["EmployeeCode"].ToString() : "");
                        wsList.Cells[index, 3].LoadFromText(result_store.Rows[i]["ContractNo"] != null ? result_store.Rows[i]["ContractNo"].ToString() : "");
                        wsList.Cells[index, 4].LoadFromText(result_store.Rows[i]["CardNumber"] != null ? result_store.Rows[i]["CardNumber"].ToString() : "");
                        wsList.Cells[index, 5].LoadFromText(result_store.Rows[i]["DepartmentCode"] != null ? result_store.Rows[i]["DepartmentCode"].ToString() : "");
                        wsList.Cells[index, 6].LoadFromText(result_store.Rows[i]["DepartmentName"] != null ? result_store.Rows[i]["DepartmentName"].ToString() : "");
                        wsList.Cells[index, 7].LoadFromText(result_store.Rows[i]["PositionName1"] !=null ? result_store.Rows[i]["PositionName1"].ToString() :"");
                        wsList.Cells[index, 8].LoadFromText(result_store.Rows[i]["PositionName2"] != null ? result_store.Rows[i]["PositionName2"].ToString() : "");
                        wsList.Cells[index, 9].LoadFromText(result_store.Rows[i]["PositionName3"] != null ? result_store.Rows[i]["PositionName3"].ToString() : "");
                    }
                    wsList.Cells[3, 2, lastRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[3, 2, lastRow, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    #endregion

                    //điều chỉnh chiều rộng của col
                    wsList.Column(1).Width = 25;
                    wsList.Column(2).Width = 9;
                    wsList.Column(3).Width = 9;
                    wsList.Column(4).Width = 12;
                    wsList.Column(5).Width = 13;
                    wsList.Column(6).Width = 20;
                    wsList.Column(7).Width = 14;
                    wsList.Column(8).Width = 14;
                    wsList.Column(9).Width = 14;

                    //chỉnh wraptext cho sheet
                    wsList.Cells[2, 1, 2, nCot].Style.WrapText = true;
                    wsList.Cells[3, 1, lastRow, 1].Style.WrapText = true;
                    wsList.Cells[3, 6, lastRow, 9].Style.WrapText = true;

                    string path = Server.MapPath("/DanhSachNhanVien.xlsx");
                    Stream stream = System.IO.File.Create(path);
                    pck.SaveAs(stream);
                    stream.Close();
                    return Json(true + "!/DanhSachNhanVien.xlsx");
                }
            }
            catch(Exception ex)
            {
                return Json(false + "!" + ex.Message);
            }
        }

        public JsonResult DeleteEmployee(long ID)
        {
            DA_Employee.Instance.Delete(ID);

            //Ghi log
            TBL_EMPLOYEE oldEmployee = DA_Employee.Instance.GetById(ID);
            WriteLog("Employee", "Delete", oldEmployee, null);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMultiEmployee(string lstEmployeeID)
        {
            var lstID = JArray.Parse(lstEmployeeID);
            List<TBL_EMPLOYEE> lstEmployee = new List<TBL_EMPLOYEE>();

            foreach (var item in lstID)
            {
                TBL_EMPLOYEE Employee = DA_Employee.Instance.GetById(int.Parse(item.ToString()));
                lstEmployee.Add(Employee);

                DA_Employee.Instance.Delete(int.Parse(item.ToString()));
            }

            //Ghi log
            WriteLog("Employee", "MultiDelete", lstEmployee, null);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
