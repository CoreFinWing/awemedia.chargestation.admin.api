using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class ChargeStation : BaseModel
    {

        public string Id { get; set; }
        [Required]
        public string Geolocation { get; set; }
        [Required]
        public int? BranchId { get; set; }
        [Required]
        public string ChargeControllerId { get; set; }
        [Required]
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
        public int Uid { get; set; }
        public Branch Branch { get; set; }
        public string MerchantName { get; set; }
        public string BranchName { get; set; }
        public string BatteryLevel { get; set; }
        public DateTime? LastPingTimeStamp { get; set; }
        public bool? IsOnline { get; set; }
        public string BatteryInfoDisplayField { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserSession> userSessions { get; set; }

    }
}
