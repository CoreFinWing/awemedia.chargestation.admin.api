﻿using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IChargeStationService
    {
        IEnumerable<ChargeStation> Get(ChargeStationSearchFilter chargeStationSearchFilter);
        Guid AddChargeStation(ChargeStation chargeStation, Guid guid = new Guid());
        object IsChargeStationExists(Guid guid);
    }
}
