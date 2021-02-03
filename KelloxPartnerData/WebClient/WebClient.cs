using Infrastructure;
using KelloxPartnerData.KelloxPartnerNav;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KelloxPartnerData.WebClient
{
    public class WebClient : IWebClient
    {
        private KelloxPartnerWS _kelloxPartnerWS;

        public WebClient(KelloxPartnerWS kelloxPartnerWS)
        {
            _kelloxPartnerWS = kelloxPartnerWS;
            _kelloxPartnerWS.Url = ConfigurationManager.AppSettings["UrlNav"];
            _kelloxPartnerWS.UseDefaultCredentials = true;
        }        

        public string ReceiveOrder(string orderInputXml)
        {
            Utils.SetXmlEncodingIBM865(ref orderInputXml);

            string outputXml = "";
            _kelloxPartnerWS.ReceiveOrder(orderInputXml, ref outputXml);

            return outputXml;
        }
    }
}
