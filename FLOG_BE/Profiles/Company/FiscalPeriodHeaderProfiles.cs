using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class FiscalPeriodHeaderProfiles : Profile
    {
        public FiscalPeriodHeaderProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.FiscalPeriodHeader, Features.Companies.FiscalPeriodHeader.GetFiscalPeriodHeader.ResponseItem>()
              .ForMember(dest => dest.FiscalHeaderId, opt => opt.MapFrom(src => src.FiscalHeaderId))
              .ForMember(dest => dest.PeriodYear, opt => opt.MapFrom(src => src.PeriodYear))
              .ForMember(dest => dest.TotalPeriod, opt => opt.MapFrom(src => src.TotalPeriod))
              .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
              .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
              .ForMember(dest => dest.ClosingYear, opt => opt.MapFrom(src => src.ClosingYear));
        }
    }
}
