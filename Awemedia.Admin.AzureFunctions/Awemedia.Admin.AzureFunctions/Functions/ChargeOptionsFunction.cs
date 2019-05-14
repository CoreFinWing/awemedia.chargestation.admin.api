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
    public class ChargeOptionsFunction
    {
        [FunctionName("Chargeoptions")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-options")] HttpRequestMessage httpRequestMessage, [Inject]IChargeOptionsService _chargeOptionsServcie, [Inject]IErrorHandler _errorHandler)
        {
            bool IsActive = false;
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (httpRequestMessage.Headers.CacheControl == null)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, _errorHandler.GetMessage(ErrorMessagesEnum.NoCacheHeaderFound));
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count > 0)
            {
                IsActive = Convert.ToBoolean(queryDictionary.Values.ElementAt(0).ToString());
            }
            return httpRequestMessage.CreateCachedResponse(HttpStatusCode.OK, _chargeOptionsServcie.Get(IsActive), httpRequestMessage.Headers.CacheControl.MaxAge);
        }
        [FunctionName("AddChargeOptions")]
        public HttpResponseMessage Add(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "charge-options")] HttpRequestMessage httpRequestMessage, [Inject]IChargeOptionsService _chargeOptionsServcie, [Inject]IErrorHandler _errorHandler)
        {
            var body = httpRequestMessage.GetBodyAsync<ChargeOptionsResponse>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!body.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", body.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            if (string.IsNullOrEmpty(httpRequestMessage.Content.ReadAsStringAsync().Result))
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.NotFound, _errorHandler.GetMessage(ErrorMessagesEnum.PostedDataNotFound));
            ChargeOptionsResponse chargeOptionsResponse = JsonHelper.JsonDeserialize<ChargeOptionsResponse>(httpRequestMessage.Content.ReadAsStringAsync().Result);
            bool isAdded = _chargeOptionsServcie.Add(chargeOptionsResponse, out bool isDuplicateRecord);
            if (isDuplicateRecord)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.Conflict, _errorHandler.GetMessage(ErrorMessagesEnum.DuplicateRecordFound));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
        [FunctionName("Active_InActive_ChargeOptions")]
        public HttpResponseMessage Active_InActive(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "charge-options")] HttpRequestMessage httpRequestMessage, [Inject]IChargeOptionsService _chargeOptionsServcie, [Inject]IErrorHandler _errorHandler)
        {
            var body = httpRequestMessage.GetBodyAsync<List<BaseChargeOptionsFilterResponse>>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!body.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", body.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            if (string.IsNullOrEmpty(httpRequestMessage.Content.ReadAsStringAsync().Result))
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.NotFound, _errorHandler.GetMessage(ErrorMessagesEnum.PostedDataNotFound));
            List<BaseChargeOptionsFilterResponse> baseChargeOptionsResponses = JsonHelper.JsonDeserialize<List<BaseChargeOptionsFilterResponse>>(httpRequestMessage.Content.ReadAsStringAsync().Result);
            _chargeOptionsServcie.MarkActiveInActive(baseChargeOptionsResponses);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
    }
}
