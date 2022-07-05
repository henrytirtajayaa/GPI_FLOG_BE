using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class CustomerProfiles : Profile
    {
        public CustomerProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Customer, Features.Companies.Customer.GetCustomer.ResponseItem>()
                  .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                  .ForMember(dest => dest.CustomerCode, opt => opt.MapFrom(src => src.CustomerCode))
                  .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                  .ForMember(dest => dest.AddressName, opt => opt.MapFrom(src => (src.CustomerAddress != null ? src.CustomerAddress.AddressName : "")))
                  .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => (src.CustomerAddress != null ? src.CustomerAddress.ContactPerson : "")))
                  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => (src.CustomerAddress != null ? src.CustomerAddress.Address : "")))
                  .ForMember(dest => dest.AddressCode, opt => opt.MapFrom(src => src.AddressCode))
                  .ForMember(dest => dest.TaxRegistrationNo, opt => opt.MapFrom(src => src.TaxRegistrationNo))
                  .ForMember(dest => dest.CustomerTaxName, opt => opt.MapFrom(src => src.CustomerTaxName))
                  .ForMember(dest => dest.CustomerGroupCode, opt => opt.MapFrom(src => src.CustomerGroupCode))
                  .ForMember(dest => dest.PaymentTermCode, opt => opt.MapFrom(src => src.PaymentTermCode))
                  .ForMember(dest => dest.HasCreditLimit, opt => opt.MapFrom(src => src.HasCreditLimit))
                  .ForMember(dest => dest.CreditLimit, opt => opt.MapFrom(src => src.CreditLimit))
                  .ForMember(dest => dest.ShipToAddressCode, opt => opt.MapFrom(src => src.ShipToAddressCode))
                  .ForMember(dest => dest.BillToAddressCode, opt => opt.MapFrom(src => src.BillToAddressCode))
                  .ForMember(dest => dest.TaxAddressCode, opt => opt.MapFrom(src => src.TaxAddressCode))
                  .ForMember(dest => dest.ReceivableAccountNo, opt => opt.MapFrom(src => src.ReceivableAccountNo))
                  .ForMember(dest => dest.AccruedReceivableAccountNo, opt => opt.MapFrom(src => src.AccruedReceivableAccountNo))
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                  .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive));
              
             
        }
    }
}
