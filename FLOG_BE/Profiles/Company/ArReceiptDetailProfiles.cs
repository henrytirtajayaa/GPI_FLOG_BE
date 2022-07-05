using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class ArReceiptDetailProfiles : Profile
    {
        public ArReceiptDetailProfiles()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.ArReceiptDetail, Features.Finance.ArReceipt.GetDetailCustomerReceipt.ResponseItem>()
                .ForMember(dest => dest.ReceiptDetailId, opt => opt.MapFrom(src => src.ReceiptDetailId))
                .ForMember(dest => dest.ReceiptHeaderId, opt => opt.MapFrom(src => src.ReceiptHeaderId))
                .ForMember(dest => dest.ReceiveTransactionId, opt => opt.MapFrom(src => src.ReceiveTransactionId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.OriginatingBalance, opt => opt.MapFrom(src => src.OriginatingBalance))
                .ForMember(dest => dest.FunctionalBalance, opt => opt.MapFrom(src => src.FunctionalBalance))
                .ForMember(dest => dest.OriginatingPaid, opt => opt.MapFrom(src => src.OriginatingPaid))
                .ForMember(dest => dest.FunctionalPaid, opt => opt.MapFrom(src => src.FunctionalPaid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
