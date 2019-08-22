using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBaseService<DAL.DataContracts.Branch> _baseService;
        private readonly IMerchantService _merchantService;

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
                if (!string.IsNullOrEmpty(branchSearchFilter.Search))
                {
                    branchSearchFilter.Search = branchSearchFilter.Search.ToLower();
                    exp = GetFilteredBySearch(branchSearchFilter);
                    branches = branches.Where(exp).AsQueryable();
                }
                branches = branches.OrderBy(branchSearchFilter.Order + (Convert.ToBoolean(branchSearchFilter.Dir) ? " descending" : ""));
                branches = branches.Skip((Convert.ToInt32(branchSearchFilter.Start) - 1) * Convert.ToInt32(branchSearchFilter.Size)).Take(Convert.ToInt32(branchSearchFilter.Size));
            }
            return branches.Select(t => MappingProfile.MapBranchModelObject(t)).ToList();
        }
        private static Expression<Func<DAL.DataContracts.Branch, bool>> GetFilteredBySearch(BaseSearchFilter branchSearchFilter)
        {
            return e => e.Address.ToLower().Contains(branchSearchFilter.Search) || e.ContactName.ToString().ToLower().Contains(branchSearchFilter.Search) || e.CreatedDate.ToString().ToLower().Contains(branchSearchFilter.Search) || e.Email.ToString().ToLower().Contains(branchSearchFilter.Search) || e.Geolocation.ToString().ToLower().Contains(branchSearchFilter.Search) || e.Id.ToString().ToLower().Contains(branchSearchFilter.Search) || e.MerchantId.ToString().ToLower().Contains(branchSearchFilter.Search) || e.ModifiedDate.ToString().ToLower().Contains(branchSearchFilter.Search) || e.Name.ToString().ToLower().Contains(branchSearchFilter.Search) || e.Name.ToString().ToLower().Contains(branchSearchFilter.Search) || e.PhoneNum.ToString().ToLower().Contains(branchSearchFilter.Search);
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
                        var branch = _baseService.GetById(branchId);
                        if (branch != null)
                        {
                            branch.IsActive = IsActive;
                            branch.ModifiedDate = DateTime.Now;
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
