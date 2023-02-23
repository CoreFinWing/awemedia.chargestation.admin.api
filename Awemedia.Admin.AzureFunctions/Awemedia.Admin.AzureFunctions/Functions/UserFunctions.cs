using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Resolver;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using AzureFunctions.Autofac;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using OidcApiAuthorization.Abstractions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class UserFunctions
    {
        private readonly IApiAuthorization _apiAuthorization;

        public UserFunctions(IApiAuthorization apiAuthorization)
        {
            _apiAuthorization = apiAuthorization;
        }

        [FunctionName("users")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequestMessage httpRequestMessage, [Inject] IUserService _userService, [Inject] IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _userSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
                _userSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _userService.Get(_userSearchFilter, out int totalRecords, Convert.ToBoolean(String.IsNullOrEmpty(_userSearchFilter.IsActive) == true ? "false" : _userSearchFilter.IsActive)), total = totalRecords });
        }

        [FunctionName("user")]
        public HttpResponseMessage GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{Id}")] HttpRequestMessage httpRequestMessage, [Inject] IUserService _userService, [Inject] IErrorHandler _errorHandler, int Id)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _userService.GetById(Id));
        }

        [FunctionName("AddUser")]
        public HttpResponseMessage Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "users")] HttpRequestMessage httpRequestMessage, [Inject] IUserService _userService, [Inject] IErrorHandler _errorHandler)
        {
            var userBody = httpRequestMessage.GetBodyAsync<UserModel>();
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!userBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", userBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            UserModel user = userBody.Value;

            if (_userService.IsUserDuplicate(user))
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.Conflict, "Error - Duplicate User");

            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _userService.AddUser(user));
        }

        [FunctionName("UpdateUser")]
        public HttpResponseMessage Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "users/{id}")] HttpRequestMessage httpRequestMessage, [Inject] IUserService _userService, [Inject] IErrorHandler _errorHandler, int id)
        {
            var userBody = httpRequestMessage.GetBodyAsync<UserModel>();
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!userBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", userBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            if (id <= 0)
                return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            UserModel user = userBody.Value;
            _userService.UpdateUser(user, id);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }

        [FunctionName("roles")]
        public HttpResponseMessage GetRoles(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "roles")] HttpRequestMessage httpRequestMessage, [Inject] IRoleService _roleService, [Inject] IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var result = _apiAuthorization.AuthorizeAsync(httpRequestMessage.Headers).Result;
            if (result.Success)
            {
                var hasOwnerRole = result.ClaimsPrincipal?.Claims?.Any(s => s.Type.Equals("extension_UserRoles", StringComparison.OrdinalIgnoreCase) && s.Value.Equals("owner", StringComparison.OrdinalIgnoreCase)) ?? false;
                if (hasOwnerRole)
                {
                    return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _roleService.GetAll(true, true) });
                }
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _roleService.GetAll(false, true) });
        }

        [FunctionName("role")]
        public HttpResponseMessage GetRoleById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "roles/{Id}")] HttpRequestMessage httpRequestMessage, [Inject] IRoleService _roleService, [Inject] IErrorHandler _errorHandler, int Id)
        {
            if (!httpRequestMessage.IsAuthorized(_apiAuthorization))
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _roleService.GetById(Id));
        }
    }
}