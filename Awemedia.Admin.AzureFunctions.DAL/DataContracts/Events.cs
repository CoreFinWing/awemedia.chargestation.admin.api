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
        public Guid DeviceId { get; set; }

        public ChargeStation Device { get; set; }
        public EventType EventType { get; set; }
    }
}
