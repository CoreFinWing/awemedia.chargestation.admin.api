using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class Country
    {
        public Country()
        {
            ChargeOptions = new HashSet<ChargeOptions>();
            Branch = new HashSet<Branch>();
            User = new HashSet<User>();
        }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Currency { get; set; }
        
        public ICollection<ChargeOptions> ChargeOptions { get; set; }
        public ICollection<Branch> Branch { get; set; }
        public ICollection<User> User { get; set; }
    }
}
