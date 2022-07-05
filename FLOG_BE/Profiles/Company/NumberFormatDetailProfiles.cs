using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class NumberFormatDetailProfiles : Profile
    {
        public NumberFormatDetailProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.NumberFormatDetail, Features.Companies.NumberFormatDetail.GetNumberFormatDetail.ResponseItem>()
                .ForMember(dest => dest.FormatHeaderId, opt => opt.MapFrom(src => src.FormatHeaderId))
                .ForMember(dest => dest.SegmentNo, opt => opt.MapFrom(src => src.SegmentNo))
                .ForMember(dest => dest.SegmentType, opt => opt.MapFrom(src => src.SegmentType))
                .ForMember(dest => dest.SegmentLength, opt => opt.MapFrom(src => src.SegmentLength))
                .ForMember(dest => dest.MaskFormat, opt => opt.MapFrom(src => src.MaskFormat))
                .ForMember(dest => dest.StartingValue, opt => opt.MapFrom(src => src.StartingValue))
                .ForMember(dest => dest.EndingValue, opt => opt.MapFrom(src => src.EndingValue))
                .ForMember(dest => dest.Increase, opt => opt.MapFrom(src => src.Increase));
        }
    }
}
