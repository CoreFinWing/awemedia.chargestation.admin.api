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
using Newtonsoft.Json.Linq;

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
            BaseSearchFilter _chargeStationSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _chargeStationSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, new { data = _chargeStationService.Get(_chargeStationSearchFilter, out int totalRecords), total = totalRecords });
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
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "charge-stations/{chargeStationId}")] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler, int chargeStationId)
        {
            var branchId = JObject.Parse(httpRequestMessage.Content.ReadAsStringAsync().Result);
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (string.IsNullOrEmpty(branchId["BranchId"].ToString()))
                httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            var _chargeStation = _chargeStationService.GetById(Convert.ToInt32(chargeStationId));
            ChargeStation chargeStation = new ChargeStation
            {
                BranchId = (int)branchId["BranchId"]
            };
            object device = _chargeStationService.IsChargeStationExists(_chargeStation.Id);
            if (device == DBNull.Value)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DeviceNotRegistered));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationService.UpdateChargeStation(chargeStation, _chargeStation.Id));
        }

        [FunctionName("start-charge")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "charge-stations/{id}/start-charge")] HttpRequestMessage httpRequestMessage, [Inject]INotificationService _notificationService, [Inject] IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler, string id)
        {
            var notificationPayloadBody = httpRequestMessage.GetBodyAsync<NotificationPayload>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!notificationPayloadBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", notificationPayloadBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            NotificationPayload notificationPayload = notificationPayloadBody.Value;
            notificationPayload.Command = "Start Charge";
            var maxDuration = Environment.GetEnvironmentVariable("MaxDuration");
            if (Convert.ToInt32(notificationPayload.CommandParams["Duration"]) <= Convert.ToInt32(maxDuration))
            {
                var chargeStation = _chargeStationService.GetById(Convert.ToInt32(id));
                Notification paymentNotification = new Notification
                {
                    DeviceId = chargeStation.DeviceId,
                    LoggedDateTime = DateTime.Now,
                    Payload = notificationPayload,
                    NotificationTitle = "Payment status notification."
                };
                _notificationService.SendNotification(paymentNotification);
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            }
            return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, "Validation Failed.");
        }
        [FunctionName("charge-station-detail")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-station-detail/{chargeStationId}")] HttpRequestMessage httpRequestMessage, [Inject] IChargeStationService _chargeStationService, [Inject]IErrorHandler errorHandler, string chargeStationId)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!string.IsNullOrEmpty(chargeStationId))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationService.GetById(Guid.Parse(chargeStationId)));
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
