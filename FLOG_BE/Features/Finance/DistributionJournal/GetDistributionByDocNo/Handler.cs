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

namespace FLOG_BE.Features.Finance.DistributionJournal.GetDistributionByDocNo
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

            var list = await PaginatedList<Entities.DistributionJournalHeader, ResponseHeader>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
                      
            return ApiResult<Response>.Ok(new Response()
            {
                DistributionJournal = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.DistributionJournalHeader> GetTransactions(string personId, RequestFilter filter)
        {                       
            var query = (from h in _context.DistributionJournalHeaders
                         join curr in _context.Currencies on h.CurrencyCode equals curr.CurrencyCode
                         where h.DocumentNo == filter.DocumentNo
                         select new Entities.DistributionJournalHeader
                         {
                            DistributionHeaderId = h.DistributionHeaderId,
                            DocumentNo = h.DocumentNo,
                            DocumentDate = h.DocumentDate,
                            CurrencyCode = h.CurrencyCode,
                            CurrencyDecimalPoint = curr.DecimalPlaces,
                            ExchangeRate = h.ExchangeRate,
                            TransactionId = h.TransactionId,
                            Description = h.Description,
                            TrxModule = h.TrxModule,
                            OriginatingTotal = h.OriginatingTotal,
                            FunctionalTotal = h.FunctionalTotal,
                            Status = h.Status,                            
                            DistributionJournalDetails = (from det in _context.DistributionJournalDetails
                                                          join coa in _context.Accounts on det.AccountId equals coa.AccountId
                                                          where det.DistributionHeaderId == h.DistributionHeaderId
                                                          orderby det.Status ascending, det.OriginatingDebit descending, det.AccountId ascending
                                                          select new Entities.DistributionJournalDetail
                                                          {
                                                             DistributionDetailId = det.DistributionDetailId,
                                                             DistributionHeaderId = det.DistributionHeaderId,
                                                             AccountId = det.AccountId,
                                                             AccountDesc = coa.Description,
                                                             ChargesId = det.ChargesId,
                                                             JournalNote = det.JournalNote,
                                                             OriginatingDebit = det.OriginatingDebit,
                                                             OriginatingCredit = det.OriginatingCredit,
                                                             FunctionalDebit = det.FunctionalDebit,
                                                             FunctionalCredit = det.FunctionalCredit,
                                                             Status = det.Status
                                                          }).ToList()
                         }).AsEnumerable().AsQueryable();

            return query;
        }

        private IQueryable<Entities.DistributionJournalHeader> GetTransactionSorted(IQueryable<Entities.DistributionJournalHeader> input, List<string> sort)
        {
            var query = input.OrderByDescending(x => x.DocumentNo);

            return query;
        }
    }
}
