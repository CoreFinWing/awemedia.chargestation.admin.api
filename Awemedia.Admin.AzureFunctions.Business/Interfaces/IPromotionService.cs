using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IPromotionService
    {
        IEnumerable<object> Get(BaseSearchFilter promotionSearchFilter, out int totalRecords);
        bool Add(Promotion promotion, out bool isDuplicateRecord, int id = 0);
        void UpdatePromotion(Promotion promotionModel, int id);
        Promotion GetById(int id);
        void Remove(int id);
        void MarkActiveInActive(dynamic promotionSetToActiveInActive);
    }
}
