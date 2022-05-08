using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Functions
{
   
    public class UserFunction
    {
        [FunctionName("AddUser")]
        public HttpResponseMessage Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "branches")] HttpRequestMessage httpRequestMessage)
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
    }
}
