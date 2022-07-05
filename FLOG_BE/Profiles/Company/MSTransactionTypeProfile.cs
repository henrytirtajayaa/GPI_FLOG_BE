using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class MSTransactionTypeProfile : Profile
    {
        public MSTransactionTypeProfile()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.MSTransactionType, Features.Companies.MSTransactionType.GetTransactionType.ResponseItem>()
                .ForMember(dest => dest.TransactionTypeId, opt => opt.MapFrom(src => src.TransactionTypeId))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.TransactionName, opt => opt.MapFrom(src => src.TransactionName))
                .ForMember(dest => dest.PaymentCondition, opt => opt.MapFrom(src => src.PaymentCondition))
                .ForMember(dest => dest.RequiredSubject, opt => opt.MapFrom(src => src.RequiredSubject))
                .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive));
        }
    }
}
