using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class User
    {
        public User()
        {
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public int PostalCode { get; set; }
        public Role Role { get; set; }
        public string MappedMerchant { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Country Country { get; set; }
    }
}
