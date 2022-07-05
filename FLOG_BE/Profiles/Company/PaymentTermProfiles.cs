using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
    public class PaymentTermProfiles : Profile
    {
        public PaymentTermProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.PaymentTerm, Features.Companies.PaymentTerm.GetPaymentTerm.ResponsePaymentTermItem>()
              .ForMember(dest => dest.PaymentTermId, opt => opt.MapFrom(src => src.PaymentTermId))
              .ForMember(dest => dest.PaymentTermCode, opt => opt.MapFrom(src => src.PaymentTermCode))
              .ForMember(dest => dest.PaymentTermDesc, opt => opt.MapFrom(src => src.PaymentTermDesc))
              .ForMember(dest => dest.Due, opt => opt.MapFrom(src => src.Due))
              .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => (EnumUnit)(int)src.Unit)); 
        }
    }
}
