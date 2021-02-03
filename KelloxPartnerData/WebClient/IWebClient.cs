using KelloxPartnerData.KelloxPartnerNav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KelloxPartnerData.WebClient
{
    public interface IWebClient
    {
        string ReceiveOrder(string orderInputXml);
    }
}
