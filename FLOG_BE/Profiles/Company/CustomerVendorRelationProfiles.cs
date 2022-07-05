using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class CustomerVendorRelationProfiles : Profile
    {
        public CustomerVendorRelationProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.CustomerVendorRelation, Features.Companies.CustomerVendorRelation.GetCustomerVendorRelation.ResponseItem>()
              .ForMember(dest => dest.CustomerVendorRelationId, opt => opt.MapFrom(src => src.RelationId))
              .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customers.CustomerId))
              .ForMember(dest => dest.CustomerCode, opt => opt.MapFrom(src => src.Customers.CustomerCode))
              .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customers.CustomerName))
              .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.Vendors.VendorCode))
              .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendors.VendorName));

        }
    }
}
