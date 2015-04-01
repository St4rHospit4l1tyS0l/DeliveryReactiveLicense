using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeliveryReactiveLicensing.Startup))]
namespace DeliveryReactiveLicensing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
