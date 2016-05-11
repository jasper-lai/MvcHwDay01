
function ajaxFailedFunc(ajaxContext) {
    $.each(ajaxContext.responseJSON, (i, item) => {
        $("[data-valmsg-for='" + item.ClientId + "'").append(item.ErrorMessage);
    });
}

////[問題]: 放在這裡, 且在 _Layout.cshtml 的最末加入 script src; IE11 (in Windows 7) 無法展開日期選單.
////註: 在 Chrome / Firefox is OK
////註: 若放在 _Layout.cshtml, 則 Chrome / Firefox / IE11 都 OK
//$(function () {
//    $("[data-datetimepicker]").datetimepicker({
//        closeOnDateSelect: true,
//        format: 'Y-m-d',
//        timepicker: false,
//        //lang: 'zh-TW',
//        //formatDate: 'Y-m-d H:i',
//    });
//});
