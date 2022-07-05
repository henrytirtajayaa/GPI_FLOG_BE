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

namespace FLOG_BE.Features.Finance.BankReconcile.PutBankReconcile
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var header = _context.BankReconcileHeaders.Where(x => x.BankReconcileId == request.Body.BankReconcileId).FirstOrDefault();

                if(header != null)
                {
                    header.TransactionDate = request.Body.TransactionDate;
                    header.CheckbookCode = request.Body.CheckbookCode;
                    header.CurrencyCode = request.Body.CurrencyCode;
                    header.BankCutoffStart = request.Body.BankCutoffStart;
                    header.BankCutoffEnd = request.Body.BankCutoffEnd;
                    header.Description = request.Body.Description;
                    header.BankEndingOrgBalance = request.Body.BankEndingOrgBalance;
                    header.CheckbookEndingOrgBalance = request.Body.CheckbookEndingOrgBalance;
                    header.PrevBankReconcileId = request.Body.PrevBankReconcileId;
                    header.ModifiedBy = request.Initiator.UserId;
                    header.ModifiedDate = DateTime.Now;
                    header.Status = DOCSTATUS.NEW;

                    _context.BankReconcileHeaders.Update(header);

                    var details = await InsertDetails(request.Body, header);

                    var adjustments = await InsertAdjustments(request.Body, header);

                    if (details.Any())
                    {
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return ApiResult<Response>.Ok(new Response()
                        {
                            BankReconcileId = header.BankReconcileId,
                            Message = string.Format("Bank Reconciliation #{0} successfully updated.", header.DocumentNo)
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
                    return ApiResult<Response>.ValidationError("Bank Reconciliation can not be found !");
                }               
            }
        }

        private async Task<List<Entities.BankReconcileDetail>> InsertDetails( RequestReconcileBody body, Entities.BankReconcileHeader header)
        {
            List<Entities.BankReconcileDetail> result = new List<Entities.BankReconcileDetail>();

            if (body.ReconcileDetails != null)
            {
                //REMOVE EXISTING
                _context.BankReconcileDetails
                .Where(x => x.BankReconcileId == body.BankReconcileId).ToList().ForEach(p => _context.Remove(p));

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

        private async Task<List<Entities.BankReconcileAdjustment>> InsertAdjustments( RequestReconcileBody body, Entities.BankReconcileHeader header)
        {
            List<Entities.BankReconcileAdjustment> result = new List<Entities.BankReconcileAdjustment>();

            if (body.ReconcileDetails != null)
            {
                //REMOVE EXISTING
                _context.BankReconcileAdjustments
                .Where(x => x.BankReconcileId == body.BankReconcileId).ToList().ForEach(p => _context.Remove(p));

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
