using Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace KelloxPartnerWCF.Inspectors
{
    public class LoggerMessageInspector : IDispatchMessageInspector
    {           
        private readonly ILog _logger;
        private readonly bool _logOn;
        private const string Tag = "KelloxPartnerService";

        public LoggerMessageInspector(ILog logger)
        {
            _logger = logger;
            string logging = ConfigurationManager.AppSettings["logging"];
            if ( !string.IsNullOrEmpty(logging) )
            {
                if (!Boolean.TryParse(logging, out _logOn) )
                {
                    _logOn = false;
                }
            }            
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            string requestAsString = MessageToString(ref request);
            if (_logOn)
            {                
                if (requestAsString.Length < 32766)
                {
                    System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Request: \n{0}", requestAsString));
                }
                else
                {
                    string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "KelloxPartnerLog.log");
                    System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Generated dump at: {0}", filePath));
                    _logger.Info(string.Format("Request: \n\n{0}\n\n", requestAsString));
                } 
            }
            return requestAsString;
        }

        private string MessageToString(ref Message message)
        {
            if (message.IsEmpty) return "";

            WebContentFormat format = WebContentFormat.Default;

            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
            {
                format = ((WebBodyFormatMessageProperty)message.Properties[WebBodyFormatMessageProperty.Name]).Format;
            }

            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = null;
            switch (format)
            {
                case WebContentFormat.Default:
                case WebContentFormat.Xml: writer = XmlDictionaryWriter.CreateTextWriter(ms);
                    break;                
                case WebContentFormat.Raw:                    
                    return RawMessageToString(ref message);
                //case WebContentFormat.Json:
                //    break;
            }

            message.WriteMessage(writer);
            writer.Flush();

            string messageAsString = Encoding.UTF8.GetString(ms.ToArray());

            ms.Position = 0;
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);

            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageAsString;
        }

        private string RawMessageToString(ref Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement("Binary");
            byte[] requestBody = bodyReader.ReadContentAsBase64();
            string messageAsString = Encoding.UTF8.GetString(requestBody);

            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(ms);
            writer.WriteStartElement("Binary");
            writer.WriteBase64(requestBody, 0, requestBody.Length);
            writer.WriteEndElement();
            writer.Flush();

            ms.Position = 0;

            XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageAsString;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();

            string requestMessage = (string)correlationState;
            string replyAsString = MessageToString(ref reply);
            if (_logOn)
            {
                if (replyAsString.Length < 32766)
                {
                    System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Response: \n{0}", replyAsString));
                }
                else
                {
                    string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "KelloxPartnerLog.log");
                    System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Generated dump at: {0}", filePath));
                    _logger.Info(string.Format("Response: \n\n{0}\n\n", replyAsString));
                }                
            }
            ResponseHandler.CreateOrderLog(buffer.CreateMessage());

        }

    }
}