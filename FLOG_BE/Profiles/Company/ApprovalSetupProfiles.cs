using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class ApprovalSetupProfiles : Profile
    {
        public ApprovalSetupProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ApprovalSetupHeader, Features.Companies.ApprovalSetup.GetApprovalSetup.ResponseItem>()
              .ForMember(dest => dest.ApprovalSetupHeaderId, opt => opt.MapFrom(src => src.ApprovalSetupHeaderId))
              .ForMember(dest => dest.ApprovalCode, opt => opt.MapFrom(src => src.ApprovalCode))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
