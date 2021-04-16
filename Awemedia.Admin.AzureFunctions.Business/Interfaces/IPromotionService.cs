using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IPromotionService
    {
        IEnumerable<object> Get(BaseSearchFilter promotionSearchFilter, out int totalRecords);
    }
}
