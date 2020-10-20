using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KelloxPartnerWCF.Repository
{
    public interface IKelloxPartnerRepository
    {
        string ReceiveOrder(string orderInputXml);
    }
}
