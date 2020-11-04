using KelloxPartnerWCF.Inspectors;
using KelloxPartnerWCF.KelloxPartnerNav;
using KelloxPartnerWCF.Repository;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Xml;

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

        public XmlElement ReceiveOrder(Stream data)
        {            
            StreamReader reader = new StreamReader(data);
            string inputXml = reader.ReadToEnd();

            string outputXml = _kelloxPartnerRepository.ReceiveOrder(inputXml);
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(outputXml);

            return xmlDoc.DocumentElement;
        }
        
    }

}