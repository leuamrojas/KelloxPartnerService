using Autofac;
using KelloxPartnerHost.Config;
using KelloxPartnerWCF.Contracts;
using KelloxPartnerWCF.KelloxPartnerNav;
using KelloxPartnerWCF.Services;

namespace KelloxPartnerHost.Initializer
{
    public class ServiceBootstrapper
    {
        public static ContainerBuilder RegisterContainerBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            var kelloxWS = new KelloxPartnerWS
            {
                Url = ConfigurationHandler.Config.AppSettings.Settings["UrlNav"].Value.ToString(),
                UseDefaultCredentials = true
            };

            builder.Register(c => kelloxWS)
                .InstancePerLifetimeScope();

            builder.Register(c => new KelloxPartnerService(c.Resolve<KelloxPartnerWS>()))
                .As<IKelloxPartnerService>()
                .InstancePerLifetimeScope();

            return builder;
        }
    }
}
