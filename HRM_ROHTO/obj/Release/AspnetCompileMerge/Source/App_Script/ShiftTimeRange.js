function btnSaveShiftTimeRangeClick() {
    if ($("#BeginTime").val() == "") {
        ShowError("Nhập giờ bắt đầu");
        $("#BeginTime").focus();
        return false;
    }

    if ($("#EndTime").val() == "") {
        ShowError("Nhập giờ kết thúc");
        $("#EndTime").focus();
        return false;
    }

    if ($("#BeginTime").val() > $("#EndTime").val()) {
        ShowError("Giờ kết thúc không đươc nhỏ hơn giờ bắt đầu");
        $("#EndTime").focus();
        return false;
    }
}