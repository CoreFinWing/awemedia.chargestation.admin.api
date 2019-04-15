using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System;

namespace Awemedia.Chargestation.AzureFunctions.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChargeStationResponse, ChargeStation>()
            .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.Now))
            .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.Now));
        }
    }
}
