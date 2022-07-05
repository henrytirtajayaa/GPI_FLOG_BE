using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class JournalEntryProfiles : Profile
    {
        public JournalEntryProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.JournalEntryHeader, Features.Finance.JournalEntry.GetProgressJournalEntry.ResponseItem>()
                .ForMember(dest => dest.JournalEntryHeaderId, opt => opt.MapFrom(src => src.JournalEntryHeaderId))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.SourceDocument, opt => opt.MapFrom(src => src.SourceDocument))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotal, opt => opt.MapFrom(src => src.OriginatingTotal))
                .ForMember(dest => dest.FunctionalTotal, opt => opt.MapFrom(src => src.FunctionalTotal))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedName, opt => opt.MapFrom(src => src.CreatedByName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByName))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy));
            
            CreateMap<FLOG_BE.Model.Companies.Entities.JournalEntryHeader, Features.Finance.JournalEntry.GetHistoryJournalEntry.ResponseItem>()
               .ForMember(dest => dest.JournalEntryHeaderId, opt => opt.MapFrom(src => src.JournalEntryHeaderId))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.SourceDocument, opt => opt.MapFrom(src => src.SourceDocument))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotal, opt => opt.MapFrom(src => src.OriginatingTotal))
                .ForMember(dest => dest.FunctionalTotal, opt => opt.MapFrom(src => src.FunctionalTotal))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedName, opt => opt.MapFrom(src => src.CreatedByName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByName))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.VoidByName, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate));

            CreateMap<FLOG_BE.Model.Companies.Entities.JournalEntryDetail, Features.Finance.JournalEntry.GetDetailJournalEntry.ResponseItem>()
               .ForMember(dest => dest.JournalEntryHeaderId, opt => opt.MapFrom(src => src.JournalEntryHeaderId))
                .ForMember(dest => dest.JournalEntryDetailId, opt => opt.MapFrom(src => src.JournalEntryDetailId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.AccountDescription, opt => opt.MapFrom(src => src.AccountDescription))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingDebit, opt => opt.MapFrom(src => src.OriginatingDebit))
                .ForMember(dest => dest.OriginatingCredit, opt => opt.MapFrom(src => src.OriginatingCredit))
                .ForMember(dest => dest.FunctionalDebit, opt => opt.MapFrom(src => src.FunctionalDebit))
                .ForMember(dest => dest.FunctionalCredit, opt => opt.MapFrom(src => src.FunctionalCredit))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
