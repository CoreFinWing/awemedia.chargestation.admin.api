using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IBaseService<ChargeStation> _baseService;

        public ChargeStationService(IBaseService<ChargeStation> baseService)
        {
            _baseService = baseService;
        }

        public IEnumerable<ChargeStationResponse> Get(ChargeStationSearchFilter chargeStationSearchFilter)
        {
            Expression<Func<ChargeStation, bool>> exp = null;
            IQueryable<ChargeStationResponse> chargeStationResponses = _baseService.GetAll().Select(t => MappingProfile.MapChargeStationResponseObject(t)).AsQueryable();
            if (chargeStationSearchFilter != null)

            {
                if (!string.IsNullOrEmpty(chargeStationSearchFilter.Search))
                {
                    chargeStationSearchFilter.Search = chargeStationSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(chargeStationSearchFilter);
                    chargeStationResponses = _baseService.Where(exp).Select(t => MappingProfile.MapChargeStationResponseObject(t)).AsQueryable();
                }
                chargeStationResponses = chargeStationResponses.OrderBy(chargeStationSearchFilter.Order + (Convert.ToBoolean(chargeStationSearchFilter.Dir) ? " descending" : ""));
                chargeStationResponses = chargeStationResponses.Skip((Convert.ToInt32(chargeStationSearchFilter.Start) - 1) * Convert.ToInt32(chargeStationSearchFilter.Size)).Take(Convert.ToInt32(chargeStationSearchFilter.Size));
            }
            return chargeStationResponses.ToList();
        }

        private static Expression<Func<ChargeStation, bool>> GetFilteredBySearch(ChargeStationSearchFilter chargeStationSearchFilter)
        {
            return e => e.ChargeControllerId.ToLower().Contains(chargeStationSearchFilter.Search) || e.CreatedDate.ToString().ToLower().Contains(chargeStationSearchFilter.Search) || e.Geolocation.ToLower().Contains(chargeStationSearchFilter.Search) || e.Id.ToString().ToLower().Contains(chargeStationSearchFilter.Search) || e.MerchantId.ToLower().Contains(chargeStationSearchFilter.Search) || e.ModifiedDate.ToString().ToLower().Contains(chargeStationSearchFilter.Search);
        }
    }
}
