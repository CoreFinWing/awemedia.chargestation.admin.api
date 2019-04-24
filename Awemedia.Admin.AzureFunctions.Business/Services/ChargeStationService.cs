using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
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
        private readonly IMapper _mapper;

        public ChargeStationService(IBaseService<ChargeStation> baseService, IMapper mapper)
        {
            _baseService = baseService;
            _mapper = mapper;
        }
        public IEnumerable<ChargeStationResponse> GetAll()
        {
            return _baseService.GetAll().Select(t => _mapper.Map<ChargeStation, ChargeStationResponse>(t));
        }

        public IEnumerable<ChargeStationResponse> GetFiltered(BaseFilterResponse baseFilterResponse)
        {
            Expression<Func<ChargeStation, bool>> exp = null;
            IQueryable<ChargeStationResponse> chargeStationResponses = null;
            if (baseFilterResponse != null)
            {
                chargeStationResponses = GetAll().AsQueryable();
                if (!string.IsNullOrEmpty(baseFilterResponse.SearchText))
                {
                    baseFilterResponse.SearchText = baseFilterResponse.SearchText.ToLower();
                    exp = GetFilteredBySearch(baseFilterResponse);
                    chargeStationResponses = _baseService.Where(exp).Select(t => _mapper.Map<ChargeStation, ChargeStationResponse>(t)).AsQueryable();
                }
                chargeStationResponses = chargeStationResponses.OrderBy(baseFilterResponse.SortBy + (Convert.ToBoolean(baseFilterResponse.Desc) ? " descending" : ""));
                chargeStationResponses = chargeStationResponses.Skip((Convert.ToInt32(baseFilterResponse.PageNum) - 1) * Convert.ToInt32(baseFilterResponse.ItemsPerPage)).Take(Convert.ToInt32(baseFilterResponse.ItemsPerPage));
            }
            return chargeStationResponses.ToList();
        }

        private static Expression<Func<ChargeStation, bool>> GetFilteredBySearch(BaseFilterResponse baseFilterResponse)
        {
            return e => e.ChargeControllerId.ToLower().Contains(baseFilterResponse.SearchText) || e.CreatedDate.ToString().ToLower().Contains(baseFilterResponse.SearchText) || e.Geolocation.ToLower().Contains(baseFilterResponse.SearchText) || e.Id.ToString().ToLower().Contains(baseFilterResponse.SearchText) || e.MerchantId.ToLower().Contains(baseFilterResponse.SearchText) || e.ModifiedDate.ToString().ToLower().Contains(baseFilterResponse.SearchText);
        }
    }
}
