using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Account, Features.Companies.Account.GetAccount.ResponseItem>()
              .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
              .ForMember(dest => dest.Segment1, opt => opt.MapFrom(src => src.Segment1))
              .ForMember(dest => dest.Segment2, opt => opt.MapFrom(src => src.Segment2))
              .ForMember(dest => dest.Segment3, opt => opt.MapFrom(src => src.Segment3))
              .ForMember(dest => dest.Segment4, opt => opt.MapFrom(src => src.Segment4))
              .ForMember(dest => dest.Segment5, opt => opt.MapFrom(src => src.Segment5))
              .ForMember(dest => dest.Segment6, opt => opt.MapFrom(src => src.Segment6))
              .ForMember(dest => dest.Segment7, opt => opt.MapFrom(src => src.Segment7))
              .ForMember(dest => dest.Segment8, opt => opt.MapFrom(src => src.Segment8))
              .ForMember(dest => dest.Segment9, opt => opt.MapFrom(src => src.Segment9))
              .ForMember(dest => dest.Segment10, opt => opt.MapFrom(src => src.Segment10))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.PostingType, opt => opt.MapFrom(src =>  src.PostingType))
              .ForMember(dest => dest.PostingType, opt => opt.MapFrom(src =>  src.PostingType == true? "Profit Loss" : "Balance Sheet"))
              .ForMember(dest => dest.NormalBalance, opt => opt.MapFrom(src => src.NormalBalance == true ? "Debit" : "Credit"))
              .ForMember(dest => dest.NoDirectEntry, opt => opt.MapFrom(src => src.NoDirectEntry == true?"Yes":"No" ))
              .ForMember(dest => dest.Revaluation, opt => opt.MapFrom(src => src.Revaluation))
              .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive == true ? "Inactive":"Active"));
        }
    }
}
