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

namespace FLOG_BE.Features.Companies.Account.GetAccount
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

            var query = getAccount(request.Filter);
            query = getAccountSorted(query, request.Sort);

            
            var list = await PaginatedList<Entities.Account, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                Accounts = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Account> getAccount(RequestFilter filter)
        {
            var query = _context.Accounts.AsQueryable();

            var filterAccountId = filter.AccountId?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAccountId.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterCoa in filterAccountId)
                {
                    predicate = predicate.Or(x => x.AccountId.Contains(filterCoa, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment1 = filter.Segment1?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment1.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment1)
                {
                    predicate = predicate.Or(x => x.Segment1.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment2 = filter.Segment2?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment2.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment2)
                {
                    predicate = predicate.Or(x => x.Segment2.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment3 = filter.Segment3?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment3.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment3)
                {
                    predicate = predicate.Or(x => x.Segment3.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment4 = filter.Segment4?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment4.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment4)
                {
                    predicate = predicate.Or(x => x.Segment4.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment5 = filter.Segment5?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment5.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment5)
                {
                    predicate = predicate.Or(x => x.Segment5.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment6 = filter.Segment6?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment6.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment6)
                {
                    predicate = predicate.Or(x => x.Segment6.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment7 = filter.Segment7?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment7.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment7)
                {
                    predicate = predicate.Or(x => x.Segment7.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment8 = filter.Segment8?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment8.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment8)
                {
                    predicate = predicate.Or(x => x.Segment8.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment9 = filter.Segment9?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment9.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment9)
                {
                    predicate = predicate.Or(x => x.Segment9.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSegment10 = filter.Segment10?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSegment10.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterSegment10)
                {
                    predicate = predicate.Or(x => x.Segment10.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                predicate = predicate.Or(x => x.Inactive == filter.Inactive.Value);
                query = query.Where(predicate);
            }

            if (filter.NoDirectEntry.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                predicate = predicate.Or(x => x.NoDirectEntry == filter.NoDirectEntry.Value);
                query = query.Where(predicate);
            }

            if (filter.NormalBalance.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                predicate = predicate.Or(x => x.NormalBalance == filter.NormalBalance.Value);
                query = query.Where(predicate);
            }

            if (filter.PostingType.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                predicate = predicate.Or(x => x.PostingType == filter.PostingType.Value);
                query = query.Where(predicate);
            }

            if (filter.Revaluation.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                predicate = predicate.Or(x => x.Revaluation == filter.Revaluation.Value);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Account>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.Account> getAccountSorted(IQueryable<Entities.Account> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("AccountId", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AccountId) : query.ThenBy(x => x.AccountId);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("Segment1", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment1) : query.ThenBy(x => x.Segment1);
                }
                if (item.Contains("Segment2", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment2) : query.ThenBy(x => x.Segment2);
                }
                if (item.Contains("Segment3", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment3) : query.ThenBy(x => x.Segment3);
                }
                if (item.Contains("Segment4", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment4) : query.ThenBy(x => x.Segment4);
                }
                if (item.Contains("Segment5", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment5) : query.ThenBy(x => x.Segment5);
                }
                if (item.Contains("Segment6", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment6) : query.ThenBy(x => x.Segment6);
                }
                if (item.Contains("Segment7", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment7) : query.ThenBy(x => x.Segment7);
                }
                if (item.Contains("Segment8", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment8) : query.ThenBy(x => x.Segment8);
                }
                if (item.Contains("Segment9", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment9) : query.ThenBy(x => x.Segment9);
                }
                if (item.Contains("Segment10", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Segment10) : query.ThenBy(x => x.Segment10);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
                }
                if (item.Contains("CreatedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedBy) : query.ThenBy(x => x.CreatedBy);
                }
                if (item.Contains("CreatedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedDate) : query.ThenBy(x => x.CreatedDate);
                }
                if (item.Contains("ModifiedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedBy) : query.ThenBy(x => x.ModifiedBy);
                }
                if (item.Contains("ModifiedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedDate) : query.ThenBy(x => x.ModifiedDate);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.AccountId);
            }

            return query;
        }
    }
}
