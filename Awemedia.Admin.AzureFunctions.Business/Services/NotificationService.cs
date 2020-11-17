using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IBaseService<DAL.DataContracts.Notification> _baseService;
        private readonly IChargeStationService _chargeStationService;

        public NotificationService(IBaseService<DAL.DataContracts.Notification> baseService, IChargeStationService chargeStationService)
        {
            _baseService = baseService;
            _chargeStationService = chargeStationService;
        }

        public void SendNotification(Models.Notification notification)
        {
            string result = string.Empty;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Environment.GetEnvironmentVariable("fcm_web_address"));
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", Environment.GetEnvironmentVariable("fcm_server_key")));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", Environment.GetEnvironmentVariable("fcm_sender_id")));
            httpWebRequest.Method = "POST";
            var device = _chargeStationService.GetById(MappingProfile.StringToGuid(notification.DeviceId));
            if (device != null)
            {
                notification.DeviceToken = device.DeviceToken;
                var payload = new
                {
                    to = notification.DeviceToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = notification.Payload,
                        title = notification.NotificationTitle
                    },
                };
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(payload, Formatting.Indented);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                    notification.NotificationResult = result;
                }
                _baseService.AddOrUpdate(MappingProfile.MapNotificationResponseObject(notification), 0);
            }
        }
    }
}
