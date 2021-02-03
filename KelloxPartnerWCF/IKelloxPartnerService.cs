using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KelloxPartnerWCF
{
    [ServiceContract]
    public interface IKelloxPartnerService
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/ReceiveOrder",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml)]
        XmlElement ReceiveOrder(Stream orderInputXml);
    }
}
