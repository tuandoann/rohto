function btnSaveUserClick() {
    debugger;
    if ($('#UserName').val() == "") {
        ShowError('Vui lòng nhập tên đăng nhập');
        $('#UserName').focus();
        return false;
    }
    if ($('#Password').val() == "") {
        ShowError('Vui lòng nhập mật khẩu');
        $('#Password').focus();
        return false;
    }
    if ($('#FullName').val() == "") {
        ShowError('Vui lòng nhập họ và tên');
        $('#FullName').focus();
        return false;
    }

    // kiem tra ten dang nhap trung:
    var UserName = $('#UserName').val();

    var flag = true;
    debugger;
    $.ajax({
        url: "/User/CheckUserNameNotExist",
        type: 'POST',
        data: {
            UserName: UserName
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Tên đăng nhập đã tồn tại, vui lòng nhập lại');
                $('#UserName').focus();
                flag = false;
            }
        },

        error: function (xhr) {
        }
    });

    return flag;
}

function btnEditSaveUserClick() {
    debugger;
    if ($('#UserName').val() == "") {
        ShowError('Vui lòng nhập tên đăng nhập');
        $('#UserName').focus();
        return false;
    }
    if ($('#FullName').val() == "") {
        ShowError('Vui lòng nhập họ và tên');
        $('#FullName').focus();
        return false;
    }

    // kiem tra ten dang nhap trung:
    var UserID = $("#UserID").val();
    var UserName = $('#UserName').val();

    var flag = true;
    $.ajax({
        url: "/User/CheckEditUserNameNotExist",
        type: 'POST',
        data: {
            UserID: UserID,
            UserName: UserName
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Tên đăng nhập đã tồn tại, vui lòng nhập lại');
                $('#UserName').focus();
                flag = false;
            }
        },

        error: function (xhr) {
        }
    });

    return flag;
}

