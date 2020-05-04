using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IEventService
    {
        IEnumerable<object> Get(BaseSearchFilter chargeStationSearchFilter, out int totalRecords);
        object GetById(int id);
    }
}
