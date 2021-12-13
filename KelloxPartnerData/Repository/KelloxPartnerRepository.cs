using KelloxPartnerData.WebClient;

namespace KelloxPartnerData.Repository
{
    public class KelloxPartnerRepository : IKelloxPartnerRepository
    {
        private IWebClient _webClient;

        public KelloxPartnerRepository(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public string ReceiveOrder(string orderInputXml)
        {
            return _webClient.ReceiveOrder(orderInputXml);
        }
    }
}
