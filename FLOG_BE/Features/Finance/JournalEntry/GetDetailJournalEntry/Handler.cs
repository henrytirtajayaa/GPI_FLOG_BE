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

namespace FLOG_BE.Features.Finance.JournalEntry.GetDetailJournalEntry
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = GetTransactions(request.Initiator.UserId, request.Filter);
            query = GetTransactionSorted(query, request.Sort);

            var list = await PaginatedList<Entities.JournalEntryDetail, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
                      
            return ApiResult<Response>.Ok(new Response()
            {
                DetailEntries = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.JournalEntryDetail> GetTransactions(string personId, RequestFilter filter)
        {                       
            var query = (from x in _context.JournalEntryDetails
                         join h in _context.JournalEntryHeaders on x.JournalEntryHeaderId equals h.JournalEntryHeaderId
                         join coa in _context.Accounts on x.AccountId equals coa.AccountId
                         where x.JournalEntryHeaderId == filter.JournalEntryHeaderId
                         orderby x.RowIndex ascending
                         select new Entities.JournalEntryDetail
                         {
                             JournalEntryDetailId = x.JournalEntryDetailId,
                             JournalEntryHeaderId = x.JournalEntryHeaderId,
                             AccountId = x.AccountId,
                             AccountDescription = coa.Description,
                             Description = x.Description,
                             OriginatingDebit = x.OriginatingDebit,
                             OriginatingCredit = x.OriginatingCredit,
                             FunctionalDebit = x.FunctionalDebit,
                             FunctionalCredit = x.FunctionalCredit,
                             Status = x.Status,

                         }).AsEnumerable().AsQueryable();

            return query;
        }

        private IQueryable<Entities.JournalEntryDetail> GetTransactionSorted(IQueryable<Entities.JournalEntryDetail> input, List<string> sort)
        {
            var query = input.OrderBy(x=>x.RowIndex);

            return query;
        }
    }
}
