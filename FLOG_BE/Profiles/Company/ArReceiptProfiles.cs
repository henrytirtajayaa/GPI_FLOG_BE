using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class ArReceiptProfiles : Profile
    {
        public ArReceiptProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ArReceiptHeader, Features.Finance.ArReceipt.GetProgressCustomerReceipt.ResponseItem>()
                .ForMember(dest => dest.ReceiptHeaderId, opt => opt.MapFrom(src => src.ReceiptHeaderId))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.CheckbookCode, opt => opt.MapFrom(src => src.CheckbookCode))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotalPaid, opt => opt.MapFrom(src => src.OriginatingTotalPaid))
                .ForMember(dest => dest.FunctionalTotalPaid, opt => opt.MapFrom(src => src.FunctionalTotalPaid))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByName))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.VoidBy, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidByName, opt => opt.MapFrom(src => src.VoidByName))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment));

            CreateMap<FLOG_BE.Model.Companies.Entities.ArReceiptHeader, Features.Finance.ArReceipt.GetHistoryCustomerReceipt.ResponseItem>()
                .ForMember(dest => dest.ReceiptHeaderId, opt => opt.MapFrom(src => src.ReceiptHeaderId))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.CheckbookCode, opt => opt.MapFrom(src => src.CheckbookCode))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotalPaid, opt => opt.MapFrom(src => src.OriginatingTotalPaid))
                .ForMember(dest => dest.FunctionalTotalPaid, opt => opt.MapFrom(src => src.FunctionalTotalPaid))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByName))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.VoidBy, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidByName, opt => opt.MapFrom(src => src.VoidByName))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment));
        }
    }
}
