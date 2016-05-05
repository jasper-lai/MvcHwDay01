using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHwDay01.Areas.Admin.Filters
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 主要針對不是指定的登入或角色, 使其無法執行其對應的功能
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            var result = new RedirectResult(urlHelper.Action("Index", new { Controller = "Home", Area = "" }));
            filterContext.Result = result;
        }
    }
}