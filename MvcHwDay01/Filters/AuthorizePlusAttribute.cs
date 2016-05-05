using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHwDay01.Filters
{
    public class AuthorizePlusAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 主要針對 AJAX 的授權作處理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            ////如果要完全走自己的, 就把 base.OnAuthorization(...) remark 掉
            //base.OnAuthorization(filterContext);

            //支援 MVC5 新增的 AllowAnonymous
            var skipAuthorization =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute),
                    inherit: true);

            //有設定 AllowAnonymous 就跳過
            if (skipAuthorization)
            {
                return;
            }

            //驗證是否是授權的連線。
            if (filterContext.HttpContext.User.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }

            //驗證是否是授權的連線，並且是 AJAX 呼叫。
            if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ContentResult cr = new ContentResult();
                cr.Content = "<p style=\"color:Red;font-weight:bold;\">您尚未登入，無法新增資料！請先登入後再嘗試。</p>";
                filterContext.Result = cr;
            }

        }
    }
}