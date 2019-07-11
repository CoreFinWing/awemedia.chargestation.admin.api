using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class NotificationPayload
    {
        public Dictionary<string, string> CommandParams { get; set; }
        public string Command { get; set; }
    }
}
