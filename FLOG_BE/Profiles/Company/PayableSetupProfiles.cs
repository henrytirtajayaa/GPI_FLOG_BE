using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class PayableSetupProfiles : Profile
    {
        public PayableSetupProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.PayableSetup, Features.Companies.PayableSetup.GetPayableSetup.ResponseItem>()
                .ForMember(dest => dest.PayableSetupId, opt => opt.MapFrom(src => src.PayableSetupId))
                .ForMember(dest => dest.DefaultRateType, opt => opt.MapFrom(src => src.DefaultRateType))
                .ForMember(dest => dest.TaxRateType, opt => opt.MapFrom(src => src.TaxRateType))
                .ForMember(dest => dest.AgingByDocdate, opt => opt.MapFrom(src => src.AgingByDocdate))
                .ForMember(dest => dest.WriteoffLimit, opt => opt.MapFrom(src => src.WriteoffLimit));
        }
    }
}
