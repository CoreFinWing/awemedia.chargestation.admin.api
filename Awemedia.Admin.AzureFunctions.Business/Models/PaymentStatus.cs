using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class PaymentStatus
    {
        public string AppId { get; set; }
        [Required]
        public string TransactionTypeId { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public string CustomerAccountCode { get; set; }
        [Required]
        public string MerchantAccountCode { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public int PortCount { get; set; }
    }
}
