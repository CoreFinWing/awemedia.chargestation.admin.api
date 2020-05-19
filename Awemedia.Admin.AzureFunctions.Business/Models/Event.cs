using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string EventData { get; set; }
        public string DateTime { get; set; }
        public bool IsActive { get; set; }
        public int EventTypeId { get; set; }
        public string DeviceId { get; set; }
        public Guid? ChargeStationId { get; set; }
        public string EventName { get; set; }
        public DateTime? ServerDateTime { get; set; }
    }
}
