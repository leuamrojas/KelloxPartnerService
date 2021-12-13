using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml;

namespace KelloxPartnerWCF.Contracts
{
    [ServiceContract]
    public interface IKelloxPartnerService
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/ReceiveOrder",
            ResponseFormat = WebMessageFormat.Xml)]
        XmlElement ReceiveOrder(Stream dataStream);

        //[OperationContract]
        //[WebInvoke(
        //    Method = "POST",
        //    UriTemplate = "/ReceiveOrder",
        //    BodyStyle = WebMessageBodyStyle.Bare,
        //    RequestFormat = WebMessageFormat.Xml,
        //    ResponseFormat = WebMessageFormat.Xml)]
        //XmlElement ReceiveOrder(Stream orderInputXml);
    }
}
