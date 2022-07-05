using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class VendorProfiles : Profile
    {
        public VendorProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Vendor, Features.Companies.Vendor.GetVendor.ResponseItem>()

                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.VendorCode))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.AddressName, opt => opt.MapFrom(src => (src.VendorAddress != null ? src.VendorAddress.AddressName : "")))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => (src.VendorAddress != null ? src.VendorAddress.ContactPerson : "")))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => (src.VendorAddress != null ? src.VendorAddress.Address : "")))
                .ForMember(dest => dest.AddressCode, opt => opt.MapFrom(src => src.AddressCode))
                .ForMember(dest => dest.TaxRegistrationNo, opt => opt.MapFrom(src => src.TaxRegistrationNo))
                .ForMember(dest => dest.VendorTaxName, opt => opt.MapFrom(src => src.VendorTaxName))
                .ForMember(dest => dest.VendorGroupCode, opt => opt.MapFrom(src => src.VendorGroupCode))
                .ForMember(dest => dest.PaymentTermCode, opt => opt.MapFrom(src => src.PaymentTermCode))
                .ForMember(dest => dest.HasCreditLimit, opt => opt.MapFrom(src => src.HasCreditLimit))
                .ForMember(dest => dest.CreditLimit, opt => opt.MapFrom(src => src.CreditLimit))
                .ForMember(dest => dest.ShipToAddressCode, opt => opt.MapFrom(src => src.ShipToAddressCode))
                .ForMember(dest => dest.BillToAddressCode, opt => opt.MapFrom(src => src.BillToAddressCode))
                .ForMember(dest => dest.TaxAddressCode, opt => opt.MapFrom(src => src.TaxAddressCode))
                .ForMember(dest => dest.PayableAccountNo, opt => opt.MapFrom(src => src.PayableAccountNo))
                .ForMember(dest => dest.AccruedPayableAccountNo, opt => opt.MapFrom(src => src.AccruedPayableAccountNo));
                
        }
    }
}
