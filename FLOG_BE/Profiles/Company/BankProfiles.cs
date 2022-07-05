using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class ProfilesBank : Profile
    {
        public ProfilesBank()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Bank, Features.Companies.Bank.GetBank.ResponseItem>()
              .ForMember(dest => dest.BankId, opt => opt.MapFrom(src => src.BankId))
              .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.BankCode))
              .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
              .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
              .ForMember(dest => dest.CityCode, opt => opt.MapFrom(src => src.CityCode))
              .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Cities.CityName))
              .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Cities.Province))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive))
              .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Cities.Country.CountryName))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive));
              
        }
    }
}
