using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServicePro.Startup))]
namespace ServicePro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
