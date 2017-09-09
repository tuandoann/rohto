function btnSaveProductClick(btn) {
    var ImgSrc = document.getElementById("profileImage").getAttribute('src');
    document.getElementById("hdImage").value = ImgSrc;
    debugger;
    if ($('#ProductCode').val() == "") {
        ShowError('Vui lòng nhập mã món ăn');
        $('#ProductCode').focus();
        return false;
    }

    if ($('#ProductName').val() == "") {
        ShowError('Vui lòng nhập tên món ăn');
        $('#ProductName').focus();
        return false;
    }

    var ProductCode = $("#ProductCode").val();
    var flag = true;
    $.ajax({
        url: '/Product/CheckProductCodeIsNotExist',
        type: 'POST',
        data: {
            ProductCode: ProductCode
        },
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Mã món ăn này đã tồn tại, vui lòng nhập lại');
                $("#ProductCode").focus();
                flag = false;
            }
        }
    });

    debugger;
    if (flag == true) {
        $.ajax({
            url: '/Product/SaveData',
            type: 'POST',
            data: {
                ProductCode: $("#ProductCode").val(),
                ProductName: $("#ProductName").val(),
                ProductTypeID: $("#ProductTypeID").val(),
                Description: $("#Description").val(),
                IsActive: $("#IsActive").val(),
                hdImage: $("#hdImage").val()
            },
            async: false,
            cache: false,
            success: function (data) {
                if (data == "1") {
                    ShowSuccess("Thêm món ăn thành công");
                    flag = true;
                    if (btn == "btnSaveContionuous") {
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    }
                    else
                        window.location.href = "/Product";
                }
            }
        });
    }

    return flag;
}

function btnSaveEditProductClick() {
    var ImgSrc = document.getElementById("profileImage").getAttribute('src');
    document.getElementById("hdImage").value = ImgSrc;
    debugger;
    if ($('#ProductCode').val() == "") {
        ShowError('Vui lòng nhập mã món ăn');
        $('#ProductCode').focus();
        return false;
    }

    if ($('#ProductName').val() == "") {
        ShowError('Nhập tên món ăn');
        $('#ProductName').focus();
        return false;
    }

    var ProductID = $("#ProductID").val();
    var ProductCode = $("#ProductCode").val();

    var flag = true;
    $.ajax({
        url: '/Product/CheckEditProductCodeHasExist',
        type: 'POST',
        data: {
            ProductID: ProductID,
            ProductCode: ProductCode
        },
        async: false,
        cache: false,
        success: function (data) {
            if (data == "-1") {
                ShowError('Mã món ăn này đã tồn tại, vui lòng nhập lại');
                $("#ProductCode").focus();
                flag = false;
            }
        }
    });
    return flag;
}

function readURL(input) {
    debugger;
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#profileImage').attr('src', e.target.result);
        }
        //alert("e.target.result");
        reader.readAsDataURL(input.files[0]);
    }
    else {
        $('#profileImage').removeAttr('src');
        $("#FileUpload").val(null)
    }
}

$("#removeImg").click(function () {
    $("#hdImage").val(null);
    readURL(false)
})

$("#FileUpload").change(function () {
    debugger;
    readURL(this);
});