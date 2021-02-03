using Autofac;
using KelloxPartnerData.KelloxPartnerNav;
using KelloxPartnerData.Repository;
using KelloxPartnerData.WebClient;
using KelloxPartnerWCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KelloxPartnerWCF.Autofac
{
    public class WebServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new KelloxPartnerWS())
                .InstancePerLifetimeScope();

            builder.Register(c => new WebClient(c.Resolve<KelloxPartnerWS>()))
                .As<IWebClient>()
                .InstancePerLifetimeScope();

            builder.Register(c => new KelloxPartnerRepository(c.Resolve<IWebClient>()))
                .As<IKelloxPartnerRepository>()
                .InstancePerLifetimeScope();

            //builder.Register(c => new KelloxPartnerRepository(c.Resolve<KelloxPartnerWS>()))
            //    .As<IKelloxPartnerRepository>()
            //    .InstancePerLifetimeScope();

            builder.Register(c => new KelloxPartnerService(c.Resolve<IKelloxPartnerRepository>()))
                .As<IKelloxPartnerService>()
                .InstancePerLifetimeScope();
        }
    }
}