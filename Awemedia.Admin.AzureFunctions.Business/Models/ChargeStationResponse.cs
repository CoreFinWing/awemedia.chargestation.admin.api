using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class ChargeStationResponse : BaseResponse
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Geolocation { get; set; }
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string ChargeControllerId { get; set; }
    }
}
