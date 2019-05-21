using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System;
using System.Security.Cryptography;
using System.Text;

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
                Id = chargeStation.Id.ToString("N"),
                MerchantId = chargeStation.MerchantId,
                ModifiedDate = chargeStation.ModifiedDate,
                DeviceId = chargeStation.DeviceId,
                Uid=chargeStation.Uid
            };
        }
        public static ChargeOptionsResponse MapChargeOptionsResponseObjects(ChargeOptions chargeOptions)
        {
            return new ChargeOptionsResponse()
            {
                ChargeDuration = chargeOptions.ChargeDuration,
                CreatedDate = chargeOptions.CreatedDate,
                Currency = chargeOptions.Currency,
                Id = chargeOptions.Id,
                IsActive = chargeOptions.IsActive,
                ModifiedDate = chargeOptions.ModifiedDate,
                Price = chargeOptions.Price,
            };
        }
        public static ChargeOptions MapChargeOptionsObjects(ChargeOptionsResponse chargeOptionsResponse)
        {
            return new ChargeOptions()
            {
                ChargeDuration = chargeOptionsResponse.ChargeDuration,
                CreatedDate = DateTime.Now,
                Currency = chargeOptionsResponse.Currency,
                Id = chargeOptionsResponse.Id,
                IsActive = true,
                ModifiedDate = DateTime.Now,
                Price = chargeOptionsResponse.Price,
            };
        }
        public static ChargeStation MapChargeStationObject(ChargeStationResponse chargeStationResponse)
        {
            return new ChargeStation()
            {
                ChargeControllerId = chargeStationResponse.ChargeControllerId,
                CreatedDate = DateTime.Now,
                Geolocation = chargeStationResponse.Geolocation,
                Id = StringToGuid(chargeStationResponse.DeviceId),
                MerchantId = chargeStationResponse.MerchantId,
                ModifiedDate = DateTime.Now,
                DeviceId = chargeStationResponse.DeviceId
            };
        }

        private static Guid StringToGuid(string value)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(hash);
        }
    }
}
