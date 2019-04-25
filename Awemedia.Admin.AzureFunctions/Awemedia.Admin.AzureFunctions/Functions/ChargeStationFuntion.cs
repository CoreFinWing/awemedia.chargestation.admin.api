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

        [FunctionName("Chargestations_GetFiltered")]

        public HttpResponseMessage GetFiltered(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage httpRequestMessage)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseFilterRequest _baseFilterResponse = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _baseFilterResponse = queryDictionary.ToObject<BaseFilterRequest>();
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationServcie.GetFiltered(_baseFilterResponse));
        }
    }
}
