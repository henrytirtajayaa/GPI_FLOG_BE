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
using FLOG_BE.Model.Companies.Entities;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.PutStatusSalesOrder
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private FLOG.Core.Finance.Util.IFinanceManager _financeManager;
        private readonly HATEOASLinkCollection _linkCollection;
        private IDocumentGenerator _documentGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _documentGenerator = new DocumentGenerator(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entryHeader = await _context.SalesOrderHeaders.FirstOrDefaultAsync(x => x.SalesOrderId == request.Body.SalesOrderId);
                    
                    string docNo = "";
                    if (entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;
                        
                        if (entryHeader.Status == DOCSTATUS.NEW)
                        {
                            if (request.Body.Status == DOCSTATUS.CANCEL)
                            {
                                entryHeader.Status = DOCSTATUS.CANCEL;
                                entryHeader.StatusComment = request.Body.StatusComment;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;
                                var isUpdate = await _context.SaveChangesAsync();
                            }
                            else if (request.Body.Status == DOCSTATUS.SUBMIT)
                            {
                                entryHeader.Status = DOCSTATUS.SUBMIT;
                                entryHeader.StatusComment = request.Body.StatusComment;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                var isUpdate = await _context.SaveChangesAsync();

                                if (isUpdate  > 0)
                                {
                                    SalesOrderHeader SalesOrderData = _context.SalesOrderHeaders.Where(p => p.SalesOrderId == request.Body.SalesOrderId).FirstOrDefault();

                                    string documentUniqueNo = _documentGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_SALES, FLOG.Core.TRX_MODULE.TRX_SALES_NEGO, request.Body.TransactionType, transaction.GetDbTransaction());

                                    if (!string.IsNullOrEmpty(documentUniqueNo))
                                    {
                                        var header = new Entities.NegotiationSheetHeader()
                                        {
                                            // negotiation_sheet_id, row_id, transaction_date, transaction_type, document_no, sales_order_id, total_func_selling, total_func_buying, remark
                                            NegotiationSheetId = Guid.NewGuid(),
                                            TransactionDate = SalesOrderData.TransactionDate,
                                            TransactionType = SalesOrderData.TransactionType,
                                            DocumentNo = documentUniqueNo,
                                            SalesOrderId = SalesOrderData.SalesOrderId,
                                            TotalFuncSelling = SalesOrderData.FunctionalSellingAmount,
                                            TotalFuncBuying = SalesOrderData.FunctionalBuyingAmount,
                                            Remark = SalesOrderData.Remark,
                                            Status = request.Body.Status,
                                            StatusComment = "",
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = request.Initiator.UserId,

                                        };

                                        _context.NegotiationSheetHeaders.Add(header);

                                        if (header.SalesOrderId != null && header.SalesOrderId != Guid.Empty)
                                        {

                                            var container = await InserContainertDetails(SalesOrderData, header);
                                            var trucking = await InsertTruckingDetails(SalesOrderData, header);
                                            var selling = await InsertSellingDetails(SalesOrderData, header);

                                            await _context.SaveChangesAsync();
                                            transaction.Commit();
                                            return ApiResult<Response>.Ok(new Response()
                                            {
                                                SalesOrderId = header.SalesOrderId,
                                                Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.Status))

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

                                        return ApiResult<Response>.ValidationError("Negotiation Sheet Document No can not be created !");
                                    }
                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError("Record can not be update.");
                        }

                    }

                    var response = new Response()
                    {
                        SalesOrderId = request.Body.SalesOrderId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError(ex.Message);
                }
            }
        }


        private async Task<List<Entities.NegotiationSheetContainer>> InserContainertDetails(SalesOrderHeader salesOrderData, Entities.NegotiationSheetHeader header)
        {
            List<Entities.NegotiationSheetContainer> result = new List<Entities.NegotiationSheetContainer>();
            
            List<SalesOrderContainer> container = _context.SalesOrderContainers.Where(p => p.SalesOrderId == header.SalesOrderId).ToList();
            
            if (container != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in container)
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
        private async Task<List<Entities.NegotiationSheetTrucking>> InsertTruckingDetails(SalesOrderHeader body, Entities.NegotiationSheetHeader header)
        {

            List<Entities.NegotiationSheetTrucking> result = new List<Entities.NegotiationSheetTrucking>();
            List<SalesOrderTrucking> Trucking = _context.SalesOrderTruckings.Where(p => p.SalesOrderId == header.SalesOrderId).ToList();
            if (Trucking != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in Trucking)
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

        private async Task<List<Entities.NegotiationSheetSelling>> InsertSellingDetails(SalesOrderHeader body, Entities.NegotiationSheetHeader header)
        {

            List<Entities.NegotiationSheetSelling> result = new List<Entities.NegotiationSheetSelling>();
            List<Entities.NegotiationSheetBuying> result2 = new List<Entities.NegotiationSheetBuying>();

            List<SalesOrderSelling> Selling = _context.SalesOrderSellings.Where(p => p.SalesOrderId == header.SalesOrderId).ToList();
            List<SalesOrderBuying> Buying = _context.SalesOrderBuyings.Where(p => p.SalesOrderId == header.SalesOrderId).ToList();

            if (Selling != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in Selling)
                {
                    var Detail = new Entities.NegotiationSheetSelling()
                    {
                  //      ns_selling_id, row_id, negotiation_sheet_id, charge_id, currency_code, exchange_rate, is_multiply, originating_amount, originating_tax, originating_discount, originating_extended_amount, functional_tax, functional_discount,
                  //functional_extended_amount, tax_schedule_id, is_tax_after_discount, percent_discount, payment_condition, customer_id, remark, status

                        NsSellingId = Guid.NewGuid(),
                        NegotiationSheetId = Guid.NewGuid(),
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
                        UnitAmount = item.UnitAmount,
                        Quantity = item.Quantity
                    };
                    result.Add(Detail);
                    foreach (var buy in Buying)
                    {

                        var buydtl = new Entities.NegotiationSheetBuying()
                        {

                            NsBuyingId = Guid.NewGuid(),
                            NegotiationSheetId = header.NegotiationSheetId,
                            NsSellingId = Detail.NsSellingId,
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
                            UnitAmount = buy.UnitAmount,
                            Quantity = buy.Quantity
                        };
                        result2.Add(buydtl);
                        await _context.NegotiationSheetBuyings.AddRangeAsync(result2);
                    }

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
