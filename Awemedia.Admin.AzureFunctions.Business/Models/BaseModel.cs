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
    public class BaseModel
    {
        [JsonConverter(typeof(DateFormatConverter), "MM/dd/yyyy hh:mm:ss tt")]
        public DateTime CreatedDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "MM/dd/yyyy hh:mm:ss tt")]
        public DateTime ModifiedDate { get; set; }
    }
}
