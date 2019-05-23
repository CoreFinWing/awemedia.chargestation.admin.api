using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class ChargeOptionsService : IChargeOptionService
    {
        private readonly IBaseService<DAL.DataContracts.ChargeOptions> _baseService;

        public ChargeOptionsService(IBaseService<DAL.DataContracts.ChargeOptions> baseService)
        {
            _baseService = baseService;
        }
        public bool Add(ChargeOption chargeOptionsResponse, out bool isDuplicateRecord, int id = 0)
        {
            isDuplicateRecord = false;
            if (chargeOptionsResponse == null)
                return false;
            try
            {
                DAL.DataContracts.ChargeOptions chargeOptions = _baseService.AddOrUpdate((DAL.DataContracts.ChargeOptions)MappingProfile.MapChargeOptionsObjects(chargeOptionsResponse), id);
                return chargeOptions == null ? false : true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("IX_ChargeOptions"))
                    isDuplicateRecord = true;
                return false;
            }
        }

        public void MarkActiveInActive(List<BaseChargeOptionsFilterModel> baseChargeOptionsFilterResponses)
        {
            if (baseChargeOptionsFilterResponses != null)
            {
                if (baseChargeOptionsFilterResponses.Any())
                {
                    foreach (var item in baseChargeOptionsFilterResponses)
                    {
                        DAL.DataContracts.ChargeOptions chargeOption = _baseService.GetById(item.Id);
                        if (chargeOption != null)
                        {
                            chargeOption.IsActive = item.IsActive;
                            chargeOption.ModifiedDate = DateTime.Now;
                            _baseService.AddOrUpdate(chargeOption, item.Id);
                        }
                    }
                }
            }
        }
        public IEnumerable<ChargeOption> Get(bool isActive = true)
        {
            if (isActive)
                return _baseService.GetAll().Select(t => MappingProfile.MapChargeOptionsResponseObjects(t)).Where(item => item.IsActive.Equals(isActive));
            else
                return _baseService.GetAll().Select(t => MappingProfile.MapChargeOptionsResponseObjects(t));
        }
    }
}
