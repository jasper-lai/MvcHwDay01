using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcHwDay01.Startup))]
namespace MvcHwDay01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
