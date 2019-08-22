﻿using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class Branch
    {
        public Branch()
        {
            ChargeStation = new HashSet<ChargeStation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int MerchantId { get; set; }
        public string Geolocation { get; set; }
        public bool IsActive { get; set; }

        public Merchant Merchant { get; set; }
        public ICollection<ChargeStation> ChargeStation { get; set; }
    }
}
