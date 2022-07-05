using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class AccountSegmentProfiles : Profile
    {
        public AccountSegmentProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.AccountSegment, Features.Companies.AccountSegment.GetAccountSegment.ResponseItem>()
              .ForMember(dest => dest.SegmentId, opt => opt.MapFrom(src => src.SegmentId))
              .ForMember(dest => dest.SegmentNo, opt => opt.MapFrom(src => src.SegmentNo))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
              .ForMember(dest => dest.IsMainAccount, opt => opt.MapFrom(src => src.IsMainAccount));
        }
    }
}
