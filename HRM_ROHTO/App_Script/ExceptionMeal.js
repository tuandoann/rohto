function btnSaveExceptionMealClick() {
    debugger;
    if ($("#ExceptionDate").val() == "") {
        ShowError("Vui lòng nhập ngày ");
        $("#ExceptionDate").focus();
        return false;
    }

    debugger;
    var date = $("#ExceptionDate").val();
    var flag = true;
    $.ajax({
        url: "/ExceptionMeal/CheckDateExist",
        type: "POST",
        datatype: "json",
        data: {
            Date: date,
        },
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError("Ngày đã tồn tại, vui lòng nhập lại");
                flag = false;
            }
        }
    })

    $("#lv_list tbody tr").each(function () {
        var ExceptionQty = $(this).find("#ExceptionQty").val();
        if (parseFloat(ExceptionQty) < 0 || ExceptionQty == "") {
            ShowError("Số lượng không hợp lệ, vui lòng nhập lại");
            flag = false;
        }
    })

    if (flag == true) {
        var lstShiftID = [];
        var lstExceptionQty = [];

        $("#lv_list tbody tr").each(function () {
            var shiftID = $(this).find("#ShiftID").val();
            lstShiftID.push(shiftID);
            var ExceptionQty = $(this).find("#ExceptionQty").val();
            lstExceptionQty.push(ExceptionQty);
        })

        debugger;
        var strShiftID = JSON.stringify(lstShiftID);
        var strExceptionQty = JSON.stringify(lstExceptionQty);
        $.ajax({
            "url": "/ExceptionMeal/SaveData",
            "type": "POST",
            "datatype": "json",
            async: true,
            cache: false,
            data: {
                ExceptionDate: date,
                lstShiftID: strShiftID,
                lstExceptionQty: strExceptionQty,
            },
            success: function (data) {
                if (data == "1")
                    window.location.href = "/ExceptionMeal";
            }
        })
    }

    return flag;
}

function btnEditSaveExceptionMealClick() {
    if ($("#ExceptionDate").val() == "") {
        ShowError("Vui lòng nhập ngày ");
        $("#ExceptionDate").focus();
        return false;
    }

    debugger;
    var date = $("#ExceptionDate").val();
    var flag = true;
    $.ajax({
        url: "/ExceptionMeal/CheckEditDateExist",
        type: "POST",
        datatype: "json",
        data: {
            ID: $("#ExceptionMealID").val(),
            Date: date,
        },
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError("Ngày đã tồn tại, vui lòng nhập lại");
                flag = false;
            }
        }
    })

    $("#lv_list tbody tr").each(function () {
        var ExceptionQty = $(this).find("#ExceptionQty").val();
        if (parseFloat(ExceptionQty) < 0 || ExceptionQty == "") {
            ShowError("Số lượng không hợp lệ, vui lòng nhập lại");
            flag = false;
        }
    })

    if (flag == true) {
        var lstShiftID = [];
        var lstExceptionQty = [];
        var lstExceptionQtyUsed = [];

        $("#lv_list tbody tr").each(function () {
            var shiftID = $(this).find("#ShiftID").val();
            lstShiftID.push(shiftID);
            var ExceptionQty = $(this).find("#ExceptionQty").val();
            lstExceptionQty.push(ExceptionQty);
            var ExceptionQtyUsed = $(this).find("#ExceptionQtyUsed").val();
            lstExceptionQtyUsed.push(ExceptionQtyUsed);
        })

        debugger;
        var strShiftID = JSON.stringify(lstShiftID);
        var strExceptionQty = JSON.stringify(lstExceptionQty);
        var strExceptionQtyUsed = JSON.stringify(lstExceptionQtyUsed);
        $.ajax({
            "url": "/ExceptionMeal/SaveEditData",
            "type": "POST",
            "datatype": "json",
            async: true,
            cache: false,
            data: {
                ExceptionMealID: $("#ExceptionMealID").val(),
                ExceptionDate: date,
                lstShiftID: strShiftID,
                lstExceptionQty: strExceptionQty,
                lstExceptionQtyUsed: strExceptionQtyUsed,
            },
            success: function (data) {
                if (data == "1")
                    window.location.href = "/ExceptionMeal";
            }
        })
    }

    return flag;
}