using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Common
{
    public static class Enums
    {
        public enum SessionStatus
        {
            New = 1,
            AuthorizationSuccessful = 2,
            AuthorizationFailure = 3,
            PaymentRequestSent = 4,
            PaymentCompleted = 5,
            PaymentFailure = 6,
            PaymentTimeout = 7,
            PaymentCanceledByUser = 8,
            Charging = 9,
            ChargingCompleted = 10,
            ChargeFailed = 11,
            SessionTimedOut = 12
        }
    }
}
