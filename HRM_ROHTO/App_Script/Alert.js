function ShowError(text) {
    $('#lblError').fadeIn();
    $('#lblError').html('');
    $('#lblInfo').html('');
    $('#lblSuccess').html('');
    var str = "<div class='alert alert-danger alert-dismissable' style='background-color: rgba(247, 71, 49, 0.62) !important;border-color: rgba(255, 0, 0, 0.05); padding: 5px; margin: 10px;'>"
    + text +
    "</div>";
    $('#lblError').html(str);
    $('#lblError').delay(4000).fadeOut(1000);
}

function ShowSuccess(text) {
    $('#lblSuccess').fadeIn();
    $('#lblError').html('');
    $('#lblInfo').html('');
    $('#lblSuccess').html('');
    var str = "<div class='alert alert-danger alert-dismissable' style='background-color: rgba(0, 166, 90, 0.6) !important;border-color: rgba(94, 195, 152, 0.52); padding: 5px; margin: 10px;'>"
    + text +
    "</div>";
    $('#lblSuccess').html(str);
    $('#lblSuccess').delay(4000).fadeOut(1000);
}

function ShowToastSuccess(text)
{
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    var $toast = toastr["success"](text, "");
    $toastlast = $toast;
}

function ShowToastError(text) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    var $toast = toastr["error"](text, "");
    $toastlast = $toast;
}

