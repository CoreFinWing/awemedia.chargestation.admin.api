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
using Newtonsoft.Json;
using OidcApiAuthorization.Abstractions;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class ChargeStationFuntions
    {
        private readonly IApiAuthorization _apiAuthorization;
        public ChargeStationFuntions(IApiAuthorization apiAuthorization)
        {
            _apiAuthorization = apiAuthorization;
        }

        [FunctionName("charge-stations")]
        public HttpResponseMessage GetFiltered(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-stations")] HttpRequestMessage httpRequestMessage, [Inject] IChargeStationService _chargeStationService, [Inject]IErrorHandler errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _chargeStationSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _chargeStationSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();

            var email = _apiAuthorization.AuthorizeAsync(httpRequestMessage.Headers).Result.ClaimsPrincipal?.Claims?.FirstOrDefault(s => s.Type.Equals("emails", StringComparison.OrdinalIgnoreCase)).Value;
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _chargeStationService.Get(_chargeStationSearchFilter, out int totalRecords, email, Convert.ToBoolean(String.IsNullOrEmpty(_chargeStationSearchFilter.IsActive) == true ? "false" : _chargeStationSearchFilter.IsActive)), total = totalRecords });
        }
        [FunctionName("AddChargeStation")]
        public HttpResponseMessage Post(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "charge-stations")] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler)
        {
            var chargeStationBody = httpRequestMessage.GetBodyAsync<ChargeStation>();
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!chargeStationBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", chargeStationBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            ChargeStation chargeStation = chargeStationBody.Value;
            Guid guid = chargeStation.DeviceId.StringToGuid();
            object device = _chargeStationService.IsChargeStationExists(guid);//todo: function should return boolean value.
            if (device != DBNull.Value)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DuplicateRecordFound));
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _chargeStationService.AddChargeStation(chargeStation));
        }
        [FunctionName("UpdateChargeStation")]
        public HttpResponseMessage Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "charge-stations/{chargeStationId}")] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler, string chargeStationId, [Inject]IBranchService _branchService)
        {
            var branchId = JObject.Parse(httpRequestMessage.Content.ReadAsStringAsync().Result);
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            ChargeStation chargeStation = new ChargeStation();
            if (string.IsNullOrEmpty(branchId["BranchId"].ToString()))
            {
                chargeStation.BranchId = null;
            }
            else
            {
                chargeStation.BranchId = (int)branchId["BranchId"];
                var branch = _branchService.GetById(chargeStation.BranchId.GetValueOrDefault());
                if (branch == null)
                    return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, "Branch doesn't exist.");
            }
            var _chargeStation = _chargeStationService.GetById(Guid.Parse(chargeStationId));
            object device = _chargeStationService.IsChargeStationExists(Guid.Parse(_chargeStation.Id));//todo: function should return boolean value.
            if (device == DBNull.Value)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DeviceNotRegistered));
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _chargeStationService.UpdateChargeStation(chargeStation, Guid.Parse(_chargeStation.Id)));
        }

        [FunctionName("start-charge")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "charge-stations/{id}/start-charge")] HttpRequestMessage httpRequestMessage, [Inject]INotificationService _notificationService, [Inject] IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler, string id)
        {
            var notificationPayloadBody = httpRequestMessage.GetBodyAsync<NotificationPayload>();
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!notificationPayloadBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", notificationPayloadBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            NotificationPayload notificationPayload = notificationPayloadBody.Value;
            notificationPayload.Command = "Start Charge";
            var maxDuration = Environment.GetEnvironmentVariable("remote_push_max_charge_duration");
            if (Convert.ToInt32(notificationPayload.CommandParams["Duration"]) <= Convert.ToInt32(maxDuration))
            {
                var chargeStation = _chargeStationService.GetById(Convert.ToInt32(id));
                Notification paymentNotification = new Notification
                {
                    DeviceId = chargeStation.DeviceId,
                    LoggedDateTime = DateTime.Now.ToUniversalTime(),
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-stations/{chargeStationId}")] HttpRequestMessage httpRequestMessage, [Inject] IChargeStationService _chargeStationService, [Inject]IErrorHandler errorHandler, string chargeStationId)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!string.IsNullOrEmpty(chargeStationId))
            {
                return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _chargeStationService.GetById(Guid.Parse(chargeStationId)));
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
        [FunctionName("Active_InActive_ChargeStation")]
        public HttpResponseMessage Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Patch", Route = "charge-stations")] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler)
        {
            var jsonContent = httpRequestMessage.Content.ReadAsStringAsync().Result;
            var definition = new[] { new { Id = "", IsActive = "" } };
            var chargeStationsToSetActiveInActive = JsonConvert.DeserializeAnonymousType(jsonContent, definition);
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (chargeStationsToSetActiveInActive.Count() > 0)
            {
                _chargeStationService.MarkActiveInActive(chargeStationsToSetActiveInActive);
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);

        }

        [FunctionName("DetachFromBranch")]
        public HttpResponseMessage DetachFromBranch(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Patch", Route = "charge-stations/{chargeStationId}")] HttpRequestMessage httpRequestMessage, [Inject]IChargeStationService _chargeStationService, [Inject]IErrorHandler _errorHandler, string chargeStationId, [Inject]IBranchService _branchService)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            ChargeStation chargeStation = new ChargeStation { BranchId = null };
            var _chargeStation = _chargeStationService.GetById(Guid.Parse(chargeStationId));
            object device = _chargeStationService.IsChargeStationExists(Guid.Parse(_chargeStation.Id));
            if (device == DBNull.Value)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.OK, _errorHandler.GetMessage(ErrorMessagesEnum.DeviceNotRegistered));
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _chargeStationService.UpdateChargeStation(chargeStation, Guid.Parse(_chargeStation.Id)));
        }
    }
}
