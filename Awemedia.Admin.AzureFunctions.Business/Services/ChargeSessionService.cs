using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
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
        readonly string[] navigationalProperties = new string[] { "SessionStatusNavigation", "SessionTypeNavigation", };
        readonly string[] includedProperties = new string[] { "ChargeStation", "ChargeStation.Branch.Merchant" };
        public ChargeSessionService(IBaseService<DAL.DataContracts.UserSession> baseService)
        {
            _baseService = baseService;
        }
        public IEnumerable<UserSession> Get(BaseSearchFilter userSessionSearchFilter, out int totalRecords)
        {
            Expression<Func<DAL.DataContracts.UserSession, bool>> exp = null;
            totalRecords = 0;
            IQueryable<DAL.DataContracts.UserSession> userSessions = _baseService.GetAll("SessionStatusNavigation", "SessionTypeNavigation", "ChargeStation", "ChargeStation.Branch.Merchant").AsQueryable();
            totalRecords = userSessions.Count();
            if (userSessionSearchFilter != null)
            {
                if (!string.IsNullOrEmpty(userSessionSearchFilter.Search))
                {
                    userSessionSearchFilter.Search = userSessionSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(userSessionSearchFilter);
                    userSessions = userSessions.Where(e => e.ChargeStation.Branch.Merchant.BusinessName.ToLower().Contains(userSessionSearchFilter.Search)).AsQueryable();
                    totalRecords = userSessions.Count();
                }
                userSessions = userSessions.OrderBy(userSessionSearchFilter.Order + (Convert.ToBoolean(userSessionSearchFilter.Dir) ? " descending" : ""));
                userSessions = userSessions.Skip((Convert.ToInt32(userSessionSearchFilter.Start) - 1) * Convert.ToInt32(userSessionSearchFilter.Size)).Take(Convert.ToInt32(userSessionSearchFilter.Size));
                userSessions = userSessions.Skip((Convert.ToInt32(userSessionSearchFilter.Start) - 1) * Convert.ToInt32(userSessionSearchFilter.Size)).Take(Convert.ToInt32(userSessionSearchFilter.Size));
            }
            return userSessions.Select(t => MappingProfile.MapUserSessionModelObject(t)).ToList();
        }

        private static Expression<Func<DAL.DataContracts.UserSession, bool>> GetFilteredBySearch(BaseSearchFilter userSessionSearchFilter)
        {
            return e => !string.IsNullOrEmpty(e.ChargeRentalRevnue.ToString()) ? e.ChargeRentalRevnue.ToString().Contains(userSessionSearchFilter.Search) : e.ChargeRentalRevnue.ToString().Equals(DBNull.Value) || e.CreatedDate.ToString().ToLower().Contains(userSessionSearchFilter.Search) || e.ChargeStation.Branch.Merchant.BusinessName.ToLower().Contains(userSessionSearchFilter.Search) || e.Id.ToString().ToLower().Contains(userSessionSearchFilter.Search) || e.ModifiedDate.ToString().ToLower().Contains(userSessionSearchFilter.Search) || e.Email.ToLower().Contains(userSessionSearchFilter.Search) || !string.IsNullOrEmpty(e.InvoiceNo.ToString()) ? e.InvoiceNo.ToString().Contains(userSessionSearchFilter.Search) : e.InvoiceNo.ToString().Equals(DBNull.Value) || !string.IsNullOrEmpty(e.Mobile.ToString()) ? e.Mobile.ToString().Contains(userSessionSearchFilter.Search) : e.Mobile.ToString().Equals(DBNull.Value) || e.SessionStatusNavigation.Status.ToLower().Contains(userSessionSearchFilter.Search) || e.SessionTypeNavigation.Type.ToLower().Contains(userSessionSearchFilter.Search) || !string.IsNullOrEmpty(e.TransactionId.ToString()) ? e.TransactionId.ToString().Contains(userSessionSearchFilter.Search) : e.TransactionId.ToString().Equals(DBNull.Value);
        }

        public UserSession GetById(Guid Id)
        {
            IQueryable<DAL.DataContracts.UserSession> userSessions = _baseService.GetAll("SessionStatusNavigation", "SessionTypeNavigation", "ChargeStation", "ChargeStation.Branch.Merchant").AsQueryable();
            var userSession = MappingProfile.MapUserSessionModelObject(userSessions.Where(u => u.Id == Id).FirstOrDefault());
            return userSession;
        }
    }
}
