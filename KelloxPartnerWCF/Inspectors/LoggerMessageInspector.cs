using log4net;
using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace KelloxPartnerWCF.Inspectors
{
    public class LoggerMessageInspector : IDispatchMessageInspector
    {
        private const string Tag = "KelloxPartnerService";
        private readonly ILog _logger;
        private readonly bool _loggingOn;
        private readonly string _logDir;

        public LoggerMessageInspector(ILog logger)
        {
            _logger = logger;            
            _logDir = ConfigurationManager.AppSettings["LogDir"];
            var loggingOnStr = ConfigurationManager.AppSettings["Logging"];
            if ( !string.IsNullOrEmpty(loggingOnStr) && !bool.TryParse(loggingOnStr, out _loggingOn))
            {
                _loggingOn = false;
            }            
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var requestAsString = MessageToString(ref request);
            if (_loggingOn)
            {
                WriteLog(requestAsString);
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

            var ms = new MemoryStream();
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

            var messageAsString = Encoding.UTF8.GetString(ms.ToArray());

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
            var messageAsString = Encoding.UTF8.GetString(requestBody);

            var ms = new MemoryStream();
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

        private void WriteLog(string requestAsString)
        {
            if (requestAsString.Length < 32766)
            {
                System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Request: \n{0}", requestAsString));
            }
            else
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KelloxPartnerLog.log");
                System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Generated dump at: {0}", filePath));
                _logger.Info(string.Format("Request: \n\n{0}\n\n", requestAsString));
            }
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            MessageBuffer buffer = reply.CreateBufferedCopy(int.MaxValue);
            reply = buffer.CreateMessage();
            var replyAsString = MessageToString(ref reply);
            if (_loggingOn)
            {
                if (replyAsString.Length < 32766)
                {
                    System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Response: \n{0}", replyAsString));
                }
                else
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KelloxPartnerLog.log");
                    System.Diagnostics.EventLog.WriteEntry(Tag, string.Format("Generated dump at: {0}", filePath));
                    _logger.Info(string.Format("Response: \n\n{0}\n\n", replyAsString));
                }                
            }
            ResponseHandler.CreateOrderLog(buffer.CreateMessage(), _logDir);

        }

    }
}