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

namespace FLOG_BE.Features.Finance.ArReceipt.PostCustomerReceipt
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IDocumentGenerator _docGenerator;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(context);
            _financeManager = new FinanceManager(context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByCheckbook(request.Body.TransactionDate, request.Body.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_RECEIPT, transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var app = new Entities.ArReceiptHeader()
                    {
                        ReceiptHeaderId = Guid.NewGuid(),
                        TransactionDate = request.Body.TransactionDate,
                        TransactionType = request.Body.TransactionType,
                        DocumentNo = documentUniqueNo,
                        CurrencyCode = request.Body.CurrencyCode,
                        ExchangeRate = request.Body.ExchangeRate,
                        IsMultiply = request.Body.IsMultiply,
                        CheckbookCode = request.Body.CheckbookCode,
                        CustomerId = request.Body.CustomerId,
                        Description = request.Body.Description,
                        OriginatingTotalPaid = request.Body.OriginatingTotalPaid,
                        FunctionalTotalPaid = request.Body.FunctionalTotalPaid,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.NEW,
                        StatusComment = request.Body.StatusComment
                    };

                    _context.ArReceiptHeaders.Add(app);

                    JournalResponse jResponse = new JournalResponse();

                    if (app.ReceiptHeaderId != null && app.ReceiptHeaderId != Guid.Empty)
                    {
                        var details = await InsertArReceiptDetails(request.Body, app);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        jResponse = await _financeManager.CreateDistributionJournalAsync(app, details);

                        if (jResponse.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(new Response()
                            {
                                ReceiptHeaderId = app.ReceiptHeaderId,
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
                        return ApiResult<Response>.ValidationError("Receivable Receipt can not be stored.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Doc No can not be created. Please check Doc No setup!");
                }
            }
        }

        private async Task<List<Entities.ArReceiptDetail>> InsertArReceiptDetails(RequestCustomerReceiptBody body, Entities.ArReceiptHeader header)
        {
            List<Entities.ArReceiptDetail> result = new List<Entities.ArReceiptDetail>();

            if (body.ArReceiptDetails != null)
            {
                foreach (var item in body.ArReceiptDetails)
                {
                    var detail = new Entities.ArReceiptDetail()
                    {
                        ReceiptDetailId = Guid.NewGuid(),
                        ReceiptHeaderId = header.ReceiptHeaderId,
                        ReceiveTransactionId = item.ReceiveTransactionId,
                        NsDocumentNo = item.NsDocumentNo,
                        MasterNo = item.MasterNo,
                        AgreementNo = item.AgreementNo,
                        Description = item.Description,
                        OriginatingBalance = item.OriginatingBalance,
                        FunctionalBalance = item.FunctionalBalance,
                        OriginatingPaid = item.OriginatingPaid,
                        FunctionalPaid = item.FunctionalPaid,
                        Status = DOCSTATUS.NEW,

                    };

                    result.Add(detail);
                }

                if (result.Count > 0)
                {
                    await _context.ArReceiptDetails.AddRangeAsync(result);
                }
            }

            return result;
        }
    }
}
