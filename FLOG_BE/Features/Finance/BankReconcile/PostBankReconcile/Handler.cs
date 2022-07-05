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

namespace FLOG_BE.Features.Finance.BankReconcile.PostBankReconcile
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
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
                string documentUniqueNo = _documentGenerator.UniqueDocumentNoByCheckbook(request.Body.TransactionDate, request.Body.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_RECONCILE, transaction.GetDbTransaction());

                var header = new Entities.BankReconcileHeader()
                {
                    BankReconcileId = Guid.NewGuid(),
                    TransactionDate = request.Body.TransactionDate,
                    DocumentNo = documentUniqueNo,
                    CheckbookCode = request.Body.CheckbookCode,
                    CurrencyCode = request.Body.CurrencyCode,
                    BankCutoffStart = request.Body.BankCutoffStart,
                    BankCutoffEnd = request.Body.BankCutoffEnd,
                    Description = request.Body.Description,
                    BankEndingOrgBalance = request.Body.BankEndingOrgBalance,
                    CheckbookEndingOrgBalance = request.Body.CheckbookEndingOrgBalance,
                    PrevBankReconcileId = request.Body.PrevBankReconcileId,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now,
                    Status = DOCSTATUS.NEW,
                };
                
                _context.BankReconcileHeaders.Add(header);

                if (header.BankReconcileId != null && header.BankReconcileId != Guid.Empty)
                {
                    var details = await InsertDetails(request.Body, header);

                    var adjustments = await InsertAdjustments(request.Body, header);

                    if(details.Any())
                    {
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return ApiResult<Response>.Ok(new Response()
                        {
                            DocumentNo = header.DocumentNo,
                            BankReconcileId = header.BankReconcileId
                        });
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Bank Reconciliation details can not be processed !");
                    }                    
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Bank Reconciliation can not be stored !");
                }
            }
        }

        private async Task<List<Entities.BankReconcileDetail>> InsertDetails(RequestReconcileBody body, Entities.BankReconcileHeader header)
        {
            List<Entities.BankReconcileDetail> result = new List<Entities.BankReconcileDetail>();

            if (body.ReconcileDetails != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ReconcileDetails)
                {
                    if (item.IsChecked)
                    {
                        var applyDetail = new Entities.BankReconcileDetail()
                        {
                            BankReconcileDetailId = Guid.NewGuid(),
                            BankReconcileId = header.BankReconcileId,
                            TransactionId = item.TransactionId,
                            Modul = item.Modul,
                            TransactionDate = item.TransactionDate,
                            Status = DOCSTATUS.NEW,
                        };

                        result.Add(applyDetail);
                    }
                }

                if (result.Count > 0)
                {
                    await _context.BankReconcileDetails.AddRangeAsync(result);
                }
            }

            return result;
        }

        private async Task<List<Entities.BankReconcileAdjustment>> InsertAdjustments(RequestReconcileBody body, Entities.BankReconcileHeader header)
        {
            List<Entities.BankReconcileAdjustment> result = new List<Entities.BankReconcileAdjustment>();

            if (body.ReconcileDetails != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ReconcileAdjustments)
                {
                    var adjDetail = new Entities.BankReconcileAdjustment()
                    {
                        BankReconcileAdjustmentId = Guid.NewGuid(),
                        BankReconcileId = header.BankReconcileId,
                        TransactionDate = item.TransactionDate,
                        DocumentType = item.DocumentType,
                        TransactionType = "NORMAL",
                        ChargesId = item.ChargesId,
                        CurrencyCode = item.CurrencyCode,
                        ExchangeRate = item.ExchangeRate,
                        IsMultiply = item.IsMultiply,
                        PaidSubject = item.PaidSubject,
                        Description = item.Description,
                        OriginatingAmount = item.OriginatingAmount,
                        CheckbookTransactionId = Guid.Empty, //SET TO EMPTY
                        TransactionDetailId = Guid.Empty, //SET TO EMPTY
                        Status = DOCSTATUS.NEW
                    };

                    result.Add(adjDetail);
                }

                if (result.Count > 0)
                {
                    await _context.BankReconcileAdjustments.AddRangeAsync(result);
                }
            }

            return result;
        }

    }
}
