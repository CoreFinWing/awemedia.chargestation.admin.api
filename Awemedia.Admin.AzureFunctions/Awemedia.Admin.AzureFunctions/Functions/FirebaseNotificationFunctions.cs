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
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using AzureFunctions.Autofac;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Chargestation.AzureFunctions.Helpers;
using System.Linq;
using Awemedia.Chargestation.AzureFunctions.Extensions;
using Awemedia.Admin.AzureFunctions.Resolver;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class FirebaseNotificationFunctions
    {
        [FunctionName("trigger-notification")]
        public HttpResponseMessage Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "trigger-notification")] HttpRequestMessage httpRequestMessage, [Inject]INotificationService _notificationService, [Inject]IErrorHandler _errorHandler)
        {
            var paymentStatusBody = httpRequestMessage.GetBodyAsync<PaymentStatus>();
            if (!httpRequestMessage.IsAuthorized())
                return httpRequestMessage.CreateResponse(HttpStatusCode.Unauthorized);
            if (!paymentStatusBody.IsValid)
                return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, $"Model is invalid: {string.Join(", ", paymentStatusBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            PaymentStatus paymentStatus = paymentStatusBody.Value;
            var maxDuration = Environment.GetEnvironmentVariable("MaxDuration");
            if (Convert.ToInt32(paymentStatus.Duration) <= Convert.ToInt32(maxDuration))
            {
                NotificationPayload notificationPayload = new NotificationPayload
                {
                    Command = "Start Charge",
                    CommandParams = new CommandParams { Duration = paymentStatus.Duration + " mins", PortCount = paymentStatus.PortCount }
                };
                Notification paymentNotification = new Notification
                {
                    DeviceId = paymentStatus.Note,
                    LoggedDateTime = DateTime.Now,
                    Payload = notificationPayload,
                    NotificationTitle = "Payment status notification."
                };
                _notificationService.SendNotification(paymentNotification);
                return httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            }
            return httpRequestMessage.CreateErrorResponse(HttpStatusCode.BadRequest,"Validation Failed.");
        }
    }
}
