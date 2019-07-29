using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class UserSession
    {
        public Guid Id { get; set; }
        public decimal? ChargeRentalRevnue { get; set; }
        public string InvoiceNo { get; set; }
        public string DeviceId { get; set; }
        public string TransactionId { get; set; }
        public DateTime? SessionStartTime { get; set; }
        public DateTime? SessionEndTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ApplicationId { get; set; }
        public string AppKey { get; set; }
        public string UserAccountId { get; set; }
        public string Email { get; set; }
        public int? SessionType { get; set; }
        public int? SessionStatus { get; set; }
        public string ChargeParams { get; set; }

        public SessionStatus SessionStatusNavigation { get; set; }
        public SessionType SessionTypeNavigation { get; set; }
    }
}
