using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcHwDay01
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //加上這行可以強制 url 小寫
            //所有 Url Helper, Html Helper 產生的 url, 都是小寫
            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "skilltreeCreate",
                url: "skilltree",
                defaults: new
                {
                    controller = "Bill",
                    action = "Create",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "MvcHwDay01.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    //一切還是回到 HomeController, 再按 Button 到欲執行的頁面
                    controller = "Home",    //Bill
                    action = "Index",      //Create
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "MvcHwDay01.Controllers" }
            );

        }
    }
}
