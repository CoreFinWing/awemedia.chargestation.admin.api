using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace Awemedia.Chargestation.AzureFunctions.Extensions
{
    public class HttpResponseBody<T>
    {
        public bool IsValid { get; set; }
        public T Value { get; set; }

        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }

    public static class ModelValidationExtension
    {
        public static HttpResponseBody<T> GetBodyAsync<T>(this HttpRequestMessage request)
        {
            var body = new HttpResponseBody<T>();
            var bodyString = request.Content.ReadAsStringAsync().Result;
            body.Value = JsonConvert.DeserializeObject<T>(bodyString);
            var results = new List<ValidationResult>();
            body.IsValid = Validator.TryValidateObject(body.Value, new ValidationContext(body.Value, null, null), results, true);
            body.ValidationResults = results;
            return body;
        }
    }
}
