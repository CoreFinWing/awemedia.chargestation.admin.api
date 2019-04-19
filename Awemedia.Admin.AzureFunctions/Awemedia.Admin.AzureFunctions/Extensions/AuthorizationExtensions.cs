using Awemedia.Chargestation.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Awemedia.Chargestation.AzureFunctions.Helpers
{
    public static class AuthorizationExtensions
    {
        public static bool IsAuthorized(this HttpRequestMessage req)
        {
            return true;
        }
    }
}
