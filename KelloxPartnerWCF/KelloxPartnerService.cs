using KelloxPartnerWCF.Inspectors;
using KelloxPartnerWCF.KelloxPartnerNav;
using KelloxPartnerWCF.Repository;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace KelloxPartnerWCF
{
    [LoggerMessageInspectorBehavior]
    public class KelloxPartnerService : IKelloxPartnerService
    {
        private readonly IKelloxPartnerRepository _kelloxPartnerRepository;

        public KelloxPartnerService(IKelloxPartnerRepository kelloxPartnerRepository)
        {
            _kelloxPartnerRepository = kelloxPartnerRepository;
        }

        public string ReceiveOrder(string orderInputXml)
        {
            return _kelloxPartnerRepository.ReceiveOrder(orderInputXml);
        }
    }
}