function btnSaveSpecialCaseClick() {
    if ($("#BeginTime").val() == "") {
        ShowError("Vui lòng nhập ngày bắt đầu");
        $("#BeginTime").focus();
        return false;
    }
    
    if ($("#EndTime").val() == "") {
        ShowError("Vui lòng nhập ngày kết thúc");
        $("#EndTime").focus();
        return false;
    }

    debugger;
    if ($("#EndTime").val() != null) {
        var beginTime = new Date($("#BeginTime").val());
        var endTime = new Date($("#EndTime").val());
        if (beginTime > endTime) {
            ShowError("Vui lòng nhập ngày bắt đầu không được lớn hơn ngày kết thúc");
            return false;
        }
    }

    if ($("#Message").val() == "") {
        ShowError("Vui lòng nhập nội dung");
        $("#Message").focus();
        return false;
    }
}