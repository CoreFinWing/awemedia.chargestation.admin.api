using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IMerchantService
    {
        IEnumerable<Merchant> Get(BaseSearchFilter chargeStationSearchFilter);
        int AddChargeStation(Merchant merchantModel, int id = 0);
        int UpdateChargeStation(Merchant merchantModel, int id);
        object IsMerchantExists(int id);
    }
}
