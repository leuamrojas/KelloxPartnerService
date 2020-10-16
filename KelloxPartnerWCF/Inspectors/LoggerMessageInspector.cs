using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web;
using System.Xml;

namespace KelloxPartnerWCF
{
    public class LoggerMessageInspector : IDispatchMessageInspector
    {        
        private const string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"ISO-8859-2\"?>";

        //private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public ILog Log { get; set; }

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

            //System.Diagnostics.EventLog.WriteEntry("KelloxServiceHost", "InputXml: " + System.Net.WebUtility.HtmlDecode(sb.ToString()));
            //_logger.Info(string.Format("Received: \n\n{0}\n\nClient IP address: {1}", System.Net.WebUtility.HtmlDecode(sb.ToString()), Utilities.TryToGetClientIpAddress()));            
            
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();

            SaveToLog(buffer.CreateMessage());
        }

        private void SaveToLog(Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();

            var xmlStr = System.Net.WebUtility.HtmlDecode(bodyReader.ReadOuterXml()).Replace(XmlDeclaration, "");
            xmlStr = XmlDeclaration + xmlStr;

            System.Diagnostics.EventLog.WriteEntry("KelloxServiceHost", "Message Body: \n" + xmlStr);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            var OrderNo = xmlDoc.DocumentElement.GetElementsByTagName("OrderNo").Item(0).InnerText;

            CreateLogFile(OrderNo);
        }

        private void CreateLogFile(string OrderNo) 
        {
            if (string.IsNullOrEmpty(OrderNo))
            {
                File.WriteAllText(@"Log\Failed_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt", Utilities.TryToGetClientIpAddress());
                return;
            }
            File.WriteAllText(@"Log\" + OrderNo + ".txt", Utilities.TryToGetClientIpAddress());
        }

    }
}