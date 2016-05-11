using System.Web.Mvc;

namespace MvcHwDay01.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Admin_query",
                url: "skilltree/Admin/{year}/{month}",
                defaults: new
                {
                    action = "Index",
                    controller = "Bill",
                    year = UrlParameter.Optional,
                    month = UrlParameter.Optional
                }
            );

            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new
                {
                    action = "Index",
                    controller = "Bill",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}