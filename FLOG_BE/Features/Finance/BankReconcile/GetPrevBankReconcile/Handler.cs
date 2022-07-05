using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Companies;
using FLOG.Core;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.BankReconcile.GetPrevBankReconcile
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext contextCentral, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getTransactions(request.Initiator.UserId, request.Filter);

            var existNewReconcile = _context.BankReconcileHeaders.Where(x => x.CheckbookCode == request.Filter.CheckbookCode && x.Status == DOCSTATUS.NEW).AsQueryable();

            bool allowNewReconcile = true;
            if (existNewReconcile.Any())
            {
                allowNewReconcile = false;
            }

            var list = await PaginatedList<Entities.BankReconcileHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            return ApiResult<Response>.Ok(new Response()
            {
                BankReconcile = list.FirstOrDefault(),
                AllowNew = allowNewReconcile,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.BankReconcileHeader> getTransactions(string personId, RequestFilter filter)
        {
            var query = (from x in _context.BankReconcileHeaders
                          .Where(x => x.Status == DOCSTATUS.POST && 
                                      x.CheckbookCode.Trim().ToUpper()==filter.CheckbookCode.Trim().ToUpper())
                          .OrderByDescending(s=>s.BankCutoffEnd)
                         select new Entities.BankReconcileHeader
                         {
                             BankReconcileId = x.BankReconcileId,
                             TransactionDate = x.TransactionDate,
                             DocumentNo = x.DocumentNo,
                             CurrencyCode = x.CurrencyCode,
                             CheckbookCode = x.CheckbookCode,
                             BankCutoffStart = x.BankCutoffStart,
                             BankCutoffEnd = x.BankCutoffEnd,
                             Description = x.Description,
                             BankEndingOrgBalance = x.BankEndingOrgBalance,
                             CheckbookEndingOrgBalance = x.CheckbookEndingOrgBalance,
                             BalanceDifference = (x.BankEndingOrgBalance - x.CheckbookEndingOrgBalance),
                             Status = x.Status
                         }).AsEnumerable().ToList().AsQueryable();

            return query;
        }

    }
}
