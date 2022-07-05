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
using FLOG.Core.Finance.Util;
using Infrastructure;

namespace FLOG_BE.Features.Finance.Receivable.PostReceivableTransaction
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
            _financeManager = new FinanceManager(context);
            _docGenerator = new DocumentGenerator(context);
        }
        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = "";
                if (request.Body.DocumentType.Trim().ToUpper().Contains("DEBIT"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_RECEIVABLE, DOCNO_FEATURE.TRXTYPE_DEBITNOTE, request.Body.TransactionType, transaction.GetDbTransaction());
                }
                else if (request.Body.DocumentType.Trim().ToUpper().Contains("CREDIT"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_RECEIVABLE, DOCNO_FEATURE.TRXTYPE_CREDITNOTE, request.Body.TransactionType, transaction.GetDbTransaction());
                }
                else if (request.Body.DocumentType.Trim().ToUpper().Contains("INVOICE"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_RECEIVABLE, DOCNO_FEATURE.TRXTYPE_INVOICE, request.Body.TransactionType, transaction.GetDbTransaction());
                }
                else if (request.Body.DocumentType.Trim().ToUpper().Contains("DEMURRAGE"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_DEPOSIT, DOCNO_FEATURE.TRXTYPE_DEPOSIT_DEMURRAGE, request.Body.TransactionType, transaction.GetDbTransaction());
                }
                else if (request.Body.DocumentType.Trim().ToUpper().Contains("CONTAINER GUARANTEE"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_DEPOSIT, DOCNO_FEATURE.TRXTYPE_CONTAINER_GUARANTEE, request.Body.TransactionType, transaction.GetDbTransaction());
                }
                else
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_DEPOSIT, DOCNO_FEATURE.TRXTYPE_DETENTION, request.Body.TransactionType, transaction.GetDbTransaction());
                }


                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var app = new Entities.ReceivableTransactionHeader()
                    {
                        ReceiveTransactionId = Guid.NewGuid(),
                        DocumentType = request.Body.DocumentType,
                        DocumentNo = documentUniqueNo,
                        BranchCode = request.Body.BranchCode,
                        TransactionDate = request.Body.TransactionDate,
                        TransactionType = request.Body.TransactionType,
                        CurrencyCode = request.Body.CurrencyCode,
                        ExchangeRate = request.Body.ExchangeRate,
                        IsMultiply = request.Body.IsMultiply,
                        CustomerId = request.Body.CustomerId,
                        PaymentTermCode = request.Body.PaymentTermCode,
                        CustomerAddressCode = request.Body.CustomerAddressCode,
                        SoDocumentNo = request.Body.SoDocumentNo,
                        NsDocumentNo = request.Body.NsDocumentNo,
                        Description = request.Body.Description,
                        SubtotalAmount = request.Body.SubtotalAmount,
                        DiscountAmount = request.Body.DiscountAmount,
                        TaxAmount = request.Body.TaxAmount,
                        Status = DOCSTATUS.NEW,
                        StatusComment = request.Body.StatusComment,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now,
                        BillToAddressCode = request.Body.BillToAddressCode,
                        ShipToAddressCode = request.Body.ShipToAddressCode,
                    };

                    _context.ReceivableTransactionHeaders.Add(app);

                    JournalResponse jResponse = new JournalResponse();
                    if (app.ReceiveTransactionId != null && app.ReceiveTransactionId != Guid.Empty)
                    {
                        var receivableDetails = await InsertReceivableDetails(_context, request.Body, app.ReceiveTransactionId);

                        var receivableTaxes = await InsertReceivableTax(_context, request.Body, app.ReceiveTransactionId);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        jResponse = await _financeManager.CreateDistributionJournalAsync(app, receivableDetails, receivableTaxes);
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Receive Transaction can not be stored.");
                    }

                    if (jResponse.Valid)
                    {
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            ReceiveTransactionId = app.ReceiveTransactionId,
                            DocumentNo = app.DocumentNo
                        });
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError(jResponse.ErrorMessage);
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Doc No can not be created. Please check Doc No setup!");
                }                
            }
        }

        private async Task<List<Entities.ReceivableTransactionDetail>> InsertReceivableDetails(CompanyContext ctx, RequestReceivableBody body,Guid ReceiveTransactionId)
        {
            List<Entities.ReceivableTransactionDetail> details = new List<Entities.ReceivableTransactionDetail>();

            if (body.RequestReceivableDetails != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.RequestReceivableDetails)
                {
                    var ReceivableDetails = new Entities.ReceivableTransactionDetail()
                    {
                        TransactionDetailId = Guid.NewGuid(),
                        ReceiveTransactionId = ReceiveTransactionId,
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

                    details.Add(ReceivableDetails);
                }
                
                if(details.Count > 0)
                    await _context.ReceivableTransactionDetails.AddRangeAsync(details);
            }

            return details;
        }

        private async Task<List<Entities.ReceivableTransactionTax>> InsertReceivableTax(CompanyContext ctx, RequestReceivableBody body, Guid ReceiveTransactionId)
        {
            List<Entities.ReceivableTransactionTax> taxes = new List<Entities.ReceivableTransactionTax>();

            if (body.RequestReceivableTaxes != null)
            {
                //INSERT NEW ROWS Tax
                foreach (var item in body.RequestReceivableTaxes)
                {
                    var ReceivableTax = new Entities.ReceivableTransactionTax()
                    {
                        TransactionTaxId = Guid.NewGuid(),
                        ReceiveTransactionId = ReceiveTransactionId,
                        TaxScheduleId = item.TaxScheduleId,
                        IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                        TaxScheduleCode = item.TaxScheduleCode,
                        TaxBasePercent = item.TaxBasePercent,
                        TaxBaseOriginatingAmount = item.TaxBaseOriginatingAmount,
                        TaxablePercent = item.TaxablePercent,
                        OriginatingTaxAmount = item.OriginatingTaxAmount,
                        Status = item.Status
                    };

                    taxes.Add(ReceivableTax);
                }

                if(taxes.Count > 0)
                    await _context.ReceivableTransactionTaxes.AddRangeAsync(taxes);
            }

            return taxes;

        }
    }
}
