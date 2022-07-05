using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class ContainerDepotProfiles : Profile
    {
        public ContainerDepotProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ContainerDepot, Features.Companies.ContainerDepot.GetContainerDepot.ResponseItem>()
              .ForMember(dest => dest.ContainerDepotId, opt => opt.MapFrom(src => src.ContainerDepotId))
              .ForMember(dest => dest.DepotCode, opt => opt.MapFrom(src => src.DepotCode))
              .ForMember(dest => dest.DepotName, opt => opt.MapFrom(src => src.DepotName))
              .ForMember(dest => dest.OwnerVendorId, opt => opt.MapFrom(src => src.OwnerVendorId))
              .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.VendorCode) ? src.VendorCode : ""))
              .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.VendorName)?src.VendorName : ""))
              .ForMember(dest => dest.CityCode, opt => opt.MapFrom(src => src.CityCode))
              .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Cities.CityName))
              .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Cities.Province))
              .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Cities.Country.CountryName))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive));
              
        }
    }
}
