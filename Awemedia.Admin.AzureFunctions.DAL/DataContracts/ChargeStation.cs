﻿using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class ChargeStation
    {
        public ChargeStation()
        {
            Events = new HashSet<Events>();
        }

        public Guid Id { get; set; }
        public string Geolocation { get; set; }
        public string MerchantId { get; set; }
        public string ChargeControllerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<Events> Events { get; set; }
    }
}
