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
        
        [FunctionName("AddUser")]
        public async Task<HttpResponseMessage> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "users")] HttpRequestMessage httpRequestMessage)
        {
            var userModel = httpRequestMessage.GetBodyAsync<UserModel>();
            if (!httpRequestMessage.IsAuthorized())
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            }
            UserProfile profile = new UserProfile();
            profile.accountEnabled = true;
            profile.signInNames = new List<SignInName>();
            profile.signInNames.Add(new SignInName()
            {
                type = "emailAddress",
                value = userModel.Value.emailAddress
            });
            profile.creationType = "LocalAccount";
            profile.displayName = userModel.Value.displayName;
            profile.mailNickname = userModel.Value.mailNickname;
            profile.passwordProfile = new Passwordprofile();
            profile.passwordProfile.password = userModel.Value.password;
            profile.passwordProfile.forceChangePasswordNextLogin = false;
            profile.passwordPolicies = "DisablePasswordExpiration";
            profile.city = userModel.Value.city;
            profile.country = userModel.Value.country;
            profile.givenName = userModel.Value.givenName;
            profile.mail = userModel.Value.mail;
            profile.mobile = userModel.Value.mobile;
            profile.postalCode = userModel.Value.postalCode;
            profile.state = userModel.Value.state;
            profile.streetAddress = userModel.Value.streetAddress;
            profile.surname = userModel.Value.surname;
            profile.otherMails = new List<string>();
            B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);
            await client.CreateUser(JsonConvert.SerializeObject(profile));
            return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
        }
    }
}
