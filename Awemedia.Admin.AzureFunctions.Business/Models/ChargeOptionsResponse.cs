using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class ChargeOptionsResponse : BaseResponse
    {
        public int Id { get; set; }
        [Required]
        public string ChargeDuration { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Currency { get; set; }
        public bool IsActive { get; set; }
    }
}
