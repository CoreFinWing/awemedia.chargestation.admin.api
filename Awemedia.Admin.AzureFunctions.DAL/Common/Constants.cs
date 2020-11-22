using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.DAL.Common
{
    public static class Constants
    {
        public static Dictionary<int, string> sessionTypes = new Dictionary<int, string>
        {
                { 1,"Promotion" },
                { 2,"Paid"  },
        };
        public static Dictionary<int, string> sessionStatuses = new Dictionary<int, string>
        {
               { 1,"New" },
                { 2,"AuthorizationSuccessful"},
                { 3,"AuthorizationFailure"},
                { 4,"PaymentRequestSent"},
                { 5,"PaymentCompleted"},
                { 6,"PaymentFailure"},
                { 7,"PaymentTimeout"},
                { 8,"PaymentCanceledByUser"},
                { 9,"Charging"},
                { 10,"ChargingCompleted"},
                { 11,"ChargeFailed"},
                { 12,"SessionTimeout"},
                { 13,"AuthorizationTimeout"},
                { 14,"PaymentRequestTimeout"},
                { 15,"ServerTimeOut"}
        };
        public static Dictionary<int, string> industryTypes = new Dictionary<int, string>
        {
                { 1,"F&b (food and beverage)"},
                { 2,"Karaoke" },
                { 3,"Service" },
                { 4,"Event"},
        };
        public static Dictionary<int, string> eventTypes = new Dictionary<int, string>
        {
                { 1,"app started"},
                { 2,"charge session started"},
                { 3,"charge session ended"},
                { 7,"charging session failed"},
                { 8,"charging station connection failed" },
                { 9,"charge station not returning the battery level"},
                { 12,"video started" },
                { 13,"video ended" },
                { 14,"video failed"}
        };
    }
}
