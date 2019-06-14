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
using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.AspNetCore.WebUtilities;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using System.Net;
using System.Linq;
using Awemedia.Admin.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class BranchFunctions
    {
        [FunctionName("Branches")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "branches")] HttpRequestMessage httpRequestMessage, [Inject]IBranchService _branchService, [Inject]IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _branchSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _branchSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, new { data = _branchService.Get(_branchSearchFilter), total = _branchService.Get(_branchSearchFilter).Count() });
        }
        [FunctionName("Branch")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "branches/{Id}")] HttpRequestMessage httpRequestMessage, [Inject]IBranchService _branchService, [Inject]IErrorHandler _errorHandler, int Id)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK, _branchService.GetById(Id));
        }
        [FunctionName("AddBranch")]
        public HttpResponseMessage Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "branches")] HttpRequestMessage httpRequestMessage, [Inject]IBranchService _branchService, [Inject]IErrorHandler _errorHandler)
        {
            var branchBody = httpRequestMessage.GetBodyAsync<Branch>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!branchBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", branchBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            Branch branch = branchBody.Value;
            _branchService.AddBranch(branch, branch.MerchantId);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
        [FunctionName("Active_InActive_Branch")]
        public HttpResponseMessage Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Patch", Route = "branches/mark_active_inactive")] HttpRequestMessage httpRequestMessage, [Inject]IBranchService _branchService, [Inject]IErrorHandler _errorHandler)
        {
            var jsonContent = httpRequestMessage.Content.ReadAsStringAsync().Result;
            var definition = new[] { new { Id = "", IsActive = "" } };
            var branchesSetToActiveInActive = JsonConvert.DeserializeAnonymousType(jsonContent, definition);
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (branchesSetToActiveInActive.Length > 0)
            {
                _branchService.MarkActiveInActive(branchesSetToActiveInActive);
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            }
            return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        }
        [FunctionName("UpdateBranch")]
        public HttpResponseMessage Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "branches/{id}")] HttpRequestMessage httpRequestMessage, [Inject]IBranchService _branchService, [Inject]IErrorHandler _errorHandler, int id)
        {
            var branchBody = httpRequestMessage.GetBodyAsync<Branch>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (id <= 0)
                httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            Branch branch = branchBody.Value;
            _branchService.UpdateBranch(branch, id);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
    }
}
