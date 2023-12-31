﻿using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IBranchService
    {
        IEnumerable<object> Get(BaseSearchFilter chargeStationSearchFilter, out int totalRecords, string email, bool isActive = true);
        void AddBranch(Branch branch, int merchantId);
        void UpdateBranch(Branch branchModel, int id);
        void MarkActiveInActive(dynamic branchesSetToActiveInActive);
        Branch GetById(int id);
        List<object> Search(string keyword);

    }
}
