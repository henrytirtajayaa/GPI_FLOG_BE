using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class FinancialSetupProfiles : Profile
    {
        public FinancialSetupProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.FinancialSetup, Features.Companies.FinancialSetup.GetFinancialSetup.ResponseItem>()
                .ForMember(dest => dest.FinancialSetupId, opt => opt.MapFrom(src => src.FinancialSetupId))
                .ForMember(dest => dest.FuncCurrencyCode, opt => opt.MapFrom(src => src.FuncCurrencyCode))
                .ForMember(dest => dest.DefaultRateType, opt => opt.MapFrom(src => src.DefaultRateType))
                .ForMember(dest => dest.TaxRateType, opt => opt.MapFrom(src => src.TaxRateType))
                .ForMember(dest => dest.DeptSegmentNo, opt => opt.MapFrom(src => src.DeptSegmentNo))
                .ForMember(dest => dest.CheckbookChargesType, opt => opt.MapFrom(src => src.CheckbookChargesType))
                .ForMember(dest => dest.UomScheduleCode, opt => opt.MapFrom(src => src.UomScheduleCode))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
