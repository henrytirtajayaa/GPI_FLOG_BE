using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class CurrencyProfiles : Profile
    {
        public CurrencyProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Currency, Features.Companies.Currency.GetCurrency.ResponseItem>()
              .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
              .ForMember(dest => dest.DecimalPlaces, opt => opt.MapFrom(src => src.DecimalPlaces))
              .ForMember(dest => dest.RealizedGainAcc, opt => opt.MapFrom(src => src.RealizedGainAcc))
              .ForMember(dest => dest.RealizedLossAcc, opt => opt.MapFrom(src => src.RealizedLossAcc))
              .ForMember(dest => dest.UnrealizedGainAcc, opt => opt.MapFrom(src => src.UnrealizedGainAcc))
              .ForMember(dest => dest.UnrealizedLossAcc, opt => opt.MapFrom(src => src.UnrealizedLossAcc))
              .ForMember(dest => dest.IsFunctional, opt => opt.MapFrom(src => src.IsFunctional))
              .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive))
              .ForMember(dest => dest.CurrencyUnit, opt => opt.MapFrom(src => src.CurrencyUnit))
              .ForMember(dest => dest.CurrencySubUnit, opt => opt.MapFrom(src => src.CurrencySubUnit)); 
        }
    }
}
