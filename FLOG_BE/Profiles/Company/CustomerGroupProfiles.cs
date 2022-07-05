using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class CustomerGroupProfiles : Profile
    {
        public CustomerGroupProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.CustomerGroup, Features.Companies.CustomerGroup.GetCustomerGroup.ResponseItem>()
              .ForMember(dest => dest.CustomerGroupId, opt => opt.MapFrom(src => src.CustomerGroupId))
              .ForMember(dest => dest.CustomerGroupCode, opt => opt.MapFrom(src => src.CustomerGroupCode))
              .ForMember(dest => dest.CustomerGroupName, opt => opt.MapFrom(src => src.CustomerGroupName))
              //.ForMember(dest => dest.AddressCode, opt => opt.MapFrom(src => src.AddressCode))
              .ForMember(dest => dest.PaymentTermCode, opt => opt.MapFrom(src => src.PaymentTermCode))
              .ForMember(dest => dest.ReceivableAccountNo, opt => opt.MapFrom(src => src.ReceivableAccountNo))
              .ForMember(dest => dest.AccruedReceivableAccountNo, opt => opt.MapFrom(src => src.AccruedReceivableAccountNo))
              .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive)); 
        }
    }
}
