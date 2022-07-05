using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class NumberFormatHeaderProfiles : Profile
    {
        public NumberFormatHeaderProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.NumberFormatHeader, Features.Companies.NumberFormatHeader.GetNumberFormatHeader.ResponseItem>()
                .ForMember(dest => dest.FormatHeaderId , opt => opt.MapFrom(src => src.FormatHeaderId))
                .ForMember(dest => dest.DocumentId , opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(dest => dest.Description , opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.LastGeneratedNo , opt => opt.MapFrom(src => src.LastGeneratedNo))
                .ForMember(dest => dest.NumberFormat , opt => opt.MapFrom(src => src.NumberFormat))
                .ForMember(dest => dest.InActive , opt => opt.MapFrom(src => src.InActive))
                .ForMember(dest => dest.IsMonthlyReset , opt => opt.MapFrom(src => src.IsMonthlyReset))
                .ForMember(dest => dest.IsYearlyReset , opt => opt.MapFrom(src => src.IsYearlyReset));
        }
    }
}
