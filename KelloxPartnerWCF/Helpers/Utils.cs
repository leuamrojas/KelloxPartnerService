﻿using KelloxPartnerWCF.Helpers;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace KelloxPartnerWCF.Helpers
{
    public class Utils
    {
        //private const string XmlEncodingIBM865 = "<?xml version=\"1.0\" encoding=\"IBM865\"?>";

        public static void AddEncodingIBM865(ref string xmlData)
        {
            xmlData = Constants.XmlDeclaration + xmlData;
            xmlData = xmlData.Replace("&", "");
        }

        /// <summary>
        /// This may return an IPv4 or IPv6 format address. It will return null if unable to retrieve
        /// this information.
        /// </summary>
        public static string TryToGetClientIpAddress()
        {
            var currentOperationContext = OperationContext.Current;

            if (currentOperationContext == null)
                return null;

            object nameMessagePropertyRaw;
            if (!currentOperationContext.IncomingMessageProperties.TryGetValue(
                RemoteEndpointMessageProperty.Name,
                out nameMessagePropertyRaw)
            )
                return null;

            var nameMessageProperty = nameMessagePropertyRaw as RemoteEndpointMessageProperty;
            if (nameMessageProperty == null)
                return null;

            var ipAddress = nameMessageProperty.Address;
            if (string.IsNullOrWhiteSpace(ipAddress))
                return null;

            return ipAddress.Trim();
        }
    }
}