using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class ProfilesCity : Profile
    {
        public ProfilesCity()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.City, Features.Companies.City.GetCity.ResponseItem>()
              .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
              .ForMember(dest => dest.CityCode, opt => opt.MapFrom(src => src.CityCode))
              .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.CityName))
              .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryID))
              .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.CountryName))
              .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province));
              
        }
    }
}
