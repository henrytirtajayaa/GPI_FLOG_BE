using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class TaskScheduleProfiles : Profile
    {
        public TaskScheduleProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.TaxSchedule, Features.Companies.TaxSchedule.GetTaxSchedule.ResponseItem>()
              .ForMember(dest => dest.TaxScheduleId, opt => opt.MapFrom(src => src.TaxScheduleId))
              .ForMember(dest => dest.TaxScheduleCode, opt => opt.MapFrom(src => src.TaxScheduleCode))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.IsSales, opt => opt.MapFrom(src => src.IsSales == true ? "Sales" : "Purchase"))
              .ForMember(dest => dest.PercentOfSalesPurchase, opt => opt.MapFrom(src => src.PercentOfSalesPurchase))
              .ForMember(dest => dest.TaxablePercent, opt => opt.MapFrom(src => src.TaxablePercent))
              .ForMember(dest => dest.RoundingType, opt => opt.MapFrom(src => src.RoundingType == 0 ? "Rounding Up" : (src.RoundingType == 1 ? "Rounding Down" : "Half Rounding")))
              .ForMember(dest => dest.RoundingLimitAmount, opt => opt.MapFrom(src => src.RoundingLimitAmount))
              .ForMember(dest => dest.TaxInclude, opt => opt.MapFrom(src => src.TaxInclude))
              .ForMember(dest => dest.WithHoldingTax, opt => opt.MapFrom(src => src.WithHoldingTax))
              .ForMember(dest => dest.TaxAccountNo, opt => opt.MapFrom(src => src.TaxAccountNo))
              .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive ? "Inactive" : "Active"));
        }
    }
}
