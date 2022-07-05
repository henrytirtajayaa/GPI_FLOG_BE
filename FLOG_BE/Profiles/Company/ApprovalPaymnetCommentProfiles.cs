using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class ApprovalPaymnetCommentProfiles : Profile
    {
        public ApprovalPaymnetCommentProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ApPaymentApprovalComment, Features.Finance.ApPayment.GetApprovalComment.ResponseItem>()

                .ForMember(dest => dest.ApprovalCommentId, opt => opt.MapFrom(src => src.PaymentApprovalCommentId))
                .ForMember(dest => dest.PaymentApprovalId, opt => opt.MapFrom(src => src.PaymentApprovalId))
                .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.CommentDate))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        }
    }
}
