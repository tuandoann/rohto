function btnSaveClick() { // remove format cua input number
    $('form').submit(function () {
        var form = $(this);
        $('form').find('input.isNumber').each(function () {
            var self = $(this);
            var v = self.autoNumeric('get');
            self.autoNumeric('destroy');
            self.val(v);
        });
    });
    return CheckBeforeSave();
}


var $eventSelect = $(".address1");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 2);
    for(var i = 3; i<=10; i++)
    {
        debugger;
        ClearAllDataOfComboboxChildAddress(i);
    }

});

var $eventSelect = $(".address2");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 3);
    for (var i = 4; i <= 10; i++) {
        ClearAllDataOfComboboxChildAddress(i);
    }

});

var $eventSelect = $(".address3");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 4);
    for (var i = 5; i <= 10; i++) {
        ClearAllDataOfComboboxChildAddress(i);
    }

});

var $eventSelect = $(".address4");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 5);
    for (var i = 6; i <= 10; i++) {
        ClearAllDataOfComboboxChildAddress(i);
    }

});

var $eventSelect = $(".address5");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 6);
    for (var i = 7; i <= 10; i++) {
        ClearAllDataOfComboboxChildAddress(i);
    }

});

var $eventSelect = $(".address6");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 7);
    for (var i = 8; i <= 10; i++) {
        ClearAllDataOfComboboxChildAddress(i);
    }

});

var $eventSelect = $(".address7");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 8);
    for (var i = 9; i <= 10; i++) {
        ClearAllDataOfComboboxChildAddress(i);
    }

});

var $eventSelect = $(".address8");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 9);
    ClearAllDataOfComboboxChildAddress(10);


});

var $eventSelect = $(".address9");
$eventSelect.on("change", function (e) {
    var AddressID = $(this).val();
    LoadComboboxAddress(AddressID, 10);

});

function LoadComboboxAddress(AddressID, LevelID) {
    $.ajax({
        url: "/Zone/LoadComboboxAddressByParentAndLevel",
        type: 'POST',
        data: {
            AddressID: AddressID, LevelID: LevelID
        },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Lưu thông tin không thành công');
                flag = false;
            }
            else {
                var appenddata1 = "";
                for (var i = 0; i < data.length; i++) {
                    appenddata1 += "<option value = '" + data[i].Value + "'>" + data[i].Text + " </option>";
                }
                $(".address" + LevelID).val('');
                $(".address" + LevelID).html('');
                $(".address" + LevelID).append(appenddata1);
            }
        },
        error: function (xhr) {
        }
    });
}

function ClearAllDataOfComboboxChildAddress(LevelID) {
    $(".address" + LevelID).val('');
    $(".address" + LevelID).html('');
}