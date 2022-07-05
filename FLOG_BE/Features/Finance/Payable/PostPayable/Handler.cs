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
using FLOG.Core.Finance.Util;
using Infrastructure;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Finance.Payable.PostPayable
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _docGenerator = new DocumentGenerator(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = "";
                if (request.Body.DocumentType.Trim().ToUpper().Contains("DEBIT"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_PAYABLE, DOCNO_FEATURE.TRXTYPE_DEBITNOTE, request.Body.TransactionType, transaction.GetDbTransaction());
                }
                else if (request.Body.DocumentType.Trim().ToUpper().Contains("CREDIT"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_PAYABLE, DOCNO_FEATURE.TRXTYPE_CREDITNOTE, request.Body.TransactionType, transaction.GetDbTransaction());
                }
                else if (request.Body.DocumentType.Trim().ToUpper().Contains("INVOICE"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_PAYABLE, DOCNO_FEATURE.TRXTYPE_INVOICE, request.Body.TransactionType, transaction.GetDbTransaction());
                }

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var payableheader = new Entities.PayableTransactionHeader()
                    {
                        PayableTransactionId = Guid.NewGuid(),
                        DocumentType = request.Body.DocumentType,
                        DocumentNo = documentUniqueNo,
                        BranchCode = request.Body.BranchCode,
                        TransactionDate = request.Body.TransactionDate,
                        TransactionType = request.Body.TransactionType,
                        CurrencyCode = request.Body.CurrencyCode,
                        ExchangeRate = request.Body.ExchangeRate,
                        IsMultiply = request.Body.IsMultiply,
                        VendorId = request.Body.VendorId,
                        PaymentTermCode = request.Body.PaymentTermCode,
                        VendorAddressCode = request.Body.VendorAddressCode,
                        VendorDocumentNo = request.Body.VendorDocumentNo,
                        NsDocumentNo = request.Body.NsDocumentNo,
                        Description = request.Body.Description,
                        SubtotalAmount = request.Body.SubtotalAmount,
                        DiscountAmount = request.Body.DiscountAmount,
                        TaxAmount = request.Body.TaxAmount,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.NEW,
                        StatusComment = request.Body.StatusComment,
                        BillToAddressCode = request.Body.BillToAddressCode,
                        ShipToAddressCode = request.Body.ShipToAddressCode,
                    };

                    _context.PayableTransactionHeaders.Add(payableheader);

                    JournalResponse jResponse = new JournalResponse();
                    if (payableheader.PayableTransactionId != null && payableheader.PayableTransactionId != Guid.Empty)
                    {
                        var payablesDetails = await InsertPayableDetails(_context, request.Body, payableheader.PayableTransactionId);

                        var payableTaxes = await InsertPayableTax(_context, request.Body, payableheader.PayableTransactionId);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        jResponse = await _financeManager.CreateDistributionJournalAsync(payableheader, payablesDetails, payableTaxes, true);
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Payable Transaction can not be stored.");
                    }

                    if (jResponse.Valid)
                    {
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            PayableTransactionId = payableheader.PayableTransactionId,
                            DocumentNo = payableheader.DocumentNo
                        });
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(jResponse.ErrorMessage) ? jResponse.ErrorMessage : "Payable Transaction not valid.");
                    }
                }
                else
                {
                    transaction.Rollback();

                    return ApiResult<Response>.ValidationError("Payable Document No can not be created !");
                }                
            }
        }

        private async Task<List<Entities.PayableTransactionDetail>> InsertPayableDetails(CompanyContext ctx, RequestPayableBody body, Guid PayableTransactionId)
        {
            List<Entities.PayableTransactionDetail> result = new List<Entities.PayableTransactionDetail>();

            if (body.RequestPayableDetails != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.RequestPayableDetails)
                {
                    var payable = new Entities.PayableTransactionDetail()
                    {
                        TransactionDetailId = Guid.NewGuid(),
                        PayableTransactionId = PayableTransactionId,
                        ChargesId = item.ChargesId,
                        ChargesDescription = item.ChargesDescription,
                        OriginatingAmount = item.OriginatingAmount,
                        OriginatingTax = item.OriginatingTax,
                        OriginatingDiscount = item.OriginatingDiscount,
                        OriginatingExtendedAmount = item.OriginatingExtendedAmount,
                        FunctionalTax = item.FunctionalTax,
                        FunctionalDiscount = item.FunctionalDiscount,
                        FunctionalExtendedAmount = item.FunctionalExtendedAmount,
                        Status = item.Status,
                        TaxScheduleId = item.TaxScheduleId,
                        IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                        PercentDiscount = item.PercentDiscount,
                    };
                    result.Add(payable);                    
                }

                if(result.Count > 0)
                {
                    await _context.PayableTransactionDetails.AddRangeAsync(result);
                }
            }

            return result;
        }

        private async Task<List<Entities.PayableTransactionTax>> InsertPayableTax(CompanyContext ctx, RequestPayableBody body, Guid PayableTransactionId)
        {
            List<Entities.PayableTransactionTax> result = new List<Entities.PayableTransactionTax>();

            if (body.RequestPayableDetails != null)
            {
                //INSERT NEW ROWS Tax
                foreach (var item in body.RequestPayableTaxes)
                {
                    var tax = new Entities.PayableTransactionTax()
                    {
                        TransactionTaxId = Guid.NewGuid(),
                        PayableTransactionId = PayableTransactionId,
                        TaxScheduleId = item.TaxScheduleId,
                        IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                        TaxScheduleCode = item.TaxScheduleCode,
                        TaxBasePercent = item.TaxBasePercent,
                        TaxBaseOriginatingAmount = item.TaxBaseOriginatingAmount,
                        TaxablePercent = item.TaxablePercent,
                        OriginatingTaxAmount = item.OriginatingTaxAmount,
                        Status = item.Status
                    };

                    result.Add(tax);
                }
               
                if(result.Count>0)
                {
                    await _context.PayableTransactionTaxes.AddRangeAsync(result);
                }
            }

            return result;
        }
    }
}
