using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class Merchant:BaseModel
    {
        public int Id { get; set; }
        [Required]
        public string BusinessName { get; set; }
        public string LicenseNum { get; set; }
        public string Dba { get; set; }
        public string ContactName { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        [Required]
        public string Email { get; set; }
        public string ProfitSharePercentage { get; set; }
        public string ChargeStationsOrdered { get; set; }
        public string DepositMoneyPaid { get; set; }
        public int? IndustryTypeId { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryPhone { get; set; }
        public string IndustryName { get; set; }
        public IndustryType IndustryType { get; set; }
        public ICollection<Branch> Branch { get; set; }
    }
}
