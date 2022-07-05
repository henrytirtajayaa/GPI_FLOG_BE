using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class ShippingLineProfiles : Profile
    {
        public ShippingLineProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ShippingLine, Features.Companies.ShippingLine.GetShippingLine.ResponseItem>()
                .ForMember(dest => dest.ShippingLineId, opt => opt.MapFrom(src => src.ShippingLineId))
                .ForMember(dest => dest.ShippingLineCode, opt => opt.MapFrom(src => src.ShippingLineCode))
                .ForMember(dest => dest.ShippingLineName, opt => opt.MapFrom(src => src.ShippingLineName))
                .ForMember(dest => dest.ShippingLineType, opt => opt.MapFrom(src => src.ShippingLineType))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.VendorName))
                .ForMember(dest => dest.IsFeeder, opt => opt.MapFrom(src => src.IsFeeder))
                .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
