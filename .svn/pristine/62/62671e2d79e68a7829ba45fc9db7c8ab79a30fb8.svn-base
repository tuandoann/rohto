using System;
using System.Collections.Generic;
using System.Text;

namespace HRM_ROHTO.Global
{
    public class Pagination
    {
        /// <summary>
        /// Thiết lập bao nhiêu mục cần chia nhỏ trong trang.
        /// </summary>
        public static int _pagination =10;
        /// <summary>
        /// Tống số dòng hiển thị trên lưới,
        /// </summary>
        public static int _rowPerPage = 10;


        /// <summary>
        /// Tạo phân trang.
        /// Có sử dụng Pagination.css
        /// </summary>
        /// <param name="CurrenPage"> trang đang được chọn hiện tại</param>
        /// <param name="totalRow">Tổng số Record trong database.</param>
        /// <param name="url">Url trang cần redirect</param>
        /// <param name="paraName">Tên của các tham số nếu có. (thường là các tham số của bộ lọc tìm kiếm)</param>
        /// <param name="paraValue">Giá trị của tham số. (thường là các tham số của bộ lọc tìm kiếm)</param>
        public static string fn_Pagination(int CurrenPage, int totalRow, string url, List<string> paraName, List<string> paraValue)
        {
            string tmp = "";
            if (paraName.Count > 0)
            {
                tmp += paraName[0] + "=" + paraValue[0];
            }
            for (int i = 1; i < paraName.Count; i++)
            {
                tmp += "&" + paraName[i] + "=" + paraValue[i];
            }
            if (tmp != "")
                tmp = "&" + tmp;
            string s = "";
            if (totalRow == 0 || totalRow <= _rowPerPage)
                return s;

            int totalPage = 1;//tổng số mục cần phân trang
            totalPage = totalRow % _pagination == 0 ? totalRow / _pagination : totalRow / _pagination + 1;
            int page = 1; //Tổng số Page cần dùng cho các mục phân trang.. mỗi Page = _pagination mục.
            page = totalPage % _pagination == 0 ? totalPage / _pagination : totalPage / _pagination + 1;
            int count = 1;
            if (totalPage <= _pagination)//Số lượng mục cần chia không quá 1 Page.
            {
                while (count <= totalPage)
                {
                    s += "<a href='" + url + "?page=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                    count++;
                }
            }
            else
            {
                if (CurrenPage <= _pagination)//các mục phân trang hiện tại đang nằm ở Page 1
                {
                    while (count <= _pagination)
                    {
                        s += "<a href='" + url + "?page=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                        count++;
                    }
                    if (totalPage > 1)//Có hơn _pagination cần phân trang nên làm thêm 2 button.để chuyển qua page mới.
                    {
                        s += "<a href='" + url + "?page=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
                else
                {
                    if (CurrenPage > (page - 1) * _pagination)// Các mục phân trang nằm ở Page cuối cùng
                    {
                        count = totalPage;
                        while (count > (page - 1) * _pagination)
                        {
                            s = @"<a href='" + url + "?page=" + count + tmp + "' " + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>" + s;
                            count--;
                        }
                        s = "<a href='" + url + "?page=" + count + tmp + "'>...</a>" + s;
                        s = "<a href='" + url + "?page=1" + tmp + "'><<&nbsp;</a>" + s;
                    }
                    else
                    {
                        int start = 1, end = _pagination, i = 1;
                        while (CurrenPage < start || CurrenPage > end)
                        {
                            start = (i - 1) * _pagination + 1;
                            end = i * _pagination;
                            i++;
                        }
                        count = start;
                        s += "<a href='" + url + "?page=1" + tmp + "'><<&nbsp;</a>";
                        s += "<a href='" + url + "?page=" + (count - 1) + tmp + "'>...</a>";
                        while (count <= end)
                        {
                            s += "<a href='" + url + "?page=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>";
                            count++;
                        }
                        s += "<a href='" + url + "?page=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// Tạo phân trang.
        /// Có sử dụng Pagination.css
        /// </summary>
        /// <param name="CurrenPage"> trang đang được chọn hiện tại</param>
        /// <param name="totalRow">Tổng số Record trong database.</param>
        /// <param name="url">Url trang cần redirect</param>
        /// <param name="paraName">Tên của các tham số nếu có. (thường là các tham số của bộ lọc tìm kiếm)</param>
        /// <param name="paraValue">Giá trị của tham số. (thường là các tham số của bộ lọc tìm kiếm)</param>
        /// <param name="prToPage">QueryString được gán theo số Page</param>
        public static string fn_Pagination(int CurrenPage, int totalRow, string url, List<string> paraName, List<string> paraValue, string prToPage)
        {
            string tmp = "";
            if (paraName.Count > 0)
            {
                tmp += paraName[0] + "=" + paraValue[0];
            }
            for (int i = 1; i < paraName.Count; i++)
            {
                tmp += "&" + paraName[i] + "=" + paraValue[i];
            }
            if (tmp != "")
                tmp = "&" + tmp;
            string s = "";
            if (totalRow == 0 || totalRow <= _rowPerPage)
                return s;

            int totalPage = 1;//tổng số mục cần phân trang
            totalPage = totalRow % _pagination == 0 ? totalRow / _pagination : totalRow / _pagination + 1;
            int page = 1; //Tổng số Page cần dùng cho các mục phân trang.. mỗi Page = _pagination mục.
            page = totalPage % _pagination == 0 ? totalPage / _pagination : totalPage / _pagination + 1;
            int count = 1;
            if (totalPage <= _pagination)//Số lượng mục cần chia không quá 1 Page.
            {
                while (count <= totalPage)
                {
                    s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                    count++;
                }
            }
            else
            {
                if (CurrenPage <= _pagination)//các mục phân trang hiện tại đang nằm ở Page 1
                {
                    while (count <= _pagination)
                    {
                        s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                        count++;
                    }
                    if (totalPage > 1)//Có hơn _pagination cần phân trang nên làm thêm 2 button.để chuyển qua page mới.
                    {
                        s += "<a href='" + url + "?page=" + "&" + prToPage + "=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + "&" + prToPage + "=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
                else
                {
                    if (CurrenPage > (page - 1) * _pagination)// Các mục phân trang nằm ở Page cuối cùng
                    {
                        count = totalPage;
                        while (count > (page - 1) * _pagination)
                        {
                            s = @"<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "' " + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>" + s;
                            count--;
                        }
                        s = "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'>...</a>" + s;
                        s = "<a href='" + url + "?page=1" + "&" + prToPage + "=1" + tmp + "'><<&nbsp;</a>" + s;
                    }
                    else
                    {
                        int start = 1, end = _pagination, i = 1;
                        while (CurrenPage < start || CurrenPage > end)
                        {
                            start = (i - 1) * _pagination + 1;
                            end = i * _pagination;
                            i++;
                        }
                        count = start;
                        s += "<a href='" + url + "?page=1" + "&" + prToPage + "=1" + tmp + "'><<&nbsp;</a>";
                        s += "<a href='" + url + "?page=" + (count - 1) + "&" + prToPage + "=" + (count - 1) + tmp + "'>...</a>";
                        while (count <= end)
                        {
                            s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>";
                            count++;
                        }
                        s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + totalPage + "&" + prToPage + "=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// Tạo phân trang.
        /// Có sử dụng Pagination.css
        /// </summary>
        /// <param name="CurrenPage"> trang đang được chọn hiện tại</param>
        /// <param name="RowPerPage">Số dòng trên 1 trang</param>
        /// <param name="totalRow">Tổng số Record trong database.</param>
        /// <param name="url">Url trang cần redirect</param>
        /// <param name="paraName">Tên của các tham số nếu có. (thường là các tham số của bộ lọc tìm kiếm)</param>
        /// <param name="paraValue">Giá trị của tham số. (thường là các tham số của bộ lọc tìm kiếm)</param>
        public static string fn_Pagination(int CurrenPage, int RowPerPage, int totalRow, string url, List<string> paraName, List<string> paraValue)
        {
            string tmp = "";
            if (paraName.Count > 0)
            {
                tmp += paraName[0] + "=" + paraValue[0];
            }
            for (int i = 1; i < paraName.Count; i++)
            {
                tmp += "&" + paraName[i] + "=" + paraValue[i];
            }
            if (tmp != "")
                tmp = "&" + tmp;
            string s = "";
            if (totalRow == 0 || totalRow <= RowPerPage)
                return s;

            int totalPage = 1;//tổng số mục cần phân trang
            totalPage = totalRow % RowPerPage == 0 ? totalRow / RowPerPage : totalRow / RowPerPage + 1;
            int page = 1; //Tổng số Page cần dùng cho các mục phân trang.. mỗi Page = _pagination mục.
            page = totalPage % RowPerPage == 0 ? totalPage / RowPerPage : totalPage / RowPerPage + 1;
            int count = 1;
            if (totalPage <= _pagination)//Số lượng mục cần chia không quá 1 Page.
            {
                while (count <= totalPage)
                {
                    s += "<a href='" + url + "?page=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                    count++;
                }
            }
            else
            {
                if (CurrenPage <= _pagination)//các mục phân trang hiện tại đang nằm ở Page 1
                {
                    while (count <= _pagination)
                    {
                        s += "<a href='" + url + "?page=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                        count++;
                    }
                    if (totalPage > 1)//Có hơn _pagination cần phân trang nên làm thêm 2 button.để chuyển qua page mới.
                    {
                        s += "<a href='" + url + "?page=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
                else
                {
                    if (CurrenPage > (page - 1) * _pagination)// Các mục phân trang nằm ở Page cuối cùng
                    {
                        count = totalPage;
                        while (count > (page - 1) * _pagination)
                        {
                            s = @"<a href='" + url + "?page=" + count + tmp + "' " + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>" + s;
                            count--;
                        }
                        s = "<a href='" + url + "?page=" + count + tmp + "'>...</a>" + s;
                        s = "<a href='" + url + "?page=1" + tmp + "'><<&nbsp;</a>" + s;
                    }
                    else
                    {
                        int start = 1, end = _pagination, i = 1;
                        while (CurrenPage < start || CurrenPage > end)
                        {
                            start = (i - 1) * _pagination + 1;
                            end = i * _pagination;
                            i++;
                        }
                        count = start;
                        s += "<a href='" + url + "?page=1" + tmp + "'><<&nbsp;</a>";
                        s += "<a href='" + url + "?page=" + (count - 1) + tmp + "'>...</a>";
                        while (count <= end)
                        {
                            s += "<a href='" + url + "?page=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>";
                            count++;
                        }
                        s += "<a href='" + url + "?page=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// Tạo phân trang.
        /// Có sử dụng Pagination.css
        /// </summary>
        /// <param name="CurrenPage"> trang đang được chọn hiện tại</param>
        /// <param name="RowPerPage">Số dòng trên 1 trang</param>
        /// <param name="totalRow">Tổng số Record trong database.</param>
        /// <param name="url">Url trang cần redirect</param>
        /// <param name="paraName">Tên của các tham số nếu có. (thường là các tham số của bộ lọc tìm kiếm)</param>
        /// <param name="paraValue">Giá trị của tham số. (thường là các tham số của bộ lọc tìm kiếm)</param>
        /// <param name="prToPage">QueryString được gán theo số Page</param>
        public static string fn_Pagination(int CurrenPage, int RowPerPage, int totalRow, string url, List<string> paraName, List<string> paraValue, string prToPage)
        {
            string tmp = "";
            if (paraName.Count > 0)
            {
                tmp += paraName[0] + "=" + paraValue[0];
            }
            for (int i = 1; i < paraName.Count; i++)
            {
                tmp += "&" + paraName[i] + "=" + paraValue[i];
            }
            if (tmp != "")
                tmp = "&" + tmp;
            string s = "";
            if (totalRow == 0 || totalRow <= RowPerPage)
                return s;

            int totalPage = 1;//tổng số mục cần phân trang
            totalPage = totalRow % RowPerPage == 0 ? totalRow / RowPerPage : totalRow / RowPerPage + 1;
            int page = 1; //Tổng số Page cần dùng cho các mục phân trang.. mỗi Page = _pagination mục.
            page = totalPage % RowPerPage == 0 ? totalPage / RowPerPage : totalPage / RowPerPage + 1;
            int count = 1;
            if (totalPage <= _pagination)//Số lượng mục cần chia không quá 1 Page.
            {
                while (count <= totalPage)
                {
                    s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                    count++;
                }
            }
            else
            {
                if (CurrenPage <= _pagination)//các mục phân trang hiện tại đang nằm ở Page 1
                {
                    while (count <= _pagination)
                    {
                        s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : " ") + ">" + count + "</a>";
                        count++;
                    }
                    if (totalPage > 1)//Có hơn _pagination cần phân trang nên làm thêm 2 button.để chuyển qua page mới.
                    {
                        s += "<a href='" + url + "?page=" + "&" + prToPage + "=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + "&" + prToPage + "=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
                else
                {
                    if (CurrenPage > (page - 1) * _pagination)// Các mục phân trang nằm ở Page cuối cùng
                    {
                        count = totalPage;
                        while (count > (page - 1) * _pagination)
                        {
                            s = @"<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "' " + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>" + s;
                            count--;
                        }
                        s = "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'>...</a>" + s;
                        s = "<a href='" + url + "?page=1" + "&" + prToPage + "=1" + tmp + "'><<&nbsp;</a>" + s;
                    }
                    else
                    {
                        int start = 1, end = _pagination, i = 1;
                        while (CurrenPage < start || CurrenPage > end)
                        {
                            start = (i - 1) * _pagination + 1;
                            end = i * _pagination;
                            i++;
                        }
                        count = start;
                        s += "<a href='" + url + "?page=1" + "&" + prToPage + "=1" + tmp + "'><<&nbsp;</a>";
                        s += "<a href='" + url + "?page=" + (count - 1) + "&" + prToPage + "=" + (count - 1) + tmp + "'>...</a>";
                        while (count <= end)
                        {
                            s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'" + (count == CurrenPage ? " class='active' " : "") + ">" + count + "</a>";
                            count++;
                        }
                        s += "<a href='" + url + "?page=" + count + "&" + prToPage + "=" + count + tmp + "'>...</a>";
                        s += "<a href='" + url + "?page=" + totalPage + "&" + prToPage + "=" + totalPage + tmp + "'>&nbsp;>></a>";
                    }
                }
            }
            return s;
        }

    }
}
