using Autofac;
using Licensing.WebService.Proxy;

namespace Licensing.HostWebService
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LicProxySvc>().As<ILicProxySvc>();
            //builder.RegisterType<SettingService>().As<ISettingService>();
            //builder.RegisterType<SettingRepository>().As<ISettingRepository>();
            
            return builder.Build();
        }
    }
}