﻿using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using ChargeStation = Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeStation;
using ChargeStationModel = Awemedia.Admin.AzureFunctions.Business.Models.ChargeStation;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IBaseService<ChargeStation> _baseService;

        public ChargeStationService(IBaseService<ChargeStation> baseService)
        {
            _baseService = baseService;
        }

        public IEnumerable<ChargeStationModel> Get(ChargeStationSearchFilter chargeStationSearchFilter)
        {
            Expression<Func<ChargeStation, bool>> exp = null;
            IQueryable<ChargeStationModel> chargeStations = _baseService.GetAll().Select(t => MappingProfile.MapChargeStationResponseObject(t)).AsQueryable();
            if (chargeStationSearchFilter != null)

            {
                if (!string.IsNullOrEmpty(chargeStationSearchFilter.Search))
                {
                    chargeStationSearchFilter.Search = chargeStationSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(chargeStationSearchFilter);
                    chargeStations = _baseService.Where(exp).Select(t => MappingProfile.MapChargeStationResponseObject(t)).AsQueryable();
                }
                chargeStations = chargeStations.OrderBy(chargeStationSearchFilter.Order + (Convert.ToBoolean(chargeStationSearchFilter.Dir) ? " descending" : ""));
                chargeStations = chargeStations.Skip((Convert.ToInt32(chargeStationSearchFilter.Start) - 1) * Convert.ToInt32(chargeStationSearchFilter.Size)).Take(Convert.ToInt32(chargeStationSearchFilter.Size));
            }
            return chargeStations.ToList();
        }

        private static Expression<Func<ChargeStation, bool>> GetFilteredBySearch(ChargeStationSearchFilter chargeStationSearchFilter)
        {
            return e => e.ChargeControllerId.ToLower().Contains(chargeStationSearchFilter.Search) || e.CreatedDate.ToString().ToLower().Contains(chargeStationSearchFilter.Search) || e.Geolocation.ToLower().Contains(chargeStationSearchFilter.Search) || e.Id.ToString().ToLower().Contains(chargeStationSearchFilter.Search) || e.MerchantId.ToLower().Contains(chargeStationSearchFilter.Search) || e.ModifiedDate.ToString().ToLower().Contains(chargeStationSearchFilter.Search);
        }

        public Guid AddChargeStation(ChargeStationModel chargeStation, Guid guid = default(Guid))
        {
            if (chargeStation == null)
            {
                return default(Guid);
            }
            ChargeStation model = _baseService.AddOrUpdate(MappingProfile.MapChargeStationObject(chargeStation), guid);
            return model.Id;
        }
        public object IsChargeStationExists(Guid guid)
        {
            if (_baseService.GetById(guid) == null)
                return DBNull.Value;
            else
                return _baseService.GetById(guid).Id;
        }
    }
}
