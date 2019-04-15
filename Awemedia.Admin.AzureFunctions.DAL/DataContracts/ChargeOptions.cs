using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class ChargeOptions
    {
        public int Id { get; set; }
        public string ChargeDuration { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
