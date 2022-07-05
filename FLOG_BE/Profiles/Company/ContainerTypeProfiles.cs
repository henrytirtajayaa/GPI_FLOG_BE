using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class ConntainerTypeProfiles : Profile
    {
        public ConntainerTypeProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Container, Features.Companies.Container.GetContainer.ResponseItem>()
              .ForMember(dest => dest.ContainerId, opt => opt.MapFrom(src => src.ContainerId))
              .ForMember(dest => dest.ContainerCode, opt => opt.MapFrom(src => src.ContainerCode))
              .ForMember(dest => dest.ContainerName, opt => opt.MapFrom(src => src.ContainerName))
              .ForMember(dest => dest.ContainerSize, opt => opt.MapFrom(src => src.ContainerSize))
              .ForMember(dest => dest.ContainerType, opt => opt.MapFrom(src => src.ContainerType))
              .ForMember(dest => dest.ContainerTeus, opt => opt.MapFrom(src => src.ContainerTeus))
              .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive));
        }
    }
}
