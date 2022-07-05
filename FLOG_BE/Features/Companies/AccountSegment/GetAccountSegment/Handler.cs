using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;

namespace FLOG_BE.Features.Companies.AccountSegment.GetAccountSegment
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getAccountsegment(request.Filter);
            query = getAccountsegmentSorted(query, request.Sort);

            var list = await PaginatedList<Entities.AccountSegment, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            var getCoa = _contextCentral.Companies.FirstOrDefault(x => x.CompanyId == request.Initiator.CompanyId);
            return ApiResult<Response>.Ok(new Response()
            {
                AccountSegments = list.OrderBy(x => x.SegmentNo).ToList(),
                ListInfo = list.ListInfo,
                CoaTotalLength = (getCoa != null ? getCoa.CoaTotalLength : 0 )
            });
        }

        private IQueryable<Entities.AccountSegment> getAccountsegment(RequestFilter filter)
        {
            var query = _context.AccountSegments.AsQueryable();

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.AccountSegment>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterSegment = filter.SegmentNo?.Where(x => x > 0).ToList();
            if (filterSegment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.AccountSegment>(true);
                foreach (var filterItem in filterSegment)
                {
                    predicate = predicate.Or(x => x.SegmentNo == filterItem);
                }
                query = query.Where(predicate);
            }
            var filterLength = filter.Length?.Where(x => x > 0).ToList();
            if (filterLength.Any())
            {
                var predicate = PredicateBuilder.New<Entities.AccountSegment>(true);
                foreach (var filterItem in filterLength)
                {
                    predicate = predicate.Or(x => x.Length == filterItem);
                }
                query = query.Where(predicate);
            }
            if (filter.IsMainAccount == true)
            {
                var predicate = PredicateBuilder.New<Entities.AccountSegment>(true);
                predicate = predicate.Or(x => x.IsMainAccount == filter.IsMainAccount);
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.AccountSegment> getAccountsegmentSorted(IQueryable<Entities.AccountSegment> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("SegmentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SegmentNo) : query.ThenBy(x => x.SegmentNo);
                }
            }

            if (!sortingList.Any(x => x.Contains("Description", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.Description);
            }
            if (!sortingList.Any(x => x.Contains("SegmentNo", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.SegmentNo);
            }

            return query;
        }
    }
}
