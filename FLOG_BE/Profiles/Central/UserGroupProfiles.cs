using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Central
{
    public class UserGroupProfiles : Profile
    {
        public UserGroupProfiles()
        {
            CreateMap<FLOG_BE.Model.Central.Entities.PersonCategory, Features.Central.UserGroup.GetUserGroup.ReponseItem>()
              .ForMember(dest => dest.UserGroupId, opt => opt.MapFrom(src => src.PersonCategoryId))
              .ForMember(dest => dest.UserGroupCode, opt => opt.MapFrom(src => src.PersonCategoryCode))
              .ForMember(dest => dest.UserGroupName, opt => opt.MapFrom(src => src.PersonCategoryName));
        }
    }
}
