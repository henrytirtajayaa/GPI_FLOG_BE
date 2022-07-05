using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class ExchangeRateHeaderProfiles : Profile
    {
        public ExchangeRateHeaderProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ExchangeRateHeader, Features.Companies.ExchangeRateHeader.GetExchangeRateHeader.ResponseItem>()
              .ForMember(dest => dest.ExchangeRateHeaderId, opt => opt.MapFrom(src => src.ExchangeRateHeaderId))
              .ForMember(dest => dest.ExchangeRateCode, opt => opt.MapFrom(src => src.ExchangeRateCode))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
              .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => src.RateType))
              .ForMember(dest => dest.ExpiredPeriod, opt => opt.MapFrom(src => src.ExpiredPeriod))
              .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => src.CalculationType))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        }
    }
}
