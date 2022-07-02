using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class UserModel : BaseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public int PostalCode { get; set; }
        public string RoleName { get; set; }
        public RoleModel Role { get; set; }
        public int RoleId { get; set; }
        public string MappedMerchant { get; set; }
        public bool IsActive { get; set; }
        public Country Country { get; set; }
    }
}
