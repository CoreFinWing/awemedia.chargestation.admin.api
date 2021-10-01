using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class ChargeOption : BaseModel
    {
        public int Id { get; set; }
        [Required]
        public int ChargeDuration { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public Country Country { get; set; }
    }
}
