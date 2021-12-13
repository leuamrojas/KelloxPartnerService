using KelloxPartnerWCF.Helpers;
using System;
using System.IO;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Xml;

namespace KelloxPartnerWCF.Inspectors
{
    public class ResponseHandler
    {
        public static void CreateOrderLog(Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();

            var xmlStr = WebUtility.HtmlDecode(bodyReader.ReadOuterXml()).Replace(Constants.XmlDeclaration, "");
            xmlStr = Constants.XmlDeclaration + xmlStr;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            var orderNo = xmlDoc.DocumentElement.GetElementsByTagName("OrderNo").Item(0).InnerText;
            SaveIpAddress(orderNo);

            var strHttpCode = xmlDoc.DocumentElement.GetElementsByTagName("httpCode").Item(0).InnerText;

            if (int.TryParse(strHttpCode, out int httpCode))
            {
                UpdateHttpResponse(httpCode);
            }
        }

        private static void SaveIpAddress(string OrderNo)
        {
            if (string.IsNullOrEmpty(OrderNo))
            {
                File.WriteAllText(@"Log\Failed_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt", Utils.TryToGetClientIpAddress());
                return;
            }
            File.WriteAllText(@"Log\" + OrderNo + ".txt", Utils.TryToGetClientIpAddress());
        }

        private static void UpdateHttpResponse(int httpCode)
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