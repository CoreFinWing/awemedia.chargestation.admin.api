using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string DeviceToken { get; set; }
        public string Payload { get; set; }
        public DateTime LoggedDateTime { get; set; }
        public string DeviceId { get; set; }
        public string NotificationResult { get; set; }
        public string Status { get; set; }
        public bool IsConsumed { get; set; }
        public DateTime ConsumedDateTime { get; set; }
    }
}
