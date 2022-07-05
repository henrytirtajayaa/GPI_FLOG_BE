using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class APApplyProfiles : Profile
    {
        public APApplyProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.APApplyHeader, Features.Finance.APApply.GetProgressApplyPayable.ResponseItem>()
                .ForMember(dest => dest.PayableApplyId, opt => opt.MapFrom(src => src.PayableApplyId))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.PaymentHeaderId, opt => opt.MapFrom(src => src.PaymentHeaderId))
                .ForMember(dest => dest.CheckbookTransactionId, opt => opt.MapFrom(src => src.CheckbookTransactionId))
                .ForMember(dest => dest.PayableTransactionId, opt => opt.MapFrom(src => src.PayableTransactionId))                
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.ReffDocumentNo, opt => opt.MapFrom(src => src.ReffDocumentNo))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.VendorCode))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotalPaid, opt => opt.MapFrom(src => src.OriginatingTotalPaid))
                .ForMember(dest => dest.FunctionalTotalPaid, opt => opt.MapFrom(src => src.FunctionalTotalPaid))
                .ForMember(dest => dest.OriginatingTotalInvoice, opt => opt.MapFrom(src => src.OriginatingTotalInvoice))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByName))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.VoidBy, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment));

            CreateMap<FLOG_BE.Model.Companies.Entities.APApplyHeader, Features.Finance.APApply.GetHistoryApplyPayable.ResponseItem>()
                .ForMember(dest => dest.PayableApplyId, opt => opt.MapFrom(src => src.PayableApplyId))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.PaymentHeaderId, opt => opt.MapFrom(src => src.PaymentHeaderId))
                .ForMember(dest => dest.CheckbookTransactionId, opt => opt.MapFrom(src => src.CheckbookTransactionId))
                .ForMember(dest => dest.PayableTransactionId, opt => opt.MapFrom(src => src.PayableTransactionId))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.ReffDocumentNo, opt => opt.MapFrom(src => src.ReffDocumentNo))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.VendorCode))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotalPaid, opt => opt.MapFrom(src => src.OriginatingTotalPaid))
                .ForMember(dest => dest.FunctionalTotalPaid, opt => opt.MapFrom(src => src.FunctionalTotalPaid))
                .ForMember(dest => dest.OriginatingTotalInvoice, opt => opt.MapFrom(src => src.OriginatingTotalInvoice))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByName))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.VoidBy, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment));

        }
    }
}
