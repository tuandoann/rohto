function btnSaveSpecialCaseClick() {
    if ($("#SpecialDate").val() == "") {
        ShowError("Vui lòng nhập ngày");
        $("#SpecialDate").focus();
        return false;
    }
    var url = $("#SpecialID").val() == "" ? "actionCreate" : "actionEdit";
    $.ajax({
        url: '/SpecialCase/' + url,
        type: 'post',
        datatype: 'json',
        data: {
            'SpecialID' : $("#SpecialID").val(),
            'SpecialDate': $("#SpecialDate").val(),
            'Notes': $("#Notes").val(),
            'Quantity': $("#Quantity").val(),
        },
        async: false,
        cache: false,
        success: function (data) {
            switch(data)
            {
                //thành công
                case 1:
                    window.location.href = 'Index';
                    break;
                case 0:
                    ShowError("Xuất báo cáo thất bại");
                    flag = false;
                    break;
                case -1:
                    ShowError("Vui lòng chọn ngày khác.");
                    $("#SpecialDate").focus();
                    flag = false;
                    break;
            }
        }
    })
}