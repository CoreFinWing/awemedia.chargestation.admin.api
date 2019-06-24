using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

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
                            chargeOption.ModifiedDate = DateTime.Now;
                            _baseService.AddOrUpdate(chargeOption, chargeOptionId);
                        }
                    }
                }
            }
        }
        public IEnumerable<ChargeOption> Get(BaseSearchFilter chargeOptionSearchFilter, out int totalRecords, bool isActive = true)
        {
            Expression<Func<DAL.DataContracts.ChargeOptions, bool>> exp = null;
            totalRecords = 0;
            IQueryable<ChargeOption> chargeOptions = _baseService.GetAll().Select(t => MappingProfile.MapChargeOptionsResponseObjects(t)).AsQueryable();
            if (isActive)
                chargeOptions = chargeOptions.Where(item => item.IsActive.Equals(isActive)).AsQueryable();
            if (chargeOptionSearchFilter != null)
            {
                if (!string.IsNullOrEmpty(chargeOptionSearchFilter.Search))
                {
                    chargeOptionSearchFilter.Search = chargeOptionSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(chargeOptionSearchFilter);
                    chargeOptions = _baseService.Where(exp).Select(t => MappingProfile.MapChargeOptionsResponseObjects(t)).AsQueryable();
                }
                chargeOptions = chargeOptions.OrderBy(chargeOptionSearchFilter.Order + (Convert.ToBoolean(chargeOptionSearchFilter.Dir) ? " descending" : ""));
                chargeOptions = chargeOptions.Skip((Convert.ToInt32(chargeOptionSearchFilter.Start) - 1) * Convert.ToInt32(chargeOptionSearchFilter.Size)).Take(Convert.ToInt32(chargeOptionSearchFilter.Size));
            }
            totalRecords = chargeOptions.Count();
            return chargeOptions.ToList();
        }
        private static Expression<Func<DAL.DataContracts.ChargeOptions, bool>> GetFilteredBySearch(BaseSearchFilter chargeOptionsSearchFilter)
        {
            return e => e.ChargeDuration.ToLower().Contains(chargeOptionsSearchFilter.Search) || e.CreatedDate.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search) || e.Currency.ToLower().Contains(chargeOptionsSearchFilter.Search) || e.Id.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search) || e.Price.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search) || e.ModifiedDate.ToString().ToLower().Contains(chargeOptionsSearchFilter.Search);
        }
    }
}
