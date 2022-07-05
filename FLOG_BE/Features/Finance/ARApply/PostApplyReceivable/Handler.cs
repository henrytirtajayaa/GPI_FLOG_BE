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

namespace FLOG_BE.Features.Finance.ARApply.PostApplyReceivable
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
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_RECEIVABLE, DOCNO_FEATURE.NOTRX_RECEIVABLE_APPLY, "", transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    decimal exchangeRate = 1;
                    bool isMultiply = true;
                    if (request.Body.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE, StringComparison.OrdinalIgnoreCase))
                    {
                        var cn = _context.ReceivableTransactionHeaders.Where(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId).FirstOrDefault();
                        if (cn != null)
                        {
                            exchangeRate = cn.ExchangeRate;
                            isMultiply = cn.IsMultiply;
                        }
                    }
                    else if (request.Body.DocumentType.Equals(DOCUMENTTYPE.RECEIPT, StringComparison.OrdinalIgnoreCase))
                    {
                        var rv = _context.ArReceiptHeaders.Where(x => x.ReceiptHeaderId == request.Body.ReceiptHeaderId).FirstOrDefault();
                        if (rv != null)
                        {
                            exchangeRate = rv.ExchangeRate;
                            isMultiply = rv.IsMultiply;
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

                    var applyReceiveHeader = new Entities.ARApplyHeader()
                    {
                        ReceivableApplyId = Guid.NewGuid(),
                        TransactionDate = request.Body.TransactionDate,
                        DocumentType = request.Body.DocumentType,
                        DocumentNo = documentUniqueNo,
                        ReceiptHeaderId = request.Body.ReceiptHeaderId,
                        CheckbookTransactionId = request.Body.CheckbookTransactionId,
                        ReceiveTransactionId = request.Body.ReceiveTransactionId,
                        CustomerId = request.Body.CustomerId,
                        Description = request.Body.Description,
                        OriginatingTotalPaid = request.Body.OriginatingTotalPaid,
                        FunctionalTotalPaid = CALC.FunctionalAmount(isMultiply, request.Body.OriginatingTotalPaid, exchangeRate),
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.NEW,
                    };

                    _context.ARApplyHeaders.Add(applyReceiveHeader);

                    if (applyReceiveHeader.ReceivableApplyId != null && applyReceiveHeader.ReceivableApplyId != Guid.Empty)
                    {
                        var details = await InsertDetails(request.Body, applyReceiveHeader, exchangeRate, isMultiply);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        var resp = await _financeManager.CreateDistributionJournalAsync(applyReceiveHeader, details);

                        if (resp.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();
                            return ApiResult<Response>.Ok(new Response()
                            {
                                DocumentNo = applyReceiveHeader.DocumentNo,
                                ReceivableApplyId = applyReceiveHeader.ReceivableApplyId
                            });
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Apply Receivable Journal can not be processed.");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Apply Receivable can not be stored.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Doc No can not be created. Please check Doc No setup!");
                }
                
            }
        }

        private async Task<List<Entities.ARApplyDetail>> InsertDetails(RequestPaymentBody body, Entities.ARApplyHeader header, decimal exchangeRate, bool isMultiply)
        {
            List<Entities.ARApplyDetail> result = new List<Entities.ARApplyDetail>();

            if (body.ARApplyDetails != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ARApplyDetails)
                {
                    var applyDetail = new Entities.ARApplyDetail()
                    {
                        ReceivableApplyDetailId = Guid.NewGuid(),
                        ReceivableApplyId = header.ReceivableApplyId,
                        ReceiveTransactionId = item.ReceiveTransactionId,
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
                    await _context.ARApplyDetails.AddRangeAsync(result);
                }
            }

            return result;
        }


    }
}
