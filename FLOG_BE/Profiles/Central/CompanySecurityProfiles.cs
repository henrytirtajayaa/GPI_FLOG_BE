using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Central
{
    public class CompanySecurityProfiles : Profile
    {
        public CompanySecurityProfiles()
        {
            CreateMap<FLOG_BE.Model.Central.Entities.CompanySecurity, Features.Central.CompanySecurity.GetCompanySecurity.ReponseItem>()
              .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.SecurityRoleId))
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.PersonId))
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Person.PersonFullName))
              .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
              .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName))
              .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.SecurityRole.SecurityRoleName));

        }
    }
}
