function CheckBeforeSave() {
    if ($('#DepartmentCode').val() == "") {
        ShowError('Vui lòng nhập mã phòng ban');
        $('#DepartmentCode').focus();
        return false;
    }

    if ($('#DepartmentName').val() == "") {
        ShowError('Vui lòng nhập tên phòng ban');
        $('#DepartmentName').focus();
        return false;
    }
        
    // kiem tra parent Department not is this Department
    var ID = $('#DepartmentID').val().length > 0 ? $('#DepartmentID').val() : "0";
    var ParentID = $('#ParentID').val();
    if (ID == ParentID) {
        ShowError('Cấp cha không hợp lệ');
        return false;
    }

    // check Department code not exist
    var flag = true;
    var Code = $('#DepartmentCode').val().trim();

    $.ajax({
        url: "/Department/CheckDepartmentCodeNotExist",
        type: 'GET',
        data: {
            DepartmentID: ID, DepartmentCode: Code
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
        ShowError('Mã phòng ban này đã tồn tại, vui lòng nhập lại');
        $('#DepartmentCode').focus();
    }
    return flag;
}

function CheckEditBeforeSave() {
    if ($('#DepartmentCode').val() == "") {
        ShowError('Vui lòng nhập mã phòng ban');
        $('#DepartmentCode').focus();
        return false;
    }

    if ($('#DepartmentName').val() == "") {
        ShowError('Vui lòng nhập tên phòng ban');
        $('#DepartmentName').focus();
        return false;
    }
    // kiem tra parent Department not is this Department
    var ID = $('#DepartmentID').val().length > 0 ? $('#DepartmentID').val() : "0";
    var ParentID = $('#ParentID').val();
    if (ID == ParentID) {
        ShowError('Cấp cha không hợp lệ');
        return false;
    }

    // check Department code not exist
    var flag = true;
    var Code = $('#DepartmentCode').val().trim();

    debugger;
    $.ajax({
        url: "/Department/CheckLevelID_Edit",
        type: 'POST',
        data: {
            deparmentID: ID,
            parentID: ParentID
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
        ShowError('Cấp cha không hợp lệ');
    }
    return flag;
}