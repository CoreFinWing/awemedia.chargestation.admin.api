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

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class ChargeStationFuntion
    {
        [FunctionName("Chargestations")]
        public HttpResponseMessage GetFiltered(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage httpRequestMessage, [Inject] IChargeStationService _chargeStationServcie, [Inject]IErrorHandler errorHandler)
        {
            //if (!httpRequestMessage.IsAuthorized())
            //{
            //    return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            //}
            ChargeStationSearchFilter _chargeStationSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _chargeStationSearchFilter = queryDictionary.ToObject<ChargeStationSearchFilter>();
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _chargeStationServcie.Get(_chargeStationSearchFilter));
        }
    }
}
