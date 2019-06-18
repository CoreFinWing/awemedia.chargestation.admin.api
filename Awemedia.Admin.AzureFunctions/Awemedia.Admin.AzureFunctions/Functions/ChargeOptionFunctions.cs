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
using Awemedia.Admin.AzureFunctions.Extensions;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class ChargeOptionFunctions
    {
        [FunctionName("charge-options")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "charge-options")] HttpRequestMessage httpRequestMessage, [Inject]IChargeOptionService _chargeOptionService, [Inject]IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            BaseSearchFilter _chargeOptionSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count > 0)
            {
                _chargeOptionSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, new { data = _chargeOptionService.Get(_chargeOptionSearchFilter, Convert.ToBoolean(String.IsNullOrEmpty(_chargeOptionSearchFilter.IsActive) == true ? "false" : _chargeOptionSearchFilter.IsActive)), total = _chargeOptionService.Get(_chargeOptionSearchFilter, Convert.ToBoolean(String.IsNullOrEmpty(_chargeOptionSearchFilter.IsActive) == true ? "false" : _chargeOptionSearchFilter.IsActive)).Count() });
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
            var jsonContent = httpRequestMessage.Content.ReadAsStringAsync().Result;
            var definition = new[] { new { Id = "", IsActive = "" } };
            var optionsSetToActiveInActive = JsonConvert.DeserializeAnonymousType(jsonContent, definition);
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (optionsSetToActiveInActive.Length > 0)
            {
                _chargeOptionService.MarkActiveInActive(optionsSetToActiveInActive);
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
