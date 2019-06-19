using Awemedia.Admin.AzureFunctions.Business.Interfaces;
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

        public IEnumerable<ChargeStationModel> Get(BaseSearchFilter chargeStationSearchFilter, out int totalRecords)
        {
            Expression<Func<ChargeStation, bool>> exp = null;
            totalRecords = 0;
            IQueryable<ChargeStationModel> chargeStations = _baseService.GetAll().Select(t => MappingProfile.MapChargeStationResponseObject(t)).AsQueryable();
            totalRecords = chargeStations.Count();
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

        private static Expression<Func<ChargeStation, bool>> GetFilteredBySearch(BaseSearchFilter chargeStationSearchFilter)
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
            var chargeStation = _baseService.GetById(guid);
            if (chargeStation == null)
                return DBNull.Value;
            else
                return chargeStation.Id;
        }

        public Guid UpdateChargeStation(ChargeStationModel chargeStationModel, Guid guid)
        {
            if (chargeStationModel == null)
            {
                return default(Guid);
            }
            string[] excludedProps = { "Uid" };
            var chargeStation = _baseService.GetById(guid);
            if (chargeStation != null)
            {
                chargeStation.MerchantId = chargeStationModel.MerchantId;
                chargeStation.ModifiedDate = DateTime.Now;
            }
            ChargeStation model = _baseService.AddOrUpdate(chargeStation, guid, excludedProps);
            return model.Id;
        }
    }
}
