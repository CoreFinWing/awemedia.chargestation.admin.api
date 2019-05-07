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
    public class ChargeOptionsService : IChargeOptionsService
    {
        private readonly IBaseService<ChargeOptions> _baseService;

        public ChargeOptionsService(IBaseService<ChargeOptions> baseService)
        {
            _baseService = baseService;
        }
        public bool Add(ChargeOptionsResponse chargeOptionsResponse, out bool isDuplicateRecord, int id = 0)
        {
            isDuplicateRecord = false;
            if (chargeOptionsResponse == null)
                return false;
            try
            {
                ChargeOptions chargeOptions = _baseService.AddOrUpdate(MappingProfile.MapChargeOptionsObjects(chargeOptionsResponse), id);
                return chargeOptions == null ? false : true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("IX_ChargeOptions"))
                    isDuplicateRecord = true;
                return false;
            }
        }

        public void MarkActiveInActive(List<BaseChargeOptionsFilterResponse> baseChargeOptionsFilterResponses)
        {
            if (baseChargeOptionsFilterResponses != null)
            {
                if (baseChargeOptionsFilterResponses.Any())
                {
                    foreach (var item in baseChargeOptionsFilterResponses)
                    {
                        ChargeOptions chargeOption = _baseService.GetById(item.Id);
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
        public IEnumerable<ChargeOptionsResponse> Get(bool isActive = true)
        {
            if (isActive)
                return _baseService.GetAll().Select(t => MappingProfile.MapChargeOptionsResponseObjects(t)).Where(item => item.IsActive.Equals(isActive));
            else
                return _baseService.GetAll().Select(t => MappingProfile.MapChargeOptionsResponseObjects(t));
        }

        public ChargeOptionsResponse GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
