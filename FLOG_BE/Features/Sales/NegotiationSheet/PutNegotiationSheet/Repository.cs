using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Companies;
using Entities = FLOG_BE.Model.Companies.Entities;
using Infrastructure;
using System.Collections.Generic;
using FLOG.Core;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Sales.NegotiationSheet.PutNegotiationSheet
{
    public class Repository
    {
        private readonly CompanyContext _context;
        private readonly IDocumentGenerator _docGenerator;

        public Repository(CompanyContext context)
        {
            _context = context;
            _docGenerator = new DocumentGenerator(_context);
        }
        
        public async Task<Entities.NegotiationSheetHeader> CreateOrUpdateHeader(RequestNegotiationSheetBody requestBody, UserLogin user)
        {
            Entities.NegotiationSheetHeader nsHeader = null;
            
            if (requestBody.NegotiationSheetId == null || requestBody.NegotiationSheetId == Guid.Empty)
            {
                //CREATE
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(requestBody.TransactionDate, TRX_MODULE.TRX_SALES, DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET, requestBody.TransactionType, _context.Database.CurrentTransaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    nsHeader = new Entities.NegotiationSheetHeader();

                    nsHeader.NegotiationSheetId = Guid.NewGuid();
                    nsHeader.BranchCode = requestBody.BranchCode;
                    nsHeader.DocumentNo = documentUniqueNo;
                    nsHeader.TransactionType = requestBody.TransactionType;
                    nsHeader.TransactionDate = requestBody.TransactionDate;
                    nsHeader.SalesOrderId = requestBody.SalesOrderId;
                    nsHeader.CustomerId = requestBody.CustomerId;
                    nsHeader.CustomerAddressCode = requestBody.CustomerAddressCode;
                    nsHeader.CustomerBillToAddressCode = requestBody.CustomerBillToAddressCode;
                    nsHeader.CustomerShipToAddressCode = requestBody.CustomerShipToAddressCode;
                    nsHeader.SalesCode = requestBody.SalesCode;
                    nsHeader.QuotDocumentNo = requestBody.QuotDocumentNo;
                    nsHeader.ShipperId = requestBody.ShipperId;
                    nsHeader.ShipperAddressCode = requestBody.ShipperAddressCode;
                    nsHeader.ShipperBillToAddressCode = requestBody.ShipperBillToAddressCode;
                    nsHeader.ShipperShipToAddressCode = requestBody.ShipperShipToAddressCode;
                    nsHeader.ShipmentStatus = requestBody.ShipmentStatus;
                    nsHeader.MasterNo = requestBody.MasterNo;
                    nsHeader.AgreementNo = requestBody.AgreementNo;
                    nsHeader.BookingNo = requestBody.BookingNo;
                    nsHeader.HouseNo = requestBody.HouseNo;
                    nsHeader.ConsigneeId = requestBody.ConsigneeId;
                    nsHeader.ConsigneeAddressCode = requestBody.ConsigneeAddressCode;
                    nsHeader.ConsigneeBillToAddressCode = requestBody.ConsigneeBillToAddressCode;
                    nsHeader.ConsigneeShipToAddressCode = requestBody.ConsigneeShipToAddressCode;
                    nsHeader.IsDifferentNotifyPartner = requestBody.IsDifferentNotifyPartner;

                    if (nsHeader.IsDifferentNotifyPartner)
                    {
                        //IF DIFFERENT
                        nsHeader.NotifyPartnerId = requestBody.NotifyPartnerId;
                        nsHeader.NotifyAddressCode = requestBody.NotifyAddressCode;
                        nsHeader.NotifyBillToAddressCode = requestBody.NotifyBillToAddressCode;
                        nsHeader.NotifyShipToAddressCode = requestBody.NotifyShipToAddressCode;
                    }
                    else
                    {
                        //IS SAME ADDRESS
                        nsHeader.NotifyPartnerId = nsHeader.ConsigneeId;
                        nsHeader.NotifyAddressCode = nsHeader.ConsigneeAddressCode;
                        nsHeader.NotifyBillToAddressCode = nsHeader.ConsigneeBillToAddressCode;
                        nsHeader.NotifyShipToAddressCode = nsHeader.ConsigneeShipToAddressCode;
                    }
                    
                    nsHeader.ShippingLineId = requestBody.ShippingLineId;
                    nsHeader.IsShippingLineMaster = requestBody.IsShippingLineMaster;
                    nsHeader.ShippingLineCode = requestBody.ShippingLineCode;
                    nsHeader.ShippingLineName = requestBody.ShippingLineName;
                    nsHeader.ShippingLineVesselCode = requestBody.ShippingLineVesselCode;
                    nsHeader.ShippingLineVesselName = requestBody.ShippingLineVesselName;
                    nsHeader.ShippingLineShippingNo = requestBody.ShippingLineShippingNo;
                    nsHeader.ShippingLineETD = requestBody.ShippingLineETD;
                    nsHeader.ShippingLineETA = requestBody.ShippingLineETA;
                    nsHeader.TermOfShipment = requestBody.TermOfShipment;
                    nsHeader.FinalDestination = requestBody.FinalDestination;
                    nsHeader.PortOfLoading = requestBody.PortOfLoading;
                    nsHeader.PortOfDischarge = requestBody.PortOfDischarge;
                    nsHeader.Commodity = requestBody.Commodity;
                    nsHeader.CargoGrossWeight = requestBody.CargoGrossWeight;
                    nsHeader.CargoNetWeight = requestBody.CargoNetWeight;
                    nsHeader.CargoDescription = requestBody.CargoDescription;
                    nsHeader.TotalFuncSelling = requestBody.TotalFuncSelling;
                    nsHeader.TotalFuncBuying = requestBody.TotalFuncBuying;
                    nsHeader.Remark = requestBody.Remark;
                    nsHeader.Status = DOCSTATUS.NEW;
                    //nsHeader.StatusComment = requestBody.StatusComment;
                    nsHeader.CreatedDate = DateTime.Now;
                    nsHeader.CreatedBy = user.UserId;
                }                
            }
            else
            {
                //UPDATE
                nsHeader = await _context.NegotiationSheetHeaders.FirstOrDefaultAsync(x => x.NegotiationSheetId == requestBody.NegotiationSheetId);

                if (nsHeader != null)
                {
                    nsHeader.TransactionType = requestBody.TransactionType;
                    nsHeader.TransactionDate = requestBody.TransactionDate;
                    nsHeader.BranchCode = requestBody.BranchCode;
                    nsHeader.SalesOrderId = requestBody.SalesOrderId;
                    nsHeader.CustomerId = requestBody.CustomerId;
                    nsHeader.CustomerAddressCode = requestBody.CustomerAddressCode;
                    nsHeader.CustomerBillToAddressCode = requestBody.CustomerBillToAddressCode;
                    nsHeader.CustomerShipToAddressCode = requestBody.CustomerShipToAddressCode;
                    nsHeader.SalesCode = requestBody.SalesCode;
                    nsHeader.QuotDocumentNo = requestBody.QuotDocumentNo;
                    nsHeader.ShipperId = requestBody.ShipperId;
                    nsHeader.ShipperAddressCode = requestBody.ShipperAddressCode;
                    nsHeader.ShipperBillToAddressCode = requestBody.ShipperBillToAddressCode;
                    nsHeader.ShipperShipToAddressCode = requestBody.ShipperShipToAddressCode;
                    nsHeader.ShipmentStatus = requestBody.ShipmentStatus;
                    nsHeader.MasterNo = requestBody.MasterNo;
                    nsHeader.AgreementNo = requestBody.AgreementNo;
                    nsHeader.BookingNo = requestBody.BookingNo;
                    nsHeader.HouseNo = requestBody.HouseNo;
                    nsHeader.ConsigneeId = requestBody.ConsigneeId;
                    nsHeader.ConsigneeAddressCode = requestBody.ConsigneeAddressCode;
                    nsHeader.ConsigneeBillToAddressCode = requestBody.ConsigneeBillToAddressCode;
                    nsHeader.ConsigneeShipToAddressCode = requestBody.ConsigneeShipToAddressCode;
                    nsHeader.IsDifferentNotifyPartner = requestBody.IsDifferentNotifyPartner;

                    if (nsHeader.IsDifferentNotifyPartner)
                    {
                        //IF DIFFERENT
                        nsHeader.NotifyPartnerId = requestBody.NotifyPartnerId;
                        nsHeader.NotifyAddressCode = requestBody.NotifyAddressCode;
                        nsHeader.NotifyBillToAddressCode = requestBody.NotifyBillToAddressCode;
                        nsHeader.NotifyShipToAddressCode = requestBody.NotifyShipToAddressCode;
                    }
                    else
                    {
                        //IS SAME ADDRESS
                        nsHeader.NotifyPartnerId = nsHeader.ConsigneeId;
                        nsHeader.NotifyAddressCode = nsHeader.ConsigneeAddressCode;
                        nsHeader.NotifyBillToAddressCode = nsHeader.ConsigneeBillToAddressCode;
                        nsHeader.NotifyShipToAddressCode = nsHeader.ConsigneeShipToAddressCode;
                    }

                    nsHeader.ShippingLineId = requestBody.ShippingLineId;
                    nsHeader.IsShippingLineMaster = requestBody.IsShippingLineMaster;
                    nsHeader.ShippingLineCode = requestBody.ShippingLineCode;
                    nsHeader.ShippingLineName = requestBody.ShippingLineName;
                    nsHeader.ShippingLineVesselCode = requestBody.ShippingLineVesselCode;
                    nsHeader.ShippingLineVesselName = requestBody.ShippingLineVesselName;
                    nsHeader.ShippingLineShippingNo = requestBody.ShippingLineShippingNo;
                    nsHeader.ShippingLineETD = requestBody.ShippingLineETD;
                    nsHeader.ShippingLineETA = requestBody.ShippingLineETA;
                    nsHeader.TermOfShipment = requestBody.TermOfShipment;
                    nsHeader.FinalDestination = requestBody.FinalDestination;
                    nsHeader.PortOfLoading = requestBody.PortOfLoading;
                    nsHeader.PortOfDischarge = requestBody.PortOfDischarge;
                    nsHeader.Commodity = requestBody.Commodity;
                    nsHeader.CargoGrossWeight = requestBody.CargoGrossWeight;
                    nsHeader.CargoNetWeight = requestBody.CargoNetWeight;
                    nsHeader.CargoDescription = requestBody.CargoDescription;
                    nsHeader.TotalFuncSelling = requestBody.TotalFuncSelling;
                    nsHeader.TotalFuncBuying = requestBody.TotalFuncBuying;
                    nsHeader.Remark = requestBody.Remark;
                    nsHeader.ModifiedDate = DateTime.Now;
                    nsHeader.ModifiedBy = user.UserId;
                }
            }

            return nsHeader;

        }

        public async Task<List<Entities.NegotiationSheetContainer>> InsertContainers(RequestNegotiationSheetBody body)
        {
            List<Entities.NegotiationSheetContainer> result = new List<Entities.NegotiationSheetContainer>();

            if (body.NsContainers != null)
            {
                // REMOVE EXISTING
                _context.NegotiationSheetContainers
               .Where(x => x.NegotiationSheetId == body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));
                await _context.SaveChangesAsync();

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.NsContainers.OrderBy(x => x.RowIndex))
                {
                    var Detail = new Entities.NegotiationSheetContainer()
                    {
                        NSContainerId = Guid.NewGuid(),
                        NegotiationSheetId = body.NegotiationSheetId,
                        ContainerId = item.ContainerId,
                        Qty = item.Qty,
                        UomDetailId = item.UomDetailId,
                        Remark = item.Remark,
                        Status = DOCSTATUS.NEW,
                        RowIndex = item.RowIndex,
                    };

                    result.Add(Detail);
                    await _context.NegotiationSheetContainers.AddAsync(Detail);
                }
            }

            return result;
        }

        public async Task<List<Entities.NegotiationSheetTrucking>> InsertTruckingDetails(RequestNegotiationSheetBody body, Entities.NegotiationSheetHeader header)
        {
            List<Entities.NegotiationSheetTrucking> result = new List<Entities.NegotiationSheetTrucking>();
            if (body.NsTruckings != null)
            {
                _context.NegotiationSheetTruckings
               .Where(x => x.NegotiationSheetId == body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));
                await _context.SaveChangesAsync();

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.NsTruckings.OrderBy(x => x.RowIndex))
                {
                    var Detail = new Entities.NegotiationSheetTrucking()
                    {
                        NsTruckingId = Guid.NewGuid(),
                        NegotiationSheetId = header.NegotiationSheetId,
                        VehicleTypeId = item.VehicleTypeId,
                        TruckloadTerm = item.TruckloadTerm,
                        VendorId = item.VendorId,
                        Qty = item.Qty,
                        UomDetailId = item.UomDetailId,
                        Remark = item.Remark,
                        Status = DOCSTATUS.NEW,
                        RowIndex = item.RowIndex,
                    };

                    await _context.NegotiationSheetTruckings.AddAsync(Detail);

                    result.Add(Detail);

                }
            }

            return result;
        }

        public async Task<List<Entities.NegotiationSheetSelling>> InsertSellingDetails(RequestNegotiationSheetBody body, Entities.NegotiationSheetHeader header)
        {
            List<Entities.NegotiationSheetSelling> sellingResult = new List<Entities.NegotiationSheetSelling>();
            List<Entities.NegotiationSheetBuying> buyingResult = new List<Entities.NegotiationSheetBuying>();

            if (body.NsSellings != null)
            {
                _context.NegotiationSheetSellings
              .Where(x => x.NegotiationSheetId == body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));

                _context.NegotiationSheetBuyings
              .Where(x => x.NegotiationSheetId == body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));
                await _context.SaveChangesAsync();

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.NsSellings.OrderBy(x => x.RowIndex))
                {
                    var sellDetail = new Entities.NegotiationSheetSelling()
                    {
                        NsSellingId = Guid.NewGuid(),
                        NegotiationSheetId = header.NegotiationSheetId,
                        ChargeId = item.ChargeId,
                        CurrencyCode = item.CurrencyCode,
                        ExchangeRate = item.ExchangeRate,
                        IsMultiply = item.IsMultiply,
                        OriginatingAmount = item.OriginatingAmount,
                        OriginatingTax = item.OriginatingTax,
                        OriginatingDiscount = item.OriginatingDiscount,
                        OriginatingExtendedAmount = item.OriginatingExtendedAmount,
                        FunctionalTax = item.FunctionalTax,
                        FunctionalDiscount = item.FunctionalDiscount,
                        FunctionalExtendedAmount = item.FunctionalExtendedAmount,
                        TaxScheduleId = item.TaxScheduleId,
                        IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                        PercentDiscount = item.PercentDiscount,
                        PaymentCondition = item.PaymentCondition,
                        CustomerId = item.CustomerId,
                        Remark = item.Remark,
                        Status = DOCSTATUS.NEW,
                        RowIndex = item.RowIndex,
                    };

                    await _context.NegotiationSheetSellings.AddAsync(sellDetail);

                    buyingResult = new List<Entities.NegotiationSheetBuying>();
                    foreach (var buy in item.NsBuyings.OrderBy(x => x.RowIndex))
                    {
                        var buyDetail = new Entities.NegotiationSheetBuying()
                        {
                            NsBuyingId = Guid.NewGuid(),
                            NegotiationSheetId = header.NegotiationSheetId,
                            NsSellingId = sellDetail.NsSellingId,
                            ChargeId = buy.ChargeId,
                            CurrencyCode = buy.CurrencyCode,
                            ExchangeRate = buy.ExchangeRate,
                            IsMultiply = buy.IsMultiply,
                            OriginatingAmount = buy.OriginatingAmount,
                            OriginatingTax = buy.OriginatingTax,
                            OriginatingDiscount = buy.OriginatingDiscount,
                            OriginatingExtendedAmount = buy.OriginatingExtendedAmount,
                            FunctionalTax = buy.FunctionalTax,
                            FunctionalDiscount = buy.FunctionalDiscount,
                            FunctionalExtendedAmount = buy.FunctionalExtendedAmount,
                            TaxScheduleId = buy.TaxScheduleId,
                            IsTaxAfterDiscount = buy.IsTaxAfterDiscount,
                            PercentDiscount = buy.PercentDiscount,
                            PaymentCondition = buy.PaymentCondition,
                            VendorId = buy.VendorId,
                            Remark = buy.Remark,
                            Status = DOCSTATUS.NEW,
                            RowIndex = buy.RowIndex,
                        };

                        await _context.NegotiationSheetBuyings.AddAsync(buyDetail);

                        buyingResult.Add(buyDetail);
                    }

                    sellDetail.NsBuyings = buyingResult;

                    sellingResult.Add(sellDetail);
                }
            }

            return sellingResult;
        }

    }
}
