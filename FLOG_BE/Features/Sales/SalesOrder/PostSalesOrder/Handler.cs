using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.PostSalesOrder
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_SALES, DOCNO_FEATURE.TRXTYPE_SALES_ORDER, request.Body.TransactionType, transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var header = new Entities.SalesOrderHeader()
                    {
                        SalesOrderId = Guid.NewGuid(),
                        TransactionType = request.Body.TransactionType,
                        TransactionDate = request.Body.TransactionDate,
                        DocumentNo = documentUniqueNo,
                        BranchCode = request.Body.BranchCode,
                        CustomerId = request.Body.CustomerId,
                        CustomerAddressCode = request.Body.CustomerAddressCode,
                        CustomerBillToAddressCode = request.Body.CustomerBillToAddressCode,
                        CustomerShipToAddressCode = request.Body.CustomerShipToAddressCode,

                        SalesCode = request.Body.SalesCode,
                        QuotDocumentNo = request.Body.QuotDocumentNo,
                        ShipperId = request.Body.ShipperId,
                        ShipperAddressCode = request.Body.ShipperAddressCode,
                        ShipmentStatus = request.Body.ShipmentStatus,
                        MasterNo = request.Body.MasterNo,
                        AgreementNo = request.Body.AgreementNo,
                        BookingNo = request.Body.BookingNo,
                        HouseNo = request.Body.HouseNo,
                        ShipperBillToAddressCode = request.Body.ShipperBillToAddressCode,
                        ShipperShipToAddressCode = request.Body.ShipperShipToAddressCode,

                        ConsigneeId = request.Body.ConsigneeId,
                        ConsigneeAddressCode = request.Body.ConsigneeAddressCode,
                        ConsigneeBillToAddressCode = request.Body.ConsigneeBillToAddressCode,
                        ConsigneeShipToAddressCode = request.Body.ConsigneeShipToAddressCode,

                        IsDifferentNotifyPartner = request.Body.IsDifferentNotifyPartner,

                        NotifyPartnerId = request.Body.NotifyPartnerId,
                        NotifyPartnerAddressCode = request.Body.NotifyPartnerAddressCode,
                        NotifyPartnerBilltoAddressCode = request.Body.NotifyPartnerBilltoAddressCode,
                        NotifyPartnerShipToAddressCode = request.Body.NotifyPartnerShipToAddressCode,

                        ShippingLineId = request.Body.ShippingLineId,
                        IsShippingLineMaster = request.Body.IsShippingLineMaster,
                        ShippingLineCode = request.Body.ShippingLineCode,
                        ShippingLineName = request.Body.ShippingLineName,
                        ShippingLineVesselCode = request.Body.ShippingLineVesselCode,
                        ShippingLineVesselName = request.Body.ShippingLineVesselName,
                        ShippingLineShippingNo = request.Body.ShippingLineShippingNo,
                        ShippingLineDelivery = request.Body.ShippingLineDelivery,
                        ShippingLineArrival = request.Body.ShippingLineArrival,
                        FeederLineId = request.Body.FeederLineId,
                        IsFeederLineMaster = request.Body.IsFeederLineMaster,
                        FeederLineCode = request.Body.FeederLineCode,
                        FeederLineName = request.Body.FeederLineName,
                        FeederLineVesselCode = request.Body.FeederLineVesselCode,
                        FeederLineVesselName = request.Body.FeederLineVesselName,
                        FeederLineShippingNo = request.Body.FeederLineShippingNo,
                        FeederLineDelivery = request.Body.FeederLineDelivery,
                        FeederLineArrival = request.Body.FeederLineArrival,
                        TermOfShipment = request.Body.TermOfShipment,
                        FinalDestination = request.Body.FinalDestination,
                        PortOfLoading = request.Body.PortOfLoading,
                        PortOfDischarge = request.Body.PortOfDischarge,
                        Commodity = request.Body.Commodity,
                        CargoGrossWeight = request.Body.CargoGrossWeight,
                        CargoNetWeight = request.Body.CargoNetWeight,
                        CargoDescription = request.Body.CargoDescription,
                        TotalFuncSelling = request.Body.TotalFuncSelling,
                        TotalFuncBuying = request.Body.TotalFuncBuying,
                        Remark = request.Body.Remark,
                        Status = DOCSTATUS.NEW,
                        StatusComment = request.Body.StatusComment,
                        CreatedDate = DateTime.Now,
                        CreatedBy = request.Initiator.UserId,
                    };

                    if (header.SalesOrderId != null && header.SalesOrderId != Guid.Empty)
                    {

                        var container = await InserContainertDetails(request.Body, header);
                        var trucking = await InsertTruckingDetails(request.Body, header);
                        var selling = await InsertSellingDetails(request.Body, header);

                        //SET TOTAL
                        header.TotalFuncSelling = selling.Sum(s => s.FunctionalExtendedAmount);
                        header.TotalFuncBuying = selling.Sum(s => s.SoBuyings.Sum(buy => buy.FunctionalExtendedAmount));

                        _context.SalesOrderHeaders.Add(header);

                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            SalesOrderId = header.SalesOrderId,
                            DocumentNo = header.DocumentNo
                        });

                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Data can not be stored.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Document No can not be created. Please check Document No Setup!");
                }
            }
        }

        private async Task<List<Entities.SalesOrderContainer>> InserContainertDetails(RequestSalesOrderBody body, Entities.SalesOrderHeader header)
        {
            List<Entities.SalesOrderContainer> result = new List<Entities.SalesOrderContainer>();

            if (body.SalesOrderContainers != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.SalesOrderContainers.OrderBy(x=>x.RowId))
                {
                    var Detail = new Entities.SalesOrderContainer()
                    {
                        SalesOrderContainerId = Guid.NewGuid(),
                        SalesOrderId = header.SalesOrderId,
                        ContainerId = item.ContainerId,
                        Qty = item.Qty,
                        UomDetailId = item.UomDetailId,
                        Status = DOCSTATUS.NEW,
                    };

                    result.Add(Detail);
                }

                if (result.Count > 0)
                {
                    await _context.SalesOrderContainers.AddRangeAsync(result);
                }
            }

            return result;
        }
        private async Task<List<Entities.SalesOrderTrucking>> InsertTruckingDetails(RequestSalesOrderBody body, Entities.SalesOrderHeader header)
        {
            List<Entities.SalesOrderTrucking> result = new List<Entities.SalesOrderTrucking>();
            if (body.SalesOrderContainers != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.SalesOrderTruckings.OrderBy(x => x.RowId))
                {
                    var Detail = new Entities.SalesOrderTrucking()
                    {
                        SalesOrderTruckingId = Guid.NewGuid(),
                        SalesOrderId = header.SalesOrderId,
                        VehicleTypeId = item.VehicleTypeId,
                        TruckloadTerm = item.TruckloadTerm,
                        VendorId = item.VendorId,
                        Qty = item.Qty,
                        UomDetailId = item.UomDetailId,
                        Remark = item.Remark,
                        Status = DOCSTATUS.NEW,
                    };
                    result.Add(Detail);
                }
                if (result.Count > 0)
                {
                    await _context.SalesOrderTruckings.AddRangeAsync(result);
                }
            }

            return result;
        }
     
        private async Task<List<Entities.SalesOrderSelling>> InsertSellingDetails(RequestSalesOrderBody body, Entities.SalesOrderHeader header)
        {

            List<Entities.SalesOrderSelling> result = new List<Entities.SalesOrderSelling>();
            List<Entities.SalesOrderBuying> result2 = new List<Entities.SalesOrderBuying>();
            if (body.SalesOrderSellings != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.SalesOrderSellings.OrderBy(x => x.RowId))
                {
                    var Detail = new Entities.SalesOrderSelling()
                    {
                        SalesOrderSellingId= Guid.NewGuid(),
                        SalesOrderId = header.SalesOrderId,
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
                        UnitAmount = item.UnitAmount,
                        Quantity = item.Quantity
                    };

                    result2 = new List<Entities.SalesOrderBuying>();
                    foreach (var buy in item.SoBuyings.OrderBy(x => x.RowId))
                    {
                        var buydtl = new Entities.SalesOrderBuying()
                        {
                            SalesOrderBuyingId = Guid.NewGuid(),
                            SalesOrderId = header.SalesOrderId,
                            SalesOrderSellingId = Detail.SalesOrderSellingId,
                            ChargeId = buy.ChargeId,
                            CurrencyCode = buy.CurrencyCode,
                            ExchangeRate = buy.ExchangeRate,
                            IsMultiply = buy.IsMultiply,
                            OriginatingAmount = buy.OriginatingAmount,
                            OriginatingTax = buy.OriginatingTax,
                            OriginatingDiscount = buy.OriginatingDiscount,
                            OriginatingExtendedAmount = buy.OriginatingExtendedAmount,
                            FunctionalTax = buy.OriginatingTax * buy.ExchangeRate,
                            FunctionalDiscount = buy.OriginatingDiscount * buy.ExchangeRate,
                            FunctionalExtendedAmount = buy.OriginatingExtendedAmount * buy.ExchangeRate,
                            TaxScheduleId = buy.TaxScheduleId,
                            IsTaxAfterDiscount = buy.IsTaxAfterDiscount,
                            PercentDiscount = buy.PercentDiscount,
                            PaymentCondition = buy.PaymentCondition,
                            VendorId = buy.VendorId,
                            Remark = buy.Remark,
                            Status = DOCSTATUS.NEW,
                            UnitAmount = buy.UnitAmount,
                            Quantity = buy.Quantity
                        };
                        result2.Add(buydtl);

                        await _context.SalesOrderBuyings.AddAsync(buydtl);
                    }

                    Detail.SoBuyings = result2;

                    result.Add(Detail);
                }
                if (result.Count > 0)
                {
                    await _context.SalesOrderSellings.AddRangeAsync(result);
                }
            }

            return result;
        }


    }
}
