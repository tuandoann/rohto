function CheckBeforeSave() { 
    if($('#FromTime').val() == "")
    {
        ShowError('Chưa nhập từ giờ');
        $('#FromTime').focus();
        return false;
    }

    if ($('#ToTime').val() == "") {
        ShowError('Chưa nhập đến giờ');
        $('#ToTime').focus();
        return false;
    }

    if ($('#SortOrder').val() == "") {
        ShowError('Chưa nhập thứ tự');
        $('#SortOrder').focus();
        return false;
    }
    return true;
}