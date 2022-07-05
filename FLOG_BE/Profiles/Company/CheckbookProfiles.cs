using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class CheckbookProfiles : Profile
    {
        public CheckbookProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.Checkbook, Features.Companies.Checkbook.GetCheckbook.ResponseItem>()
                .ForMember(dest => dest.CheckbookCode, opt => opt.MapFrom(src => src.CheckbookCode))
                .ForMember(dest => dest.CheckbookName, opt => opt.MapFrom(src => src.CheckbookName))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.BankCode))
                .ForMember(dest => dest.BankAccountCode, opt => opt.MapFrom(src => src.BankAccountCode))
                .ForMember(dest => dest.CheckbookAccountNo, opt => opt.MapFrom(src => src.CheckbookAccountNo))
                .ForMember(dest => dest.HasCheckoutApproval, opt => opt.MapFrom(src => src.HasCheckoutApproval))
                .ForMember(dest => dest.ApprovalCode, opt => opt.MapFrom(src => src.ApprovalCode))
                .ForMember(dest => dest.CheckbookInDocNo, opt => opt.MapFrom(src => src.CheckbookInDocNo))
                .ForMember(dest => dest.CheckbookOutDocNo, opt => opt.MapFrom(src => src.CheckbookOutDocNo))
                .ForMember(dest => dest.ReceiptDocNo, opt => opt.MapFrom(src => src.ReceiptDocNo))
                .ForMember(dest => dest.PaymentDocNo, opt => opt.MapFrom(src => src.PaymentDocNo))
                .ForMember(dest => dest.ReconcileDocNo, opt => opt.MapFrom(src => src.ReconcileDocNo))
                .ForMember(dest => dest.IsCash, opt => opt.MapFrom(src => src.IsCash))
                .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive));
        }
    }
}
