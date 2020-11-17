using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace Awemedia.Chargestation.AzureFunctions.Helpers
{
    public static class AuthorizationExtensions
    {
        public static bool IsAuthorized(this HttpRequestMessage req)
        {
            try
            {
                bool isAuthorized = false;
                if (!string.IsNullOrEmpty(Convert.ToString(req.Headers.Authorization)))
                {
                    string jwtToken = req.Headers.Authorization.Parameter;
                    TokenValidationParameters validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidAudience = Environment.GetEnvironmentVariable("cognito_pool_web_client_id"),
                        ValidIssuer = Environment.GetEnvironmentVariable("cognito_issuer"),
                        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            var blobData = Admin.AzureFunctions.Business.Helpers.Utility.DownloadTextFromBlobAsync().Result;
                            if (blobData != null)
                            {
                                if (!string.IsNullOrWhiteSpace(blobData["ExpirationDate"]))
                                {
                                    if (Convert.ToDateTime(blobData["ExpirationDate"]) > DateTime.Now.ToUniversalTime())
                                    {
                                        if (!string.IsNullOrWhiteSpace(blobData["Json"]))
                                        {
                                            return JsonConvert.DeserializeObject<JsonWebKeySet>(Convert.ToString(blobData["Json"])).Keys;
                                        }
                                    }
                                }
                            }
                            WebClient client = new WebClient();
                            var clientData = client.DownloadData(parameters.ValidIssuer + "/.well-known/jwks.json");
                            var jwksJson = client.DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                            WebHeaderCollection webHeaderCollection = client.ResponseHeaders;
                            Admin.AzureFunctions.Business.Helpers.Utility.UploadTextToBlob(jwksJson);
                            return JsonConvert.DeserializeObject<JsonWebKeySet>(jwksJson).Keys;
                        }
                    };
                    ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
                    isAuthorized = true;
                }
                return isAuthorized;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
