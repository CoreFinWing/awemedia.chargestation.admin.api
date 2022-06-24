using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awemedia.Admin.AzureFunctions.Functions
{

    public class UserFunction
    {
        private string tenant = Environment.GetEnvironmentVariable("b2c-Tenant");
        private string clientId = Environment.GetEnvironmentVariable("b2c-ClientId");
        private string clientSecret = Environment.GetEnvironmentVariable("b2c-ClientSecret");
        private string FromName = Environment.GetEnvironmentVariable("email-name");
        private string FromEmail = Environment.GetEnvironmentVariable("email-id");
        private string APIKey = Environment.GetEnvironmentVariable("email-apikey");

        [FunctionName("AddUser")]
        public async Task<HttpResponseMessage> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "users")] HttpRequestMessage httpRequestMessage)
        {
            var userModel = httpRequestMessage.GetBodyAsync<UserModel>();
            var userBody = httpRequestMessage.GetBodyAsync<Value>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            Value user = userBody.Value;
            UserProfile profile = new UserProfile();
            profile.accountEnabled = true;
            profile.signInNames = new List<SignInName>();
            profile.signInNames.Add(new SignInName()
            {
                type = "emailAddress",
                value = userModel.Value.Email
            });
            profile.creationType = "LocalAccount";
            profile.displayName = user.givenName;
            profile.mailNickname = user.givenName;
            profile.passwordProfile = new Passwordprofile();
            profile.passwordProfile.password = RandomPassword.GetRandomPassword(10);
            profile.passwordProfile.forceChangePasswordNextLogin = true;
            profile.passwordPolicies = "DisablePasswordExpiration";
            profile.city = userModel.Value.city;
            profile.country = userModel.Value.country;
            profile.givenName = user.givenName;
            profile.mail = userModel.Value.mail;
            profile.mobile = userModel.Value.mobile;
            profile.postalCode = user.postalCode;
            profile.state = userModel.Value.state;
            profile.streetAddress = userModel.Value.streetAddress;
            profile.surname = userModel.Value.surname;
            profile.UserRoles = user.UserRoles;
            profile.otherMails = new List<string>();
            B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);
            await client.CreateUser(JsonConvert.SerializeObject(profile));
            EmailSenderConfig config = new EmailSenderConfig();
            config.APIKey = APIKey;
            config.FromName = FromName;
            config.FromEmail = FromEmail;
            config.ToEmail = userModel.Value.Email;
            config.ToName = userModel.Value.givenName;
            config.Subject = "Your AweMedia account created successfully";
            config.TextBody = string.Format("Hi {0}, You account has been created successfully on awemedia. Please use the password {1} and your email to login",config.ToName, profile.passwordProfile.password);
            config.HtmlBody = string.Format("Hi {0}, <br/><br/> You account has been created successfully on awemedia. <br/> <br/><br/> Please use the password <b>{1}</b> and your email to login", config.ToName, profile.passwordProfile.password);
            SendGridEmailHelper.SendEmail(config);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }


        [FunctionName("GetUsers")]
        public async Task<HttpResponseMessage> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequestMessage httpRequestMessage)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);
            var res = await client.GetAllUsers(null);
            GetUserResponse response = JsonConvert.DeserializeObject<GetUserResponse>(res);
           
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK,new {data=response.value, total= response.value.Length});
        }

        [FunctionName("GetUsersId")] 
        public async Task<HttpResponseMessage> GetUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{Id}")] HttpRequestMessage httpRequestMessage, string Id)
        {
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);
            var res = await client.GetUserByObjectId(Id);
            Value response = JsonConvert.DeserializeObject<Value>(res);
            return httpRequestMessage.CreateResponseWithData(HttpStatusCode.OK,  response );
        }

        [FunctionName("UpdateUser")]
        public async Task<HttpResponseMessage> Put(
           [HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "users/{id}")] HttpRequestMessage httpRequestMessage, string id)
        {
            var userBody = httpRequestMessage.GetBodyAsync<Value>();
            var userModel = httpRequestMessage.GetBodyAsync<UserModel>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (string.IsNullOrEmpty(id))
                httpRequestMessage.CreateResponse(HttpStatusCode.BadRequest);
            Value user = userBody.Value;

            UserProfile profile = new UserProfile();
            profile.signInNames = new List<SignInName>();
            profile.signInNames.Add(new SignInName()
            {
                type = "emailAddress",
                value = userModel.Value.Email
            });
            profile.displayName = userModel.Value.givenName;
            profile.mailNickname = user.givenName;
            profile.city = userModel.Value.city;
            profile.country = userModel.Value.country;
            profile.givenName = user.givenName;
            profile.mail = userModel.Value.mail;
            profile.mobile = userModel.Value.mobile;
            profile.postalCode = user.postalCode;
            profile.state = userModel.Value.state;
            profile.streetAddress = userModel.Value.streetAddress;
            profile.surname = userModel.Value.surname;
            profile.UserRoles = user.UserRoles;
            profile.otherMails = new List<string>();
            B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);
            string userData = JsonConvert.SerializeObject(profile);
            await client.UpdateUser(id, userData);
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
    }
}
