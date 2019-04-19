using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
                            var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                            var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                            return (IEnumerable<SecurityKey>)keys;
                        }
                    };
                    ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 
