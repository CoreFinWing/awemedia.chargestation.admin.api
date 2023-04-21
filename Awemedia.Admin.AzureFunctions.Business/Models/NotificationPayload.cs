using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class NotificationPayload
    {
        public Dictionary<string, string> CommandParams { get; set; }
        [Required]
        public string Command { get; set; }
    }
}
