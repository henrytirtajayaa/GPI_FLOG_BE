using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class TaxRefferenceNumberProfiles : Profile
    {
        public TaxRefferenceNumberProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.TaxRefferenceNumber, Features.Companies.TaxRefferenceNumber.GetTaxRefferenceNumber.ResponseItem>()
              .ForMember(dest => dest.TaxRefferenceId, opt => opt.MapFrom(src => src.TaxRefferenceId))
              .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
              .ForMember(dest => dest.ReffNoStart, opt => opt.MapFrom(src => src.ReffNoStart))
              .ForMember(dest => dest.ReffNoEnd, opt => opt.MapFrom(src => src.ReffNoEnd))
              .ForMember(dest => dest.DocLength, opt => opt.MapFrom(src => src.DocLength))
              .ForMember(dest => dest.LastNo, opt => opt.MapFrom(src => src.LastNo))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == 1 ? "Inactive" : "Active"));
             
        }
    }
}
