function btnSaveScheduleMenuClick() {
    debugger;   
    if ($("#From").val() == "") {
        ShowError("Nhập ngày bắt đầu");
        $("#From").focus();
        return false;
    }

    if ($("#To").val() == "") {
        ShowError("Nhập ngày kết thúc");
        $("#To").focus();
        return false;
    }

    var dt = new Date($("#From").val());
    var date = new Date();
    if (dt <= date) {
        ShowError("Ngày bắt đầu phải lớn hơn ngày hôm nay");
        $("#From").focus();
        return false;
    }

    if ($("#From").val() > $("#To").val()) {
        ShowError("Ngày kết thúc không đươc nhỏ hơn ngày bắt đầu");
        $("#To").focus();
        return false;
    }

    $.ajax({
        url: '/ScheduleMenu/CheckBeforeSave',
        type: 'post',
        datatype: 'json',
        data: {
            From: $("#From").val(),
            To: $("#To").val()
        },
        success: function (data) {
            debugger;
            var lst = JSON.stringify(data)
            var arr = lst.split('"');

            if (arr.length < 4) {
                debugger;
                $.ajax({
                    url: '/ScheduleMenu/SaveData',
                    type: 'post',
                    datatype: 'json',
                    data: {
                        From: $("#From").val(),
                        To: $("#To").val(),
                    },
                    success: function (data) {
                        location.href = "/ScheduleMenu/Index"
                    }
                })
            }

            else {
                var pattern = /Date\(([^)]+)\)/;
                var results = pattern.exec(arr[3]);
                var dt = new Date(parseFloat(results[1]));
                var from = (dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear());

                if (arr[5] != null) {
                    var pattern_1 = /Date\(([^)]+)\)/;
                    var results_1 = pattern_1.exec(arr[5]);
                    var dt_1 = new Date(parseFloat(results_1[1]));
                    var to = (dt_1.getDate() + "/" + (dt_1.getMonth() + 1) + "/" + dt_1.getFullYear());

                    if (from != "" || to != "") {
                        $("#lblInfo .alert").removeClass("sr-only");
                        $(".text-lblInfo").text(from == to ? 'ngày ' + from : 'từ ngày ' + from + 'đến ngày ' + to);
                    }
                }
                else {
                    if (from != "") {
                        $("#lblInfo .alert").removeClass("sr-only");
                        $(".text-lblInfo").text('ngày ' + from);
                    }
                }
            }
        }
    });
    return false;
}

$("#Update").on('click', function () {
    debugger;
    $.ajax({
        url: '/ScheduleMenu/SaveData',
        type: 'post',
        datatype: 'json',
        data: {
            From: $("#From").val(),
            To: $("#To").val(),
            update: "update"
        },
        success: function (data) {
            location.href = "/ScheduleMenu/Index"
        }
    })
})

$("#Cancel").on('click', function () {
    debugger;
    $.ajax({
        url: '/ScheduleMenu/SaveData',
        type: 'post',
        datatype: 'json',
        data: {
            From: $("#From").val(),
            To: $("#To").val(),
            cancel: "cancel"
        },
        success: function (data) {
            location.href = "/ScheduleMenu/Index"
        }
    })
})