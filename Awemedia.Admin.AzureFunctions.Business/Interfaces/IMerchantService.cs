using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IMerchantService
    {
        IEnumerable<object> Get(BaseSearchFilter merchantSearchFilter, out int totalRecords, bool isActive = true);
        int AddMerchant(Merchant merchantModel, int id = 0);
        void UpdateMerchant(Merchant merchantModel, int id);
        object IsMerchantExists(int id);
        void MarkActiveInActive(dynamic merchantsToSetActiveInActive);
        void UpdateLocationCount(int count, int merchantId);
        Merchant GetById(int id);
        List<object> Search(string keyword);
    }
}
