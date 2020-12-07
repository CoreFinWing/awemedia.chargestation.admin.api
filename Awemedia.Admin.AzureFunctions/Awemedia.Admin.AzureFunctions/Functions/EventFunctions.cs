using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using AzureFunctions.Autofac;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Resolver;
using System.Net;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Extensions;
using Awemedia.Chargestation.AzureFunctions.Extensions;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class EventFunctions
    {
        [FunctionName("Events")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events")] HttpRequestMessage httpRequestMessage, [Inject]IErrorHandler _errorHandler, [Inject]IEventService eventService)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _eventSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _eventSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = eventService.Get(_eventSearchFilter, out int totalRecords), total = totalRecords });
        }
        [FunctionName("event-detail")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/{id}")] HttpRequestMessage httpRequestMessage, [Inject]IErrorHandler _errorHandler, [Inject]IEventService eventService, int id)
        {
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (id > 0)
                return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, eventService.GetById(id));
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
