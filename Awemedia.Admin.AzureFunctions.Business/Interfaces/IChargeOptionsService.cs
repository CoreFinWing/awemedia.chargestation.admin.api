using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IChargeOptionsService
    {
        IEnumerable<ChargeOptionsResponse> Get(bool isActive=true);
        bool Add(ChargeOptionsResponse chargeOptionsResponse, out bool isDuplicateRecord, int id = 0);
        void MarkActiveInActive(List<BaseChargeOptionsFilterResponse> baseChargeOptionsFilterResponses);
    }
}
