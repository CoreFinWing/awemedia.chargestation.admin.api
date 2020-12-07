using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
namespace Awemedia.Chargestation.AzureFunctions.Extensions
{
    public static class CacheResponseExtensions
    {
        public static HttpResponseMessage CreateCachedResponse<T>(this HttpRequestMessage request, HttpStatusCode statusCode, T value, TimeSpan? maxAge = null)
        {
            HttpResponseMessage responseMessage = request.CreateResponse<T>(statusCode, value);
            responseMessage.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = maxAge.Value
            };
            return responseMessage;
        }
        public static HttpResponseMessage CreateResponseWithData<T>(this HttpRequestMessage requestMessage, HttpStatusCode statusCode, T content) { return new HttpResponseMessage() { StatusCode = statusCode, Content = new StringContent(JsonConvert.SerializeObject(content)) }; }
    }
}
