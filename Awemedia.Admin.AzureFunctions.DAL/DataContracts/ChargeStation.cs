using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class ChargeStation
    {
        public ChargeStation()
        {
            UserSession = new HashSet<UserSession>();
        }

        public Guid Id { get; set; }
        public string Geolocation { get; set; }
        public string ChargeControllerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
        public int Uid { get; set; }
        public int? BranchId { get; set; }
        public bool IsActive { get; set; }
        public string BatteryLevel { get; set; }
        public DateTime? LastPingTimeStamp { get; set; }
        public bool? IsOnline { get; set; }

        public Branch Branch { get; set; }
        public ICollection<UserSession> UserSession { get; set; }
    }
}
