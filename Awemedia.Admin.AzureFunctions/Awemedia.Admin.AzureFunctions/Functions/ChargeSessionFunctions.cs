using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Resolver;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using AzureFunctions.Autofac;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using OidcApiAuthorization.Abstractions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class ChargeSessionFunctions
    {
        private readonly IApiAuthorization _apiAuthorization;
        public ChargeSessionFunctions(IApiAuthorization apiAuthorization)
        {
            _apiAuthorization = apiAuthorization;
        }

        [FunctionName("charge-sessions")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-sessions")] HttpRequestMessage httpRequestMessage, [Inject]IChargeSessionService _chargeSessionService, [Inject]IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            BaseSearchFilter _chargeSessionSearchFilter = null;     
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count > 0)
            {
                _chargeSessionSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            }
            var email = _apiAuthorization.AuthorizeAsync(httpRequestMessage.Headers).Result.ClaimsPrincipal?.Claims?.FirstOrDefault(s => s.Type.Equals("emails", StringComparison.OrdinalIgnoreCase)).Value;
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _chargeSessionService.Get(_chargeSessionSearchFilter, out int totalRecords,email), total = totalRecords });
        }

        [FunctionName("charge-session")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-sessions/{Id}")] HttpRequestMessage httpRequestMessage, [Inject]IChargeSessionService _chargeSessionService, [Inject]IErrorHandler _errorHandler, string Id)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!string.IsNullOrEmpty(Id))
            {
                return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _chargeSessionService.GetById(Guid.Parse(Id)));
            }
 
           return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
