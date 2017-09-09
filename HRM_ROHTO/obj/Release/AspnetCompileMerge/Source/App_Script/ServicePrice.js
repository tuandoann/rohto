function CheckBeforeSave() {
    if ($('#ZoneID').val() == "0" || $('#ZoneID').val() == "") {
        ShowError('Khu vực không được để trống');
        return false;
    }

    if ($('#ServiceID').val() == "0" || $('#ServiceID').val() == "") {
        ShowError('Dịch vụ không được để trống');
        return false;
    }

    if ($('#FromDate').val() == "") {
        ShowError('Từ ngày không được để trống');
        $('#FromDate').focus();
        return false;
    }

    if ($('#ToDate').val() != "") {
        var fdate = new Date($('#FromDate').val());
        var tdate = new Date($('#ToDate').val());
        if (tdate < fdate) {
            ShowError('Từ ngày không được lớn hơn đến ngày');
            return false;
        }

    }
    return true;
}

var $eventSelect = $(".czone");
$eventSelect.on("change", function (e) {
    var ZoneID = $(this).val();
    LoadComboboxServiceByZoneID(ZoneID);
});

function LoadComboboxServiceByZoneID(ZoneID) {
    $.ajax({
        url: "/Service/GetServiceForComboboxByZoneID",
        type: 'GET',
        data: {
            ZoneID: ZoneID
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data != "") {
                var appenddata1 = "";
                for (var i = 0; i < data.length; i++) {
                    appenddata1 += "<option value = '" + data[i].Value + "'>" + data[i].Text + " </option>";
                }
                $(".address3").val('');
                $(".address3").html('');
                $(".address3").append(appenddata1);
            }
            else {
                $(".address3").val('');
                $(".address3").html('');
            }
        },
        error: function (xhr) {
        }

    });
}