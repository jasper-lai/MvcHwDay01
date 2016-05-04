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
