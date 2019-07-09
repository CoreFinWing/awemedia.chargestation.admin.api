using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
   public class NotificationPayload
    {
        public CommandParams CommandParams { get; set; }
        public string Command { get; set; }
    }
}
