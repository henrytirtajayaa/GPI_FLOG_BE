using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class DistributionJournalProfiles : Profile
    {
        public DistributionJournalProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.DistributionJournalHeader, Features.Finance.DistributionJournal.GetDistributionByDocNo.ResponseHeader>()
                .ForMember(dest => dest.DistributionHeaderId, opt => opt.MapFrom(src => src.DistributionHeaderId))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.DocumentDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.DecimalPoint, opt => opt.MapFrom(src => src.CurrencyDecimalPoint))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotal, opt => opt.MapFrom(src => src.OriginatingTotal))
                .ForMember(dest => dest.FunctionalTotal, opt => opt.MapFrom(src => src.FunctionalTotal))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DistributionDetails, opt => opt.MapFrom(src => src.DistributionJournalDetails));

            
            CreateMap<FLOG_BE.Model.Companies.Entities.DistributionJournalDetail, Features.Finance.DistributionJournal.GetDistributionByDocNo.ResponseDetail>()
               .ForMember(dest => dest.DistributionDetailId, opt => opt.MapFrom(src => src.DistributionDetailId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.AccountDesc, opt => opt.MapFrom(src => src.AccountDesc))
                .ForMember(dest => dest.JournalNote, opt => opt.MapFrom(src => src.JournalNote))
                .ForMember(dest => dest.OriginatingDebit, opt => opt.MapFrom(src => src.OriginatingDebit))
                .ForMember(dest => dest.OriginatingCredit, opt => opt.MapFrom(src => src.OriginatingCredit))
                .ForMember(dest => dest.FunctionalDebit, opt => opt.MapFrom(src => src.FunctionalDebit))
                .ForMember(dest => dest.FunctionalCredit, opt => opt.MapFrom(src => src.FunctionalCredit))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
