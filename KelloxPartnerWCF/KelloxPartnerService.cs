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
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
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

        /// <summary>
        /// receive RAW XML data and send response
        /// </summary>
        /// <param name="data"></param>
        /// <returns>xml response</returns>
        //public Stream ReceiveOrder(Stream data)
        //{

        //    byte[] response = null;

        //    try
        //    {
        //        //get query string parameters
        //        NameValueCollection coll =
        //            WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
        //        //get the System from query string
        //        string sys = coll["System"].ToUpper();

        //        //read data
        //        StreamReader sr = new StreamReader(data);
        //        var request = sr.ReadToEnd();
        //        sr.Close();

        //        //TODO : do smth w.request
        //        response = System.Text.Encoding.Default.GetBytes(request);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    //send response
        //    return new MemoryStream(response);
        //}

        //public string ReceiveOrder(string orderInputXml)
        //{
        //    return _kelloxPartnerRepository.ReceiveOrder(orderInputXml);
        //}
    }

}