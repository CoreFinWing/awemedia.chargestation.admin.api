using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Repositories
{
    public class ErrorResponse
    {

        public int Code { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

        // other fields
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
