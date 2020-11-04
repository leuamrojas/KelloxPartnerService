using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KelloxPartnerWCF
{
    public static class Constants
    {
        public const string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"ISO-8859-2\"?>";

        public static class HttpResponseStatusDescription
        {            
            public const string Success200 = "Order created successfully, all lines available for delivery";
            public const string Success201 = "Order created successfully, some lines not available for delivery (backorder)";
            public const string Success202 = "Order created successfully, some lines not available. One or more item lines are not available for backorder";
            public const string Success203 = "Order created successfully, at least one line where customer is not allowed to purchase item number";
            public const string Success204 = "Not in use, to be reserved for later use";
            public const string Success205 = "Order created successfully, at least one item number is not valid";
            public const string Error400 = "Not in use";
            public const string Error401 = "Item number blocked, not possible to order";
            public const string Error402 = "Item number not on stock and not available for backorder";
            public const string Error403 = "Customer is not allowed to purchase item number";
            public const string Error404 = "Not enough credit limit for the customer";
            public const string Error405 = "Invalid item number";
        }
    }
}