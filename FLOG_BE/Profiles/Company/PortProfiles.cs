using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class Port : Profile
    {
        public Port()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Port, Features.Companies.Port.GetPort.ResponseItem>()
              .ForMember(dest => dest.PortCode, opt => opt.MapFrom(src => src.PortCode))
              .ForMember(dest => dest.PortName, opt => opt.MapFrom(src => src.PortName))
              .ForMember(dest => dest.CityCode, opt => opt.MapFrom(src => src.CityCode))
              .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Cities.CityName))
              .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Cities.Province))
              .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Cities.Country.CountryName))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive))
              .ForMember(dest => dest.PortType, opt => opt.MapFrom(src => src.PortType));
        }
    }
}
