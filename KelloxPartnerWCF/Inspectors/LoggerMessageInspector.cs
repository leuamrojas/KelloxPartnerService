using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Xml;

namespace KelloxPartnerWCF
{
    public class LoggerMessageInspector : IDispatchMessageInspector
    {   
        //private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();
            Message msg = buffer.CreateMessage();
            StringBuilder sb = new StringBuilder();
            using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(sb))
            {
                msg.WriteMessage(xw);
                xw.Close();
            }            

            //System.Diagnostics.EventLog.WriteEntry("KelloxPartnerService", "InputXml: " + System.Net.WebUtility.HtmlDecode(sb.ToString()));
            //_logger.Info(string.Format("Received: \n\n{0}\n\nClient IP address: {1}", System.Net.WebUtility.HtmlDecode(sb.ToString()), Utilities.TryToGetClientIpAddress()));            
            
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();

            CreateLogResponse(buffer.CreateMessage());
        }

        private void CreateLogResponse(Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();

            var xmlStr = System.Net.WebUtility.HtmlDecode(bodyReader.ReadOuterXml()).Replace(Constants.XmlDeclaration, "");
            xmlStr = Constants.XmlDeclaration + xmlStr;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            var orderNo = xmlDoc.DocumentElement.GetElementsByTagName("OrderNo").Item(0).InnerText;
            CreateIpAddressLog(orderNo);

            var strHttpCode = xmlDoc.DocumentElement.GetElementsByTagName("httpCode").Item(0).InnerText;

            int httpCode;
            if (Int32.TryParse(strHttpCode, out httpCode))
            {
                System.Diagnostics.EventLog.WriteEntry("KelloxPartnerService", "SetHttpResponseCode before: " + httpCode);
                CreateHttpResponseCode(httpCode);                
            }
        }

        private void CreateIpAddressLog(string OrderNo)
        {
            if (string.IsNullOrEmpty(OrderNo))
            {
                File.WriteAllText(@"Log\Failed_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt", Utilities.TryToGetClientIpAddress());
                return;
            }
            File.WriteAllText(@"Log\" + OrderNo + ".txt", Utilities.TryToGetClientIpAddress());
        }

        private void CreateHttpResponseCode(int httpCode)
        {
            WebOperationContext ctx = WebOperationContext.Current;

            switch (httpCode)
            {
                case 200: ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Success200;
                    break;
                case 201: ctx.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Success201;
                    break;
                case 202: ctx.OutgoingResponse.StatusCode = HttpStatusCode.Accepted;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Success202;
                    break;
                case 203: ctx.OutgoingResponse.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Success203;
                    break;
                case 204: ctx.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Success204;
                    break;
                case 205: ctx.OutgoingResponse.StatusCode = HttpStatusCode.ResetContent;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Success205;
                    break;
                case 400: ctx.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Error400;
                    break;
                case 401: ctx.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Error401;
                    break;
                case 402: ctx.OutgoingResponse.StatusCode = HttpStatusCode.PaymentRequired;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Error402;
                    break;
                case 403: ctx.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Error403;
                    break;
                case 404: ctx.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Error404;
                    break;
                case 405: ctx.OutgoingResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                    ctx.OutgoingResponse.StatusDescription = Constants.HttpResponseStatusDescription.Error405;                    
                    break;
                case 500: ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    break;
                case 503: ctx.OutgoingResponse.StatusCode = HttpStatusCode.ServiceUnavailable;
                    break;
                default:
                    break;
            }

        }

    }
}