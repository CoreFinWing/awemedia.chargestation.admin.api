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

namespace Awemedia.Admin.AzureFunctions.Functions
{
    public class ChargeStationFuntion
    {
        private readonly IChargeStationService _chargeStationServcie;
        private readonly IErrorHandler _errorHandler;
        public ChargeStationFuntion(IChargeStationService chargeStationServcie, IMapper mapper, IErrorHandler errorHandler)
        {
            _chargeStationServcie = chargeStationServcie;
            _errorHandler = errorHandler;
        }

        [FunctionName("Chargestations")]

        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req)
        {
            if (!req.IsAuthorized())
            {
                return req.CreateResponse(HttpStatusCode.Unauthorized);
            }

            return req.CreateResponse(HttpStatusCode.OK, _chargeStationServcie.GetAll());
        }
        [FunctionName("Chargestations_GetFiltered")]

        public HttpResponseMessage GetFiltered(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req)
        {
            if (!req.IsAuthorized())
            {
                return req.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var queryDictionary = QueryHelpers.ParseQuery(req.RequestUri.Query);
            BaseFilterResponse _baseFilterResponse = queryDictionary.ToObject<BaseFilterResponse>();
            return req.CreateResponse(HttpStatusCode.OK, _chargeStationServcie.GetFiltered(_baseFilterResponse));
        }
    }
}
