using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IBranchService
    {
        IEnumerable<Branch> Get();
        void AddBranch(List<Branch> branches,int merchantId);
        void UpdateBranch(List<Branch> branches, int branchId);
        void MarkActiveInActive(List<DAL.DataContracts.Branch> branches,bool IsActive);
    }
}
