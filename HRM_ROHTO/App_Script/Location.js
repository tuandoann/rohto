function CheckBeforeSave() {
    if ($('#LocationCode').val() == "") {
        ShowError('Mã khu vực không được để trống');
        $('#LocationCode').focus();
        return false;
    }

    if ($('#LocationName').val() == "") {
        ShowError('Tên khu vực không được để trống');
        $('#LocationName').focus();
        return false;
    }
    return true;
}