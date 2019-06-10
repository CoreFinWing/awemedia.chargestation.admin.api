using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class Merchant
    {
        public Merchant()
        {
            Branch = new HashSet<Branch>();
        }

        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string LicenseNum { get; set; }
        public string Dba { get; set; }
        public string ContactName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string ProfitSharePercentage { get; set; }
        public string ChargeStationsOrdered { get; set; }
        public string DepositMoneyPaid { get; set; }
        public int? IndustryTypeId { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryPhone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IndustryType IndustryType { get; set; }
        public ICollection<Branch> Branch { get; set; }
        public bool IsActive { get; set; }
    }
}
