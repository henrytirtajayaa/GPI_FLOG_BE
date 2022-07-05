using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Central
{
    public class SecurityRoleProfiles : Profile
    {
        public SecurityRoleProfiles()
        {
            CreateMap<FLOG_BE.Model.Central.Entities.SecurityRole, Features.Authentication.GetTokenDetail.ResponseRole>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SecurityRoleId))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SecurityRoleName));

            CreateMap<FLOG_BE.Model.Central.Entities.SecurityRole, Features.Central.SecurityRoles.GetSecurityRole.ReponseItem>()
              .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.SecurityRoleId))
              .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.SecurityRoleCode))
              .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.SecurityRoleName));
              


            CreateMap<FLOG_BE.Model.Central.Entities.SecurityRole, Features.Central.SecurityRoles.GetSecurityRoleDetail.ReponseItem>()
             .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.SecurityRoleId))
             .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.SecurityRoleCode))
             .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.SecurityRoleName))
             .ForMember(dest => dest.RoleAccess, opt => opt.MapFrom(src => src.RoleAccess));

            CreateMap<FLOG_BE.Model.Central.Entities.SecurityRoleAccess, Features.Central.SecurityRoles.GetSecurityRoleDetail.ResponseRoleAccess>()
              .ForMember(dest => dest.AllowAccessOpen, opt => opt.MapFrom(src => src.AllowOpen))
              .ForMember(dest => dest.AllowAccessNew, opt => opt.MapFrom(src => src.AllowNew))
              .ForMember(dest => dest.AllowAccessEdit, opt => opt.MapFrom(src => src.AllowEdit))
              .ForMember(dest => dest.AllowAccessDelete, opt => opt.MapFrom(src => src.AllowDelete))
              .ForMember(dest => dest.AllowAccessPost, opt => opt.MapFrom(src => src.AllowPost))
              .ForMember(dest => dest.AllowAccessPrint, opt => opt.MapFrom(src => src.AllowPrint))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Form.FormName)); 
        



        }
    }
}
