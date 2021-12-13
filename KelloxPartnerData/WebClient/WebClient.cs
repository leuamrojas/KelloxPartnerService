using KelloxPartnerData.KelloxPartnerNav;
using System.Configuration;

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
            Utils.AddEncodingIBM865(ref orderInputXml);

            string outputXml = "";
            _kelloxPartnerWS.ReceiveOrder(orderInputXml, ref outputXml);

            return outputXml;
        }
    }
}
