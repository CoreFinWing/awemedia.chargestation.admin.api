using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Xunit;

namespace Awemedia.Admin.AzureFunctions.Tests
{
    public class TokenValidationTest
    {
        [Fact]
        public void Test1()
        {

        }

        private const string ValidIssuer = "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_BL7nh5Rsn";
        private const string ValidAudience = "2ji778677bv2u2dhs1n6b1admf";
        private const string jwtToken = "eyJraWQiOiJrakZyaVwvdEY5N3Rod0xVQUdQWnBZUUs3MElQNkFmXC9SS1NwMDU0N2V6N1E9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiI0NGEwZDZlNS1kYzBjLTQ0NDItYWE5Mi0yZWNmZGRhZThhNzIiLCJhdWQiOiIyamk3Nzg2NzdidjJ1MmRoczFuNmIxYWRtZiIsImV2ZW50X2lkIjoiYzY2ODI3MzQtNjAyZC0xMWU5LThlYTctOTUxMzVkYTcxMjE3IiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE1NTU0MDg1MzMsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTEuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0xX0JMN25oNVJzbiIsImNvZ25pdG86dXNlcm5hbWUiOiI0NGEwZDZlNS1kYzBjLTQ0NDItYWE5Mi0yZWNmZGRhZThhNzIiLCJleHAiOjE1NTU0MTIxMzMsImlhdCI6MTU1NTQwODUzMywiZW1haWwiOiJncmlzaGF2YWxlbnN5YW5AZ21haWwuY29tIn0.tijscUdRqMHlPwiUkvlFtDFu3unT_6TS8Q7j3QAJj--CV0TmNiMpZZNqivz8Lp8Nd4HkaVXYuseJx4OH8RjESqsEdftaOcNv6b1IJvKD760zi9ear1_io5R5wgR7C1qYXDAbEZETSyovfFDqL2Fk0JZ4tPSyuSlSksJt85NqwAygAEIvTjwQID1Q12NNjpyoxACsD9v-gy4Zjp502Z4RL_3Wdyf76LBrLD9ukwl4-a5U7Sjt2KOjEbwFdepZcOwQJYQ9TjFCk50IIPMjelJ49dQ6220HyPnubf58NIde9w13yFbctSnXs-AH1TiF9H2mOG_vCb1BndUi1EhouK5RN";

       // [Fact]
        public void TokenValidation()
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience = ValidAudience,
                ValidIssuer = ValidIssuer,
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                // get JsonWebKeySet from AWS
                var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                // serialize the result
                var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                // cast the result to be the type expected by IssuerSigningKeyResolver
                return (IEnumerable<SecurityKey>)keys;
                }
            };

            //IdentityModelEventSource.ShowPII = true;
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
            Assert.Equal("expectedClaimKeyValue", principal.FindFirst("claimKey").Value);
        }
    }
}
