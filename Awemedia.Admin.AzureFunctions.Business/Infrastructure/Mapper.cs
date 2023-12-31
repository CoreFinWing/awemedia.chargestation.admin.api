﻿using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Promotion = Awemedia.Admin.AzureFunctions.Business.Models.Promotion;
using UserSession = Awemedia.Admin.AzureFunctions.DAL.DataContracts.UserSession;

namespace Awemedia.Admin.AzureFunctions.Business.Infrastructure
{
    public class MappingProfile
    {
        private static string malaysiaTimeZone = Environment.GetEnvironmentVariable("malaysia_time_zone");
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
                IsOnline = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(chargeStation.ModifiedDate, malaysiaTimeZone)) >= Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(DateTime.Now.ToUniversalTime(), malaysiaTimeZone)).AddMinutes(-Convert.ToDouble(Environment.GetEnvironmentVariable("chargestation_online_check_interval_mins"))) ? true : false,
                LastPingTimeStamp = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(chargeStation.LastPingTimeStamp.GetValueOrDefault(), malaysiaTimeZone)),
                BatteryInfoDisplayField = !string.IsNullOrEmpty(chargeStation.BatteryLevel) ? (chargeStation.BatteryLevel == null ? null : chargeStation.BatteryLevel + " as of " + chargeStation.LastPingTimeStamp.GetValueOrDefault().ToString("yyyy-MM-dd hh:mm:ss tt")) : null,
                IsActive = chargeStation.IsActive,
                userSessions = MapSessionList(chargeStation.UserSession),
                AppVersion = chargeStation.AppVersion,
                LastBatteryInfoDisplayField = chargeStation.BatteryLevel == "n/a" ? (chargeStation.LastBatteryLevel == null ? null : chargeStation.LastBatteryLevel + " as of " + chargeStation.LastBatteryLevelAvailablityTime.GetValueOrDefault().ToString("yyyy-MM-dd hh:mm:ss tt")) : null
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
                Price = chargeOptions.Price
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
                CountryId = chargeOptionsResponse.CountryId
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
            if (merchant == null)
                return null;
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
                CreatedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(merchant.CreatedDate.GetValueOrDefault(), malaysiaTimeZone)),
                ModifiedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(merchant.ModifiedDate.GetValueOrDefault(), malaysiaTimeZone)),
                Branch = MapBranchModelsObject(merchant.Branch.ToList()),
                IsActive = merchant.IsActive,
                NumOfActiveLocations = merchant.NumOfActiveLocations
            };
        }

        public static Models.UserModel MapUserModelObject(DAL.DataContracts.User user)
        {
            if (user == null)
                return null;
            return new Models.UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                City = user.City,
                Country = MapCountryModelObject(user.Country),
                CountryId = user.CountryId,
                Mobile = user.Mobile,
                PostalCode = user.PostalCode,
                RoleName = user.Role?.DisplayName,
                RoleId = user.RoleId,
                State = user.State,
                CreatedDate = user.CreatedDate.Value,
                MappedMerchant = user.MappedMerchant.Select(x => new MappedMerchant { Id = x.MerchantId, Name = x.Merchant?.BusinessName }).ToList(),
                ModifiedDate = user.ModifiedDate.Value
            };
        }

        public static Models.RoleModel MapRoleModelObject(DAL.DataContracts.Role role)
        {
            if (role == null)
            {
                return null;
            }
            return new Models.RoleModel()
            {
                Id = role.Id,
                Name = role.Name,
                DisplayName = role.DisplayName,
                IsActive = role.IsActive
            };
        }

        public static Models.Country MapCountryModelObject(DAL.DataContracts.Country country)
        {
            if (country == null)
                return null;
            return new Models.Country()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName,
                Currency = country.Currency
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

        public static DAL.DataContracts.User MapUserObject(Models.UserModel user, DAL.DataContracts.User _user, string userId)
        {
            _user.Id = user.Id;
            _user.Email = user.Email;
            _user.Name = user.Name;
            _user.Mobile = user.Mobile;
            _user.PostalCode = user.PostalCode;
            _user.Role = MapRoleObject(user.Role);
            _user.RoleId = user.RoleId;
            _user.State = user.State;
            _user.City = user.City;
            _user.CountryId = user.CountryId;
            _user.CreatedDate = user.Id == 0 ? DateTime.Now.ToUniversalTime() : user.CreatedDate.ToUniversalTime();
            _user.ModifiedDate = DateTime.Now.ToUniversalTime();
            _user.UserId = userId;
            _user.MappedMerchant = new List<UserMerchantMapping>();
            foreach (var merchantMapping in user.MappedMerchant)
            {
                _user.MappedMerchant.Add(new UserMerchantMapping() 
                {
                    UserId=_user.Id,
                    MerchantId=merchantMapping.Id,
                    CreatedDate=DateTime.Now
                });
            }
            return _user;
        }

        public static DAL.DataContracts.User MapUpdateUserObject(Models.UserModel user, DAL.DataContracts.User _user, string userId)
        {
            _user.Id = user.Id;
            _user.Email = user.Email;
            _user.Name = user.Name;
            _user.Mobile = user.Mobile;
            _user.PostalCode = user.PostalCode;
            _user.Role = MapRoleObject(user.Role);
            _user.RoleId = user.RoleId;
            _user.State = user.State;
            _user.City = user.City;
            _user.CountryId = user.CountryId;
            _user.CreatedDate = user.Id == 0 ? DateTime.Now.ToUniversalTime() : user.CreatedDate.ToUniversalTime();
            _user.ModifiedDate = DateTime.Now.ToUniversalTime();
            _user.UserId = userId;
            return _user;
        }

        public static Role MapRoleObject(RoleModel role)
        {
            if (role == null)
            {
                return null;
            }
            return new Role()
            {
                Id = role.Id,
                Name = role.Name,
                DisplayName = role.DisplayName,
                IsActive = role.IsActive
            };
        }
        public static ADB2C.Models.AweMediaUser MapAweMediaUserObject(Models.UserModel user, ADB2C.Models.AweMediaUser _user)
        {
            _user.Id = user.Id;
            _user.Email = user.Email;
            _user.Name = user.Name;
            //_user.MappedMerchant = user.MappedMerchant;
            _user.Mobile = user.Mobile;
            _user.PostalCode = user.PostalCode;
            _user.State = user.State;
            _user.City = user.City;
            _user.CreatedDate = user.Id == 0 ? DateTime.Now.ToUniversalTime() : user.CreatedDate.ToUniversalTime();
            _user.ModifiedDate = DateTime.Now.ToUniversalTime();
            return _user;
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
                            IsActive = item.IsActive,
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
            _branch.City = branch.City;
            _branch.PostalCode = branch.PostalCode;
            _branch.CountryId = branch.CountryId;
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
                City = branch.City,
                PostalCode = branch.PostalCode,
                CountryId = branch.CountryId,
                CountryName = branch.Country != null ? branch.Country.CountryName : "",
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
                TransactionTypeId = userSession.TransactionTypeId,
                Mobile = userSession.Mobile,
                ModifiedDate = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(userSession.ModifiedDate.GetValueOrDefault(), malaysiaTimeZone)),
                SessionEndTime = userSession.SessionEndTime != null ? Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(userSession.SessionEndTime.GetValueOrDefault(), malaysiaTimeZone)) : (DateTime?)null,
                SessionStartTime = userSession.SessionStartTime != null ? Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(userSession.SessionStartTime.GetValueOrDefault(), malaysiaTimeZone)) : (DateTime?)null,
                SessionStatus = userSession.SessionStatusNavigation.Status,
                SessionType = userSession.SessionTypeNavigation?.Type,
                TransactionId = userSession.TransactionId,
                UserAccountId = userSession.UserAccountId,
                MerchantName = userSession.ChargeStation.Branch?.Merchant.BusinessName,
                BranchName = userSession.ChargeStation.Branch?.Name
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
                            TransactionTypeId = userSession.TransactionTypeId,
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

        public static Event MapEventsResponseObject(DAL.DataContracts.Events events)
        {
            return new Event()
            {
                DateTime = events.DateTime,
                DeviceId = events.DeviceId,
                EventData = events.EventData,
                EventTypeId = events.EventTypeId,
                IsActive = events.IsActive,
                Id = events.Id,
                ChargeStationId = events.ChargeStationId,
                EventName = events.EventType?.Name,
                ServerDateTime = events.ServerDateTime != null ? Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(events.ServerDateTime.GetValueOrDefault(), malaysiaTimeZone)) : (DateTime?)null,
            };
        }

        public static Promotion MapPromotionModelObject(DAL.DataContracts.Promotion promotion)
        {
            return new Promotion()
            {
                Id = promotion.Id,
                PromotionDesc = promotion.PromotionDesc,
                BranchId = promotion.BranchId,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                BranchName = promotion.Branch?.Name,
                PromotionType = promotion.PromotionType,
                Mobile = promotion.Mobile,
                IsActive = promotion.IsActive
            };
        }

        public static DAL.DataContracts.Promotion MapPromotionObject(Models.Promotion promotion)
        {
            return new DAL.DataContracts.Promotion()
            {
                Id = promotion.Id,
                PromotionDesc = promotion.PromotionDesc,
                BranchId = promotion.BranchId,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                PromotionType = promotion.PromotionType,
                Mobile = promotion.Mobile,
                IsActive = promotion.IsActive
            };
        }
    }
}
