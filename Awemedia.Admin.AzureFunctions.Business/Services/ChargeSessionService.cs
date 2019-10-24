using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class ChargeSessionService : IChargeSessionService
    {
        private readonly IBaseService<DAL.DataContracts.UserSession> _baseService;
        private readonly IChargeOptionService _chargeOptionService;
        readonly string[] navigationalProperties = new string[] { "SessionStatusNavigation", "SessionTypeNavigation", };
        readonly string[] includedProperties = new string[] { "ChargeStation", "ChargeStation.Branch.Merchant" };
        public ChargeSessionService(IBaseService<DAL.DataContracts.UserSession> baseService, IChargeOptionService chargeOptionService)
        {
            _baseService = baseService;
            _chargeOptionService = chargeOptionService;
        }
        public IEnumerable<UserSession> Get(BaseSearchFilter userSessionSearchFilter, out int totalRecords)
        {
            Expression<Func<UserSession, bool>> exp = null;
            totalRecords = 0;
            IQueryable<DAL.DataContracts.UserSession> userSessions = _baseService.GetAll("SessionStatusNavigation", "SessionTypeNavigation", "ChargeStation", "ChargeStation.Branch.Merchant").AsQueryable();
            var _userSessions = userSessions.Select(t => MappingProfile.MapUserSessionModelObject(t)).AsQueryable();
            totalRecords = _userSessions.Count();
            if (userSessionSearchFilter != null)
            {
                if (Convert.ToInt32(userSessionSearchFilter.MerchantId) > 0)
                {
                    _userSessions = _userSessions.Where(a => a.ChargeStation.Branch == null ? true : a.ChargeStation.Branch.MerchantId == Convert.ToInt32(userSessionSearchFilter.MerchantId)).AsQueryable();
                    totalRecords = _userSessions.Count();
                }
                if (!string.IsNullOrEmpty(userSessionSearchFilter.Search) && !string.IsNullOrEmpty(userSessionSearchFilter.Type))
                {
                    userSessionSearchFilter.Search = userSessionSearchFilter.Search.ToLower();
                    exp = PredicateHelper<UserSession>.CreateSearchPredicate(userSessionSearchFilter.Type, userSessionSearchFilter.Search);
                    _userSessions = _userSessions.Where(u => u.ChargeStation.Branch == null ? false : true);
                    _userSessions = _userSessions.Where(exp).AsQueryable();
                    totalRecords = _userSessions.Count();
                }
                _userSessions = _userSessions.OrderBy(userSessionSearchFilter.Order + (Convert.ToBoolean(userSessionSearchFilter.Dir) ? " descending" : ""));
                _userSessions = _userSessions.Skip((Convert.ToInt32(userSessionSearchFilter.Start) - 1) * Convert.ToInt32(userSessionSearchFilter.Size)).Take(Convert.ToInt32(userSessionSearchFilter.Size));

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
                userSession.ChargePorts = (int)jObject.SelectToken("ChargeOptionId");
                userSession.ChargeOption = _chargeOptionService.GetById(Convert.ToInt32(chargeOptionId));
            }
            return userSession;
        }
    }
}
