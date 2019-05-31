using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using MerchantModel = Awemedia.Admin.AzureFunctions.Business.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Text;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Merchant = Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IBaseService<Merchant> _baseService;
        public MerchantService(IBaseService<Merchant> baseService)
        {
            _baseService = baseService;
        }
        public IEnumerable<MerchantModel> Get(BaseSearchFilter merchantSearchFilter)
        {
            Expression<Func<Merchant, bool>> exp = null;
            IQueryable<MerchantModel> merchants = _baseService.GetAll("Branch", "IndustryType").Select(t => MappingProfile.MapMerchantModelObject(t)).AsQueryable();
            if (merchantSearchFilter != null)

            {
                if (!string.IsNullOrEmpty(merchantSearchFilter.Search))
                {
                    merchantSearchFilter.Search = merchantSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(merchantSearchFilter);
                    merchants = _baseService.Where(exp).Select(t => MappingProfile.MapMerchantModelObject(t)).AsQueryable();
                }
                merchants = merchants.OrderBy(merchantSearchFilter.Order + (Convert.ToBoolean(merchantSearchFilter.Dir) ? " descending" : ""));
                merchants = merchants.Skip((Convert.ToInt32(merchantSearchFilter.Start) - 1) * Convert.ToInt32(merchantSearchFilter.Size)).Take(Convert.ToInt32(merchantSearchFilter.Size));
            }
            return merchants.ToList();
        }
        private static Expression<Func<Merchant, bool>> GetFilteredBySearch(BaseSearchFilter merchantSearchFilter)
        {
            return e => e.BusinessName.ToLower().Contains(merchantSearchFilter.Search) || e.ChargeStationsOrdered.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.Dba.ToLower().Contains(merchantSearchFilter.Search) || e.Id.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.DepositMoneyPaid.ToLower().Contains(merchantSearchFilter.Search) || e.Email.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.LicenseNum.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.ModifiedDate.ToString().ToLower().Contains(merchantSearchFilter.Search) ||  e.ContactName.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.PhoneNum.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.ProfitSharePercentage.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.SecondaryContact.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.SecondaryPhone.ToString().ToLower().Contains(merchantSearchFilter.Search) || e.IndustryType.Name.ToString().ToLower().Contains(merchantSearchFilter.Search);
        }
        public int AddChargeStation(MerchantModel merchantModel, int id = 0)
        {
            throw new NotImplementedException();
        }

        public int UpdateChargeStation(MerchantModel merchantModel, int id)
        {
            throw new NotImplementedException();
        }

        public object IsMerchantExists(int id)
        {
            var merchant = _baseService.GetById(id);
            if (merchant == null)
                return DBNull.Value;
            else
                return merchant.Id;
        }
    }
}
