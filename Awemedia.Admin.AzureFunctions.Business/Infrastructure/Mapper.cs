using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System;

namespace Awemedia.Admin.AzureFunctions.Business.Infrastructure
{
    public class MappingProfile
    {
        public static ChargeStationResponse MapChargeStationResponseObject(ChargeStation chargeStation)
        {
            return new ChargeStationResponse()
            {
                ChargeControllerId = chargeStation.ChargeControllerId,
                CreatedDate = chargeStation.CreatedDate,
                Geolocation = chargeStation.Geolocation,
                Id = chargeStation.Id,
                MerchantId = chargeStation.MerchantId,
                ModifiedDate = chargeStation.ModifiedDate,
            };
        }
    }
}
