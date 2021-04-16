using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IBaseService<DAL.DataContracts.Promotion> _baseService;
        private readonly string[] navigationalProps = { "Branch" };
        public PromotionService(IBaseService<DAL.DataContracts.Promotion> baseService)
        {
            _baseService = baseService;
        }
        
        public IEnumerable<object> Get(BaseSearchFilter promotionSearchFilter, out int totalRecords)
        {
            totalRecords = 0;
            IEnumerable<Promotion> _promotion = _baseService.GetAll().Select(t => MappingProfile.MapPromotionModelObject(t)).ToList();
            totalRecords = _promotion.Count();
            if (promotionSearchFilter != null)
            {
                Expression<Func<DAL.DataContracts.Promotion, int>> orderByDesc = x => x.Id;
                _promotion = _baseService.GetAll(navigationalProps).ToList().Select(t => MappingProfile.MapPromotionModelObject(t));
                totalRecords = _promotion.Count();

                if (!string.IsNullOrEmpty(promotionSearchFilter.Search) && !string.IsNullOrEmpty(promotionSearchFilter.Type))
                {
                    _promotion = _promotion.Search(promotionSearchFilter.Type, promotionSearchFilter.Search);
                    totalRecords = _promotion.Count();
                }
                _promotion = _promotion.Skip((Convert.ToInt32(promotionSearchFilter.Start) - 1) * Convert.ToInt32(promotionSearchFilter.Size)).Take(Convert.ToInt32(promotionSearchFilter.Size));
                _promotion = _promotion.OrderBy(promotionSearchFilter.Order, promotionSearchFilter.Dir).ToList();
            }
            else
            {
                _promotion = _baseService.GetAll("Branch").Select(t => MappingProfile.MapPromotionModelObject(t)).ToList();
            }
            var groupedData = _promotion.GroupBy(r => r.BranchName)
                                           .Select(group => new { group.Key, Value = group.Select(x => new { x.Id, x.PromotionDesc, x.StartDate, x.EndDate, x.PromotionType }) });
            totalRecords = groupedData.Count();
            groupedData = groupedData.Skip((Convert.ToInt32(promotionSearchFilter.Start) - 1) * Convert.ToInt32(promotionSearchFilter.Size))
                                         .Take(Convert.ToInt32(promotionSearchFilter.Size));
            return groupedData;
        }
    }
}
