using Autofac;
using KelloxPartnerWCF.KelloxPartnerNav;
using KelloxPartnerWCF.Repository;
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

            builder.Register(c => new KelloxPartnerRepository(c.Resolve<KelloxPartnerWS>()))
                .As<IKelloxPartnerRepository>()
                .InstancePerLifetimeScope();

            builder.Register(c => new KelloxPartnerService(c.Resolve<IKelloxPartnerRepository>()))
                .As<IKelloxPartnerService>()
                .InstancePerLifetimeScope();
        }
    }
}