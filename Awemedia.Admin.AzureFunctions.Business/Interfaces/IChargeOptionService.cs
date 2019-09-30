using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IChargeOptionService
    {
        IEnumerable<ChargeOption> Get(BaseSearchFilter chargeOptionSearchFilter, out int totalRecords, bool isActive = true);
        bool Add(ChargeOption chargeOption, out bool isDuplicateRecord, int id = 0);
        void MarkActiveInActive(dynamic optionsSetToActiveInActive);
        ChargeOption GetById(int chargeOptionId);
    }
}
