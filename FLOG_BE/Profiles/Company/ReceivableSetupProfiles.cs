using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class ReceivableSetupProfiles : Profile
    {
        public ReceivableSetupProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ReceivableSetup, Features.Companies.ReceivableSetup.GetReceivableSetup.ResponseItem>()
                .ForMember(dest => dest.ReceivableSetupId, opt => opt.MapFrom(src => src.ReceivableSetupId))
                .ForMember(dest => dest.DefaultRateType, opt => opt.MapFrom(src => src.DefaultRateType))
                .ForMember(dest => dest.TaxRateType, opt => opt.MapFrom(src => src.TaxRateType))
                .ForMember(dest => dest.AgingByDocdate, opt => opt.MapFrom(src => src.AgingByDocdate))
                .ForMember(dest => dest.WriteoffLimit, opt => opt.MapFrom(src => src.WriteoffLimit));
        }
    }
}
