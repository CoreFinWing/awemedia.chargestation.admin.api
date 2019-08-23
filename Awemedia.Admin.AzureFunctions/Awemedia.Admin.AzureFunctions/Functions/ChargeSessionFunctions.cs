using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using System.Net.Http;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using AzureFunctions.Autofac;
using Awemedia.Admin.AzureFunctions.Resolver;
using Awemedia.Admin.AzureFunctions.Extensions;

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
    }
}
