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

namespace FLOG_BE.Features.Finance.DepositSettlement.PostDepositSettlement
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
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_DEPOSIT, DOCNO_FEATURE.NOTRX_DEPOSIT_SETTLEMENT, "", transaction.GetDbTransaction());
                
                if (await _context.DepositSettlementHeaders.AnyAsync(x => x.DepositNo == request.Body.DepositNo))
                {
                    return ApiResult<Response>.ValidationError("Deposit No. already exist");
                }

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var app = new Entities.DepositSettlementHeader()
                    {
                        SettlementHeaderId = Guid.NewGuid(),
                        TransactionDate = request.Body.TransactionDate,
                        DocumentType = request.Body.DocumentType,
                        DocumentNo = documentUniqueNo,
                        ReceiveTransactionId = request.Body.ReceiveTransactionId,
                        DepositNo = request.Body.DepositNo,
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

                    _context.DepositSettlementHeaders.Add(app);
                    
                    JournalResponse jResponse = new JournalResponse();

                    if (app.SettlementHeaderId != null && app.SettlementHeaderId != Guid.Empty)
                    {
                        var details = await InsertDepositSettlementDetails(request.Body, app);

                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(new Response()
                            {
                                SettlementHeaderId = app.SettlementHeaderId,
                                DocumentNo = app.DocumentNo
                            });
                        
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Deposit Settlement can not be stored.");
                    } 
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Doc No can not be created. Please check Doc No setup!");
                }
            }
        }

        private async Task<List<Entities.DepositSettlementDetail>> InsertDepositSettlementDetails(RequestDepositSettlement body, Entities.DepositSettlementHeader header)
        {
            List<Entities.DepositSettlementDetail> result = new List<Entities.DepositSettlementDetail>();

            if (body.DepositSettlementDetails != null)
            {
                foreach (var item in body.DepositSettlementDetails)
                {
                    var detail = new Entities.DepositSettlementDetail()
                    {
                        SettlementDetailId = Guid.NewGuid(),
                        SettlementHeaderId = header.SettlementHeaderId,
                        ReceiveTransactionId = item.ReceiveTransactionId,
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
                    await _context.DepositSettlementDetails.AddRangeAsync(result);
                }
            }

            return result;
        }
    }
}
