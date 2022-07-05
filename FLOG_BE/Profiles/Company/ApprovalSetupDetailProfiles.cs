using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class ApprovalSetupDetailProfiles : Profile
    {
        public ApprovalSetupDetailProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ApprovalSetupDetail, Features.Companies.ApprovalSetupDetail.GetApprovalSetupDetail.ResponseItem>()
              .ForMember(dest => dest.ApprovalSetupDetailId, opt => opt.MapFrom(src => src.ApprovalSetupDetailId))
              .ForMember(dest => dest.ApprovalSetupHeaderId, opt => opt.MapFrom(src => src.ApprovalSetupHeaderId))
              .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
              .ForMember(dest => dest.PersonCategoryId, opt => opt.MapFrom(src => src.PersonCategoryId))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
              .ForMember(dest => dest.HasLimit, opt => opt.MapFrom(src => src.HasLimit))
              .ForMember(dest => dest.ApprovalLimit, opt => opt.MapFrom(src => src.ApprovalLimit))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
