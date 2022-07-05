using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class CheckbookTransactionProfiles : Profile
    {
        public CheckbookTransactionProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.CheckbookTransactionHeader, Features.Finance.Checkbook.GetProgress.ReponseItem>()
                .ForMember(dest => dest.CheckbookTransactionId, opt => opt.MapFrom(src => src.CheckbookTransactionId))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.CheckbookCode, opt => opt.MapFrom(src => src.CheckbookCode))
                .ForMember(dest => dest.IsVoid, opt => opt.MapFrom(src => src.IsVoid))
                .ForMember(dest => dest.VoidDocumentNo, opt => opt.MapFrom(src => src.VoidDocumentNo))
                .ForMember(dest => dest.PaidSubject, opt => opt.MapFrom(src => src.PaidSubject))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.SubjectCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotalAmount, opt => opt.MapFrom(src => src.OriginatingTotalAmount))
                .ForMember(dest => dest.FunctionalTotalAmount, opt => opt.MapFrom(src => src.FunctionalTotalAmount))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.VoidBy, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy));


            CreateMap<FLOG_BE.Model.Companies.Entities.CheckbookTransactionHeader, Features.Finance.Checkbook.GetHistory.ReponseItem>()
                .ForMember(dest => dest.CheckbookTransactionId, opt => opt.MapFrom(src => src.CheckbookTransactionId))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.DocumentNo, opt => opt.MapFrom(src => src.DocumentNo))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.CheckbookCode, opt => opt.MapFrom(src => src.CheckbookCode))
                .ForMember(dest => dest.IsVoid, opt => opt.MapFrom(src => src.IsVoid))
                .ForMember(dest => dest.VoidDocumentNo, opt => opt.MapFrom(src => src.VoidDocumentNo))
                .ForMember(dest => dest.PaidSubject, opt => opt.MapFrom(src => src.PaidSubject))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.SubjectCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingTotalAmount, opt => opt.MapFrom(src => src.OriginatingTotalAmount))
                .ForMember(dest => dest.FunctionalTotalAmount, opt => opt.MapFrom(src => src.FunctionalTotalAmount))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.VoidBy, opt => opt.MapFrom(src => src.VoidBy))
                .ForMember(dest => dest.VoidDate, opt => opt.MapFrom(src => src.VoidDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusComment, opt => opt.MapFrom(src => src.StatusComment))
                .ForMember(dest => dest.IsMultiply, opt => opt.MapFrom(src => src.IsMultiply))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy));


            CreateMap<FLOG_BE.Model.Companies.Entities.CheckbookTransactionDetail, Features.Finance.Checkbook.GetDetail.ResponseItem>()
                .ForMember(dest => dest.CheckbookTransactionId, opt => opt.MapFrom(src => src.CheckbookTransactionId))
                .ForMember(dest => dest.ChargesDescription, opt => opt.MapFrom(src => src.ChargesDescription))
                .ForMember(dest => dest.ChargesId, opt => opt.MapFrom(src => src.ChargesId))
                .ForMember(dest => dest.FunctionalAmount, opt => opt.MapFrom(src => src.FunctionalAmount))
                .ForMember(dest => dest.OriginatingAmount, opt => opt.MapFrom(src => src.OriginatingAmount))
                .ForMember(dest => dest.TransactionDetailId, opt => opt.MapFrom(src => src.TransactionDetailId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<FLOG_BE.Model.Companies.Entities.CheckbookApprovalComment, Features.Finance.Checkbook.GetApprovalComment.ResponseItem>()
                .ForMember(dest => dest.CheckbookTransactionApprovalId, opt => opt.MapFrom(src => src.CheckbookTransactionApprovalId))
                .ForMember(dest => dest.ApprovalCommentId, opt => opt.MapFrom(src => src.ApprovalCommentId))
                .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.CommentDate))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        }

    }
}
