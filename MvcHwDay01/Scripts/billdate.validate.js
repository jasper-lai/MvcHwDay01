/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

// 第一個參數必須是伺服器端ValidationType的值;
// 第二個參數必須是伺服器端ValidationParameters.Add()的值
$.validator.unobtrusive.adapters.addBool("billdate");

// 增加處理 data-val-* 方法
// value：取得欄位輸入值
// element：取得input元素
// param：取得限制值
$.validator.addMethod("billdate", function (value, element, param) {
	if (value) {
	    var now = new Date();
	    var yyyy = now.getFullYear().toString();
	    var mm = (now.getMonth() + 1).toString(); // getMonth() is zero-based
	    var dd = now.getDate().toString();
	    var today = yyyy + "-" + (mm[1] ? mm : "0" + mm[0]) + "-" + (dd[1] ? dd : "0" + dd[0]); // padding
		var inputValue = value;
		var validateValue = today;
        if (inputValue > validateValue) {
            return false;
        }
    }
    return true;
});