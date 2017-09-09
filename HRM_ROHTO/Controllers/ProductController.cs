using HRM_ROHTO.Models.BUS;
using HRM_ROHTO.Models.ListExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM_ROHTO.Models;
using System.IO;
using System.Text;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Collections;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace HRM_ROHTO.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            if (!CheckPermission("Product", 1))
                return RedirectToAction("Login", "Login", "Login");
            return View();
        }

        DataTable dt;
        DataTable dt_2;

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileUpload, IEnumerable<HttpPostedFileBase> fileUploadAll)
        {
            if (!CheckPermission("Product", 1))
                return RedirectToAction("Login", "Login", "Login");

            #region FileUpload
            if (fileUpload != null)
            {
                string path = "";
                OleDbConnection connection = new OleDbConnection();

                try
                {
                    var file = Path.GetFileName(fileUpload.FileName);
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        path = Path.Combine(Server.MapPath(@"Content/Excel/"), file);

                        fileUpload.SaveAs(path);

                        string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                        connection.ConnectionString = excelConnectionString;
                        OleDbCommand command = new OleDbCommand("select * from [Product$]", connection);
                        OleDbCommand command_2 = new OleDbCommand("select Distinct(ProductCode) from [Product$]", connection);
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

                        ArrayList arr = new ArrayList();
                        string flag = "1";
                        string errorProductCodeNull = "";
                        string errorProductCodeExist = "";
                        string errorProductNameNull = "";
                        string errorProductNameLenght = "";
                        string errorProductTypeNull = "";
                        string errorProductTypeNotExist = "";
                        string errorDescriptionLenght = "";

                        //Kiểm tra null và độ dài
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (string.IsNullOrEmpty(dt.Rows[i][0].ToString().Trim()))
                            {
                                errorProductCodeNull += (i + 2) + ", ";
                                flag = "-1";
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i][1].ToString().Trim()))
                            {
                                errorProductNameNull += (i + 2) + ", ";
                                flag = "-1";
                            }
                            if (dt.Rows[i][1].ToString().Trim().Length > 150)
                            {
                                errorProductNameLenght += (i + 2) + ", ";
                                flag = "-1";
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i][2].ToString().Trim()))
                            {
                                errorProductTypeNull += (i + 2) + ", ";
                                flag = "-1";
                            }
                            if (dt.Rows[i][3].ToString().Trim().Length > 200)
                            {
                                errorDescriptionLenght += (i + 2) + ", ";
                                flag = "-1";
                            }
                        }
                        //------

                        //Kiểm tra trùng product code
                        if (dt.Rows.Count > dt_2.Rows.Count && flag == "1")
                        {
                            string lineSameProductCode = "";
                            int count = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (dt.Rows[i]["ProductCode"].ToString() == dt.Rows[j]["ProductCode"].ToString() && dt.Rows[i]["ProductCode"].ToString() != "")
                                    {
                                        if (i != j)
                                        {
                                            lineSameProductCode += (i + 2) + ", " + (j + 2) + ", ";
                                            arr.Add((i + 2) + "," + (j + 2));
                                            count++;
                                        }
                                    }
                                }
                            }

                            if (count > 1)
                            {
                                errorProductCodeExist += lineSameProductCode;
                                flag = "-1";
                            }
                        }
                        //------

                        //Kiểm tra Product Type
                        if (flag == "1")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                SYS_PRODUCT_TYPE ProductType = DA_ProductType.Instance.GetAll().FirstOrDefault(x => x.ProductTypeCode == dt.Rows[i][2].ToString());
                                if (ProductType == null)
                                {
                                    errorProductTypeNotExist += (i + 2) + ", ";
                                }
                            }
                        }
                        //------

                        if (flag == "-1")
                        {
                            ViewBag.errorDetails = "<div class='alert alert-danger alert-dismissable'>" +
        "<p>" + (errorProductCodeNull != "" ? "Mã món không được bỏ trống dòng thứ: " + errorProductCodeNull : "") + "</p>" +
        "<p>" + (errorProductCodeExist != "" ? "Trùng Product Code dòng thứ: " + errorProductCodeExist : "") + "</p>" +
        "<p>" + (errorProductNameNull != "" ? "Tên món không được bỏ trống dòng thứ: " + errorProductNameNull : "") + "</p>" +
        "<p>" + (errorProductNameLenght != "" ? "Tên món không được quá 150 ký tự dòng thứ: " + errorProductNameLenght : "") + "</p>" +
        "<p>" + (errorProductTypeNull != "" ? "Mã loại món không được bỏ trống dòng thứ: " + errorProductTypeNull : "") + "</p>" +
        "<p>" + (errorProductTypeNotExist != "" ? "Mã loại món không hợp lệ dòng thứ: " + errorProductTypeNotExist : "") + "</p>" +
        "<p>" + (errorDescriptionLenght != "" ? "Mô tả không được quá 200 ký tự dòng thứ: " + errorDescriptionLenght : "") + "</p>" +
                                                    "</div>";
                        }

                        if (flag == "1")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString().Trim()))
                                {
                                    int ProductTypeID = DA_ProductType.Instance.GetAll().FirstOrDefault(x => x.ProductTypeCode == dt.Rows[i][2].ToString()).ProductTypeID;

                                    TBL_PRODUCT Product = DA_Product.Instance.GetAll().FirstOrDefault(x => x.ProductCode == dt.Rows[i][0].ToString());

                                    TBL_PRODUCT ProductInsert = new TBL_PRODUCT();
                                    ProductInsert.ProductCode = dt.Rows[i][0].ToString();
                                    ProductInsert.ProductName = dt.Rows[i][1].ToString();
                                    ProductInsert.ProductTypeID = ProductTypeID;
                                    ProductInsert.Description = dt.Rows[i][3].ToString();
                                    ProductInsert.IsActive = true;

                                    if (Product == null)
                                    {
                                        DA_Product.Instance.Insert(ProductInsert);
                                    }
                                    else
                                    {
                                        ProductInsert.ProductID = Product.ProductID;
                                        DA_Product.Instance.Update(ProductInsert);
                                    }
                                }
                            }
                        }
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.errorDetails = "<div class='alert alert-danger alert-dismissable'> ("+ ex + ")File excel không hợp lệ, vui lòng kiểm tra lại hoặc có thể tại file mẫu tại đây <a href='Content/Excel/FileMau/01_Import_Product.xlsx'><span class='fa fa-cloud-download'></span></a></div>";
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                    if (path != "")
                    {
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    return View();
                }
            }
            #endregion

            #region FileUploadAll
            else if (fileUploadAll != null)
            {
                string error = "";
                foreach (var file in fileUploadAll)
                {

                    string[] folder = file.FileName.Split('/');
                    string fileName = folder[folder.Length - 1];
                    string code = folder[folder.Length - 1].Split('.')[0];
                    TBL_PRODUCT tblProduct = DA_Product.Instance.GetAll().FirstOrDefault(x => x.ProductCode == code);

                    if (tblProduct != null)
                    {
                        string path = Path.Combine(Server.MapPath(@"~/Content/Images/Product"), fileName);
                        file.SaveAs(path);
                        byte[] bytesImg = System.IO.File.ReadAllBytes(path);
                        tblProduct.ImageProduct = bytesImg;
                        DA_Product.Instance.Update(tblProduct);
                        System.IO.File.Delete(path);
                    }
                    else
                        error += file.FileName + ", ";
                }
                if (error != "")
                    ViewBag.errorDetails = @"<div class='alert alert-danger alert-dismissable'>
                                            Những hình ảnh sao đây không tìm thấy mã trong cơ sở dữ liệu:" + error +
                                        "</div>";
            }
            return View();
            #endregion
        }

        [HttpGet]
        public JsonResult GetAllProduct()
        {
            IList<ProductExt> List = DA_Product.Instance.GetProductExt().OrderBy(x => x.ProductName).ToList();
            return Json(new { data = List}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.ListProductType = DA_ProductType.Instance.GetComboboxProductType();
            return View();
        }

        [HttpPost]
        public JsonResult SaveData(TBL_PRODUCT item, FormCollection fc)
        {
            string flag = "";
            try
            {
                ViewBag.ListProductType = DA_ProductType.Instance.GetComboboxProductType();
                string a = fc["hdImage"];
                if (!string.IsNullOrEmpty(a))
                {
                    string[] lA = a.Split(',');
                    byte[] byteArray = Convert.FromBase64String(lA[1]);
                    //byteArray = Utils.Globals.Resize2Max50Kbytes(byteArray);
                    item.ImageProduct = byteArray;
                }
                DA_Product.Instance.Insert(item);

                // Ghi log
                WriteLog("Product", "Create", null, item);

                flag = "1";
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                flag = "-1";
                throw;
            }
        }

        [HttpPost]
        public string CheckProductCodeIsNotExist(string ProductCode)
        {
            return DA_Product.Instance.CheckProductCodeNotExist(ProductCode) == true ? "1" : "-1";
        }

        [HttpPost]
        public string CheckEditProductCodeHasExist(int ProductID, string ProductCode)
        {
            return DA_Product.Instance.CheckEditProductCodeHasExist(ProductID, ProductCode) == true ? "1" : "-1";
        }

        [HttpGet]
        public ActionResult Edit(int ProductID)
        {
            ViewBag.ListProductType = DA_ProductType.Instance.GetComboboxProductType();
            TBL_PRODUCT cus = DA_Product.Instance.GetById(ProductID);
            if (cus == null)
                return HttpNotFound();
            return View(cus);
        }

        [HttpPost]
        public ActionResult Edit(TBL_PRODUCT item, FormCollection fc)
        {
            try
            {
                ViewBag.ListProductType = DA_ProductType.Instance.GetComboboxProductType();
                string a = fc["hdImage"];
                if (!string.IsNullOrEmpty(a))
                {
                    string[] lA = a.Split(',');
                    byte[] byteArray = Convert.FromBase64String(lA[1]);
                    item.ImageProduct = byteArray;
                }
                DA_Product.Instance.Update(item);

                // Ghi log
                TBL_PRODUCT oldProduct = DA_Product.Instance.GetById(item.ProductID);
                WriteLog("Product", "Edit", oldProduct, item);

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }

        public JsonResult DeleteProduct(int ProductID)
        {
            bool flag = true;
            if (!DA_Product.Instance.CheckBeforeDelete(ProductID))
                flag = false;
            else
            {
                // Ghi log
                TBL_PRODUCT oldProduct = DA_Product.Instance.GetById(ProductID);
                WriteLog("Product", "Delete", oldProduct, null);

                DA_Product.Instance.Delete(ProductID);
                flag = true;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMultiProduct(string lstProductID)
        {
            bool flag = true;
            var lstID = JArray.Parse(lstProductID);
            foreach (var item in lstID)
            {
                if (!DA_Product.Instance.CheckBeforeDelete(int.Parse(item.ToString())))
                {
                    flag = false;
                    break;
                }
            }
            if (flag == true)
            {
                // Tạo List Old Product
                List<TBL_PRODUCT> lstOldProduct = new List<TBL_PRODUCT>();
                foreach (var item in lstID)
                {
                    // Add Vào List Old Product
                    TBL_PRODUCT oldProduct = DA_Product.Instance.GetById(int.Parse(item.ToString()));
                    lstOldProduct.Add(oldProduct);
                                     
                    DA_Product.Instance.Delete(int.Parse(item.ToString()));
                }
                // Ghi log
                WriteLog("Product", "MultiDelete", lstOldProduct, null);
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExportExcel()
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var ds = DA_Product.Instance.getPrductForExport();
                    var wsList = pck.Workbook.Worksheets.Add("DanhSachMonAn");
                    int ndong = ds.Count;
                    int lastRow = ndong + 2;
                    int nCol = ds.ElementAt(0).GetType().GetProperties().Count();

                    //định danh font cho toàn sheet
                    wsList.Cells[1, 1, lastRow, nCol].Style.Font.Name = "Tahoma";
                    wsList.Cells[1, 1, lastRow, nCol].Style.Font.Size = 11;
                    wsList.Cells[2, 1, lastRow, nCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[2, 1, lastRow, nCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[2, 1, lastRow, nCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsList.Cells[2, 1, lastRow, nCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    wsList.Cells[1, 1].LoadFromText("Danh sách món ăn");
                    wsList.Cells[2, 1].LoadFromText("STT");
                    wsList.Cells[2, 2].LoadFromText("Mã Món");
                    wsList.Cells[2, 3].LoadFromText("Tên món");
                    wsList.Cells[2, 4].LoadFromText("Loại");
                    wsList.Cells[2, 5].LoadFromText("Trạng thái");
                    wsList.Cells[2, 6].LoadFromText("Mô tả");
                    wsList.Cells[1, 1, 1, nCol].Merge = true;
                    wsList.Cells[1, 1, 1, nCol].Style.Font.Bold = true;
                    wsList.Cells[2, 1, 2, nCol].Style.Font.Bold = true;
                    wsList.Cells[2, 1, 2, nCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[2, 1, 2, nCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[1, 1, 1, nCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 1, 1, nCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    #region đỗ dữ liệu
                    int indexRow = 2;
                    foreach (var item in ds)
                    {
                        ++indexRow;
                        int indexCol = 0;
                        foreach (var prop in item.GetType().GetProperties())
                        {
                            wsList.Cells[indexRow, ++indexCol].LoadFromText((prop.Name == "STT") ? (indexRow - 2).ToString() : (prop.GetValue(item, null) == null ? "" : prop.GetValue(item, null)).ToString());
                        }
                    }
                    #endregion

                    //định dạnh width cho sheet
                    wsList.Column(1).Width = 5;
                    wsList.Column(2).Width = 10;
                    wsList.Column(3).Width = 22;
                    wsList.Column(4).Width = 15;
                    wsList.Column(5).Width = 15;
                    wsList.Column(6).Width = 20;

                    wsList.Cells[3, 6, lastRow, 6].Style.WrapText = true;
                    wsList.Cells[3, 3, lastRow, 3].Style.WrapText = true;
                    wsList.Cells[3, 4, lastRow, 4].Style.WrapText = true;

                    wsList.Cells[1, 1, lastRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 1, lastRow, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[1, 2, lastRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 2, lastRow, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[1, 4, lastRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 4, lastRow, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsList.Cells[1, 5, lastRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsList.Cells[1, 5, lastRow, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    string path = Server.MapPath("/DanhSachMonAn.xlsx");
                    Stream stream = System.IO.File.Create(path);
                    pck.SaveAs(stream);
                    stream.Close();
                    return Json("/DanhSachMonAn.xlsx");
                }
            }
            catch (Exception ex)
            {
                return Json("/DanhSachMonAn.xlsx");
            }
        }
    }
}
