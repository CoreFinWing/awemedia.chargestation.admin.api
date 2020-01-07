using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Awemedia.Admin.AzureFunctions.Business.Helpers;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class ChargeOptionsService : IChargeOptionService
    {
        private readonly IBaseService<DAL.DataContracts.ChargeOptions> _baseService;

        public ChargeOptionsService(IBaseService<DAL.DataContracts.ChargeOptions> baseService)
        {
            _baseService = baseService;
        }
        public bool Add(ChargeOption chargeOptionsResponse, out bool isDuplicateRecord, int id = 0)
        {
            isDuplicateRecord = false;
            if (chargeOptionsResponse == null)
                return false;
            try
            {
                return _baseService.AddOrUpdate(MappingProfile.MapChargeOptionsObjects(chargeOptionsResponse), id) == null ? false : true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("IX_ChargeOptions"))
                    isDuplicateRecord = true;
                return false;
            }
        }

        public void MarkActiveInActive(dynamic optionsSetToActiveInActive)
        {
            if (optionsSetToActiveInActive != null)
            {
                if (optionsSetToActiveInActive.Length > 0)
                {
                    foreach (var item in optionsSetToActiveInActive)
                    {
                        int chargeOptionId = Convert.ToInt32(item.GetType().GetProperty("Id").GetValue(item, null));
                        bool IsActive = Convert.ToBoolean(item.GetType().GetProperty("IsActive").GetValue(item, null));
                        var chargeOption = _baseService.GetById(chargeOptionId);
                        if (chargeOption != null)
                        {
                            chargeOption.IsActive = IsActive;
                            chargeOption.ModifiedDate = DateTime.Now.ToUniversalTime();
                            _baseService.AddOrUpdate(chargeOption, chargeOptionId);
                        }
                    }
                }
            }
        }
        public IEnumerable<ChargeOption> Get(BaseSearchFilter chargeOptionSearchFilter, out int totalRecords, bool isActive = true)
        {
            IQueryable<ChargeOption> _chargeOptions = new List<ChargeOption>().AsQueryable();
            IQueryable<DAL.DataContracts.ChargeOptions> chargeOptions = _baseService.GetAll().AsQueryable();
            _chargeOptions = chargeOptions.Select(t => MappingProfile.MapChargeOptionsResponseObjects(t)).AsQueryable();
            totalRecords = _chargeOptions.Count();
            if (isActive)
            {
                _chargeOptions = _chargeOptions.Where(item => item.IsActive.Equals(isActive)).AsQueryable();
                totalRecords = _chargeOptions.Count();
            }
            if (chargeOptionSearchFilter != null)
            {
                if (!string.IsNullOrEmpty(chargeOptionSearchFilter.Search) && !string.IsNullOrEmpty(chargeOptionSearchFilter.Type))
                {
                    _chargeOptions = _chargeOptions.Search(chargeOptionSearchFilter.Type, chargeOptionSearchFilter.Search);
                    totalRecords = _chargeOptions.Count();
                }
                _chargeOptions = _chargeOptions.OrderBy(chargeOptionSearchFilter.Order + (Convert.ToBoolean(chargeOptionSearchFilter.Dir) ? " descending" : ""));
                if (!Convert.ToBoolean(chargeOptionSearchFilter.Export))
                {
                    _chargeOptions = _chargeOptions.Skip((Convert.ToInt32(chargeOptionSearchFilter.Start) - 1) * Convert.ToInt32(chargeOptionSearchFilter.Size)).Take(Convert.ToInt32(chargeOptionSearchFilter.Size));
                }
            }
            return _chargeOptions.ToList();
        }
        private static Expression<Func<DAL.DataContracts.ChargeOptions, bool>> GetFilteredBySearch(BaseSearchFilter chargeOptionsSearchFilter)
        {
            return e => e.ChargeDuration.ToString().Contains(chargeOptionsSearchFilter.Search) || e.CreatedDate.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search) || e.Currency.ToLower().Contains(chargeOptionsSearchFilter.Search) || e.Id.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search) || e.Price.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search) || e.ModifiedDate.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search);
        }

        public ChargeOption GetById(int chargeOptionId)
        {
            return MappingProfile.MapChargeOptionsResponseObjects(_baseService.GetById(chargeOptionId));
        }
    }
}
