function CheckBeforeSave() {
    if ($('#ServiceCode').val() == "") {
        ShowError('Mã dịch vụ không được để trống');
        $('#ServiceCode').focus();
        return false;
    }

    if ($('#ServiceName').val() == "") {
        ShowError('Tên dịch vụ không được để trống');
        $('#ServiceName').focus();
        return false;
    }

    if ($('#LevelID').val() == "") {
        ShowError('Cấp dịch vụ không được để trống');
        $('#LevelID').focus();
        return false;
    }

    // check address code not exist
    var flag = true;
    var Code = $('#ServiceCode').val().trim();
    var ID = $('#ServiceID').length > 0 ? $('#ServiceID').val() : "0";
    $.ajax({
        url: "/Service/CheckServiceCodeNotExist",
        type: 'GET',
        data: {
            ID: ID, Code: Code
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data != "1") {
                flag = false;
            }
        },

        error: function (xhr) {
        }
    });
    if (flag == false) {
        ShowError('Mã dịch vụ đã tồn tại');
        $('#ZoneCode').focus();
    }

    return flag;
}