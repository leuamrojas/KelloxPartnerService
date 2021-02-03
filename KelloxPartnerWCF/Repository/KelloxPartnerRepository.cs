using Infrastructure;
using KelloxPartnerWCF.KelloxPartnerNav;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace KelloxPartnerWCF.Repository
{
    public class KelloxPartnerRepository : IKelloxPartnerRepository
    {
        private KelloxPartnerWS _navClient;

        public KelloxPartnerRepository(KelloxPartnerWS navClient)
        {
            _navClient = navClient;
            _navClient.Url = ConfigurationManager.AppSettings["UrlNav"];
            _navClient.UseDefaultCredentials = true;
        }

        public string ReceiveOrder(string orderInputXml)
        {            
            Utils.SetXmlEncodingIBM865(ref orderInputXml);

            System.Diagnostics.EventLog.WriteEntry("KelloxPartnerService", "InputXml: " + orderInputXml);

            string outputXml = "";
            _navClient.ReceiveOrder(orderInputXml, ref outputXml);

            System.Diagnostics.EventLog.WriteEntry("KelloxPartnerService", "OutputXml: " + outputXml);

            return outputXml;
        }
    }
}