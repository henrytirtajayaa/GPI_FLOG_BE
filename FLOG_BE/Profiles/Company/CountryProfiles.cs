using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class Country : Profile
    {
        public Country()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Country, Features.Companies.Country.GetCountry.ResponseItem>()
              .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
              .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.CountryName))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive)); 
        }
    }
}
