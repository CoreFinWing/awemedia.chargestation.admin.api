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
                        ValidAudience = Environment.GetEnvironmentVariable("ValidAudience"),
                        ValidIssuer = Environment.GetEnvironmentVariable("ValidIssuer"),
                        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            var json = Admin.AzureFunctions.Business.Helpers.Utility.DownloadTextFromBlobAsync();
                            if (!string.IsNullOrWhiteSpace(json))
                            {
                                return JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                            }
                            var jwksJson = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
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
