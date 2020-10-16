﻿using Autofac;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;

namespace KelloxPartnerWCF
{
    public class ServiceHostInitializer : ServiceHostInitializerBase<KelloxPartnerService, IKelloxPartnerService>
    {

        public ServiceHostInitializer(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {            
        }

        protected override ServiceHost CreateServiceHost()
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            var strAdrHTTPS = ConfigurationManager.AppSettings["Url"];

            ServiceHost host = new ServiceHost(typeof(KelloxPartnerWCF.KelloxPartnerService), new Uri[] { new Uri(strAdrHTTPS) });

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = false;
            smb.HttpsGetEnabled = true;

            host.Description.Behaviors.Add(smb);
            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });

            BasicHttpBinding httpbs = new BasicHttpBinding();
            httpbs.Security.Mode = BasicHttpSecurityMode.Transport;
            httpbs.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            httpbs.MaxBufferSize = 1000000;
            httpbs.MaxReceivedMessageSize = 1000000;

            host.AddServiceEndpoint(typeof(KelloxPartnerWCF.IKelloxPartnerService), httpbs, strAdrHTTPS);
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpsBinding(), "mex");

            System.Diagnostics.EventLog.WriteEntry("KelloxServiceHost", "Kellox Partner Service starting at " + strAdrHTTPS);

            return host;
        }
    }
}