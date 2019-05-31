﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class Branch:BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        
        public int MerchantId { get; set; }
        public string Geolocation { get; set; }

        public Merchant Merchant { get; set; }
    }
}