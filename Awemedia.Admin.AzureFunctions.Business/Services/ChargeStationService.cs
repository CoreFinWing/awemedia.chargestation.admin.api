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
        public IEnumerable<ChargeStationResponse> GetAll()
        {
            return _baseService.GetAll().Select(t => MappingProfile.MapChargeStationResponseObject(t));
        }

        public IEnumerable<ChargeStationResponse> GetFiltered(BaseFilterRequest baseFilterRequest)
        {
            Expression<Func<ChargeStation, bool>> exp = null;
            IQueryable<ChargeStationResponse> chargeStationResponses = GetAll().AsQueryable();
            if (baseFilterRequest != null)
            {
                if (!string.IsNullOrEmpty(baseFilterRequest.SearchText))
                {
                    baseFilterRequest.SearchText = baseFilterRequest.SearchText.ToLower();
                    exp = GetFilteredBySearch(baseFilterRequest);
                    chargeStationResponses = _baseService.Where(exp).Select(t => MappingProfile.MapChargeStationResponseObject(t)).AsQueryable();
                }
                chargeStationResponses = chargeStationResponses.OrderBy(baseFilterRequest.SortBy + (Convert.ToBoolean(baseFilterRequest.Desc) ? " descending" : ""));
                chargeStationResponses = chargeStationResponses.Skip((Convert.ToInt32(baseFilterRequest.PageNum) - 1) * Convert.ToInt32(baseFilterRequest.ItemsPerPage)).Take(Convert.ToInt32(baseFilterRequest.ItemsPerPage));
            }
            return chargeStationResponses.ToList();
        }

        private static Expression<Func<ChargeStation, bool>> GetFilteredBySearch(BaseFilterRequest baseFilterRequest)
        {
            return e => e.ChargeControllerId.ToLower().Contains(baseFilterRequest.SearchText) || e.CreatedDate.ToString().ToLower().Contains(baseFilterRequest.SearchText) || e.Geolocation.ToLower().Contains(baseFilterRequest.SearchText) || e.Id.ToString().ToLower().Contains(baseFilterRequest.SearchText) || e.MerchantId.ToLower().Contains(baseFilterRequest.SearchText) || e.ModifiedDate.ToString().ToLower().Contains(baseFilterRequest.SearchText);
        }
    }
}
