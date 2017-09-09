function btnSavePositionClick() {
    debugger;
    if ($('#PositionCode').val() == "") {
        ShowError('Vui lòng nhập mã chức vụ');
        $('#PositionCode').focus();
        return false;
    }
    if ($('#PositionName').val() == "") {
        ShowError('Vui lòng nhập tên chức vụ');
        $('#PositionName').focus();
        return false;
    }

    var PositionCode;
    if ($("#PositionCode").length) {
        PositionCode = $("#PositionCode").val();
    }
    var flag = true;
    $.ajax({
        url: '/Position/CheckPositionCodeIsNotExist',
        type: 'POST',
        data: {
            PositionCode: PositionCode
        },
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Mã chức vụ này đã tồn tại, vui lòng nhập lại');
                $("#PositionCode").focus();
                flag = false;
            }
        }
    });
    return flag;
}

function btnSaveEditPositionClick() {
    debugger;
    if ($('#PositionCode').val() == "") {
        ShowError('Vui lòng nhập mã chức vụ');
        $('#PositionCode').focus();
        return false;
    }
    if ($('#PositionName').val() == "") {
        ShowError('Vui lòng nhập tên chức vụ');
        $('#PositionName').focus();
        return false;
    }

    var PositionCode = $("#PositionCode").val();
    var PositionID = $("#PositionID").val();

    var flag = true;
    $.ajax({
        url: '/Position/CheckEditPositionCodeHasExist',
        type: 'POST',
        data: {
            PositionID: PositionID,
            PositionCode: PositionCode
        },
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Mã chức vụ này đã tồn tại, vui lòng nhập lại');
                $("#PositionCode").focus();
                flag = false;
            }
        }
    });
    return flag;
}