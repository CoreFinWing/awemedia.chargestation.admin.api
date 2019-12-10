using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UserSession = Awemedia.Admin.AzureFunctions.DAL.DataContracts.UserSession;

namespace Awemedia.Admin.AzureFunctions.Business.Infrastructure
{
    public class MappingProfile
    {
        private static string malaysiaTimeZone = Environment.GetEnvironmentVariable("MalaysiaTimeZone");
        public static Models.ChargeStation MapChargeStationResponseObject(DAL.DataContracts.ChargeStation chargeStation)
        {
            return new Models.ChargeStation()
            {
                ChargeControllerId = chargeStation.ChargeControllerId,
                CreatedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(chargeStation.CreatedDate, malaysiaTimeZone)),
                Geolocation = chargeStation.Geolocation,
                Id = chargeStation.Id.ToString(),
                BranchId = chargeStation.BranchId ?? 0,
                ModifiedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(chargeStation.ModifiedDate, malaysiaTimeZone)),
                DeviceId = chargeStation.DeviceId,
                Uid = chargeStation.Uid,
                BranchName = chargeStation.Branch?.Name,
                MerchantName = chargeStation.Branch?.Merchant.BusinessName,
                Branch = chargeStation.Branch == null ? null : MapBranchModelObject(chargeStation.Branch),
                BatteryLevel = chargeStation.BatteryLevel,
                IsOnline = chargeStation.ModifiedDate >= Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(DateTime.Now.ToUniversalTime(), malaysiaTimeZone)).AddMinutes(Convert.ToDouble(Environment.GetEnvironmentVariable("OnlineChargeStationInterval"))) ? true : false,
                LastPingTimeStamp = chargeStation.LastPingTimeStamp,
                BatteryInfoDisplayField = !string.IsNullOrEmpty(chargeStation.BatteryLevel) ? chargeStation.BatteryLevel + " as of " + chargeStation.LastPingTimeStamp : "",
                IsActive = chargeStation.IsActive,
                userSessions = MapSessionList(chargeStation.UserSession)
            };
        }
        public static Models.ChargeOption MapChargeOptionsResponseObjects(DAL.DataContracts.ChargeOptions chargeOptions)
        {
            return new Models.ChargeOption()
            {
                ChargeDuration = chargeOptions.ChargeDuration,
                CreatedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(chargeOptions.CreatedDate, malaysiaTimeZone)),
                Currency = chargeOptions.Currency,
                Id = chargeOptions.Id,
                IsActive = chargeOptions.IsActive,
                ModifiedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(chargeOptions.ModifiedDate, malaysiaTimeZone)),
                Price = chargeOptions.Price,
            };
        }
        public static DAL.DataContracts.ChargeOptions MapChargeOptionsObjects(Models.ChargeOption chargeOptionsResponse)
        {
            return new DAL.DataContracts.ChargeOptions()
            {
                ChargeDuration = chargeOptionsResponse.ChargeDuration,
                CreatedDate = DateTime.Now.ToUniversalTime(),
                Currency = chargeOptionsResponse.Currency,
                Id = chargeOptionsResponse.Id,
                IsActive = true,
                ModifiedDate = DateTime.Now.ToUniversalTime(),
                Price = chargeOptionsResponse.Price,
            };
        }
        public static DAL.DataContracts.ChargeStation MapChargeStationObject(Models.ChargeStation chargeStationResponse)
        {
            return new DAL.DataContracts.ChargeStation()
            {
                ChargeControllerId = chargeStationResponse.ChargeControllerId,
                CreatedDate = DateTime.Now.ToUniversalTime(),
                Geolocation = chargeStationResponse.Geolocation,
                Id = StringToGuid(chargeStationResponse.DeviceId),
                BranchId = chargeStationResponse.BranchId,
                ModifiedDate = DateTime.Now.ToUniversalTime(),
                DeviceId = chargeStationResponse.DeviceId,
                IsActive = true
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
                IndustryName = merchant.IndustryType?.Name,
                IndustryTypeId = merchant.IndustryTypeId,
                LicenseNumber = merchant.LicenseNum,
                ContactName = merchant.ContactName,
                PhoneNumber = merchant.PhoneNum,
                ProfitSharePercentage = merchant.ProfitSharePercentage,
                SecondaryContact = merchant.SecondaryContact,
                SecondaryPhone = merchant.SecondaryPhone,
                CreatedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(merchant.CreatedDate.GetValueOrDefault(),malaysiaTimeZone)),
                ModifiedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(merchant.ModifiedDate.GetValueOrDefault(), malaysiaTimeZone)),
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
            _merchant.CreatedDate = merchant.Id == 0 ? DateTime.Now.ToUniversalTime() : merchant.CreatedDate.ToUniversalTime();
            _merchant.ModifiedDate = DateTime.Now.ToUniversalTime();
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
                            PhoneNum = item.PhoneNum,
                            ChargeStation = MapChargeStationsModelsObject(item.ChargeStation)
                        };
                        branches.Add(_branch);
                    }
                }
            }
            return branches;

        }

        public static ICollection<Models.ChargeStation> MapChargeStationsModelsObject(ICollection<DAL.DataContracts.ChargeStation> chargeStation)
        {
            Models.ChargeStation _chargeStation = null;
            List<Models.ChargeStation> chargeStations = new List<Models.ChargeStation>();
            if (chargeStation != null)
            {
                foreach (var item in chargeStation)
                {
                    if (chargeStation.Any())
                    {
                        _chargeStation = new Models.ChargeStation
                        {
                            ChargeControllerId = item.ChargeControllerId,
                            CreatedDate = item.CreatedDate,
                            Geolocation = item.Geolocation,
                            Id = item.Id.ToString(),
                            BranchId = item.BranchId ?? 0,
                            ModifiedDate = item.ModifiedDate,
                            DeviceId = item.DeviceId,
                            Uid = item.Uid,
                            MerchantName = item.Branch?.Merchant.BusinessName,
                            BatteryLevel = item.BatteryLevel,
                            IsOnline = item.IsOnline,
                            LastPingTimeStamp = item.LastPingTimeStamp,
                            BatteryInfoDisplayField = !string.IsNullOrEmpty(item.BatteryLevel) ? item.BatteryLevel + " as of " + item.LastPingTimeStamp : "",
                            IsActive = item.IsActive
                        };
                        chargeStations.Add(_chargeStation);
                    }
                }
            }
            return chargeStations;
        }

        public static DAL.DataContracts.Branch MapBranchObject(Models.Branch branch, DAL.DataContracts.Branch _branch)
        {

            _branch.Address = branch.Address;
            _branch.ContactName = branch.ContactName;
            _branch.CreatedDate = DateTime.Now.ToUniversalTime();
            _branch.Email = branch.Email;
            _branch.Geolocation = branch.Geolocation;
            _branch.MerchantId = branch.MerchantId;
            _branch.ModifiedDate = DateTime.Now.ToUniversalTime();
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
                CreatedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(branch.CreatedDate.GetValueOrDefault(), malaysiaTimeZone)),
                Email = branch.Email,
                Geolocation = branch.Geolocation,
                Id = branch.Id,
                MerchantId = branch.MerchantId,
                ModifiedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(branch.ModifiedDate.GetValueOrDefault(), malaysiaTimeZone)),
                Name = branch.Name,
                PhoneNum = branch.PhoneNum,
                Merchant = MapMerchantModelObject(branch.Merchant),
                MerchantName = branch.Merchant.BusinessName,
                IsActive = branch.IsActive
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
                LoggedDateTime = DateTime.Now.ToUniversalTime(),
                DeviceToken = notificationResponse.DeviceToken,
                NotificationResult = notificationResponse.NotificationResult,
                Payload = JsonConvert.SerializeObject(notificationResponse.Payload)
            };
        }
        public static Models.UserSession MapUserSessionModelObject(DAL.DataContracts.UserSession userSession)
        {
            return new Models.UserSession()
            {
                AppKey = userSession.AppKey,
                ApplicationId = userSession.ApplicationId,
                ChargeParams = userSession.ChargeParams,
                ChargeRentalRevnue = userSession.ChargeRentalRevnue,
                ChargeStation = MapChargeStationResponseObject(userSession.ChargeStation),
                ChargeStationId = userSession.ChargeStationId,
                CreatedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(userSession.CreatedDate.GetValueOrDefault(), malaysiaTimeZone)),
                DeviceId = userSession.DeviceId,
                Email = userSession.Email,
                Id = userSession.Id,
                InvoiceNo = userSession.InvoiceNo,
                Mobile = userSession.Mobile,
                ModifiedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(userSession.ModifiedDate.GetValueOrDefault(), malaysiaTimeZone)),
                SessionEndTime = userSession.SessionEndTime,
                SessionStartTime = userSession.SessionStartTime,
                SessionStatus = userSession.SessionStatusNavigation.Status,
                SessionType = userSession.SessionTypeNavigation?.Type,
                TransactionId = userSession.TransactionId,
                UserAccountId = userSession.UserAccountId,
                MerchantName = userSession.ChargeStation.Branch?.Merchant.BusinessName
            };
        }
        public static ICollection<Business.Models.UserSession> MapSessionList(ICollection<DAL.DataContracts.UserSession> userSessions)
        {
            Models.UserSession _userSession = null;
            List<Models.UserSession> _userSessions = new List<Models.UserSession>();
            if (userSessions != null)
            {
                if (userSessions.Count > 0)
                {
                    foreach (var userSession in userSessions)
                    {
                        _userSession = new Models.UserSession
                        {
                            AppKey = userSession.AppKey,
                            ApplicationId = userSession.ApplicationId,
                            ChargeParams = userSession.ChargeParams,
                            ChargeRentalRevnue = userSession.ChargeRentalRevnue,
                            ChargeStationId = userSession.ChargeStationId,
                            CreatedDate = userSession.CreatedDate.GetValueOrDefault(),
                            DeviceId = userSession.DeviceId,
                            Email = userSession.Email,
                            InvoiceNo = userSession.InvoiceNo,
                            Id = userSession.Id,
                            Mobile = userSession.Mobile,
                            SessionEndTime = userSession.SessionEndTime,
                            SessionStartTime = userSession.SessionStartTime,
                            SessionStatus = userSession.SessionStatusNavigation.Status,
                            SessionType = userSession.SessionTypeNavigation?.Type,
                            TransactionId = userSession.TransactionId,
                            UserAccountId = userSession.UserAccountId
                        };
                        _userSessions.Add(_userSession);
                    }
                }
            }
            return _userSessions;
        }
    }
}
