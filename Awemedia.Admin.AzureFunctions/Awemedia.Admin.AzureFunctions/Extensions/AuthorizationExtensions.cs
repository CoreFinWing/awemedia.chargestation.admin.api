using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using OidcApiAuthorization.Abstractions;
using OidcApiAuthorization.Models;
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
        public static bool IsAuthorized(this HttpRequestMessage req, IApiAuthorization _apiAuthorization)
        {
            try
            {
                ApiAuthorizationResult authorizationResult = _apiAuthorization.AuthorizeAsync(req.Headers).Result;
                if (authorizationResult.Failed)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
