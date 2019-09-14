using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBaseService<DAL.DataContracts.Branch> _baseService;
        private readonly IMerchantService _merchantService;
        readonly string[] includedProperties = new string[] { "ChargeStation" };
        public BranchService(IBaseService<DAL.DataContracts.Branch> baseService, IMerchantService merchantService)
        {
            _baseService = baseService;
            _merchantService = merchantService;
        }
        public void AddBranch(Branch branch, int merchantId)
        {
            if (branch != null)
            {
                branch.MerchantId = merchantId;
                _baseService.AddOrUpdate(MappingProfile.MapBranchObject(branch, new DAL.DataContracts.Branch()), 0);
                Expression<Func<DAL.DataContracts.Branch, bool>> exp = e => e.MerchantId == merchantId && e.IsActive;
                int activeLocationCount = _baseService.Where(exp).Count();
                _merchantService.UpdateLocationCount(activeLocationCount, merchantId);
            }
        }
        public IEnumerable<Branch> Get(BaseSearchFilter branchSearchFilter, out int totalRecords)
        {
            Expression<Func<DAL.DataContracts.Branch, bool>> exp = null;
            totalRecords = 0;
            IQueryable<DAL.DataContracts.Branch> branches = _baseService.GetAll("Merchant").AsQueryable();
            totalRecords = branches.Count();
            if (branchSearchFilter != null)
            {
                if (Convert.ToInt32(branchSearchFilter.MerchantId) > 0)
                {
                    branches = branches.Where(m => m.MerchantId == Convert.ToInt32(branchSearchFilter.MerchantId)).AsQueryable();
                    totalRecords = branches.Count();
                }
                if (!string.IsNullOrEmpty(branchSearchFilter.Search))
                {
                    branchSearchFilter.Search = branchSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(branchSearchFilter);
                    branches = branches.Where(exp).AsQueryable();
                    totalRecords = branches.Count();
                }
                branches = branches.OrderBy(branchSearchFilter.Order + (Convert.ToBoolean(branchSearchFilter.Dir) ? " descending" : ""));
                branches = branches.Skip((Convert.ToInt32(branchSearchFilter.Start) - 1) * Convert.ToInt32(branchSearchFilter.Size)).Take(Convert.ToInt32(branchSearchFilter.Size));
            }
            return branches.Select(t => MappingProfile.MapBranchModelObject(t)).ToList();
        }
        private static Expression<Func<DAL.DataContracts.Branch, bool>> GetFilteredBySearch(BaseSearchFilter branchSearchFilter)
        {
            return e => Convert.ToString(e.Address).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.ContactName).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.CreatedDate).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.Email).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.Geolocation).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.Id).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.MerchantId).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.ModifiedDate).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.Name).ToLower().Contains(branchSearchFilter.Search) || Convert.ToString(e.PhoneNum).ToLower().Contains(branchSearchFilter.Search);
        }
        public void UpdateBranch(Branch branchModel, int id)
        {
            string[] excludedProps = { "Id", "MerchantId" };
            if (branchModel != null)
            {
                branchModel.Id = id;
                var branch = _baseService.GetById(id);
                if (branch != null)
                {
                    _baseService.AddOrUpdate(MappingProfile.MapBranchObject(branchModel, branch), id, excludedProps);
                }
            }
        }
        public void MarkActiveInActive(dynamic branchesSetToActiveInActive)
        {
            if (branchesSetToActiveInActive != null)
            {
                if (branchesSetToActiveInActive.Length > 0)
                {
                    foreach (var item in branchesSetToActiveInActive)
                    {
                        int branchId = Convert.ToInt32(item.GetType().GetProperty("Id").GetValue(item, null));
                        bool IsActive = Convert.ToBoolean(item.GetType().GetProperty("IsActive").GetValue(item, null));
                        var branch = _baseService.GetById(branchId, includedProperties);
                        if (branch != null)
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
                            _baseService.AddOrUpdate(branch, branchId);
                            Expression<Func<DAL.DataContracts.Branch, bool>> exp = e => e.MerchantId == branch.MerchantId && e.IsActive;
                            int activeLocationCount = _baseService.Where(exp).Count();
                            _merchantService.UpdateLocationCount(activeLocationCount, branch.MerchantId);
                        }
                    }
                }
            }
        }
        public Branch GetById(int id)
        {
            var branch = _baseService.GetById(id);
            return MappingProfile.MapBranchModelObject(branch);
        }
    }
}
