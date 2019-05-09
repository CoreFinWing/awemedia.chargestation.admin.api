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
    public class ChargeStationFuntion
    {
        [FunctionName("Chargestations")]
        public HttpResponseMessage GetFiltered(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage httpRequestMessage, [Inject] IChargeStationService _chargeStationServcie, [Inject]IErrorHandler errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            ChargeStationSearchFilter _chargeStationSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _chargeStationSearchFilter = queryDictionary.ToObject<ChargeStationSearchFilter>();
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationServcie.Get(_chargeStationSearchFilter));
        }
        [FunctionName("AddChargeStation")]
        public HttpResponseMessage AddChargeStation(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = null)] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationServcie, [Inject]IErrorHandler _errorHandler)
        {
            var body = httpRequestMessage.GetBodyAsync<ChargeStationResponse>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!httpRequestMessage.GetBodyAsync<ChargeStationResponse>().IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", body.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            if (string.IsNullOrEmpty(httpRequestMessage.Content.ReadAsStringAsync().Result))
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.NotFound, _errorHandler.GetMessage(ErrorMessagesEnum.PostedDataNotFound));
            ChargeStationResponse ChargeStationResponse = JsonHelper.JsonDeserialize<ChargeStationResponse>(httpRequestMessage.Content.ReadAsStringAsync().Result);
            Guid guid = ChargeStationResponse.DeviceId.StringToGuid();
            object device = _chargeStationServcie.IsChargeStationExists(guid);
            if (device != DBNull.Value)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.Conflict, _errorHandler.GetMessage(ErrorMessagesEnum.DuplicateRecordFound));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationServcie.AddChargeStation(ChargeStationResponse));
        }
    }
}
