using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using OidcApiAuthorization.Abstractions;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Awemedia.chargestation.API.tests.Common
{
    public static class Common
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            return config;
        }
        public static HttpRequestMessage CreateRequest()
        {
            var context = new DefaultHttpContext
            {
                RequestServices = CreateServices(new DefaultContentNegotiator())
            };
            HttpRequestMessage request = new HttpRequestMessage();
            request.Properties.Add(nameof(HttpContext), context);
            request.Headers.CacheControl = new CacheControlHeaderValue();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJraWQiOiJrakZyaVwvdEY5N3Rod0xVQUdQWnBZUUs3MElQNkFmXC9SS1NwMDU0N2V6N1E9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiI5MjZjODViMy1iOGFlLTRjZmItOGI1Yi1lYmQ2OWNjZTE2NzAiLCJhdWQiOiIyamk3Nzg2NzdidjJ1MmRoczFuNmIxYWRtZiIsImV2ZW50X2lkIjoiYzY5ODQzOWItNzM3YS00YmExLTg4YjUtZTlhOTgyODE1ODYyIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE1NzkwNzQ0NDMsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTEuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0xX0JMN25oNVJzbiIsImNvZ25pdG86dXNlcm5hbWUiOiI5MjZjODViMy1iOGFlLTRjZmItOGI1Yi1lYmQ2OWNjZTE2NzAiLCJleHAiOjE1NzkwNzgwNDMsImlhdCI6MTU3OTA3NDQ0MywiZW1haWwiOiJ0ZXN0dXNlckBhd2VtZWRpYS5jb20ifQ.QAEZFFa8jyuBRKw-iKlMKSk---orLH4TNABfnLNJKhYnuVis0vN4I9ZXQT4jHCxozIm3dr_M2nmuDyHnmhqEkXgsVHj2LzaI8ge4VS9DlgrkNJmBv0I1dYgQuffZ7eQ9cJn3tjxFWO1Bg4XAtNc9lA8J78QI5OR47bgfDUcUY0ftQaMfzAFYFIurz4HBZqYOtbEBnDAGQ-PUZy_GIwx1hiaVsNK6hbsW6RIymE1PwX3PRBuh4X-nf30Omlhz5S8vdJYtTYUs2VB8cC5eRV8_pLHozgMePthvWrBdBDQNwAoseK7pfKabXtRkYFkFafLFch4TJzo3levmD7kenwsICw");
            return request;
        }

        private static IServiceProvider CreateServices(IContentNegotiator contentNegotiator = null, MediaTypeFormatter formatter = null)
        {
            var options = new WebApiCompatShimOptions();

            if (formatter == null)
            {
                options.Formatters.AddRange(new MediaTypeFormatterCollection());
            }
            else
            {
                options.Formatters.Add(formatter);
            }

            var optionsAccessor = new Mock<IOptions<WebApiCompatShimOptions>>();
            optionsAccessor.SetupGet(o => o.Value).Returns(options);

            var services = new Mock<IServiceProvider>(MockBehavior.Strict);
            services
                .Setup(s => s.GetService(typeof(IOptions<WebApiCompatShimOptions>)))
                .Returns(optionsAccessor.Object);

            if (contentNegotiator != null)
            {
                services
                    .Setup(s => s.GetService(typeof(IContentNegotiator)))
                    .Returns(contentNegotiator);
            }

            return services.Object;
        }

        public static T SetAuth<T>(bool success = true, string claimValue = "owner") where T : class
        {
            var auth = new Mock<IApiAuthorization>();
            if (success)
            {
                var ci = new ClaimsIdentity();
                ci.AddClaim(new Claim("extension_UserRoles", claimValue));
                ci.AddClaim(new Claim("emails", "baljeet@awepay.com"));
                var claims = new ClaimsPrincipal(ci);

                auth
                    .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                    .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult(claims)));
            }
            else
            {
                auth
                    .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                    .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult("BadLogin")));
            }
            return (T)Activator.CreateInstance(typeof(T), auth.Object);
        }
    }
}
