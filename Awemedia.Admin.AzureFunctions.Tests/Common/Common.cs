using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Awemedia.chargestation.API.tests.Common
{
    public static class Common
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", optional: true)
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
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "bW9iaWxlYXBwOmRHVnpkQT09");
            return request;
        }

        public static IServiceProvider CreateServices(IContentNegotiator contentNegotiator = null, MediaTypeFormatter formatter = null)
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
    }
}
