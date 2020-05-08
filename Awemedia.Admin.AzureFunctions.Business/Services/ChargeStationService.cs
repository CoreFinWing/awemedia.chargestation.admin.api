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
using Awemedia.Admin.AzureFunctions.Business.Helpers;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IBaseService<ChargeStation> _baseService;

        public ChargeStationService(IBaseService<ChargeStation> baseService)
        {
            _baseService = baseService;
        }

        public IEnumerable<object> Get(BaseSearchFilter chargeStationSearchFilter, out int totalRecords, bool isActive = true)
        {
            string[] navigationalProps = { "Branch", "Branch.Merchant" };
            IEnumerable<ChargeStationModel> _chargeStations = null;
            totalRecords = 0;
            if (!string.IsNullOrEmpty(chargeStationSearchFilter.FromDate) && !string.IsNullOrEmpty(chargeStationSearchFilter.ToDate))
            {
                DateTime fromDate = DateTime.Now.ToUniversalTime();
                DateTime toDate = DateTime.Now.ToUniversalTime();
                fromDate = Utility.ParseStartAndEndDates(chargeStationSearchFilter, ref toDate);
                _chargeStations = _baseService.Where(a => a.CreatedDate.Date >= fromDate && a.CreatedDate.Date <= toDate, navigationalProps).Select(t => MappingProfile.MapChargeStationResponseObject(t)).ToList();
            }
            else
            {
                _chargeStations = _baseService.GetAll(navigationalProps).Select(t => MappingProfile.MapChargeStationResponseObject(t)).ToList();
            }
            totalRecords = _chargeStations.Count();
            if (isActive)
            {
                _chargeStations = _chargeStations.Where(item => item.IsActive.Equals(isActive)).AsQueryable();
                totalRecords = _chargeStations.Count();
            }
            if (!string.IsNullOrEmpty(chargeStationSearchFilter.IsOnline))
            {
                if (Convert.ToBoolean(chargeStationSearchFilter.IsOnline))
                {
                    _chargeStations = _chargeStations.Where(c => c.ModifiedDate >= Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(DateTime.Now.ToUniversalTime(), Environment.GetEnvironmentVariable("MalaysiaTimeZone"))).AddMinutes(Convert.ToDouble(Environment.GetEnvironmentVariable("OnlineChargeStationInterval"))) && c.IsActive).AsQueryable();
                    totalRecords = _chargeStations.Count();
                }
            }
            if (chargeStationSearchFilter != null)
            {
                if (Convert.ToInt32(chargeStationSearchFilter.MerchantId) > 0)
                {
                    _chargeStations = _chargeStations.Where(a => a.Branch == null ? false : a.Branch.MerchantId == Convert.ToInt32(chargeStationSearchFilter.MerchantId)).ToList();
                    totalRecords = _chargeStations.Count();
                }
                if (Convert.ToInt32(chargeStationSearchFilter.BranchId) > 0)
                {
                    _chargeStations = _chargeStations.Where(a => a.Branch == null ? false : a.BranchId == Convert.ToInt32(chargeStationSearchFilter.BranchId)).ToList();
                    totalRecords = _chargeStations.Count();
                }
                if (!string.IsNullOrEmpty(chargeStationSearchFilter.Search) && !string.IsNullOrEmpty(chargeStationSearchFilter.Type))
                {
                    _chargeStations = _chargeStations.Where(a => a.Branch == null ? false : true).AsQueryable();
                    _chargeStations = _chargeStations.Search(chargeStationSearchFilter.Type, chargeStationSearchFilter.Search);
                    totalRecords = _chargeStations.Count();
                }

                _chargeStations = _chargeStations.OrderBy(chargeStationSearchFilter.Order, chargeStationSearchFilter.Dir);
                if (!Convert.ToBoolean(chargeStationSearchFilter.Export))
                {
                    _chargeStations = _chargeStations.Skip((Convert.ToInt32(chargeStationSearchFilter.Start) - 1) * Convert.ToInt32(chargeStationSearchFilter.Size)).Take(Convert.ToInt32(chargeStationSearchFilter.Size));
                }
                else
                {
                    var dataToExport = _chargeStations.Select(c => new { c.Id, c.BatteryInfoDisplayField, c.BranchName, c.ChargeControllerId, CreatedDate = c.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss tt"), ModifiedDate = c.ModifiedDate.ToString("yyyy-MM-dd hh:mm:ss tt"), c.DeviceId, c.Geolocation, c.IsActive, c.IsOnline, c.LastPingTimeStamp, c.MerchantName }).AsQueryable();
                    return dataToExport.ToList();
                }

            }

            return _chargeStations.ToList();
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
            }
            ChargeStation model = _baseService.AddOrUpdate(chargeStation, guid, excludedProps);
            return model.Id;
        }
        public ChargeStationModel GetById(Guid guid)
        {
            IQueryable<ChargeStation> chargeStations = _baseService.GetAll("Branch", "Branch.Merchant", "Branch.Merchant.Branch", "UserSession", "UserSession.SessionStatusNavigation", "UserSession.SessionTypeNavigation").AsQueryable();
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
                            _baseService.AddOrUpdate(chargeStation, chargestationId, excludedProps);
                        }
                    }
                }
            }
        }
    }
}
