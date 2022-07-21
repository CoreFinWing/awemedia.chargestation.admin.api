using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class UserMerchantMapping
    {
        public UserMerchantMapping()
        {
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public User User { get; set; }
        public Merchant Merchant { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
