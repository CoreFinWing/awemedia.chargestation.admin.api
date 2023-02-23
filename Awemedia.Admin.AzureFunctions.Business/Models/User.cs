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
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int PostalCode { get; set; }
        public string RoleName { get; set; }
        public RoleModel Role { get; set; }
        [Required]
        public int RoleId { get; set; }
        public List<MappedMerchant> MappedMerchant { get; set; }
        public string AssignedMerchantsName { get; set; }
        public bool IsActive { get; set; }
        public Country Country { get; set; }
    }
}
