using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class VendorGroupProfiles : Profile
    {
        public VendorGroupProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.VendorGroup, Features.Companies.VendorGroup.GetVendorGroup.ResponseItem>()
                .ForMember(dest => dest.VendorGroupCode, opt => opt.MapFrom(src => src.VendorGroupCode))
                .ForMember(dest => dest.VendorGroupName, opt => opt.MapFrom(src => src.VendorGroupName))
                //.ForMember(dest => dest.AddressCode, opt => opt.MapFrom(src => src.AddressCode))
                .ForMember(dest => dest.PaymentTermCode, opt => opt.MapFrom(src => src.PaymentTermCode))
                .ForMember(dest => dest.PayableAccountNo, opt => opt.MapFrom(src => src.PayableAccountNo))
                .ForMember(dest => dest.AccruedPayableAccountNo, opt => opt.MapFrom(src => src.AccruedPayableAccountNo))
                .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive));
        }
    }
}
