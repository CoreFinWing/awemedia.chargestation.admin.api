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
using OidcApiAuthorization.Abstractions;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class MerchantFunctions
    {
        private readonly IApiAuthorization _apiAuthorization;
        public MerchantFunctions(IApiAuthorization apiAuthorization)
        {
            _apiAuthorization = apiAuthorization;
        }

        [FunctionName("merchants")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "merchants")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _merchantSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _merchantSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            var email = _apiAuthorization.AuthorizeAsync(httpRequestMessage.Headers).Result.ClaimsPrincipal?.Claims?.FirstOrDefault(s => s.Type.Equals("emails", StringComparison.OrdinalIgnoreCase)).Value;
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _merchantService.Get(_merchantSearchFilter, out int totalRecords, email, Convert.ToBoolean(String.IsNullOrEmpty(_merchantSearchFilter.IsActive) == true ? "false" : _merchantSearchFilter.IsActive)), total = totalRecords });
        }

        [FunctionName("merchantnames")]
        public HttpResponseMessage GetAllNames(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "merchantnames")] HttpRequestMessage httpRequestMessage, [Inject] IMerchantService _merchantService, [Inject] IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _merchantService.GetAllNames() });
        }


        [FunctionName("merchant")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "merchants/{Id}")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler, int Id)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _merchantService.GetById(Id));
        }
        [FunctionName("AddMerchant")]
        public HttpResponseMessage Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "merchants")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            var merchantBody = httpRequestMessage.GetBodyAsync<Merchant>();
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!merchantBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", merchantBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            Merchant merchant = merchantBody.Value;
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _merchantService.AddMerchant(merchant));
        }
        [FunctionName("Active_InActive_Merchant")]
        public HttpResponseMessage Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Patch", Route = "merchants")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            var jsonContent = httpRequestMessage.Content.ReadAsStringAsync().Result;
            var definition = new[] { new { Id = "", IsActive = "" } };
            var merchantsSetToActiveInActive = JsonConvert.DeserializeAnonymousType(jsonContent, definition);
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
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
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (id <= 0)
                httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            Merchant merchant = merchantBody.Value;
            _merchantService.UpdateMerchant(merchant, id);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
        [FunctionName("AutoCompleteSearchMerchant")]
        public HttpResponseMessage Search(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "auto-complete-search-merchant")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
            {
                string keyword = queryDictionary["keyword"].ToString();
                if (!string.IsNullOrEmpty(keyword))
                {
                    return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _merchantService.Search(keyword));
                }
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
