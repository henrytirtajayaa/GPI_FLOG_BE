using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class PayableTransactionProfiles : Profile
    {
        public PayableTransactionProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.PayableTransactionHeader, Features.Finance.Payable.GetProgress.ResponseItem>()
                .ForMember(dest => dest.PayableTransactionId, opt => opt.MapFrom(src => src.PayableTransactionId))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.PaymentTermCode, opt => opt.MapFrom(src => src.PaymentTermCode))
                .ForMember(dest => dest.VendorAddressCode, opt => opt.MapFrom(src => src.VendorAddressCode))
                .ForMember(dest => dest.VendorDocumentNo, opt => opt.MapFrom(src => src.VendorDocumentNo))
                .ForMember(dest => dest.NsDocumentNo, opt => opt.MapFrom(src => src.NsDocumentNo))
                .ForMember(dest => dest.MasterNo, opt => opt.MapFrom(src => src.MasterNo))
                .ForMember(dest => dest.AgreementNo, opt => opt.MapFrom(src => src.AgreementNo))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SubtotalAmount, opt => opt.MapFrom(src => src.SubtotalAmount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
                .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
                .ForMember(dest => dest.BillToAddressCode, opt => opt.MapFrom(src => src.BillToAddressCode))
                .ForMember(dest => dest.ShipToAddressCode, opt => opt.MapFrom(src => src.ShipToAddressCode))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByName))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.VoidBy, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment));

            CreateMap<FLOG_BE.Model.Companies.Entities.PayableTransactionHeader, Features.Finance.Payable.GetHistory.ResponseItem>()
                .ForMember(dest => dest.PayableTransactionId, opt => opt.MapFrom(src => src.PayableTransactionId))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.PaymentTermCode, opt => opt.MapFrom(src => src.PaymentTermCode))
                .ForMember(dest => dest.VendorAddressCode, opt => opt.MapFrom(src => src.VendorAddressCode))
                .ForMember(dest => dest.VendorDocumentNo, opt => opt.MapFrom(src => src.VendorDocumentNo))
                .ForMember(dest => dest.NsDocumentNo, opt => opt.MapFrom(src => src.NsDocumentNo))
                .ForMember(dest => dest.MasterNo, opt => opt.MapFrom(src => src.MasterNo))
                .ForMember(dest => dest.AgreementNo, opt => opt.MapFrom(src => src.AgreementNo))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SubtotalAmount, opt => opt.MapFrom(src => src.SubtotalAmount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
                .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
                .ForMember(dest => dest.BillToAddressCode, opt => opt.MapFrom(src => src.BillToAddressCode))
                .ForMember(dest => dest.ShipToAddressCode, opt => opt.MapFrom(src => src.ShipToAddressCode))
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
