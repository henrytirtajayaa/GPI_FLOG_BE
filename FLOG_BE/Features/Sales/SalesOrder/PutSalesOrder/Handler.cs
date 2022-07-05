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

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.PutSalesOrder
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            {
                SalesOrderId = request.Body.SalesOrderId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {

                var editted = await _context.SalesOrderHeaders.FirstOrDefaultAsync(x => x.SalesOrderId == request.Body.SalesOrderId);
                if (editted != null)
                {
                    editted.TransactionType = request.Body.TransactionType;         
                    editted.TransactionDate = request.Body.TransactionDate;
                    editted.DocumentNo = request.Body.DocumentNo;
                    editted.BranchCode = request.Body.BranchCode;
                    editted.CustomerId = request.Body.CustomerId;
                    editted.CustomerAddressCode = request.Body.CustomerAddressCode;
                    editted.SalesCode = request.Body.SalesCode;
                    editted.ShipperAddressCode = request.Body.ShipperAddressCode;
                    editted.ConsigneeId = request.Body.ConsigneeId;
                    editted.ConsigneeAddressCode = request.Body.ConsigneeAddressCode;
                    editted.CustomerBillToAddressCode = request.Body.CustomerBillToAddressCode;
                    editted.CustomerShipToAddressCode = request.Body.CustomerShipToAddressCode;
                    editted.QuotDocumentNo = request.Body.QuotDocumentNo;
                    editted.ShipmentStatus = request.Body.ShipmentStatus;
                    editted.MasterNo = request.Body.MasterNo;
                    editted.AgreementNo = request.Body.AgreementNo;
                    editted.BookingNo = request.Body.BookingNo;
                    editted.HouseNo = request.Body.HouseNo;
                    editted.ShipperId = request.Body.ShipperId;
                    editted.ShipperAddressCode = request.Body.ShipperAddressCode;
                    editted.ShipperBillToAddressCode = request.Body.ShipperBillToAddressCode;
                    editted.ShipperShipToAddressCode = request.Body.ShipperShipToAddressCode;
                    editted.ConsigneeId = request.Body.ConsigneeId;
                    editted.ConsigneeId = request.Body.ConsigneeId;
                    editted.ConsigneeAddressCode = request.Body.ConsigneeAddressCode;
                    editted.ConsigneeBillToAddressCode = request.Body.ConsigneeBillToAddressCode;
                    editted.ConsigneeShipToAddressCode = request.Body.ConsigneeShipToAddressCode;
                    
                    editted.IsDifferentNotifyPartner = request.Body.IsDifferentNotifyPartner;
                    editted.NotifyPartnerId = request.Body.NotifyPartnerId;
                    editted.NotifyPartnerAddressCode = request.Body.NotifyPartnerAddressCode;
                    editted.NotifyPartnerBilltoAddressCode = request.Body.NotifyPartnerBilltoAddressCode;
                    editted.NotifyPartnerShipToAddressCode = request.Body.NotifyPartnerShipToAddressCode;
                   
                    editted.ShippingLineId = request.Body.ShippingLineId;
                    editted.IsShippingLineMaster = request.Body.IsShippingLineMaster;
                    editted.ShippingLineCode = request.Body.ShippingLineCode;
                    editted.ShippingLineName = request.Body.ShippingLineName;
                    editted.ShippingLineShippingNo = request.Body.ShippingLineShippingNo;
                    editted.ShippingLineVesselCode = request.Body.ShippingLineVesselCode;
                    editted.ShippingLineVesselName = request.Body.ShippingLineVesselName;
                    editted.ShippingLineDelivery = request.Body.ShippingLineDelivery;
                    editted.ShippingLineArrival = request.Body.ShippingLineArrival;
                    editted.FeederLineId = request.Body.FeederLineId;
                    editted.IsFeederLineMaster = request.Body.IsFeederLineMaster;
                    editted.FeederLineCode = request.Body.FeederLineCode;
                    editted.FeederLineName = request.Body.FeederLineName;
                    editted.FeederLineVesselCode = request.Body.FeederLineVesselCode;
                    editted.FeederLineVesselName = request.Body.FeederLineVesselName;
                    editted.FeederLineShippingNo = request.Body.FeederLineShippingNo;
                    editted.FeederLineDelivery = request.Body.FeederLineDelivery;
                    editted.FeederLineArrival = request.Body.FeederLineArrival;
                    editted.TermOfShipment = request.Body.TermOfShipment;
                    editted.FinalDestination = request.Body.FinalDestination;
                    editted.PortOfLoading = request.Body.PortOfLoading;
                    editted.PortOfDischarge = request.Body.PortOfDischarge;
                    editted.Commodity = request.Body.Commodity;
                    editted.CargoGrossWeight = request.Body.CargoGrossWeight;
                    editted.CargoNetWeight = request.Body.CargoNetWeight;
                    editted.CargoDescription = request.Body.CargoDescription;
                    editted.Remark = request.Body.Remark;
                    //editted.Status = request.Body.Status;
                    editted.StatusComment = request.Body.StatusComment;
                    editted.Status = DOCSTATUS.NEW;
                    editted.ModifiedBy = request.Initiator.UserId;
                    editted.ModifiedDate = DateTime.Now;

                    var details = await InsertDetails(request.Body,editted);
                    var trucking = await InsertTruckingDetails(request.Body, editted);
                    var selling = await InsertSellingDetails(request.Body, editted);

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        SalesOrderId = editted.SalesOrderId
                    });
                }
                else
                {
                    transaction.Rollback();

                    return ApiResult<Response>.ValidationError("Sales Order not found.");
                }



            }
        }

        private async Task<List<Entities.SalesOrderContainer>> InsertDetails(RequestSalesOrderBody body, Entities.SalesOrderHeader header)
        {
            List<Entities.SalesOrderContainer> result = new List<Entities.SalesOrderContainer>();

            if (body.SalesOrderContainers != null)
            {
                // REMOVE EXISTING
                _context.SalesOrderContainers   
               .Where(x => x.SalesOrderId == body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));
                await _context.SaveChangesAsync();
               
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.SalesOrderContainers)
                {
                    var Detail = new Entities.SalesOrderContainer()
                    {
                        SalesOrderContainerId = Guid.NewGuid(),
                        SalesOrderId = header.SalesOrderId,
                        ContainerId = item.ContainerId,
                        Qty = item.Qty,
                        UomDetailId = item.UomDetailId,
                        Remark = item.Remark,
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
            if (body.SalesOrderTruckings != null)
            {
                _context.SalesOrderTruckings
               .Where(x => x.SalesOrderId == body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));
                await _context.SaveChangesAsync();

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.SalesOrderTruckings)
                {
                    var Detail = new Entities.SalesOrderTrucking()
                    {
                        SalesOrderTruckingId = Guid.NewGuid(),
                        SalesOrderId = item.SalesOrderId,
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
                _context.SalesOrderSellings
              .Where(x => x.SalesOrderId == body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));

                _context.SalesOrderBuyings
              .Where(x => x.SalesOrderId== body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));
                await _context.SaveChangesAsync();
                
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.SalesOrderSellings.OrderBy(x => x.RowId))
                {
                    var Detail = new Entities.SalesOrderSelling()
                    {
                        SalesOrderSellingId = Guid.NewGuid(),
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
                    result.Add(Detail);

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
                            UnitAmount = buy.UnitAmount,
                            Quantity = buy.Quantity
                        };

                        result2.Add(buydtl);
                        await _context.SalesOrderBuyings.AddRangeAsync(result2);
                    }
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
