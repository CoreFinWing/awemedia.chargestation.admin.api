using Awemedia.Admin.AzureFunctions.Business.Common;
using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class ChargeSessionService : IChargeSessionService
    {
        private readonly IBaseService<DAL.DataContracts.UserSession> _baseService;
        private readonly IBaseService<DAL.DataContracts.User> _userService;
        private readonly IChargeOptionService _chargeOptionService;
        readonly string[] navigationalProperties = new string[] { "SessionStatusNavigation", "SessionTypeNavigation", };
        readonly string[] includedProperties = new string[] { "ChargeStation", "ChargeStation.Branch.Merchant" };
        public ChargeSessionService(IBaseService<DAL.DataContracts.UserSession> baseService, IBaseService<DAL.DataContracts.User> userService, IChargeOptionService chargeOptionService)
        {
            _baseService = baseService;
            _chargeOptionService = chargeOptionService;
            _userService = userService;
        }
        public IEnumerable<object> Get(BaseSearchFilter userSessionSearchFilter, out int totalRecords,string email)
        {
            string[] navigationalProps = { "SessionStatusNavigation", "SessionTypeNavigation", "ChargeStation", "ChargeStation.Branch.Merchant" };
            IEnumerable<UserSession> _userSessions = new List<UserSession>();
            DateTime fromDate = DateTime.Now.ToUniversalTime();
            DateTime toDate = DateTime.Now.ToUniversalTime();
            fromDate = Utility.ParseStartAndEndDates(userSessionSearchFilter, ref toDate);
            totalRecords = 0;
            int days = (toDate - fromDate).Days;
            if (days <= Convert.ToInt32(Environment.GetEnvironmentVariable("charge_session_initial_date_range")))
            {
                _userSessions = _baseService.Where(a => a.CreatedDate >= fromDate && a.CreatedDate <= toDate, navigationalProps).Select(t => MappingProfile.MapUserSessionModelObject(t)).ToList();

                totalRecords = _userSessions.Count();
                if (userSessionSearchFilter != null)
                {
                    if (Convert.ToInt32(userSessionSearchFilter.MerchantId) > 0)
                    {
                        _userSessions = _userSessions.Where(a => a.ChargeStation.Branch == null ? true : a.ChargeStation.Branch.MerchantId == Convert.ToInt32(userSessionSearchFilter.MerchantId)).ToList();
                        totalRecords = _userSessions.Count();
                    }
                    if (!string.IsNullOrEmpty(userSessionSearchFilter.StationId))
                    {
                        _userSessions = _userSessions.Where(a => a.ChargeStationId == Guid.Parse(userSessionSearchFilter.StationId)).ToList();
                        totalRecords = _userSessions.Count();
                    }
                    if (!string.IsNullOrEmpty(userSessionSearchFilter.Search) && !string.IsNullOrEmpty(userSessionSearchFilter.Type))
                    {
                        _userSessions = _userSessions.Where(u => u.ChargeStation.Branch == null ? false : true);
                        _userSessions = _userSessions.Search(userSessionSearchFilter.Type, userSessionSearchFilter.Search);
                        totalRecords = _userSessions.Count();
                    }
                    if (!string.IsNullOrEmpty(userSessionSearchFilter.IsStatusPaymentCompletedOrAbove))
                    {
                        if (Convert.ToBoolean(userSessionSearchFilter.IsStatusPaymentCompletedOrAbove))
                        {
                            _userSessions = _userSessions.Where(u => (u.SessionStatus == Convert.ToString(Enums.SessionStatus.Charging) || u.SessionStatus == Convert.ToString(Enums.SessionStatus.ChargingCompleted) || u.SessionStatus == Convert.ToString(Enums.SessionStatus.PaymentCompleted)) && u.SessionType == Convert.ToString(Enums.SessionType.Paid));
                            totalRecords = _userSessions.Count();
                        }
                    }
                    _userSessions = _userSessions.OrderBy(userSessionSearchFilter.Order, userSessionSearchFilter.Dir);
                    if (!Convert.ToBoolean(userSessionSearchFilter.Export))
                    {
                        _userSessions = _userSessions.Skip((Convert.ToInt32(userSessionSearchFilter.Start) - 1) * Convert.ToInt32(userSessionSearchFilter.Size)).Take(Convert.ToInt32(userSessionSearchFilter.Size));
                    }
                    else
                    {
                        var dataToExport = _userSessions.ToList().Select(u => new { u.Id, u.ChargeRentalRevnue, Currency = u.ChargeParams != null ? (GetChargeOption(u.ChargeParams) == null ? null : GetChargeOption(u.ChargeParams).Currency) : null, u.ChargeStationId, CreatedDate = u.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss tt"), u.DeviceId, u.TransactionId, u.MerchantName, u.Mobile, SessionStartTime = u.SessionStartTime != null ? u.SessionStartTime.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : null, SessionEndTime = u.SessionEndTime != null ? u.SessionEndTime.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : null, u.SessionStatus, u.SessionType, u.TransactionTypeId, u.UserAccountId }).AsQueryable();
                        return dataToExport.ToList();
                    }
                }
            }
            return _userSessions.ToList();
        }
        public UserSession GetById(Guid Id)
        {
            IQueryable<DAL.DataContracts.UserSession> userSessions = _baseService.GetAll("SessionStatusNavigation", "SessionTypeNavigation", "ChargeStation", "ChargeStation.Branch.Merchant").AsQueryable();
            var userSession = MappingProfile.MapUserSessionModelObject(userSessions.Where(u => u.Id == Id).FirstOrDefault());
            if (userSession.ChargeParams != null)
            {
                JObject jObject = JObject.Parse(userSession.ChargeParams);
                string chargeOptionId = (string)jObject.SelectToken("ChargeOptionId");
                userSession.PortNumber = (int)jObject.SelectToken("PortNumber");
                if (Convert.ToInt32(chargeOptionId) != 0)
                    userSession.ChargeOption = _chargeOptionService.GetById(Convert.ToInt32(chargeOptionId));
                userSession.Currency = userSession.ChargeOption?.Currency;
            }
            return userSession;
        }
        private ChargeOption GetChargeOption(string chargeParams)
        {
            JObject jObject = JObject.Parse(chargeParams);
            string chargeOptionId = (string)jObject.SelectToken("ChargeOptionId");
            if (Convert.ToInt32(chargeOptionId) > 0)
            {
                return _chargeOptionService.GetById(Convert.ToInt32(chargeOptionId));
            }
            return null;
        }
        public IEnumerable<UserSession> GetSuccessfulSessions()
        {
            IQueryable<UserSession> _userSessions = new List<UserSession>().AsQueryable();
            DateTime toDate = DateTime.Now.ToUniversalTime();
            DateTime fromDate = toDate.AddDays(-7);
            IQueryable<DAL.DataContracts.UserSession> userSessions = _baseService.GetAll("SessionStatusNavigation", "SessionTypeNavigation", "ChargeStation", "ChargeStation.Branch.Merchant").Where(a => a.CreatedDate >= fromDate && a.CreatedDate <= toDate).AsQueryable();
            _userSessions = userSessions.Select(t => MappingProfile.MapUserSessionModelObject(t)).AsQueryable();
            _userSessions = _userSessions.Where(u => (u.SessionStatus == Convert.ToString(Enums.SessionStatus.Charging) || u.SessionStatus == Convert.ToString(Enums.SessionStatus.ChargingCompleted) || u.SessionStatus == Convert.ToString(Enums.SessionStatus.PaymentCompleted)) && u.SessionType == Convert.ToString(Enums.SessionType.Paid));
            _userSessions = _userSessions.Select(u => new UserSession() { Id = u.Id, ChargeRentalRevnue = u.ChargeRentalRevnue, Currency = u.ChargeParams != null ? (GetChargeOption(u.ChargeParams) == null ? null : GetChargeOption(u.ChargeParams).Currency) : null, ChargeStationId = u.ChargeStationId, CreatedDate = Convert.ToDateTime(u.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt")), ModifiedDate = Convert.ToDateTime(u.ModifiedDate.ToString("MM/dd/yyyy hh:mm:ss tt")), DeviceId = u.DeviceId, TransactionId = u.TransactionId, MerchantName = u.MerchantName, Mobile = u.Mobile, SessionStartTime = u.SessionStartTime != null ? DateTime.Parse(u.SessionStartTime.Value.ToString("MM/dd/yyyy hh:mm:ss tt")) : (DateTime?)null, SessionEndTime = u.SessionEndTime != null ? DateTime.Parse(u.SessionEndTime.Value.ToString("MM/dd/yyyy hh:mm:ss tt")) : (DateTime?)null, SessionStatus = u.SessionStatus, SessionType = u.SessionType, TransactionTypeId = u.TransactionTypeId, UserAccountId = u.UserAccountId, PortNumber = u.PortNumber }).AsQueryable();
            return _userSessions.ToList();
        }
    }
}
