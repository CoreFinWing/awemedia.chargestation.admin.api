using Awemedia.Admin.AzureFunctions.Business.Helpers;
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
        public IEnumerable<Branch> Get(BaseSearchFilter branchSearchFilter, out int totalRecords, bool isActive = true)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(branchSearchFilter.FromDate) && !string.IsNullOrEmpty(branchSearchFilter.ToDate))
            {
                fromDate = DateTime.Parse(branchSearchFilter.FromDate);
                toDate = DateTime.Parse(branchSearchFilter.ToDate);
            }
            else
            {
                fromDate = toDate.AddDays(-30);
            }
            totalRecords = 0;
            IQueryable<DAL.DataContracts.Branch> branches = _baseService.GetAll("Merchant").Where(a => a.CreatedDate >= fromDate && a.CreatedDate <= toDate).AsQueryable();
            var _branches = branches.Select(t => MappingProfile.MapBranchModelObject(t)).AsQueryable();
            totalRecords = _branches.Count();
            if (isActive)
            {
                _branches = _branches.Where(item => item.IsActive.Equals(isActive)).AsQueryable();
                totalRecords = _branches.Count();
            }
            if (branchSearchFilter != null)
            {
                if (Convert.ToInt32(branchSearchFilter.MerchantId) > 0)
                {
                    _branches = _branches.Where(m => m.MerchantId == Convert.ToInt32(branchSearchFilter.MerchantId)).AsQueryable();
                    totalRecords = _branches.Count();
                }
                if (!string.IsNullOrEmpty(branchSearchFilter.Search) && !string.IsNullOrEmpty(branchSearchFilter.Type))
                {
                    _branches = _branches.Search(branchSearchFilter.Type, branchSearchFilter.Search);
                    totalRecords = _branches.Count();
                }
                _branches = _branches.OrderBy(branchSearchFilter.Order + (Convert.ToBoolean(branchSearchFilter.Dir) ? " descending" : ""));
                if (!Convert.ToBoolean(branchSearchFilter.Export))
                {
                    _branches = _branches.Skip((Convert.ToInt32(branchSearchFilter.Start) - 1) * Convert.ToInt32(branchSearchFilter.Size)).Take(Convert.ToInt32(branchSearchFilter.Size));
                }
            }
            return _branches.ToList();
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
            IQueryable<DAL.DataContracts.Branch> branches = _baseService.GetAll("Merchant").AsQueryable();
            var branch = branches.Where(b => b.Id == id).FirstOrDefault();
            if (branch == null)
            {
                return null;
            }
            return MappingProfile.MapBranchModelObject(branch);
        }
    }
}
