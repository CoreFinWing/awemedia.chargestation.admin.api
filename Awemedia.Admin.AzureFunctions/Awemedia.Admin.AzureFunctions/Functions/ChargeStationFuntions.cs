using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using AutoMapper;
using System.Net.Http;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using System.Net;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using AzureFunctions.Autofac;
using Awemedia.Admin.AzureFunctions.Resolver;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Business.Helpers;
using System;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class ChargeStationFuntions
    {
        [FunctionName("charge-stations")]
        public HttpResponseMessage GetFiltered(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-stations")] HttpRequestMessage httpRequestMessage, [Inject] IChargeStationService _chargeStationService, [Inject]IErrorHandler errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            ChargeStationSearchFilter _chargeStationSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _chargeStationSearchFilter = queryDictionary.ToObject<ChargeStationSearchFilter>();
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationService.Get(_chargeStationSearchFilter));
        }
        [FunctionName("AddChargeStation")]
        public HttpResponseMessage Post(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "charge-stations")] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler)
        {
            var chargeStationBody = httpRequestMessage.GetBodyAsync<ChargeStation>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!chargeStationBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", chargeStationBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            ChargeStation chargeStation = chargeStationBody.Value;
            Guid guid = chargeStation.DeviceId.StringToGuid();
            object device = _chargeStationService.IsChargeStationExists(guid);
            if (device != DBNull.Value)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DuplicateRecordFound));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationService.AddChargeStation(chargeStation));
        }
        [FunctionName("UpdateChargeStation")]
        public HttpResponseMessage Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "charge-stations/{chargeStationId}/{merchantId}")] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler, string merchantId, string chargeStationId)
        {
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (string.IsNullOrEmpty(merchantId) || string.IsNullOrEmpty(chargeStationId))
                return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            ChargeStation chargeStation = new ChargeStation
            {
                Id = chargeStationId,
                MerchantId = merchantId
            };
            Guid guid = Guid.Parse(chargeStation.Id);
            object device = _chargeStationService.IsChargeStationExists(guid);
            if (device == DBNull.Value)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DeviceNotRegistered));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationService.UpdateChargeStation(chargeStation, guid));
        }
    }
}
