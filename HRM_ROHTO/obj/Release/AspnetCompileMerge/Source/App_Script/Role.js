$("#btnSave").click(function () {
    btnSaveRoleClick();
});

function btnSaveRoleClick() {
    if ($('#RoleName').val() == "") {
        ShowError('Vui lòng nhập tên vai trò');
        $('#RoleName').focus();
        return false;
    }

    var ArrayMain = [];
    var ArrayDetail = [];
    var item = {};
    var itemDetail;
    // kiem tra ten dang nhap trung:
    var RoleID = "";
    var RoleName = "";
    if ($('#RoleID').length) {
        RoleID = $('#RoleID').val();
    }
    RoleName = $('#RoleName').val();
    var flag = true;
    $.ajax({
        url: "/Role/CheckRoleNameIsNotExist",
        type: 'POST',
        data: {
            RoleID: RoleID, RoleName: RoleName
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Tên vai trò này đã tồn tài, vui lòng nhập lại');
                $('#RoleName').focus();
                flag = false;
            }
        },

        error: function (xhr) {
        }
    });

    if (flag == false)
    {
        return flag;
    }

    item["RoleID"] = RoleID;
    item["RoleName"] = RoleName;
    item["Note"] = $('#Note').val();

    $('#lv_list tbody tr').each(function () {
        var FunctionName = $(this).find('#hdFunctionName').val();
        var View = $(this).find('.ck_View')[0].checked;
        var Add = $(this).find('.ck_Add')[0].checked;
        var Edit = $(this).find('.ck_Edit')[0].checked;
        var Delete = $(this).find('.ck_Delete')[0].checked;
        var Print = $(this).find('.ck_Print')[0].checked;
        itemDetail = {};
        itemDetail["FunctionName"] = FunctionName;
        itemDetail["View"] = View;
        itemDetail["Add"] = Add;
        itemDetail["Edit"] = Edit;
        itemDetail["Delete"] = Delete;
        itemDetail["Print"] = Print;
        ArrayDetail.push(itemDetail);
    });
    item["ListDetail"] = ArrayDetail;
    ArrayMain.push(item);
    var json = JSON.stringify(ArrayMain);
   
    $.ajax({
        url: "/Role/SaveData",
        type: 'Post',
        data: {
            _Json: json,
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (data) {
            if (data != "-1") {
                window.location.href = '/Role';
            }
        },

        error: function (xhr) {
        }
    });
    return flag;
}

