using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IMerchantService
    {
        IEnumerable<Merchant> Get(BaseSearchFilter merchantSearchFilter);
        int AddMerchant(Merchant merchantModel, int id = 0);
        void UpdateMerchant(Merchant merchantModel, int id);
        object IsMerchantExists(int id);
        void MarkActiveInActive(List<BaseDeletionModel> baseChargeOptionsFilterResponses);
    }
}
