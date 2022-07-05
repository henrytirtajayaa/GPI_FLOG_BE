using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class VendorAddressProfiles : Profile
    {
        public VendorAddressProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.VendorAddress, Features.Companies.VendorAddress.GetVendorAddress.ResponseItem>()
                .ForMember(dest=>dest.VendorId, opt => opt.MapFrom(src=>src.VendorId))
                .ForMember(dest => dest.VendorAddressId, opt => opt.MapFrom(src => src.VendorAddressId))
                .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.Vendor.VendorCode))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.VendorName))
                .ForMember(dest => dest.AddressCode, opt => opt.MapFrom(src => src.AddressCode))
                .ForMember(dest => dest.AddressName, opt => opt.MapFrom(src => src.AddressName))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Handphone, opt => opt.MapFrom(src => src.Handphone))
                .ForMember(dest => dest.Phone1, opt => opt.MapFrom(src => src.Phone1))
                .ForMember(dest => dest.Extension1, opt => opt.MapFrom(src => src.Extension1))
                .ForMember(dest => dest.Phone2, opt => opt.MapFrom(src => src.Phone2))
                .ForMember(dest => dest.Extension2, opt => opt.MapFrom(src => src.Extension2))
                .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.Fax))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.HomePage))
                .ForMember(dest => dest.Neighbourhood, opt => opt.MapFrom(src => src.Neighbourhood))
                .ForMember(dest => dest.Hamlet, opt => opt.MapFrom(src => src.Hamlet))
                .ForMember(dest => dest.UrbanVillage, opt => opt.MapFrom(src => src.UrbanVillage))
                .ForMember(dest => dest.SubDistrict, opt => opt.MapFrom(src => src.SubDistrict))
                .ForMember(dest => dest.CityCode, opt => opt.MapFrom(src => src.CityCode))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityName))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.City.Province))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.City.Country.CountryName))
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode))
                .ForMember(dest => dest.IsSameAddress, opt => opt.MapFrom(src => src.IsSameAddress))
                .ForMember(dest => dest.TaxAddressId, opt => opt.MapFrom(src => src.TaxAddressId))
                .ForMember(dest => dest.Default, opt => opt.MapFrom(src => src.Default));
        }
    }
}
