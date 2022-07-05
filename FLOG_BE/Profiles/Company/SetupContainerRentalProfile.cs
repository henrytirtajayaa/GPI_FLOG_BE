using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FLOG_BE.Profiles.Company
{
    public class SetupContainerRentalProfile : Profile
    {
        public SetupContainerRentalProfile()
        {
            CreateMap<FLOG_BE.Model.Companies.Entities.SetupContainerRental, Features.Companies.SetupContainerRental.GetSetupContainerRental.ResponseItem>()
              .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
              .ForMember(dest => dest.RequestDocNo, opt => opt.MapFrom(src => src.RequestDocNo))
              .ForMember(dest => dest.DeliveryDocNo, opt => opt.MapFrom(src => src.DeliveryDocNo))
              .ForMember(dest => dest.ClosingDocNo, opt => opt.MapFrom(src => src.ClosingDocNo))
              .ForMember(dest => dest.UomScheduleCode, opt => opt.MapFrom(src => src.UomScheduleCode))
              .ForMember(dest => dest.CustomerFreeUsageDays, opt => opt.MapFrom(src => src.CustomerFreeUsageDays))
              .ForMember(dest => dest.ShippingLineFreeUsageDays, opt => opt.MapFrom(src => src.ShippingLineFreeUsageDays))
              .ForMember(dest => dest.CntOwnerFreeUsageDays, opt => opt.MapFrom(src => src.CntOwnerFreeUsageDays))
              .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
              .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate));
        }
    }
}
