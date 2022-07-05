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
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.APApply.PostApplyPayable
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
            _docGenerator = new DocumentGenerator(context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_PAYABLE, DOCNO_FEATURE.NOTRX_PAYABLE_APPLY, "", transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    decimal exchangeRate = 1;
                    bool isMultiply = true;
                    
                    if (request.Body.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE, StringComparison.OrdinalIgnoreCase))
                    {
                        var cn = _context.PayableTransactionHeaders.Where(x => x.PayableTransactionId == request.Body.PayableTransactionId).FirstOrDefault();
                        if (cn != null)
                        {
                            exchangeRate = cn.ExchangeRate;
                            isMultiply = cn.IsMultiply;
                        }
                    }
                    else if (request.Body.DocumentType.Equals(DOCUMENTTYPE.PAYMENT, StringComparison.OrdinalIgnoreCase))
                    {
                        var pv = _context.ApPaymentHeaders.Where(x => x.PaymentHeaderId == request.Body.PaymentHeaderId).FirstOrDefault();
                        if (pv != null)
                        {
                            exchangeRate = pv.ExchangeRate;
                            isMultiply =pv.IsMultiply;
                        }
                    }
                    else
                    {
                        var cb = _context.CheckbookTransactionHeaders.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId).FirstOrDefault();
                        if (cb != null)
                        {
                            exchangeRate = cb.ExchangeRate;
                            isMultiply = cb.IsMultiply;
                        }
                    }

                    var applyPaymentHeader = new Entities.APApplyHeader()
                    {
                        PayableApplyId = Guid.NewGuid(),
                        TransactionDate = request.Body.TransactionDate,
                        DocumentType = request.Body.DocumentType,
                        DocumentNo = documentUniqueNo,
                        CheckbookTransactionId = request.Body.CheckbookTransactionId,
                        PayableTransactionId = request.Body.PayableTransactionId,
                        PaymentHeaderId = request.Body.PaymentHeaderId,
                        VendorId = request.Body.VendorId,
                        Description = request.Body.Description,
                        OriginatingTotalPaid = request.Body.OriginatingTotalPaid,
                        FunctionalTotalPaid = CALC.FunctionalAmount(isMultiply, request.Body.OriginatingTotalPaid, exchangeRate),
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.NEW,
                    };

                    _context.APApplyHeaders.Add(applyPaymentHeader);

                    if (applyPaymentHeader.PayableApplyId != null && applyPaymentHeader.PayableApplyId != Guid.Empty)
                    {
                        var details = await InsertDetails(request.Body, applyPaymentHeader, exchangeRate, isMultiply);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        var resp = await _financeManager.CreateDistributionJournalAsync(applyPaymentHeader, details);

                        if (resp.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();
                            return ApiResult<Response>.Ok(new Response()
                            {
                                DocumentNo = applyPaymentHeader.DocumentNo,
                                PayableApplyId = applyPaymentHeader.PayableApplyId
                            });
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Apply Payable Journal can not be processed.");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Apply Payable can not be stored.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Doc No can not be created. Please check Doc No setup!");
                }
            }
        }

        private async Task<List<Entities.APApplyDetail>> InsertDetails(RequestPaymentBody body, Entities.APApplyHeader header, decimal exchangeRate, bool isMultiply)
        {
            List<Entities.APApplyDetail> result = new List<Entities.APApplyDetail>();

            if (body.APApplyDetails != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.APApplyDetails)
                {
                    var applyDetail = new Entities.APApplyDetail()
                    {
                        PayableApplyDetailId = Guid.NewGuid(),
                        PayableApplyId= header.PayableApplyId,
                        PayableTransactionId = item.PayableTransactionId,
                        Description = item.Description,
                        OriginatingBalance = item.OriginatingBalance,
                        FunctionalBalance = CALC.FunctionalAmount(isMultiply, item.OriginatingBalance, exchangeRate),
                        OriginatingPaid = item.OriginatingPaid,
                        FunctionalPaid = CALC.FunctionalAmount(isMultiply, item.OriginatingPaid, exchangeRate),
                        Status = item.Status,
                    };

                    result.Add(applyDetail);
                }

                if (result.Count > 0)
                {
                    await _context.APApplyDetails.AddRangeAsync(result);
                }
            }

            return result;
        }


    }
}
