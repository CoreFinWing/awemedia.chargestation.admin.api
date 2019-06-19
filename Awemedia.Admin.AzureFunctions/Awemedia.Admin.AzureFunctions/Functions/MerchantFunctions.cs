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
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using AzureFunctions.Autofac;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Extensions;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Resolver;
using System.Collections.Generic;
using System.Collections;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class MerchantFunctions
    {
        [FunctionName("merchants")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "merchants")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _merchantSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _merchantSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, new { data = _merchantService.Get(_merchantSearchFilter, out int totalRecords), total = totalRecords });
        }
        [FunctionName("merchant")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "merchants/{Id}")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler, int Id)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _merchantService.GetById(Id));
        }
        [FunctionName("AddMerchant")]
        public HttpResponseMessage Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "merchants")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            var merchantBody = httpRequestMessage.GetBodyAsync<Merchant>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!merchantBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", merchantBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            Merchant merchant = merchantBody.Value;
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _merchantService.AddMerchant(merchant));
        }
        [FunctionName("Active_InActive_Merchant")]
        public HttpResponseMessage Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Patch", Route = "merchants")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            var jsonContent = httpRequestMessage.Content.ReadAsStringAsync().Result;
            var definition = new[] { new { Id = "", IsActive = "" } };
            var merchantsSetToActiveInActive = JsonConvert.DeserializeAnonymousType(jsonContent, definition);
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (merchantsSetToActiveInActive.Count() > 0)
            {
                _merchantService.MarkActiveInActive(merchantsSetToActiveInActive);
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);

        }
        [FunctionName("UpdateMerchant")]
        public HttpResponseMessage Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "merchants/{id}")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler, int id)
        {
            var merchantBody = httpRequestMessage.GetBodyAsync<Merchant>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (id <= 0)
                httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            Merchant merchant = merchantBody.Value;
            _merchantService.UpdateMerchant(merchant, id);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
    }
}
