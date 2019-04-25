using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IChargeStationService
    {
        IEnumerable<ChargeStationResponse> GetAll();
        IEnumerable<ChargeStationResponse> GetFiltered(BaseFilterRequest baseFilterRequest);
    }
}
