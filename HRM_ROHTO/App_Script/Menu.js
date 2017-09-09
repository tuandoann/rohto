function btnSaveMenuClick() {
    debugger;
    if ($("#MenuName").val() == "") {
        ShowError("Vui lòng nhập tên menu");
        $("#MenuName").focus();
        return false;
    }

    var menuProductID = [];
    var menuShiftID = [];

    $("#lv_list_2 tbody tr").each(function () {
        var productID = $(this).find(".productID").text();
        menuProductID.push(productID);
    })

    $("#lv_list_3 tbody tr").each(function () {
        if ($(this).find(".chkShift").prop("checked") == true)
            menuShiftID.push($(this).find(".chkShift").val());
    })

    debugger;
    if (menuProductID.length == 0) {
        ShowError("Vui lòng chọn ít nhất 1 món ăn");
        return false;
    }

    if (menuShiftID.length == 0) {
        ShowError("Vui lòng chọn ít nhất 1 ca");
        return false;
    }

    var flag = true;
    var strMenuProductID = JSON.stringify(menuProductID);
    var strMenuShiftID = JSON.stringify(menuShiftID);
    var IsActive = $("#IsActive").prop('checked');

    $.ajax({
        "url": "/Menu/CheckMenuDayAndShiftIDExist",
        "type": "POST",
        "datatype": "json",
        async: false,
        cache: false,
        data: {
            MenuDay: $("#MenuDay").val(),
            menuShift: strMenuShiftID,
        },
        success: function (data) {
            debugger;
            if (data.data != "") {
                ShowError("Đã có ca " + data.data + " tồn tại trong ngày này, vui lòng chọn ca khác");
                flag = false;
            }
        }
    });

    if (flag == true) {
        $.ajax({
            "url": "/Menu/SaveCreateData",
            "type": "POST",
            "datatype": "json",
            async: true,
            cache: false,
            data: {
                MenuName: $("#MenuName").val(),
                MenuDay: $("#MenuDay").val(),
                IsActive: IsActive,
                menuProduct: strMenuProductID,
                menuShift: strMenuShiftID,
            },
            success: function (data) {
                if (data == "1")
                    location.href = "/Menu/Index";
            }
        })
    }

    return flag;
}

function btnSaveEditMenuClick() {
    if ($("#MenuName").val() == "") {
        ShowError("Vui lòng nhập tên menu");
        $("#MenuName").focus();
        return false;
    }

    var menuProductID = [];
    var menuShiftID = [];

    $("#lv_list_2 tbody tr").each(function () {
        var productID = $(this).find(".productID").text();
        menuProductID.push(productID);
    })

    $("#lv_list_3 tbody tr").each(function () {
        if ($(this).find(".chkShift").prop("checked") == true)
            menuShiftID.push($(this).find(".chkShift").val());
    })

    debugger;
    if (menuProductID.length == 0) {
        ShowError("Vui lòng chọn ít nhất 1 món ăn");
        return false;
    }

    if (menuShiftID.length == 0) {
        ShowError("Vui lòng chọn ít nhất 1 ca");
        return false;
    }

    var flag = true;
    var strMenuProductID = JSON.stringify(menuProductID);
    var strMenuShiftID = JSON.stringify(menuShiftID);

    $.ajax({
        "url": "/Menu/EditCheckMenuDayAndShiftIDExist",
        "type": "POST",
        "datatype": "json",
        async: false,
        cache: false,
        data: {
            MenuID: $("#MenuID").val(),
            MenuDay: $("#MenuDay").val(),
            menuShift: strMenuShiftID,
        },
        success: function (data) {
            debugger;
            if (data.data != "") {
                ShowError("Đã có ca " + data.data + " tồn tại trong ngày này, vui lòng chọn ca khác");
                flag = false;
            }
        }
    });

    if (flag == true) {
        $.ajax({
            "url": "/Menu/SaveEditData",
            "type": "POST",
            "datatype": "json",
            async: true,
            cache: false,
            data: {
                UserCreate: $("#UserCreate").val(),
                MenuID: $("#MenuID").val(),
                MenuName: $("#MenuName").val(),
                MenuDay: $("#MenuDay").val(),
                IsActive: $("#IsActive").prop('checked'),
                menuProduct: strMenuProductID,
                menuShift: strMenuShiftID,
            },
            success: function (data) {
                if (data == "1")
                    window.location.href = "/Menu/Index";
            }
        })
    }
    return flag;
}