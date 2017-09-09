function CheckBeforeSave() {
    if ($('#CustomerID').val() == "0") {
        ShowError('Vui lòng chọn thông tin khách hàng');
        $('#CustomerID').focus();
        return false;
    }

    return true;
}

$("#btnSave").click(function () {
    btnSaveContractClick();
});

function btnSaveContractClick() {
    if (CheckBeforeSave() == false) {
        return;
    }
    else {
        var ArrayMain = [];
        var ArrayDetail = [];
        var item = {};
        var itemDetail;
        var ContractID = 0;
        if ($('#ContractID').val() != null) {
            ContractID = $('#ContractID').val();
        }
        item["ContractID"] = ContractID;
        item["ContractNo"] = $('#ContractNo').val();
        item["ContractDate"] = $('#ContractDate').val();
        item["CustomerID"] = $('#CustomerID').val();

        if ($('#tableDetail tbody tr').length > 1) {
            var i = 0;
            var flag = true;
            $('#tableDetail tbody tr').each(function () { // ko lay dong dau tien vi no la dong an
                if (i > 0) {
                    itemDetail = {};
                    itemDetail["ContractServiceID"] = $(this).find('.hdContractServiceID').val() == undefined ? "0" : $(this).find('.hdContractServiceID').val();
                    itemDetail["ServiceID"] = $(this).find('.hdServiceID').val();
                    itemDetail["DateActiveService"] = $(this).find('.hdServiceDate').val();
                    itemDetail["DeviceNo"] = $(this).find('.hdDevice').val();
                    itemDetail["Extent1"] = $(this).find('.hdExtent1').val();
                    itemDetail["Extent2"] = $(this).find('.hdExtent2').val();
                    itemDetail["Extent3"] = $(this).find('.hdExtent3').val();
                    itemDetail["Extent4"] = $(this).find('.hdExtent4').val();
                    itemDetail["Extent5"] = $(this).find('.hdExtent5').val();
                    ArrayDetail.push(itemDetail);
                }
                i++;
            });
        }
        else {
            ShowError("Hợp đồng phải có ít nhất 1 dịch vụ");
            return;
        }

        if (flag == false) {
            return false;
        }
        item["ListDetail"] = ArrayDetail;

        ArrayMain.push(item);

        var json = JSON.stringify(ArrayMain);

        $.ajax({
            url: "/Contract/SaveData",
            type: 'Post',
            data: {
                _Json: json,
            },
            dataType: "json",
            async: true,
            cache: false,
            success: function (data) {
                if (data != "-1") {
                    window.location.href = '/Contract';
                }
            },

            error: function (xhr) {
            }
        });
    }
}

var $eventSelect = $(".customer");
$eventSelect.on("change", function (e) {
    CreateContractCode();
});

function CreateContractCode() {
    var CustomerID = $('#CustomerID').val();
    var ContractDate = $('#ContractDate').val();

    var CustomerCode = "";
    var ContractDate = ContractDate.replace(/\//g, '');
    var date = new Date(ContractDate);
    var strDate = "";
    if (ContractDate != '') {
        strDate = "_" + (date.getDate().toString().length > 1 ? date.getDate() : '0' + date.getDate()) + ((date.getMonth() + 1).toString().length > 1 ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1)) + date.getFullYear().toString().substr(2, 2);
    }
    // lay ZoneCode
    $.ajax({
        url: "/Customer/GetCustomerCodeFromCustomerID",
        type: 'GET',
        data: {
            CustomerID: CustomerID
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                flag = false;
            }
            else {
                CustomerCode = data.replace(/\ /g, '');
            }
        },
        error: function (xhr) {
        }
    });

    $('#ContractNo').val(CustomerCode + strDate);
}

$("#ContractDate").focusout(function () {
    CreateContractCode();
});

function ShowPopup() {
    ClearModal();
    debugger;
    var CustomerID = $('#CustomerID').val();
    if (CustomerID == "0") {
        ShowError("Vui lòng chọn khách hàng trước khi chọn dịch vụ");
        return;
    }
    LoadComboboxServiceByCondition();
    $('#mdAdd').modal('show');
    return false;
}

function ClearModal() {
    $('#DT_ServiceID').val('');
    $('#DT_DateAcctive').val('');
    $('#DT_DeviceNo').val('');
    $('#DT_Extent1').val('');
    $('#DT_Extent2').val('');
    $('#DT_Extent3').val('');
    $('#DT_Extent4').val('');
    $('#DT_Extent5').val('');
    $('#hdrowEditIndex').val('');
    $('#DT_ServiceID').prop('disabled', false);
}

function LoadComboboxServiceByCondition() {
    var cbbService = $('#DT_ServiceID');
    // lay customerID:
    var CustomerID = $('#CustomerID').val();

    // lay danh sach cac service
    var lstServiceID = "";
    if ($('#tableDetail tbody tr').length > 1) {
        var i = 0;
        $('#tableDetail tbody tr').each(function () { // ko lay dong dau tien vi no la dong an
            if (i > 0) {
                lstServiceID += $(this).find('.hdServiceID').val() + ",";
            }
            i++;
        });
    }

    $.ajax({
        url: "/Contract/GetComboboxServiceByCondition",
        type: 'GET',
        data: {
            listServiceID: lstServiceID, CustomerID: CustomerID
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data != "") {
                var str = "<select>";
                for (var i = 0; i < data.length; i++) {
                    str += "<option value='" + data[i]["Value"] + "'>" + data[i]["Text"] + "</option>"
                }
                str += "</select>";
                cbbService.html('');
                cbbService.html(str);
            }
            else {
                cbbService.html('');
            }

        },
        error: function (xhr) {
        }
    });
}


function LoadComboboxServiceByConditionEdit(ServiceID) {
    var cbbService = $('#DT_ServiceID');
    $.ajax({
        url: "/Contract/GetComboboxServiceWhenEdit",
        type: 'GET',
        data: {
            ServiceID: ServiceID
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data != "") {
                var str = "<select>";
                for (var i = 0; i < data.length; i++) {
                    str += "<option value='" + data[i]["Value"] + "'>" + data[i]["Text"] + "</option>"
                }
                str += "</select>";
                cbbService.html('');
                cbbService.html(str);
            }

        },
        error: function (xhr) {
        }
    });
}

// Detail
function btnSaveDetail_Click() {
    if ($('#DT_DateAcctive').val() == "") {
        $('#DT_DateAcctive').focus();
        var str = "<div class='alert alert-danger alert-dismissable' style='background-color: rgba(247, 71, 49, 0.62) !important;border-color: rgba(255, 0, 0, 0.05); padding: 5px; margin: 10px;'>"
        + "Chưa chọn ngày" +
        "</div>";
        $('#lblErrorModal').fadeIn();
        $('#lblErrorModal').html(str);
        $('#lblErrorModal').delay(3000).fadeOut(1000);
        return false;
    }

    SaveModalToTable();
    $('#mdAdd').modal('hide');
}

function btnSaveAddDetail_Click() {

    if ($('#DT_DateAcctive').val() == "") {
        $('#DT_DateAcctive').focus();
        var str = "<div class='alert alert-danger alert-dismissable' style='background-color: rgba(247, 71, 49, 0.62) !important;border-color: rgba(255, 0, 0, 0.05); padding: 5px; margin: 10px;'>"
        + "Chưa chọn ngày" +
        "</div>";
        $('#lblErrorModal').fadeIn();
        $('#lblErrorModal').html(str);
        $('#lblErrorModal').delay(3000).fadeOut(1000);
        return false;
    }
    SaveModalToTable();
    LoadComboboxServiceByCondition();
}

function FormatDate(dateObject) {
    var d = new Date(dateObject);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date = day + "/" + month + "/" + year;

    return date;
};

function SaveModalToTable() {
    var str = "";
    var ServiceID = $('#DT_ServiceID').val();
    var ServiceName = $('#DT_ServiceID :selected').text();
    var ServiceDate = new Date($('#DT_DateAcctive').val());
    var Device = $('#DT_DeviceNo').val();
    var Extent1 = $('#DT_Extent1').val();
    var Extent2 = $('#DT_Extent2').val();
    var Extent3 = $('#DT_Extent3').val();
    var Extent4 = $('#DT_Extent4').val();
    var Extent5 = $('#DT_Extent5').val();

    if (Extent1.trim() != "") {
        Extent1 += "<br/>";
    }
    if (Extent2.trim() != "") {
        Extent2 += "<br/>";
    }
    if (Extent3.trim() != "") {
        Extent3 += "<br/>";
    }
    if (Extent4.trim() != "") {
        Extent4 += "<br/>";
    }
    var dServiceDate = "";
    debugger;
    if ($('#DT_DateAcctive').val() != "") {
        dServiceDate = FormatDate($('#DT_DateAcctive').val());//ServiceDate.getDate() + "/" + (ServiceDate.getMonth() + 1) + "/" + ServiceDate.getFullYear()
    }

    str += '<tr>'
           + '<td style="display:none;">'
                + '<input type="text" id="hdContractServiceID" name="hdContractServiceID" class="hdContractServiceID" value="' + 0 + '">'
                + '<input type="text" id="hdServiceID" name="hdServiceID" class="hdServiceID" value="' + ServiceID + '">'
                + '<input type="text" id="hdExtent1" name="hdExtent1" class="hdExtent1" value="' + $('#DT_Extent1').val() + '">'
                + '<input type="text" id="hdExtent2" name="hdExtent2" class="hdExtent2" value="' + $('#DT_Extent2').val() + '">'
                + '<input type="text" id="hdExtent3" name="hdExtent3" class="hdExtent3" value="' + $('#DT_Extent3').val() + '">'
                + '<input type="text" id="hdExtent4" name="hdExtent4" class="hdExtent4" value="' + $('#DT_Extent4').val() + '">'
                + '<input type="text" id="hdExtent5" name="hdExtent5" class="hdExtent5" value="' + $('#DT_Extent5').val() + '">'
                + '<input type="text" id="hdServiceDate" name="hdServiceDate" class="hdServiceDate" value="' + $('#DT_DateAcctive').val() + '">'
                + '<input type="text" id="hdDevice" name="hdDevice" class="hdDevice" value="' + Device + '">'
           + '</td>'
           + '     <td>' + ServiceName + '</td>'
           + '     <td style="text-align: center;">' + dServiceDate + '</td>'
           + '     <td><span>' + Device + '</span></td>'
           + '     <td>' + Extent1 + Extent2 + Extent3 + Extent4 + Extent5 + '</td>'
           + '     <td style="text-align:center;">'
           + '         <a class="href btn btn-primary btn-xs" href="#" id="edit" style="width:50px;" onclick="editRow(this);"><i class="fa fa-edit"></i> Sửa</a>'
           + '         <a class="href btn btn-danger btn-xs delete" href="#" id="delete" style="width: 50px;" onclick="removeRow(this);"><i class="fa fa-trash-o"></i> Xóa</a>'
           + '     </td>'
           + ' </tr>';

    var iRowEdit = $('#hdrowEditIndex').val();
    if (iRowEdit != "") {
        var i = parseInt(iRowEdit);
        $('#tableDetail > tbody > tr').eq(i).replaceWith(str);
        $('#hdrowEditIndex').val('');
    }
    else {
        $('#tableDetail>tbody>tr:last').after(str);
    }
    ClearModal();
    SetButtonDelete();
}

function removeRow(row) {
    $(row).parent().parent().remove();
    SetButtonDelete();
};

function editRow(row) {
    var ServiceID = $(row).parent().parent().find('.hdServiceID').val();
    var ServiceDate = $(row).parent().parent().find('.hdServiceDate').val();
    var Device = $(row).parent().parent().find('.hdDevice').val();
    var Extent1 = $(row).parent().parent().find('.hdExtent1').val();
    var Extent2 = $(row).parent().parent().find('.hdExtent2').val();
    var Extent3 = $(row).parent().parent().find('.hdExtent3').val();
    var Extent4 = $(row).parent().parent().find('.hdExtent4').val();
    var Extent5 = $(row).parent().parent().find('.hdExtent5').val();
    var CustomerID = $('#CustomerID').val();
    if (CustomerID == "0") {
        ShowError("Vui lòng chọn khách hàng trước khi chọn dịch vụ");
        return;
    }
    LoadComboboxServiceByConditionEdit(ServiceID);
    $('#DT_ServiceID').val(ServiceID);
    $('#DT_DateAcctive').val(ServiceDate);
    $('#DT_DeviceNo').val(Device);
    $('#DT_Extent1').val(Extent1);
    $('#DT_Extent2').val(Extent2);
    $('#DT_Extent3').val(Extent3);
    $('#DT_Extent4').val(Extent4);
    $('#DT_Extent5').val(Extent5);
    $('#hdrowEditIndex').val($(row).parent().parent().index());
    $('#DT_ServiceID').prop('disabled', true);
    $('#mdAdd').modal('show');
    return false;
};

function LoadDetailWhenEdit() {
    var ContractID = $('#ContractID').val();
    $.ajax({
        url: "/Contract/GetListDetailByMasterID",
        type: 'GET',
        data: {
            ID: ContractID,
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (data) {
            if (data != "") {
                DrawDetail(data);
                SetButtonDelete();

            }
        },

        error: function (xhr) {
        }
    });
}

function DrawDetail(Data) {
    var str = "";
    for (var i = 0; i < Data.length; i++) {

        var Extent1 = Data[i].Extent1;
        var Extent2 = Data[i].Extent2;
        var Extent3 = Data[i].Extent3;
        var Extent4 = Data[i].Extent4;
        var Extent5 = Data[i].Extent5;

        if (Extent1.trim() != "") {
            Extent1 += "<br/>";
        }
        if (Extent2.trim() != "") {
            Extent2 += "<br/>";
        }
        if (Extent3.trim() != "") {
            Extent3 += "<br/>";
        }
        if (Extent4.trim() != "") {
            Extent4 += "<br/>";
        }
        var dServiceDate = "";
        if (Data[i].DateActiveService != null) {
            var ServiceDate = new Date(parseInt(Data[i].DateActiveService.substr(6)));

            dServiceDate = FormatDate(ServiceDate);// ServiceDate.getDate() + "/" + (ServiceDate.getMonth() + 1) + "/" + ServiceDate.getFullYear()
        }

        str += '<tr>'
           + '<td style="display:none;">'
                + '<input type="text" id="hdContractServiceID" name="hdContractServiceID" class="hdContractServiceID" value="' + Data[i].ContractServiceID + '">'
                + '<input type="text" id="hdServiceID" name="hdServiceID" class="hdServiceID" value="' + Data[i].ServiceID + '">'
                + '<input type="text" id="hdSortOrder" name="hdSortOrder" class="hdSortOrder" value="' + Data[i].SortOrder + '">'
                + '<input type="text" id="hdExtent1" name="hdExtent1" class="hdExtent1" value="' + Data[i].Extent1 + '">'
                + '<input type="text" id="hdExtent2" name="hdExtent2" class="hdExtent2" value="' + Data[i].Extent2 + '">'
                + '<input type="text" id="hdExtent3" name="hdExtent3" class="hdExtent3" value="' + Data[i].Extent3 + '">'
                + '<input type="text" id="hdExtent4" name="hdExtent4" class="hdExtent4" value="' + Data[i].Extent4 + '">'
                + '<input type="text" id="hdExtent5" name="hdExtent5" class="hdExtent5" value="' + Data[i].Extent5 + '">'
                + '<input type="text" id="hdServiceDate" name="hdServiceDate" class="hdServiceDate" value="' + Data[i].ServiceDate + '">'
                + '<input type="text" id="hdDevice" name="hdDevice" class="hdDevice" value="' + Data[i].DeviceNo + '">'
           + '</td>'
           + '     <td>' + Data[i].ServiceName + '</td>'
           + '     <td style="text-align:center;">' + dServiceDate + '</td>'
           + '     <td><span>' + Data[i].DeviceNo + '</span></td>'
           + '     <td>' + Extent1 + Extent2 + Extent3 + Extent4 + Extent5 + '</td>'
           + '     <td style="text-align:center;">'
           + '         <a class="href btn btn-primary btn-xs" href="#" id="edit" style="width:50px;" onclick="editRow(this);"><i class="fa fa-edit"></i> Sửa</a>'
           + '         <a class="href btn btn-danger btn-xs delete" href="#" id="delete" style="width: 50px;" onclick="removeRow(this);"><i class="fa fa-trash-o"></i> Xóa</a>'
           + '     </td>'
           + ' </tr>';
    }
    $('#tableDetail>tbody>tr:last').after(str);
}

// cancel contract

function LoadDetailWhenCancel() {
    var ContractID = $('#ContractID').val();
    $.ajax({
        url: "/Contract/GetListDetailByMasterID",
        type: 'GET',
        data: {
            ID: ContractID,
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (data) {
            if (data != "") {
                DrawDetailCancel(data);

            }
        },

        error: function (xhr) {
        }
    });
}

function DrawDetailCancel(Data) {
    var str = "";
    for (var i = 0; i < Data.length; i++) {

        var Extent1 = Data[i].Extent1;
        var Extent2 = Data[i].Extent2;
        var Extent3 = Data[i].Extent3;
        var Extent4 = Data[i].Extent4;
        var Extent5 = Data[i].Extent5;

        if (Extent1.trim() != "") {
            Extent1 += "<br/>";
        }
        if (Extent2.trim() != "") {
            Extent2 += "<br/>";
        }
        if (Extent3.trim() != "") {
            Extent3 += "<br/>";
        }
        if (Extent4.trim() != "") {
            Extent4 += "<br/>";
        }
        var dServiceDate = "";
        if (Data[i].DateActiveService != null) {
            var ServiceDate = new Date(parseInt(Data[i].DateActiveService.substr(6)));

            dServiceDate = ServiceDate.getDate() + "/" + (ServiceDate.getMonth() + 1) + "/" + ServiceDate.getFullYear()
        }
        var btnCancel = ' ';
        if (i == Data.length - 1) {
            btnCancel = '<a class="href btn bg-purple btn-xs" href="#" id="edit" style="width:50px;" onclick="CancelService(' + Data[i].ContractServiceID + ');"><i class="fa fa-ban"></i> Hủy</a>';
        }

        str += '<tr>'
           + '<td style="display:none;">'
                + '<input type="text" id="hdContractServiceID" name="hdContractServiceID" class="hdContractServiceID" value="' + Data[i].ContractServiceID + '">'
                + '<input type="text" id="hdServiceID" name="hdServiceID" class="hdServiceID" value="' + Data[i].ServiceID + '">'
                + '<input type="text" id="hdExtent1" name="hdExtent1" class="hdExtent1" value="' + Data[i].Extent1 + '">'
                + '<input type="text" id="hdExtent2" name="hdExtent2" class="hdExtent2" value="' + Data[i].Extent2 + '">'
                + '<input type="text" id="hdExtent3" name="hdExtent3" class="hdExtent3" value="' + Data[i].Extent3 + '">'
                + '<input type="text" id="hdExtent4" name="hdExtent4" class="hdExtent4" value="' + Data[i].Extent4 + '">'
                + '<input type="text" id="hdExtent5" name="hdExtent5" class="hdExtent5" value="' + Data[i].Extent5 + '">'
                + '<input type="text" id="hdServiceDate" name="hdServiceDate" class="hdServiceDate" value="' + Data[i].ServiceDate + '">'
                + '<input type="text" id="hdDevice" name="hdDevice" class="hdDevice" value="' + Data[i].DeviceNo + '">'
           + '</td>'
           + '     <td>' + Data[i].ServiceName + '</td>'
           + '     <td style="text-align:center;">' + dServiceDate + '</td>'
           + '     <td><span>' + Data[i].DeviceNo + '</span></td>'
           + '     <td>' + Extent1 + Extent2 + Extent3 + Extent4 + Extent5 + '</td>'
           + '     <td style="text-align:center;">'
           //+ '         <input class="ckIsCancel minimal-red" name="ckIsCancel" type="checkbox">'
           + btnCancel
           + '     </td>'
           + ' </tr>';
    }
    $('#tableDetail>tbody>tr:last').after(str);



    $('#ckIsCancelAll').on('ifClicked', function (event) {
        var checked = !event.currentTarget.checked;
        var index = event.currentTarget.value;
        ckAllChange(checked, index);
    });
}

function ckAllChange(ck, index) {
    if (ck) {
        $(".ckIsCancel").prop("checked", true);
    }
    else {
        $(".ckIsCancel").prop('checked', false);
    }
    $(".ckIsCancel").iCheck('update');

}

function CancelService(csID) {

    $('#ContractServiceCancelID').val(csID);
    $('#mdCancel').modal('show');
    return false;
}

function btnServiceCancelOK_Click() {
    var CancelDate = $('#DateCancel').val();
    if (CancelDate == "") {

        var str = "<div class='alert alert-danger alert-dismissable' style='background-color: rgba(247, 71, 49, 0.62) !important;border-color: rgba(255, 0, 0, 0.05); padding: 5px; margin: 10px;'>"
                   + "Ngày hủy không được để trống" +
                   "</div>";
        $('#lblErrorModal').fadeIn();
        $('#lblErrorModal').html(str);
        $('#lblErrorModal').delay(3000).fadeOut(1000);
        return false;

        $('#CancelDate').focus();
        return false;
    }

    if (confirm("Bạn thật sự muốn hủy dịch vụ đang chọn?")) {
        var ContractServiceID = $('#ContractServiceCancelID').val();

        var contractID = $('#ContractID').val();
        $.ajax({
            url: "/Contract/CancelService2",
            type: 'Post',
            data: {
                ContractID: contractID, ContractServiceID: ContractServiceID, CancelDate: CancelDate
            },
            dataType: "json",
            async: true,
            cache: false,
            success: function (data) {
                if (data != "-1") {
                    debugger;
                    var rowCount = $('#tableDetail tbody tr').length;
                    for (var i = 0; i < rowCount - 1; i++) {
                        $('#tableDetail tbody tr:last').remove();
                    }
                    DrawDetailCancel(data);
                    $('#mdCancel').modal('hide');
                }
            },

            error: function (xhr) {
            }
        });
    }

}

$("#btnSaveCancel").click(function () {
    var ArrayMain = [];
    var ArrayDetail = [];
    var item = {};
    var itemDetail;
    var ContractID = 0;
    if ($('#ContractID').val() != null) {
        ContractID = $('#ContractID').val();
    }
    item["ContractID"] = ContractID;
    var flag = 0;
    if ($('#tableDetail tbody tr').length > 1) {
        var i = 0;
        $('#tableDetail tbody tr').each(function () { // ko lay dong dau tien vi no la dong an
            if (i > 0) {
                itemDetail = {};
                if ($(this).find('.ckIsCancel').iCheck('update')[0].checked == true) {
                    flag++;
                    itemDetail["ContractServiceID"] = $(this).find('.hdContractServiceID').val() == undefined ? "0" : $(this).find('.hdContractServiceID').val();
                    ArrayDetail.push(itemDetail);
                }
            }
            i++;
        });
    }
    if (flag == 0) {
        ShowError("Bạn chưa chọn dịch vụ muốn hủy");
        return false;
    }

    item["ListDetail"] = ArrayDetail;

    ArrayMain.push(item);

    var json = JSON.stringify(ArrayMain);

    $.ajax({
        url: "/Contract/CancelService",
        type: 'Post',
        data: {
            _Json: json,
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (data) {
            if (data != "-1") {
                window.location.href = '/Contract';
            }
        },

        error: function (xhr) {
        }
    });
});

function LoadDetailWhenViewCancel() {
    var ContractID = $('#ContractID').val();
    $.ajax({
        url: "/Contract/GetListDetailCancelByMasterID",
        type: 'GET',
        data: {
            ID: ContractID,
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (data) {
            if (data != "") {
                DrawDetailViewCancel(data);

            }
        },

        error: function (xhr) {
        }
    });
}

function DrawDetailViewCancel(Data) {
    var str = "";
    for (var i = 0; i < Data.length; i++) {

        var dServiceDate = "";
        if (Data[i].DateActiveService != null) {
            var ServiceDate = new Date(parseInt(Data[i].DateActiveService.substr(6)));
            dServiceDate = FormatDate(ServiceDate); //ServiceDate.getDate() + "/" + (ServiceDate.getMonth() + 1) + "/" + ServiceDate.getFullYear()
        }

        var dCancelDate = "";
        if (Data[i].DateCancelService != null) {
            var CancelDate = new Date(parseInt(Data[i].DateCancelService.substr(6)));
            dCancelDate = FormatDate(CancelDate);//CancelDate.getDate() + "/" + (CancelDate.getMonth() + 1) + "/" + CancelDate.getFullYear()
        }



        str += '<tr>'
           + '<td style="display:none;">'
                + '<input type="text" id="hdContractServiceID" name="hdContractServiceID" class="hdContractServiceID" value="' + Data[i].ContractServiceID + '">'
                + '<input type="text" id="hdServiceID" name="hdServiceID" class="hdServiceID" value="' + Data[i].ServiceID + '">'
                + '<input type="text" id="hdServiceDate" name="hdServiceDate" class="hdServiceDate" value="' + Data[i].ServiceDate + '">'
                + '<input type="text" id="hdDevice" name="hdDevice" class="hdDevice" value="' + Data[i].DeviceNo + '">'
           + '</td>'
           + '     <td>' + Data[i].ServiceName + '</td>'
           + '     <td style="text-align:center;">' + dServiceDate + '</td>'
           + '     <td><span>' + Data[i].DeviceNo + '</span></td>'
           + '     <td style="text-align:center;">' + dCancelDate + '</td>'
           + '     <td>' + Data[i].UserCancelName + '</td>'

           + ' </tr>';
    }
    $('#tableDetail>tbody>tr:last').after(str);
}


function SetButtonDelete() // chi hien thi button delete o dong cuoi cung
{
    debugger;
    $('.delete').css("display", "none");
    $('#tableDetail tr:last').find('#delete').css("display", "inline-block");
    CheckButtonDeleteCanShow();
}

function ShowPopupDetail(id) {
    $.ajax({
        url: "/Contract/GetListDetailByMasterID",
        type: 'GET',
        data: {
            ID: id,
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (data) {
            if (data != "") {
                DrawDetailWhenViewByPopup(data);
                $('#mdShowDetail').modal('show');
            }
        },

        error: function (xhr) {
        }
    });
}

function DrawDetailWhenViewByPopup(Data) {
    var str = "";
    for (var i = 0; i < Data.length; i++) {

        var Extent1 = Data[i].Extent1;
        var Extent2 = Data[i].Extent2;
        var Extent3 = Data[i].Extent3;
        var Extent4 = Data[i].Extent4;
        var Extent5 = Data[i].Extent5;

        if (Extent1.trim() != "") {
            Extent1 += "<br/>";
        }
        if (Extent2.trim() != "") {
            Extent2 += "<br/>";
        }
        if (Extent3.trim() != "") {
            Extent3 += "<br/>";
        }
        if (Extent4.trim() != "") {
            Extent4 += "<br/>";
        }
        var dServiceDate = "";
        if (Data[i].DateActiveService != null) {
            var ServiceDate = new Date(parseInt(Data[i].DateActiveService.substr(6)));

            dServiceDate = FormatDate(ServiceDate);// ServiceDate.getDate() + "/" + (ServiceDate.getMonth() + 1) + "/" + ServiceDate.getFullYear()
        }

        str += '<tr class=canremove>'
           + '<td style="display:none;">'
                + '<input type="text" id="hdContractServiceID" name="hdContractServiceID" class="hdContractServiceID" value="' + Data[i].ContractServiceID + '">'
                + '<input type="text" id="hdServiceID" name="hdServiceID" class="hdServiceID" value="' + Data[i].ServiceID + '">'
                + '<input type="text" id="hdExtent1" name="hdExtent1" class="hdExtent1" value="' + Data[i].Extent1 + '">'
                + '<input type="text" id="hdExtent2" name="hdExtent2" class="hdExtent2" value="' + Data[i].Extent2 + '">'
                + '<input type="text" id="hdExtent3" name="hdExtent3" class="hdExtent3" value="' + Data[i].Extent3 + '">'
                + '<input type="text" id="hdExtent4" name="hdExtent4" class="hdExtent4" value="' + Data[i].Extent4 + '">'
                + '<input type="text" id="hdExtent5" name="hdExtent5" class="hdExtent5" value="' + Data[i].Extent5 + '">'
                + '<input type="text" id="hdServiceDate" name="hdServiceDate" class="hdServiceDate" value="' + Data[i].ServiceDate + '">'
                + '<input type="text" id="hdDevice" name="hdDevice" class="hdDevice" value="' + Data[i].DeviceNo + '">'
           + '</td>'
           + '     <td>' + Data[i].ServiceName + '</td>'
           + '     <td style="text-align:center;">' + dServiceDate + '</td>'
           + '     <td><span>' + Data[i].DeviceNo + '</span></td>'
           + '     <td>' + Extent1 + Extent2 + Extent3 + Extent4 + Extent5 + '</td>'
           + ' </tr>';
    }
    $(".canremove").remove();
    $('#tableDetail>tbody>tr:last').after(str);
}

function CheckButtonDeleteCanShow() // neu dich vu co sort order =0 thi luon cho phep xoa
{
    $('#tableDetail > tbody  > tr').each(function () {
        // kiem tra xem sort order co = 0 ko?

        if ($(this).find('#hdSortOrder').length > 0) {
            if ($(this).find('#hdSortOrder').val() == 0) {
                $(this).find('#delete').css("display", "inline-block");
            }
        }
        else
        {
            // neu ko tim thay sort id thi ve server lay?
            var sID = $(this).find('#hdServiceID').val();
            $.ajax({
                url: "/Service/GetServiceOrderByServiceID",
                type: 'GET',
                data: {
                    ID: sID,
                },
                dataType: "json",
                async: false,
                cache: false,
                success: function (data) {
                    if (data == 0) {
                        $(this).find('#delete').css("display", "inline-block");
                    }
                },
                error: function (xhr) {
                }
            });
        }
    });
}