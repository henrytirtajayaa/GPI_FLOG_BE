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
using FLOG_BE.Model.Companies.Entities;
using Infrastructure;
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.SubmitSalesOrder
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IDocumentGenerator _docGenerator;
        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(_context);
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
                    editted.ShippingLineShippingNo = request.Body.ShippingLineShippingNo;

                    editted.ShippingLineDelivery = request.Body.ShippingLineDelivery;
                    editted.ShippingLineArrival = request.Body.ShippingLineArrival;
                    editted.FeederLineId = request.Body.FeederLineId;
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
                    editted.Status = DOCSTATUS.SUBMIT;
                    editted.StatusComment = "";
                    editted.ModifiedBy = request.Initiator.UserId;
                    editted.ModifiedDate = DateTime.Now;

                    //var details = await InsertDetails(request.Body, editted);
                    //var trucking = await InsertTruckingDetails(request.Body, editted);
                    //var selling = await InsertSellingDetails(request.Body, editted);

                    var validator = await CreateNS(request.Body, editted, request);

                    if(validator.Valid)
                    {
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            SalesOrderId = editted.SalesOrderId,
                            Message = string.Format("{0} status successfully {1}", request.Body.DocumentNo, DOCSTATUS.Caption(DOCSTATUS.SUBMIT))
                        });
                    }
                    else
                    {
                        return ApiResult<Response>.ValidationError(String.Format("Negotiation Sheet can not be created. {0}!", validator.ErrorMessage));
                    }                    
                }
                else
                {
                    return ApiResult<Response>.ValidationError("Sales Order not found.");
                }
            }
        }

        private async Task<JournalResponse> CreateNS(SubmitSalesOrderBody body, Entities.SalesOrderHeader header, Request request)
        {
            JournalResponse resp = new JournalResponse { Valid = false, ErrorMessage = "", ValidStatus = 0 };

            try
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(header.TransactionDate, TRX_MODULE.TRX_SALES, DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET, header.TransactionType, _context.Database.CurrentTransaction.GetDbTransaction());
                
                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var entheader = new Entities.NegotiationSheetHeader()
                    {
                        NegotiationSheetId = Guid.NewGuid(),
                        TransactionDate = header.TransactionDate,
                        TransactionType = header.TransactionType,
                        DocumentNo = documentUniqueNo,
                        SalesOrderId = header.SalesOrderId,                        
                        CustomerId = header.CustomerId,
                        CustomerAddressCode = header.CustomerAddressCode,
                        CustomerBillToAddressCode = header.CustomerBillToAddressCode,
                        CustomerShipToAddressCode = header.CustomerShipToAddressCode,
                        SalesCode = header.SalesCode,
                        QuotDocumentNo = header.QuotDocumentNo,
                        ShipperId = header.ShipperId,
                        ShipperAddressCode = header.ShipperAddressCode,
                        ShipperBillToAddressCode = header.ShipperBillToAddressCode,
                        ShipperShipToAddressCode = header.ShipperShipToAddressCode,
                        ShipmentStatus = header.ShipmentStatus,
                        MasterNo = header.MasterNo,
                        AgreementNo = header.AgreementNo,
                        BookingNo = header.BookingNo,
                        HouseNo = header.HouseNo,
                        ConsigneeId = header.ConsigneeId,
                        ConsigneeAddressCode = header.ConsigneeAddressCode,
                        ConsigneeBillToAddressCode = header.ConsigneeBillToAddressCode,
                        ConsigneeShipToAddressCode = header.ConsigneeShipToAddressCode,
                        IsDifferentNotifyPartner = header.IsDifferentNotifyPartner,
                        NotifyPartnerId = header.NotifyPartnerId,
                        NotifyAddressCode = header.NotifyPartnerAddressCode,
                        NotifyBillToAddressCode = header.NotifyPartnerBilltoAddressCode,
                        NotifyShipToAddressCode = header.NotifyPartnerShipToAddressCode,
                        ShippingLineId = header.ShippingLineId,
                        IsShippingLineMaster = header.IsShippingLineMaster,
                        ShippingLineCode = header.ShippingLineCode,
                        ShippingLineName = header.ShippingLineName,
                        ShippingLineVesselCode = header.ShippingLineVesselCode,
                        ShippingLineVesselName = header.ShippingLineVesselName,
                        ShippingLineShippingNo = header.ShippingLineShippingNo,
                        ShippingLineETD = header.ShippingLineDelivery,
                        ShippingLineETA = header.ShippingLineArrival,
                        TermOfShipment = header.TermOfShipment,
                        FinalDestination = header.FinalDestination,
                        PortOfLoading = header.PortOfLoading,
                        PortOfDischarge = header.PortOfDischarge,
                        Commodity = header.Commodity,
                        CargoGrossWeight = header.CargoGrossWeight,
                        CargoNetWeight = header.CargoNetWeight,
                        CargoDescription = header.CargoDescription,
                        TotalFuncSelling = header.FunctionalSellingAmount,
                        TotalFuncBuying = header.FunctionalBuyingAmount,
                        Remark = header.Remark,
                        Status = DOCSTATUS.NEW,
                        StatusComment = "",
                        CreatedDate = DateTime.Now,
                        CreatedBy = request.Initiator.UserId,
                    };

                    if (entheader.NegotiationSheetId != null && entheader.NegotiationSheetId != Guid.Empty)
                    {
                        //INSERT CHARGES
                        var sellings = _context.SalesOrderSellings.AsNoTracking().Where(x => x.SalesOrderId == header.SalesOrderId).ToList();

                        foreach (var sell in sellings)
                        {
                            sell.SoBuyings = (_context.SalesOrderBuyings.AsNoTracking().Where(s => s.SalesOrderSellingId == sell.SalesOrderSellingId)).AsNoTracking().ToList();
                        }

                        var selling = await NsSellingDetails(entheader, sellings);

                        //INSERT CONTAINERS
                        List<Entities.SalesOrderContainer> containers = _context.SalesOrderContainers.AsNoTracking().Where(x=>x.SalesOrderId == header.SalesOrderId).AsNoTracking().ToList();

                        var container = await NsContainerDetails(entheader, containers);

                        //INSERT TRUCKS
                        List<Entities.SalesOrderTrucking> trucks = _context.SalesOrderTruckings.AsNoTracking().Where(x => x.SalesOrderId == header.SalesOrderId).AsNoTracking().ToList();

                        var trucking = await NsTruckingDetails(entheader, trucks);
                        
                        if (selling.Count > 0)
                        {
                            entheader.TotalFuncSelling = selling.Sum(s => s.FunctionalExtendedAmount);
                            entheader.TotalFuncBuying = selling.Sum(s => s.NsBuyings.Sum(buy => buy.FunctionalExtendedAmount));

                            _context.NegotiationSheetHeaders.Add(entheader);

                            resp.Valid = true;
                            resp.ValidMessage = "NS successfuly created ! ";
                            resp.ErrorMessage = "";
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = "No Selling charges created !";
                        }
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "NS Document No can not be created !";
                }
            }
            catch (Exception ex)
            {
                resp.Valid = false;
                resp.ErrorMessage = "NS can not be created ! " + ex.Message;
                Console.WriteLine("[CreateNS] ******* " + ex.Message);
                Console.WriteLine("[CreateNS] ******* " + ex.StackTrace);
            }

            return resp;
        }

        private async Task<List<Entities.NegotiationSheetContainer>> NsContainerDetails(NegotiationSheetHeader header, List<Entities.SalesOrderContainer> containers)
        {
            List<Entities.NegotiationSheetContainer> result = new List<Entities.NegotiationSheetContainer>();

           
            if (containers != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in containers)
                {
                    var Detail = new Entities.NegotiationSheetContainer()
                    {
                        NSContainerId = Guid.NewGuid(),
                        NegotiationSheetId = header.NegotiationSheetId,
                        ContainerId = item.ContainerId,
                        Qty = item.Qty,
                        UomDetailId = item.UomDetailId,
                        Status = item.Status,
                    };

                    result.Add(Detail);
                }

                if (result.Count > 0)
                {
                    await _context.NegotiationSheetContainers.AddRangeAsync(result);
                }
            }

            return result;
        }
        private async Task<List<Entities.NegotiationSheetTrucking>> NsTruckingDetails(NegotiationSheetHeader header, List<Entities.SalesOrderTrucking> trucks)
        {

            List<Entities.NegotiationSheetTrucking> result = new List<Entities.NegotiationSheetTrucking>();
            if (trucks != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in trucks)
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
                        Status = item.Status,
                    };
                    result.Add(Detail);
                }
                if (result.Count > 0)
                {
                    await _context.NegotiationSheetTruckings.AddRangeAsync(result);
                }
            }

            return result;
        }

        private async Task<List<Entities.NegotiationSheetSelling>> NsSellingDetails(NegotiationSheetHeader header, List<Entities.SalesOrderSelling> sellings)
        {
            List<Entities.NegotiationSheetSelling> result = new List<Entities.NegotiationSheetSelling>();
            List<Entities.NegotiationSheetBuying> result2 = new List<Entities.NegotiationSheetBuying>();
                      
            if (sellings != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in sellings)
                {
                    var detailSelling = new Entities.NegotiationSheetSelling()
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
                        Status = item.Status,
                        ReceiveTransactionId = Guid.Empty,
                        UnitAmount = item.UnitAmount,
                        Quantity = item.Quantity
                    };

                    result2.Clear();

                    foreach (var buy in item.SoBuyings)
                    {
                        var buydtl = new Entities.NegotiationSheetBuying()
                        {
                            NsBuyingId = Guid.NewGuid(),
                            NegotiationSheetId = header.NegotiationSheetId,
                            NsSellingId = detailSelling.NsSellingId,
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
                            Status = buy.Status,
                            PayableTransactionId = Guid.Empty,
                            UnitAmount = buy.UnitAmount,
                            Quantity = buy.Quantity
                        };
                        
                        result2.Add(buydtl);

                        await _context.NegotiationSheetBuyings.AddRangeAsync(result2);
                    }

                    detailSelling.NsBuyings = result2.ToList();

                    result.Add(detailSelling);
                }
                if (result.Count > 0)
                {
                    await _context.NegotiationSheetSellings.AddRangeAsync(result);
                }
            }

            return result;
        }

    }
}
