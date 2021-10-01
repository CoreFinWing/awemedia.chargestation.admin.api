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
        }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Currency { get; set; }

        public ICollection<ChargeOptions> ChargeOptions { get; set; }
    }
}
