$(function () {
    $('#lv_list tbody').on('click', '#edit', function () {
        var data = table.row($(this).parents('tr')).data();
        window.location.href = window.location.pathname.replace("/Index", "") + "/Edit?ScheduleMenuID=" + data["ScheduleMenuID"];
    });
    var arrTr = $("#lv_list tbody tr");

    //$('#date_year').datetimepicker({
    //    viewMode: 'days',
    //    format: 'dd/MM/YYYY',
    //    showClear: true,
    //    showClose: true
    //});
});
$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}
var date = new Date();
//$("#date_year").val((date.getMonth() + 1) + "/" + date.getFullYear());
//$("#date_year").val(date.getFullYear() + "-" + ((date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "-" + ((date.getDate()) < 10 ? "0" + (date.getDate()) : (date.getDate())));
//if ($.urlParam('FromDate') != null && $.urlParam('ToDate') != null) {
//    var from = $.urlParam('FromDate');
//    var to = $.urlParam('ToDate');
//    $("#FromDate").val(from);
//    $("#ToDate").val(to);
//}
$("#FromDate").val(nowDate)
var nowDate = (date.getFullYear() + "-" + ((date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "-" + ((date.getDate()) < 10 ? "0" + (date.getDate()) : (date.getDate())));
$("#FromDate").val($.urlParam('FromDate') != null ? $.urlParam('FromDate') : nowDate);
$("#ToDate").val($.urlParam('ToDate') != null ? $.urlParam('ToDate') : nowDate);

function filterByMonthAndYear() {
    debugger;
    //ReloadPartialViewIndex();
    if ($("#date_year").val() == "") {
        ShowError("Vui lòng nhập ngày");
        $("#date_year").focus();
        return false;
    }
    else {
        var FromDate = $("#FromDate").val();
        var ToDate = $('#ToDate').val();
        location.href = "/ReportSoLuongPhanComCanThanhToan/Index?FromDate=" + FromDate + "&&ToDate=" + ToDate;
    }
}

function ReloadPartialViewIndex() {
    $('#partial_Invoice').load('/ReportSoLuongPhanComCanThanhToan/Index', $("#frmSearch").serializeArray());
}

function btnSearch_Click() {
    ReloadPartialViewIndex();
}

function btnExport_Click() {
    $.ajax({
        url: '/ReportSoLuongPhanComCanThanhToan/ExportMealCard',
        type: 'post',
        datatype: 'json',
        data: $("#frmSearch").serializeArray(),
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError("Mã khách hàng đã tồn tại, vui lòng nhập lại");
                flag = false;
            }
            else {
                debugger;
                window.location.href = window.location.origin + (data);
            }
        }
    })
}