using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Company
{
   public class ChargesProfiles : Profile
    {
        public ChargesProfiles()
        {
        CreateMap<FLOG_BE.Model.Companies.Entities.Charges, Features.Companies.Charges.GetCharges.ResponseItem>()
              .ForMember(dest => dest.ChargesId, opt => opt.MapFrom(src => src.ChargesId))
              .ForMember(dest => dest.ChargesCode, opt => opt.MapFrom(src => src.ChargesCode))
              .ForMember(dest => dest.ChargesName, opt => opt.MapFrom(src => src.ChargesName))
              .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
              .ForMember(dest => dest.ChargeGroupCode, opt => opt.MapFrom(src => src.ChargeGroupCode))
              .ForMember(dest => dest.IsPurchasing, opt => opt.MapFrom(src => src.IsPurchasing))
              .ForMember(dest => dest.IsSales, opt => opt.MapFrom(src => src.IsSales))
              .ForMember(dest => dest.IsInventory, opt => opt.MapFrom(src => src.IsInventory))
              .ForMember(dest => dest.IsFinancial, opt => opt.MapFrom(src => src.IsFinancial))
              .ForMember(dest => dest.IsAsset, opt => opt.MapFrom(src => src.IsAsset))
              .ForMember(dest => dest.IsDeposit, opt => opt.MapFrom(src => src.IsDeposit))
              .ForMember(dest => dest.RevenueAccountNo, opt => opt.MapFrom(src => src.RevenueAccountNo))
              .ForMember(dest => dest.TempRevenueAccountNo, opt => opt.MapFrom(src => src.TempRevenueAccountNo))
              .ForMember(dest => dest.CostAccountNo, opt => opt.MapFrom(src => src.CostAccountNo))
              .ForMember(dest => dest.TaxScheduleCode, opt => opt.MapFrom(src => src.TaxScheduleCode))
              .ForMember(dest => dest.ShippingLineType, opt => opt.MapFrom(src => src.ShippingLineType))
              .ForMember(dest => dest.HasCosting, opt => opt.MapFrom(src => src.HasCosting))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive == true ? "Inactive":"Active"));

            CreateMap<FLOG_BE.Model.Companies.Entities.Charges, Features.Companies.Charges.GetChargesDeposit.ResponseItem>()
              .ForMember(dest => dest.ChargesId, opt => opt.MapFrom(src => src.ChargesId))
              .ForMember(dest => dest.ChargesCode, opt => opt.MapFrom(src => src.ChargesCode))
              .ForMember(dest => dest.ChargesName, opt => opt.MapFrom(src => src.ChargesName))
              .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
              .ForMember(dest => dest.ChargeGroupCode, opt => opt.MapFrom(src => src.ChargeGroupCode))
              .ForMember(dest => dest.IsPurchasing, opt => opt.MapFrom(src => src.IsPurchasing))
              .ForMember(dest => dest.IsSales, opt => opt.MapFrom(src => src.IsSales))
              .ForMember(dest => dest.IsInventory, opt => opt.MapFrom(src => src.IsInventory))
              .ForMember(dest => dest.IsFinancial, opt => opt.MapFrom(src => src.IsFinancial))
              .ForMember(dest => dest.IsAsset, opt => opt.MapFrom(src => src.IsAsset))
              .ForMember(dest => dest.IsDeposit, opt => opt.MapFrom(src => src.IsDeposit))
              .ForMember(dest => dest.RevenueAccountNo, opt => opt.MapFrom(src => src.RevenueAccountNo))
              .ForMember(dest => dest.TempRevenueAccountNo, opt => opt.MapFrom(src => src.TempRevenueAccountNo))
              .ForMember(dest => dest.CostAccountNo, opt => opt.MapFrom(src => src.CostAccountNo))
              .ForMember(dest => dest.TaxScheduleCode, opt => opt.MapFrom(src => src.TaxScheduleCode))
              .ForMember(dest => dest.ShippingLineType, opt => opt.MapFrom(src => src.ShippingLineType))
              .ForMember(dest => dest.HasCosting, opt => opt.MapFrom(src => src.HasCosting))
              .ForMember(dest => dest.InActive, opt => opt.MapFrom(src => src.InActive == true ? "Inactive" : "Active"));
        }
    }
}
