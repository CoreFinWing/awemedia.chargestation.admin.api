using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class PromotionFunctions
    {
        [FunctionName("Promotions")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "promotion")] HttpRequestMessage httpRequestMessage, [Inject]IErrorHandler _errorHandler, [Inject]IPromotionService promotionService)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _promotionSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _promotionSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = promotionService.Get(_promotionSearchFilter, out int totalRecords), total = totalRecords });
        }

        [FunctionName("AddPromotion")]
        public HttpResponseMessage Post(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "promotion")] HttpRequestMessage httpRequestMessage, [Inject]IPromotionService _promotionService, [Inject]IErrorHandler _errorHandler)
        {
            var promotionBody = httpRequestMessage.GetBodyAsync<Promotion>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!promotionBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", promotionBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            Promotion promotionResponse = promotionBody.Value;
            _promotionService.Add(promotionResponse, out bool isDuplicateRecord);
            if (isDuplicateRecord)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DuplicateRecordFound));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }

        [FunctionName("UpdatePromotion")]
        public HttpResponseMessage Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "promotion/{id}")] HttpRequestMessage httpRequestMessage, [Inject]IPromotionService _promotionService, [Inject]IErrorHandler _errorHandler, int id)
        {
            var promotionBody = httpRequestMessage.GetBodyAsync<Promotion>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (id <= 0)
                httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            Promotion promotion = promotionBody.Value;
            _promotionService.UpdatePromotion(promotion, id);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }

        [FunctionName("Promotion")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "promotion/{Id}")] HttpRequestMessage httpRequestMessage, [Inject]IPromotionService _promotionService, [Inject]IErrorHandler _errorHandler, int Id)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _promotionService.GetById(Id));
        }

        [FunctionName("Active_InActive_Promotion")]
        public HttpResponseMessage Patch(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Patch", Route = "promotion")] HttpRequestMessage httpRequestMessage, [Inject]IPromotionService _promotionService, [Inject]IErrorHandler _errorHandler)
        {
            var jsonContent = httpRequestMessage.Content.ReadAsStringAsync().Result;
            var definition = new[] { new { Id = "", IsActive = "" } };
            var promotionSetToActiveInActive = JsonConvert.DeserializeAnonymousType(jsonContent, definition);
            Status status = new Status();
            
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (promotionSetToActiveInActive.Length > 0)
            {
                status.Id =Convert.ToInt32(promotionSetToActiveInActive[0].Id);
                status.IsActive = Convert.ToBoolean(promotionSetToActiveInActive[0].IsActive);

                _promotionService.ToggleActiveStatus(status);
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}

