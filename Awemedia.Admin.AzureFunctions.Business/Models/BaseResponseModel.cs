using Awemedia.Admin.AzureFunctions.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class BaseResponse
    {
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd hh:mm:ss")]
        public DateTime CreatedDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd hh:mm:ss")]
        public DateTime ModifiedDate { get; set; }
    }
}
