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
            IQueryable<ChargeStation> chargeStations = _baseService.GetAll("Branch", "Branch.Merchant").Where(c => c.IsActive).AsQueryable();
            totalRecords = chargeStations.Count();
            if (!string.IsNullOrEmpty(chargeStationSearchFilter.IsOnline))
            {
                if (Convert.ToBoolean(chargeStationSearchFilter.IsOnline))
                {
                    chargeStations = chargeStations.Where(item => item.IsOnline.Equals(Convert.ToBoolean(chargeStationSearchFilter.IsOnline))).AsQueryable();
                    totalRecords = chargeStations.Count();
                }
            }
            if (chargeStationSearchFilter != null)
            {
                if (Convert.ToInt32(chargeStationSearchFilter.MerchantId) > 0)
                {
                    chargeStations = chargeStations.Where(a => a.Branch.MerchantId == Convert.ToInt32(chargeStationSearchFilter.MerchantId)).AsQueryable();
                    totalRecords = chargeStations.Count();
                }
                if (!string.IsNullOrEmpty(chargeStationSearchFilter.Search))
                {
                    chargeStationSearchFilter.Search = chargeStationSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(chargeStationSearchFilter);
                    chargeStations = chargeStations.Where(exp).AsQueryable();
                    totalRecords = chargeStations.Count();
                }
                chargeStations = chargeStations.OrderBy(chargeStationSearchFilter.Order + (Convert.ToBoolean(chargeStationSearchFilter.Dir) ? " descending" : ""));
                chargeStations = chargeStations.Skip((Convert.ToInt32(chargeStationSearchFilter.Start) - 1) * Convert.ToInt32(chargeStationSearchFilter.Size)).Take(Convert.ToInt32(chargeStationSearchFilter.Size));
            }
            return chargeStations.Select(t => MappingProfile.MapChargeStationResponseObject(t)).ToList();
        }

        private static Expression<Func<ChargeStation, bool>> GetFilteredBySearch(BaseSearchFilter chargeStationSearchFilter)
        {
            return e => Convert.ToString(e.ChargeControllerId).ToLower().Contains(chargeStationSearchFilter.Search) || Convert.ToString(e.CreatedDate).ToLower().Contains(chargeStationSearchFilter.Search) || Convert.ToString(e.Geolocation).ToLower().Contains(chargeStationSearchFilter.Search) || Convert.ToString(e.Id).ToLower().Contains(chargeStationSearchFilter.Search) || Convert.ToString(e.ModifiedDate).ToLower().Contains(chargeStationSearchFilter.Search) || Convert.ToString(e.Branch.Merchant.BusinessName).Contains(chargeStationSearchFilter.Search);
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
                chargeStation.BranchId = chargeStationModel.BranchId;
                chargeStation.ModifiedDate = DateTime.Now;
            }
            ChargeStation model = _baseService.AddOrUpdate(chargeStation, guid, excludedProps);
            return model.Id;
        }
        public ChargeStationModel GetById(Guid guid)
        {
            IQueryable<ChargeStation> chargeStations = _baseService.GetAll("Branch", "Branch.Merchant", "Branch.Merchant.Branch").AsQueryable();
            var chargeStation = chargeStations.Where(u => u.Id == guid).FirstOrDefault();
            if (chargeStation != null)
            {
                return MappingProfile.MapChargeStationResponseObject(chargeStation);
            }
            else
            {
                return null;
            }
        }
        public DAL.DataContracts.ChargeStation GetById(int id)
        {
            return _baseService.Where(c => c.Uid.Equals(id)).FirstOrDefault();
        }

        public void MarkActiveInActive(dynamic chargeStationsToSetActiveInActive)
        {
            string[] excludedProps = { "Uid" };
            if (chargeStationsToSetActiveInActive != null)
            {
                if (chargeStationsToSetActiveInActive.Length > 0)
                {
                    foreach (var item in chargeStationsToSetActiveInActive)
                    {
                        var chargestationId = Guid.Parse(item.GetType().GetProperty("Id").GetValue(item, null));
                        bool IsActive = Convert.ToBoolean(item.GetType().GetProperty("IsActive").GetValue(item, null));
                        var chargeStations = _baseService.GetAll();
                        var chargeStation = chargeStations.Where(c => c.Id == chargestationId).FirstOrDefault();
                        if (chargeStation != null)
                        {
                            chargeStation.IsActive = IsActive;
                            chargeStation.ModifiedDate = DateTime.Now;
                            _baseService.AddOrUpdate(chargeStation, chargestationId, excludedProps);
                        }
                    }
                }
            }
        }
    }
}
