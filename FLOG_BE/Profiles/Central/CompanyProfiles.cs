using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Central
{
    public class CompanyProfiles : Profile
    {
        public CompanyProfiles()
        {
            CreateMap<FLOG_BE.Model.Central.Entities.Company, Features.Central.Company.GetCompany.ReponseItem>()
              .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
              .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
              .ForMember(dest => dest.DatabaseId, opt => opt.MapFrom(src => src.DatabaseId))
              .ForMember(dest => dest.DatabaseAddress, opt => opt.MapFrom(src => src.DatabaseAddress))
              .ForMember(dest => dest.DatabasePassword, opt => opt.MapFrom(src => src.DatabasePassword))
              .ForMember(dest => dest.CoaSymbol, opt => opt.MapFrom(src => src.CoaSymbol))
              .ForMember(dest => dest.CoaTotalLength, opt => opt.MapFrom(src => src.CoaTotalLength))
              .ForMember(dest => dest.SmtpServer, opt => opt.MapFrom(src => src.SmtpServer))
              .ForMember(dest => dest.SmtpPort, opt => opt.MapFrom(src => src.SmtpPort))
              .ForMember(dest => dest.SmtpUser, opt => opt.MapFrom(src => src.SmtpUser))
              .ForMember(dest => dest.SmtpPassword, opt => opt.MapFrom(src => src.SmtpPassword));

            CreateMap<FLOG_BE.Model.Central.Entities.Company, Features.Authentication.GetTokenDetail.ResponseCompany>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CompanyName))
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CompanyId));
        }
    }
}
