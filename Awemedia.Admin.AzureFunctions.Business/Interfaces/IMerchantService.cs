﻿using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IMerchantService
    {
        IEnumerable<Merchant> Get(BaseSearchFilter merchantSearchFilter,out int totalRecords);
        int AddMerchant(Merchant merchantModel, int id = 0);
        void UpdateMerchant(Merchant merchantModel, int id);
        object IsMerchantExists(int id);
        void MarkActiveInActive(dynamic merchantsToSetActiveInActive);
        void UpdateLocationCount(int count,int merchantId);
        DAL.DataContracts.Merchant GetById(int id);
    }
}
