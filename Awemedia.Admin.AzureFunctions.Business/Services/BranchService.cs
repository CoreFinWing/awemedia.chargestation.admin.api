using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBaseService<DAL.DataContracts.Branch> _baseService;

        public BranchService(IBaseService<DAL.DataContracts.Branch> baseService)
        {
            _baseService = baseService;
        }
        public void AddBranch(List<Branch> branches, int merchantId)
        {
            if (branches != null)
            {
                if (branches.Count > 0)
                {
                    foreach (var branchModel in branches)
                    {
                        branchModel.MerchantId = merchantId;
                        _baseService.AddOrUpdate(MappingProfile.MapBranchObject(branchModel, new DAL.DataContracts.Branch()), 0);
                    }
                }
            }
        }

        public IEnumerable<Branch> Get()
        {
            throw new NotImplementedException();
        }

        public void UpdateBranch(List<Branch> branches, int id)
        {
            string[] excludedProps = { "Id", "MerchantId" };
            if (branches != null)
            {
                if (branches.Count > 0)
                {
                    foreach (var branchModel in branches)
                    {
                        DAL.DataContracts.Branch branch = _baseService.GetById(branchModel.Id);
                        if (branch != null)
                        {
                            _baseService.AddOrUpdate(MappingProfile.MapBranchObject(branchModel, branch), branchModel.Id, excludedProps);
                        }

                    }
                }
            }
        }
        public void MarkActiveInActive(List<DAL.DataContracts.Branch> branches, bool IsActive)
        {
            if (branches != null)
            {
                if (branches.Any())
                {
                    foreach (var item in branches)
                    {
                        DAL.DataContracts.Branch branch = _baseService.GetById(item.Id);
                        if (branch != null)
                        {
                            branch.IsActive = IsActive;
                            branch.ModifiedDate = DateTime.Now;
                            _baseService.AddOrUpdate(branch, item.Id);
                        }
                    }
                }
            }
        }
    }
}
