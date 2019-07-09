using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string DeviceToken { get; set; }
        public NotificationPayload Payload { get; set; }
        public DateTime LoggedDateTime { get; set; }
        public string DeviceId { get; set; }
        public string NotificationResult { get; set; }
        public string NotificationTitle { get; set; }
    }
}
