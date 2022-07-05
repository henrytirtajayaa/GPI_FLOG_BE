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
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Finance.DepositSettlement.PutDepositSettlement
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
            var response = new Response()
            {
                SettlementHeaderId = request.Body.SettlementHeaderId,
                DocumentType = request.Body.DocumentType,
                DocumentNo = request.Body.DocumentNo
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var depositHeader = await _context.DepositSettlementHeaders.FirstOrDefaultAsync(x => x.SettlementHeaderId == request.Body.SettlementHeaderId);
                    if (depositHeader != null)
                    {
                        depositHeader.TransactionDate = request.Body.TransactionDate;
                        depositHeader.DocumentType = request.Body.DocumentType;
                        depositHeader.CurrencyCode = request.Body.CurrencyCode;
                        depositHeader.ExchangeRate = request.Body.ExchangeRate;
                        depositHeader.CheckbookCode = request.Body.CheckbookCode;
                        depositHeader.CustomerId = request.Body.CustomerId;
                        depositHeader.Description = request.Body.Description;
                        depositHeader.OriginatingTotalPaid = request.Body.OriginatingTotalPaid;
                        depositHeader.FunctionalTotalPaid = request.Body.FunctionalTotalPaid;
                        depositHeader.ModifiedBy = request.Initiator.UserId;
                        depositHeader.ModifiedDate = DateTime.Now;

                        var receivableDetails = await InsertDepositSettlementDetails(_context, request.Body);


                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(response);

                    }
                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError($"AR Receipt error : " + e.Message);
                }
            }
        }

        private async Task<List<Entities.DepositSettlementDetail>> InsertDepositSettlementDetails(CompanyContext ctx, RequestDepositSettlement body)
        {
            List<Entities.DepositSettlementDetail> result = new List<Entities.DepositSettlementDetail>();

            if (body.DepositSettlementDetails != null)
            {
                //REMOVE EXISTING
                ctx.DepositSettlementDetails
               .Where(x => x.SettlementHeaderId == body.SettlementHeaderId).ToList().ForEach(p => ctx.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.DepositSettlementDetails)
                {
                    var depositDetail = new Entities.DepositSettlementDetail()
                    {
                        SettlementDetailId = Guid.NewGuid(),
                        SettlementHeaderId = body.SettlementHeaderId,
                        ReceiveTransactionId = item.ReceiveTransactionId,
                        Description = item.Description,
                        OriginatingBalance = item.OriginatingBalance,
                        FunctionalBalance = item.FunctionalBalance,
                        OriginatingPaid = item.OriginatingPaid,
                        FunctionalPaid = item.FunctionalPaid,
                        Status = FLOG.Core.DOCSTATUS.NEW
                    };

                    result.Add(depositDetail);
                }

                await _context.DepositSettlementDetails.AddRangeAsync(result);

                await ctx.SaveChangesAsync();
            }

            return result;
        }
    }
}
