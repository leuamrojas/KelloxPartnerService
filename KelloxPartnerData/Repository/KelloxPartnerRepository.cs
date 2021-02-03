using Infrastructure.Logging;
using KelloxPartnerData.WebClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
