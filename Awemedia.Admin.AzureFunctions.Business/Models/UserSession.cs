using Awemedia.Admin.AzureFunctions.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class UserSession : BaseModel
    {
        public Guid Id { get; set; }
        public decimal? ChargeRentalRevnue { get; set; }
        public string TransactionId { get; set; }
        public string DeviceId { get; set; }
        public string TransactionTypeId { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd hh:mm:ss tt")]
        public DateTime? SessionStartTime { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd hh:mm:ss tt")]
        public DateTime? SessionEndTime { get; set; }
        public string ApplicationId { get; set; }
        public string AppKey { get; set; }
        public string UserAccountId { get; set; }
        public string Email { get; set; }
        public string SessionType { get; set; }
        public string SessionStatus { get; set; }
        public string ChargeParams { get; set; }
        public string Mobile { get; set; }
        public Guid? ChargeStationId { get; set; }
        public string MerchantName { get; set; }
        public int PortNumber { get; set; }
        public string Currency { get; set; }
        public string BranchName { get; set; }
        public ChargeOption ChargeOption { get; set; }

        public ChargeStation ChargeStation { get; set; }
        public SessionStatus SessionStatusNavigation { get; set; }
        public SessionType SessionTypeNavigation { get; set; }
    }
}
