using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Newtonsoft.Json;
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
                RegisteredBusinessName = merchant.BusinessName,
                ChargeStationsOrdered = merchant.ChargeStationsOrdered,
                Dba = merchant.Dba,
                DepositMoneyPaid = merchant.DepositMoneyPaid,
                Email = merchant.Email,
                Id = merchant.Id,
                IndustryName = merchant.IndustryType == null ? null : merchant.IndustryType.Name,
                IndustryTypeId = merchant.IndustryTypeId,
                LicenseNumber = merchant.LicenseNum,
                ContactName = merchant.ContactName,
                PhoneNumber = merchant.PhoneNum,
                ProfitSharePercentage = merchant.ProfitSharePercentage,
                SecondaryContact = merchant.SecondaryContact,
                SecondaryPhone = merchant.SecondaryPhone,
                CreatedDate = merchant.CreatedDate.GetValueOrDefault(),
                ModifiedDate = merchant.ModifiedDate.GetValueOrDefault(),
                Branch = MapBranchModelsObject(merchant.Branch.ToList()),
                IsActive = merchant.IsActive,
                NumOfActiveLocations = merchant.NumOfActiveLocations
            };
        }
        public static DAL.DataContracts.Merchant MapMerchantObject(Models.Merchant merchant, DAL.DataContracts.Merchant _merchant)
        {
            _merchant.BusinessName = merchant.RegisteredBusinessName;
            _merchant.ChargeStationsOrdered = merchant.ChargeStationsOrdered;
            _merchant.Dba = merchant.Dba;
            _merchant.DepositMoneyPaid = merchant.DepositMoneyPaid;
            _merchant.Email = merchant.Email;
            _merchant.Id = merchant.Id;
            _merchant.IndustryTypeId = merchant.IndustryTypeId;
            _merchant.LicenseNum = merchant.LicenseNumber;
            _merchant.ContactName = merchant.ContactName;
            _merchant.PhoneNum = merchant.PhoneNumber;
            _merchant.ProfitSharePercentage = merchant.ProfitSharePercentage;
            _merchant.SecondaryContact = merchant.SecondaryContact;
            _merchant.SecondaryPhone = merchant.SecondaryPhone;
            _merchant.CreatedDate = merchant.Id == 0 ? DateTime.Now : merchant.CreatedDate;
            _merchant.ModifiedDate = DateTime.Now;
            _merchant.IsActive = true;
            return _merchant;
        }
        public static ICollection<Models.Branch> MapBranchModelsObject(List<DAL.DataContracts.Branch> branch)
        {
            Models.Branch _branch = null;
            List<Models.Branch> branches = new List<Models.Branch>();
            if (branch != null)
            {
                foreach (var item in branch)
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

        public static DAL.DataContracts.Branch MapBranchObject(Models.Branch branch, DAL.DataContracts.Branch _branch)
        {

            _branch.Address = branch.Address;
            _branch.ContactName = branch.ContactName;
            _branch.CreatedDate = DateTime.Now;
            _branch.Email = branch.Email;
            _branch.Geolocation = branch.Geolocation;
            _branch.MerchantId = branch.MerchantId;
            _branch.ModifiedDate = DateTime.Now;
            _branch.Name = branch.Name;
            _branch.PhoneNum = branch.PhoneNum;
            _branch.Id = branch.Id;
            _branch.IsActive = true;
            return _branch;

        }
        public static Models.Branch MapBranchModelObject(DAL.DataContracts.Branch branch)
        {
            return new Models.Branch()
            {
                Address = branch.Address,
                ContactName = branch.ContactName,
                CreatedDate = branch.CreatedDate.GetValueOrDefault(),
                Email = branch.Email,
                Geolocation = branch.Geolocation,
                Id = branch.Id,
                MerchantId = branch.MerchantId,
                ModifiedDate = branch.ModifiedDate.GetValueOrDefault(),
                Name = branch.Name,
                PhoneNum = branch.PhoneNum,
            };
        }
        public static Guid StringToGuid(string value)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(hash);
        }
        public static DAL.DataContracts.Notification MapNotificationResponseObject(Business.Models.Notification notificationResponse)
        {
            return new DAL.DataContracts.Notification()
            {
                DeviceId = notificationResponse.DeviceId,
                LoggedDateTime = DateTime.Now,
                DeviceToken = notificationResponse.DeviceToken,
                NotificationResult = notificationResponse.NotificationResult,
                Payload = JsonConvert.SerializeObject(notificationResponse.Payload)
            };
        }
    }
}
