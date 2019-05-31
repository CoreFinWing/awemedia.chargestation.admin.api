﻿using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IChargeStationService
    {
        IEnumerable<ChargeStation> Get(BaseSearchFilter chargeStationSearchFilter);
        Guid AddChargeStation(ChargeStation chargeStation, Guid guid = new Guid());
        Guid UpdateChargeStation(ChargeStation chargeStation,Guid guid);
        object IsChargeStationExists(Guid guid);
    }
}
