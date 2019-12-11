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
            PaymentCompleted = 2,
            Charging = 3,
            ChargeFailed = 4,
            ChargingCompleted = 5,
            UserCancelled = 6,
            Promoted = 7,
            PaymentFailed = 8,
            PaymentRequestReceived = 9,
            SessionTimedOut = 10
        }
    }
}
