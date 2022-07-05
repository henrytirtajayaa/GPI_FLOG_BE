using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Central
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<FLOG_BE.Model.Central.Entities.Person, Features.Central.User.GetUser.ReponseItem>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.PersonId))
              .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.PersonFullName))
              .ForMember(dest => dest.UserPassword, opt => opt.MapFrom(src => src.PersonPassword))
              .ForMember(dest => dest.UserGroupId, opt => opt.MapFrom(src => src.PersonCategoryId))
              .ForMember(dest => dest.UserGroupCode, opt => opt.MapFrom(src => src.PersonCategory.PersonCategoryCode))
              .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive));
        }
    }
}
