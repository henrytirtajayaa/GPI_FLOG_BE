using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class CompanySetupProfiles : Profile
    {
        public CompanySetupProfiles()
        {
      
            CreateMap<FLOG_BE.Model.Companies.Entities.CompanySetup, Features.Companies.CompanySetup.GetCompanySetup.ResponseItem>()
                  .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                  .ForMember(dest => dest.AddressName, opt => opt.MapFrom(src => src.CompanyAddress.AddressName))
                  .ForMember(dest => dest.TaxRegistrationNo, opt => opt.MapFrom(src => src.TaxRegistrationNo))
                  .ForMember(dest => dest.CompanyTaxName, opt => opt.MapFrom(src => src.CompanyTaxName))
                  .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.CompanyLogo));
           
              
        }
    }
}
