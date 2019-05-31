using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Infrastructure
{
    public class MappingProfile
    {
        public static Models.ChargeStation MapChargeStationResponseObject(DAL.DataContracts.ChargeStation chargeStation)
        {
            return new Models.ChargeStation()
            {
                ChargeControllerId = chargeStation.ChargeControllerId,
                CreatedDate = chargeStation.CreatedDate,
                Geolocation = chargeStation.Geolocation,
                Id = chargeStation.Id.ToString(),
                MerchantId = chargeStation.MerchantId,
                ModifiedDate = chargeStation.ModifiedDate,
                DeviceId = chargeStation.DeviceId,
                Uid = chargeStation.Uid
            };
        }
        public static Models.ChargeOption MapChargeOptionsResponseObjects(DAL.DataContracts.ChargeOptions chargeOptions)
        {
            return new Models.ChargeOption()
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
        public static DAL.DataContracts.ChargeOptions MapChargeOptionsObjects(Models.ChargeOption chargeOptionsResponse)
        {
            return new DAL.DataContracts.ChargeOptions()
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
        public static DAL.DataContracts.ChargeStation MapChargeStationObject(Models.ChargeStation chargeStationResponse)
        {
            return new DAL.DataContracts.ChargeStation()
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
        public static Models.Merchant MapMerchantModelObject(DAL.DataContracts.Merchant merchant)
        {
            return new Models.Merchant()
            {
                BusinessName = merchant.BusinessName,
                ChargeStationsOrdered = merchant.ChargeStationsOrdered,
                Dba = merchant.Dba,
                DepositMoneyPaid = merchant.DepositMoneyPaid,
                Email = merchant.Email,
                Id = merchant.Id,
                IndustryName = merchant.IndustryType.Name,
                IndustryTypeId = merchant.IndustryTypeId,
                LicenseNum = merchant.LicenseNum,
                ContactName = merchant.ContactName,
                PhoneNum = merchant.PhoneNum,
                ProfitSharePercentage = merchant.ProfitSharePercentage,
                SecondaryContact = merchant.SecondaryContact,
                SecondaryPhone = merchant.SecondaryPhone,
                CreatedDate=merchant.CreatedDate.GetValueOrDefault(),
                ModifiedDate=merchant.ModifiedDate.GetValueOrDefault(),
                Branch = MapBranchModelObject(merchant.Branch.ToList())
            };
        }
        public static ICollection<Models.Branch> MapBranchModelObject(List<DAL.DataContracts.Branch> branch)
        {
            Models.Branch _branch = null;
            List<Models.Branch> branches = new List<Models.Branch>();
            foreach (var item in branch)
            {
                if (branch != null)
                {
                    if (branch.Any())
                    {
                        _branch = new Models.Branch
                        {
                            Address = item.Address,
                            ContactName = item.ContactName,
                            CreatedDate = item.CreatedDate.GetValueOrDefault(),
                            Email = item.Email,
                            Geolocation = item.Geolocation,
                            Id = item.Id,
                            MerchantId = item.MerchantId,
                            ModifiedDate = item.ModifiedDate.GetValueOrDefault(),
                            Name = item.Name,
                            PhoneNum = item.PhoneNum
                        };
                        branches.Add(_branch);
                    }
                }
            }
            return branches;

        }
       
        private static Guid StringToGuid(string value)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(hash);
        }
    }
}
