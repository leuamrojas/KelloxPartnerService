using KelloxPartnerWCF.Contracts;
using KelloxPartnerWCF.Helpers;
using KelloxPartnerWCF.Inspectors;
using KelloxPartnerWCF.KelloxPartnerNav;
using System.IO;
using System.Xml;

namespace KelloxPartnerWCF.Services
{
    [LoggerMessageInspectorBehavior]    
    public class KelloxPartnerService : IKelloxPartnerService
    {
        private readonly KelloxPartnerWS _kelloxPartnerWS;

        public KelloxPartnerService(KelloxPartnerWS kelloxPartnerWS)
        {
            _kelloxPartnerWS = kelloxPartnerWS;
        }

        public XmlElement ReceiveOrder(Stream data)
        {
            var outputXml = "";
            _kelloxPartnerWS.ReceiveOrder(GetInputXml(data), ref outputXml);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(outputXml);

            return xmlDoc.DocumentElement;
        }

        private string GetInputXml(Stream data)
        {
            var reader = new StreamReader(data);
            var inputXml = reader.ReadToEnd();
            Utils.AddEncodingIBM865(ref inputXml);

            return inputXml;
        }
        
    }

}