using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KelloxPartnerData.Repository
{
    public interface IKelloxPartnerRepository
    {
        string ReceiveOrder(string orderInputXml);
    }
}
