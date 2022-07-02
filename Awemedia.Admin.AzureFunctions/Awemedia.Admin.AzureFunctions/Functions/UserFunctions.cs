using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using AzureFunctions.Autofac;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Extensions;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Resolver;
using System.Collections.Generic;
using System.Collections;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class UserFunctions
    {
        [FunctionName("users")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequestMessage httpRequestMessage, [Inject] IUserService _userService, [Inject] IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
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
            if (!httpRequestMessage.IsAuthorized())
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
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!userBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", userBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            UserModel user = userBody.Value;
            user.MappedMerchant = "";
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _userService.AddUser(user));
        }

        [FunctionName("UpdateUser")]
        public HttpResponseMessage Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "users/{id}")] HttpRequestMessage httpRequestMessage, [Inject] IUserService _userService, [Inject] IErrorHandler _errorHandler, int id)
        {
            var userBody = httpRequestMessage.GetBodyAsync<UserModel>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (id <= 0)
                httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            UserModel user = userBody.Value;
            _userService.UpdateUser(user, id);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
        //[FunctionName("AutoCompleteSearchMerchant")]
        //public HttpResponseMessage Search(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "auto-complete-search-merchant")] HttpRequestMessage httpRequestMessage, [Inject] IMerchantService _merchantService, [Inject] IErrorHandler _errorHandler)
        //{
        //    if (!httpRequestMessage.IsAuthorized())
        //    {
        //        return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
        //    }
        //    var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
        //    if (queryDictionary.Count() > 0)
        //    {
        //        string keyword = queryDictionary["keyword"].ToString();
        //        if (!string.IsNullOrEmpty(keyword))
        //        {
        //            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _merchantService.Search(keyword));
        //        }
        //    }
        //    return httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
        //}

        [FunctionName("roles")]
        public HttpResponseMessage GetRoles(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "roles")] HttpRequestMessage httpRequestMessage, [Inject] IRoleService _roleService, [Inject] IErrorHandler _errorHandler)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            BaseSearchFilter _userSearchFilter = null;
            var queryDictionary = QueryHelpers.ParseQuery(httpRequestMessage.RequestUri.Query);
            if (queryDictionary.Count() > 0)
            {
                _userSearchFilter = queryDictionary.ToObject<BaseSearchFilter>();
                return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _roleService.GetAll(Convert.ToBoolean(String.IsNullOrEmpty(_userSearchFilter.IsActive) == true ? "true" : _userSearchFilter.IsActive)) });
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, new { data = _roleService.GetAll(true) });
        }
        [FunctionName("role")]
        public HttpResponseMessage GetRoleById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "roles/{Id}")] HttpRequestMessage httpRequestMessage, [Inject] IRoleService _roleService, [Inject] IErrorHandler _errorHandler, int Id)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK, _roleService.GetById(Id));
        }
    }
}
