﻿using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Merchant = Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant;
using MerchantModel = Awemedia.Admin.AzureFunctions.Business.Models.Merchant;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IBaseService<Merchant> _baseService;
        readonly string[] navigationalProperties = new string[] { "IndustryType" };
        readonly string[] includedProperties = new string[] { "Branch" };
        public MerchantService(IBaseService<Merchant> baseService)
        {
            _baseService = baseService;
        }
        public IEnumerable<MerchantModel> Get(BaseSearchFilter merchantSearchFilter, out int totalRecords, bool isActive = true)
        {
            Expression<Func<MerchantModel, bool>> exp = null;
            totalRecords = 0;
            IQueryable<Merchant> merchants = _baseService.GetAll("Branch", "IndustryType", "Branch.ChargeStation").AsQueryable();
            var _merchants = merchants.Select(t => MappingProfile.MapMerchantModelObject(t)).AsQueryable();
            totalRecords = _merchants.Count();
            if (isActive)
            {
                _merchants = _merchants.Where(item => item.IsActive.Equals(isActive)).AsQueryable();
                totalRecords = _merchants.Count();
            }
            if (merchantSearchFilter != null)
            {
                if (!string.IsNullOrEmpty(merchantSearchFilter.Search) && !string.IsNullOrEmpty(merchantSearchFilter.Type))
                {
                    merchantSearchFilter.Search = merchantSearchFilter.Search.ToLower();
                    exp = PredicateHelper<MerchantModel>.CreateSearchPredicate(merchantSearchFilter.Type, merchantSearchFilter.Search);
                    _merchants = _merchants.Where(exp).AsQueryable();
                }
                _merchants = _merchants.OrderBy(merchantSearchFilter.Order + (Convert.ToBoolean(merchantSearchFilter.Dir) ? " descending" : ""));
                _merchants = _merchants.Skip((Convert.ToInt32(merchantSearchFilter.Start) - 1) * Convert.ToInt32(merchantSearchFilter.Size)).Take(Convert.ToInt32(merchantSearchFilter.Size));
            }
            return _merchants.ToList();
        }


        public int AddMerchant(MerchantModel merchantModel, int id = 0)
        {
            if (!IsMerchantDuplicate(merchantModel))
            {
                var merchant = _baseService.AddOrUpdate(MappingProfile.MapMerchantObject(merchantModel, new DAL.DataContracts.Merchant()), 0);
                merchantModel.Id = merchant.Id;
                return merchant.Id;
            }
            return 0;
        }

        public void UpdateMerchant(MerchantModel merchantModel, int id)
        {
            var merchant = _baseService.GetById(id);
            string[] excludedProps = { "Id" };
            if (merchant != null)
            {
                merchantModel.Id = id;
                _baseService.AddOrUpdate(MappingProfile.MapMerchantObject(merchantModel, merchant), id, excludedProps);
            }
        }
        public object IsMerchantExists(int id)
        {
            var merchant = _baseService.GetById(id);
            if (merchant == null)
                return DBNull.Value;
            else
                return merchant.Id;
        }
        public void MarkActiveInActive(dynamic merchantsToSetActiveInActive)
        {
            if (merchantsToSetActiveInActive != null)
            {
                if (merchantsToSetActiveInActive.Length > 0)
                {
                    foreach (var item in merchantsToSetActiveInActive)
                    {
                        int merchantId = Convert.ToInt32(item.GetType().GetProperty("Id").GetValue(item, null));
                        bool IsActive = Convert.ToBoolean(item.GetType().GetProperty("IsActive").GetValue(item, null));
                        var merchants = _baseService.GetAll("Branch", "IndustryType", "Branch.ChargeStation");
                        var merchant = merchants.Where(m => m.Id == merchantId).FirstOrDefault();
                        if (merchant != null)
                        {
                            merchant.IsActive = IsActive;
                            merchant.ModifiedDate = DateTime.Now;
                            if (IsActive)
                                merchant.NumOfActiveLocations = merchant.Branch.Count;
                            else
                                merchant.NumOfActiveLocations = 0;
                            if (merchant.Branch.Count > 0)
                            {
                                foreach (var branch in merchant.Branch)
                                {
                                    branch.IsActive = IsActive;
                                    branch.ModifiedDate = DateTime.Now;
                                    if (branch.ChargeStation.Count > 0)
                                    {
                                        foreach (var chargeStation in branch.ChargeStation)
                                        {
                                            chargeStation.IsActive = IsActive;
                                            chargeStation.ModifiedDate = DateTime.Now;
                                        }
                                    }
                                }
                            }
                            _baseService.AddOrUpdate(merchant, merchantId);
                        }
                    }
                }
            }
        }
        public void UpdateLocationCount(int count, int merchantId)
        {
            var merchant = _baseService.GetById(merchantId);
            if (merchant != null)
            {
                merchant.NumOfActiveLocations = count;
                _baseService.AddOrUpdate(merchant, merchantId);
            }
        }

        public MerchantModel GetById(int id)
        {
            return MappingProfile.MapMerchantModelObject(_baseService.GetById(id, navigationalProperties, includedProperties));
        }
        public List<object> Search(string keyword)
        {
            return _baseService.Where(m => m.BusinessName.ToLower().Contains(keyword)).Select(s => new { s.Id, s.BusinessName }).ToList<object>();
        }

        private bool IsMerchantDuplicate(MerchantModel merchantModel)
        {
            bool isDuplicateMerchantFound = false;
            if (merchantModel != null)
            {
                var merchant = _baseService.Where(a => a.LicenseNum.Equals(merchantModel.LicenseNumber)).FirstOrDefault();
                if (merchant != null)
                {
                    isDuplicateMerchantFound = true;
                }
            }
            return isDuplicateMerchantFound;
        }
    }
}
