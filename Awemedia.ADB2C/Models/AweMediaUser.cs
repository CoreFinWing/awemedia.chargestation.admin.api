using Awemedia.ADB2C.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.ADB2C.Models
{
    public class AweMediaUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }
        public int PostalCode { get; set; }
        public string Role { get; set; }
        public int MappedMerchant { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
