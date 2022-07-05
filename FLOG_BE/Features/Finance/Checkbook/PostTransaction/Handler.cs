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

namespace FLOG_BE.Features.Finance.Checkbook.PostTransaction
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
                if (request.Body.DocumentType.Trim().ToUpper().Contains("IN"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByCheckbook(request.Body.TransactionDate, request.Body.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_IN, transaction.GetDbTransaction());
                }
                else
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByCheckbook(request.Body.TransactionDate, request.Body.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_OUT, transaction.GetDbTransaction());
                }

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var app = new Entities.CheckbookTransactionHeader()
                    {

                        CheckbookTransactionId = Guid.NewGuid(),
                        DocumentType = request.Body.DocumentType,
                        DocumentNo = documentUniqueNo,
                        BranchCode = request.Body.BranchCode,
                        TransactionDate = request.Body.TransactionDate,
                        TransactionType = request.Body.TransactionType,
                        CurrencyCode = request.Body.CurrencyCode,
                        ExchangeRate = request.Body.ExchangeRate,
                        CheckbookCode = request.Body.CheckbookCode,
                        IsVoid = request.Body.IsVoid,
                        VoidDocumentNo = request.Body.VoidDocumentNo,
                        PaidSubject = request.Body.PaidSubject,
                        SubjectCode = request.Body.SubjectCode,
                        Description = request.Body.Description,
                        OriginatingTotalAmount = request.Body.OriginatingTotalAmount,
                        FunctionalTotalAmount = request.Body.FunctionalTotalAmount,
                        Status = DOCSTATUS.NEW,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now,
                        IsMultiply = request.Body.IsMultiply
                    };

                    _context.CheckbookTransactionHeaders.Add(app);
                    await _context.SaveChangesAsync();

                    if (app.CheckbookTransactionId != null && app.CheckbookTransactionId != Guid.Empty)
                    {
                        if (request.Body.RequestCheckbookDetails.Count > 0)
                        {
                            List<Entities.CheckbookTransactionDetail> details = new List<Entities.CheckbookTransactionDetail>();

                            foreach (var item in request.Body.RequestCheckbookDetails)
                            {
                                var CheckbookDetails = new Entities.CheckbookTransactionDetail()
                                {
                                    TransactionDetailId = Guid.NewGuid(),
                                    CheckbookTransactionId = app.CheckbookTransactionId,
                                    ChargesId = item.ChargesId,
                                    ChargesDescription = item.ChargesDescription,
                                    OriginatingAmount = item.OriginatingAmount,
                                    FunctionalAmount = item.FunctionalAmount,
                                    Status = item.Status,
                                    RowIndex = item.RowIndex,
                                };
                                details.Add(CheckbookDetails);
                            }

                            await _context.CheckbookTransactionDetails.AddRangeAsync(details);

                            //CREATE DISTRIBUTION JOURNAL HERE
                            var resp = await _financeManager.CreateDistributionJournalAsync(app, details);

                            if (resp.Valid)
                            {
                                await _context.SaveChangesAsync();

                                transaction.Commit();
                                return ApiResult<Response>.Ok(new Response()
                                {
                                    CheckbookTransactionId = app.CheckbookTransactionId,
                                    DocumentNo = app.DocumentNo,
                                    DocumentType = app.DocumentType
                                });
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Checkbook Journal can not be processed !");
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError("Checkbook Transaction  not exist !");
                        }
                    }
                    else
                    {

                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Checkbook Transaction can not be stored !");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Checkbook Document No can not be created !");
                }                
            }
        }
    }
}
