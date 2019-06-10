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
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _merchantService.Get(_merchantSearchFilter));
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
            if (merchant.Branch != null)
            {
                if (merchant.Branch.Count > 0)
                {
                    return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _merchantService.AddMerchant(merchant));
                }
            }
            return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.BranchIsRequired));

        }
        [FunctionName("Active_InActive_Merchant")]
        public HttpResponseMessage Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Patch", Route = "merchants/mark_active_inactive")] HttpRequestMessage httpRequestMessage, [Inject]IMerchantService _merchantService, [Inject]IErrorHandler _errorHandler)
        {
            var baseDeletionModelbody = httpRequestMessage.GetBodyAsync<List<BaseDeletionModel>>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!baseDeletionModelbody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", baseDeletionModelbody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            List<BaseDeletionModel> baseChargeOptionsResponses = baseDeletionModelbody.Value;
            _merchantService.MarkActiveInActive(baseChargeOptionsResponses);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
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
            if (merchant.Branch != null)
            {
                if (merchant.Branch.Count > 0)
                {
                    _merchantService.UpdateMerchant(merchant, id);
                    return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
                }
            }
            return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.BranchIsRequired));
        }
    }
}
