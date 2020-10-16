using KelloxPartnerWCF.KelloxPartnerNav;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace KelloxPartnerWCF
{
    public class KelloxPartnerRepository : IKelloxPartnerRepository
    {
        private KelloxPartnerWS _navClient;

        public KelloxPartnerRepository(KelloxPartnerWS navClient)
        {
            _navClient = navClient;
            _navClient.Url = ConfigurationManager.AppSettings["UrlNav"]; ;
            _navClient.UseDefaultCredentials = true;
        }

        public string ReceiveOrder(string orderInputXml)
        {
            string outputString = "";
            Utilities.SetXmlEncodingIBM865(ref orderInputXml);

            _navClient.ReceiveOrder(orderInputXml, ref outputString);

            return outputString;
        }
    }
}