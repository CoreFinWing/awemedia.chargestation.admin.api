using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctions.Autofac;
using Awemedia.Admin.AzureFunctions.Resolver;
using System.Net.Http;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using System.Net;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System.Linq;
using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class ChargeOptionFunctions
    {
        [FunctionName("charge-options")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-options")] HttpRequestMessage httpRequestMessage, [Inject]IChargeOptionService _chargeOptionService, [Inject]IErrorHandler _errorHandler)
        {
            bool IsActive = false;
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count > 0)
            {
                IsActive = Convert.ToBoolean(queryDictionary.Values.ElementAt(0).ToString());
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeOptionService.Get(IsActive));
        }
        [FunctionName("AddChargeOptions")]
        public HttpResponseMessage Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "charge-options")] HttpRequestMessage httpRequestMessage, [Inject]IChargeOptionService _chargeOptionService, [Inject]IErrorHandler _errorHandler)
        {
            var chargeOptionsBody = httpRequestMessage.GetBodyAsync<ChargeOption>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!chargeOptionsBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", chargeOptionsBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            ChargeOption chargeOptionsResponse = chargeOptionsBody.Value;
            _chargeOptionService.Add(chargeOptionsResponse, out bool isDuplicateRecord);
            if (isDuplicateRecord)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DuplicateRecordFound));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
        [FunctionName("Active_InActive_ChargeOptions")]
        public HttpResponseMessage Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "charge-options")] HttpRequestMessage httpRequestMessage, [Inject]IChargeOptionService _chargeOptionService, [Inject]IErrorHandler _errorHandler)
        {
            var baseChargeOptionsFilterModelbody = httpRequestMessage.GetBodyAsync<List<BaseDeletionModel>>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!baseChargeOptionsFilterModelbody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", baseChargeOptionsFilterModelbody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            List<BaseDeletionModel> baseChargeOptionsResponses = baseChargeOptionsFilterModelbody.Value;
            _chargeOptionService.MarkActiveInActive(baseChargeOptionsResponses);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
    }
}
