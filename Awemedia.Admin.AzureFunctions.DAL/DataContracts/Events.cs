using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class Events
    {
        public int Id { get; set; }
        public string EventData { get; set; }
        public string DateTime { get; set; }
        public bool IsActive { get; set; }
        public int EventTypeId { get; set; }
        public string DeviceId { get; set; }
        public Guid? ChargeStationId { get; set; }
        public DateTime? ServerDateTime { get; set; }
        public EventType EventType { get; set; }
    }
}
