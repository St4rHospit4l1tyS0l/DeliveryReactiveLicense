using Autofac;

namespace Licensing.HostWebService
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<LoginSvc>().As<ILoginSvc>();
            //builder.RegisterType<SettingSvc>().As<ISettingSvc>();
            //builder.RegisterType<DebugLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            //builder.RegisterType<AccountService>().As<IAccountService>();
            //builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            //builder.RegisterType<AddressService>().As<IAddressService>();
            //builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            //builder.RegisterType<SettingService>().As<ISettingService>();
            //builder.RegisterType<SettingRepository>().As<ISettingRepository>();
            
            return builder.Build();
        }
    }
}