function btnSaveEmployeeClick(btn) {
    if ($('#FullName').val() == "") {
        ShowError('Vui lòng nhập tên nhân viên');
        $('#FullName').focus();
        return false;
    }
    if ($("#IsLeaveOut").prop('checked')) {
        if ($("#LeaveDate").val() == "") {
            ShowError('Vui lòng nhập ngày nghỉ việc');
            $('#LeaveDate').focus();
            return false;
        }
    }

    var EmployeeCode = $("#EmployeeCode").val();
    if (EmployeeCode != null || EmployeeCode != "") {
        var flag = true;
        $.ajax({
            url: '/Employee/CheckEmployeeCodeIsNotExist',
            type: 'POST',
            data: {
                EmployeeCode: EmployeeCode
            },
            async: false,
            cache: false,
            success: function (data) {
                if (data == "-1") {
                    ShowError('Mã nhân viên này đã tồn tại, nui lòng nhập lại');
                    $("#EmployeeCode").focus();
                    flag = false;
                }
            }
        });

        debugger;
        var PositionID1 = $("#PositionID1").val();
        var PositionID2 = $("#PositionID2").val();
        var PositionID3 = $("#PositionID3").val();

        if ((PositionID1 != "-1" && PositionID2 != "-1") || (PositionID1 != "-1" && PositionID3 != "-1") || (PositionID2 != "-1" && PositionID3 != "-1") || (PositionID1 != "-1" && PositionID2 != "-1" && PositionID3 != "-1")) {

            if ((PositionID1 == PositionID2 || PositionID1 == PositionID3 || PositionID2 == PositionID3)) {
                ShowError('Vui lòng chọn chức vụ 1,2,3 phải khác nhau');
                $('#PositionID1').focus();
                return false;
            }
        }

        debugger;
        if (flag == true) {
            $.ajax({
                url: '/Employee/SaveData',
                type: 'POST',
                data: {
                    FullName: $("#FullName").val(),
                    EmployeeCode: $("#EmployeeCode").val(),
                    ContractNo: $("#ContractNo").val(),
                    CardNumber: $("#CardNumber").val(),
                    DepartmentID: $("#DepartmentID").val(),
                    PositionID1: PositionID1,
                    PositionID2: PositionID2,
                    PositionID3: PositionID3,
                    IsLeaveOut: $("#IsLeaveOut").val(),
                    LeaveDate: $("#LeaveDate").val(),
                    Remarks: $("#Remarks").val()
                },
                async: false,
                cache: false,
                success: function (data) {
                    debugger;
                    if (data == "1") {
                        ShowSuccess("Thêm nhân viên thành công");
                        flag = true;
                        if (btn == "saveContionuous") {
                            setTimeout(function () {
                                location.reload();
                            }, 1000);
                        }
                        else
                            window.location.href = "/Employee";
                    }
                }
            });
        }

        return flag;
    }
}

function btnSaveEditEmployeeClick() {
    debugger;
    if ($('#FullName').val() == "" || $('#FullName').val() == null) {
        ShowError('Vui lòng nhập tên nhân viên');
        $('#FullName').focus();
        return false;
    }
    if ($("#IsLeaveOut").prop('checked')) {
        if ($("#LeaveDate").val() == "") {
            ShowError('Vui lòng nhập ngày nghỉ việc');
            $('#LeaveDate').focus();
            return false;
        }
    }

    debugger;
    var PositionID1 = $("#PositionID1").val();
    var PositionID2 = $("#PositionID2").val();
    var PositionID3 = $("#PositionID3").val();

    if ((PositionID1 != "-1" && PositionID2 != "-1") || (PositionID1 != "-1" && PositionID3 != "-1") || (PositionID2 != "-1" && PositionID3 != "-1") || (PositionID1 != "-1" && PositionID2 != "-1" && PositionID3 != "-1")) {

        if ((PositionID1 == PositionID2 || PositionID1 == PositionID3 || PositionID2 == PositionID3)) {
            ShowError('Vui lòng chọn chức vụ 1,2,3 phải khác nhau');
            $('#PositionID1').focus();
            return false;
        }
    }

    var EmployeeCode = $("#EmployeeCode").val();
    if (EmployeeCode != null || EmployeeCode != "") {
        var EmployeeID = $("#EmployeeID").val();

        var flag = true;
        $.ajax({
            url: '/Employee/CheckEditEmployeeCodeHasExist',
            type: 'POST',
            data: {
                EmployeeID: EmployeeID,
                EmployeeCode: EmployeeCode
            },
            async: false,
            cache: false,
            success: function (data) {
                if (data == "-1") {
                    ShowError('Mã nhân viên này đã tồn tại, vui lòng nhập lại');
                    $("#EmployeeCode").focus();
                    flag = false;
                }
            }
        });
        return flag;
    }
}

function CheckIsLeaveOut() {
    if ($("#IsLeaveOut").prop('checked')) {
        $("#LeaveDate").attr("readonly", false);
    }
    else {
        $("#LeaveDate").attr("readonly", true);
        $("#LeaveDate").val(null);
    }
}

$("#IsLeaveOut").click(function () {
    CheckIsLeaveOut();
})

$(window).load(function () {
    CheckIsLeaveOut();
})

function CheckFileUpload() {
    debugger;
    var fileName = $('#fileUpload').val();
    var fileExtensions = fileName.split('.').pop();

    if (fileExtensions.toLowerCase() != "xlsx" && fileExtensions.toLowerCase() != "csv") {
        $("#error").text("Không đúng dịnh dạng file Excel (*.xlsx, *csv)");
        return false;
    }
    return true;
}

