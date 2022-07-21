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
        private readonly IBaseService<DAL.DataContracts.User> _userService;
        private readonly IMerchantService _merchantService;
        readonly string[] includedProperties = new string[] { "ChargeStation" };
        public BranchService(IBaseService<DAL.DataContracts.Branch> baseService, IMerchantService merchantService, IBaseService<DAL.DataContracts.User> userService)
        {
            _baseService = baseService;
            _merchantService = merchantService;
            _userService = userService;
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
        public IEnumerable<object> Get(BaseSearchFilter branchSearchFilter, out int totalRecords, string email, bool isActive = true)
        {
            string[] navigationalProps = { "Merchant"};
            IEnumerable<Branch> _branches = null;

            
            totalRecords = 0;
            if (!string.IsNullOrEmpty(branchSearchFilter.FromDate) && !string.IsNullOrEmpty(branchSearchFilter.ToDate))
            {
                DateTime fromDate = DateTime.Now.ToUniversalTime();
                DateTime toDate = DateTime.Now.ToUniversalTime();
                fromDate = Utility.ParseStartAndEndDates(branchSearchFilter, ref toDate);
                _branches = _baseService.Where(a => a.CreatedDate.Value.Date >= fromDate && a.CreatedDate.Value.Date <= toDate, navigationalProps).Select(t => MappingProfile.MapBranchModelObject(t)).ToList();
            }
            else
            {
                _branches = _baseService.GetAll(navigationalProps).Select(t => MappingProfile.MapBranchModelObject(t)).ToList();
            }

            //filtering based on user
            var user = _userService.Where(x => x.Email == email,new string [] {"Role" ,"MappedMerchant"}).FirstOrDefault();
            if (user?.Role?.Name == "user")
            {
                _branches = _branches.Where(x => user.MappedMerchant.Any(m=>m.MerchantId==x.MerchantId)).AsQueryable();
            }

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
                _branches = _branches.OrderBy(branchSearchFilter.Order, branchSearchFilter.Dir);
                if (!Convert.ToBoolean(branchSearchFilter.Export))
                {
                    _branches = _branches.Skip((Convert.ToInt32(branchSearchFilter.Start) - 1) * Convert.ToInt32(branchSearchFilter.Size)).Take(Convert.ToInt32(branchSearchFilter.Size));
                }
                else
                {
                    var dataToExport = _branches.Select(b => new { b.Id, b.Address, b.ContactName, CreatedDate = b.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss tt"), b.Email, b.Geolocation, b.IsActive, b.MerchantName, b.Name, b.PhoneNum }).AsQueryable();
                    return dataToExport.ToList();
                }
            }

            return _branches.ToList();
        }
        public void UpdateBranch(Branch branchModel, int id)
        {
            string[] excludedProps = { "Id", "MerchantId", "CreatedDate" };
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
                            branch.ModifiedDate = DateTime.Now.ToUniversalTime();
                            if (branch.ChargeStation.Count > 0)
                            {
                                foreach (var chargeStation in branch.ChargeStation)
                                {
                                    chargeStation.IsActive = IsActive;
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
            IQueryable<DAL.DataContracts.Branch> branches = _baseService.GetAll("Merchant","Country").AsQueryable();
            var branch = branches.Where(b => b.Id == id).FirstOrDefault();
            if (branch == null)
            {
                return null;
            }
            return MappingProfile.MapBranchModelObject(branch);
        }
        public List<object> Search(string keyword)
        {
            return _baseService.Where(m => m.Name.ToLower().Contains(keyword)).Select(s => new { s.Id, s.Name }).ToList<object>();
        }
    }
}
