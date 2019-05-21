using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class ChargeStationResponse : BaseResponse
    {

        public string Id { get; set; }
        [Required]
        public string Geolocation { get; set; }
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string ChargeControllerId { get; set; }
        [Required]
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
        public int Uid { get; set; }
    }
}
