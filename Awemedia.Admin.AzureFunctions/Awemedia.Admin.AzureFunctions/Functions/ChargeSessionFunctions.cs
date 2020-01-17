using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Resolver;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using AzureFunctions.Autofac;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class ChargeSessionFunctions
    {
        [FunctionName("charge-sessions")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-sessions")] HttpRequestMessage httpRequestMessage, [Inject]IChargeSessionService _chargeSessionService, [Inject]IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            BaseSearchFilter _chargeSessionSearchFilter = null;     
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count > 0)
            {
                _chargeSessionSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, new { data = _chargeSessionService.Get(_chargeSessionSearchFilter, out int totalRecords), total = totalRecords });
        }

        [FunctionName("charge-session")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-sessions/{Id}")] HttpRequestMessage httpRequestMessage, [Inject]IChargeSessionService _chargeSessionService, [Inject]IErrorHandler _errorHandler, string Id)
        {
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!string.IsNullOrEmpty(Id))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeSessionService.GetById(Guid.Parse(Id)));
            }
 
           return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
