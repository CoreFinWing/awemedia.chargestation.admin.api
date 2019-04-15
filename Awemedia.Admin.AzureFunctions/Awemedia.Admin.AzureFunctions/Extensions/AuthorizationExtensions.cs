using Awemedia.Chargestation.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Awemedia.Chargestation.AzureFunctions.Helpers
{
    public static class AuthorizationExtensions
    {
        private static readonly string clientId = "mobileapp";
        private static readonly string clientSecret = "dGVzdA==";

        public static bool IsAuthorized(this HttpRequestMessage req)
        {
            string decodedString = string.Empty;
            bool validFlag = false;
            if (!string.IsNullOrEmpty(Convert.ToString(req.Headers.Authorization)))
            {
                string encodedString = req.Headers.Authorization.Parameter;
                if(!string.IsNullOrEmpty(encodedString))
                {
                    decodedString = encodedString.Base64ToString();
                    string[] parts = decodedString.ToString().Split(new char[] { ':' });
                    string _clientId = parts[0];
                    string _clientSecret = parts[1];
                    if (IsVarifiedCredentials(_clientId, _clientSecret))
                    {
                        validFlag = true;
                    }
                }
            }
            else
            {
                validFlag = false;
            }
            return validFlag;
        }
        private static bool IsVarifiedCredentials(string _clientId, string _clientSecret)
        {
            return _clientId.Equals(clientId) && _clientSecret.Equals(clientSecret);
        }
    }
}
