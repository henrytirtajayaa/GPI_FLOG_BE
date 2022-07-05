using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Central;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using LinqKit;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.Smartview.GetSmartview
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getSmartview(request.Initiator.UserId, request.Filter);
            query = getSmartviewSorted(query, request.Sort);

            var list = await PaginatedList<SmartView, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                Smartviews = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<SmartView> getSmartview(string personId, RequestFilter filter)
        {
            var query = _context.Smartviews.AsQueryable();

            var filterCode = filter.GroupName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<SmartView>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.GroupName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.SmartTitle?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<SmartView>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.SmartTitle.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.SqlViewName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<SmartView>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.SqlViewName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }


            if (filter.isFunction.HasValue)
            {
                var predicate = PredicateBuilder.New<SmartView>(false);
                predicate = predicate.Or(x => x.isFunction == filter.isFunction);
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<SmartView> getSmartviewSorted(IQueryable<SmartView> input, List<string> sort)
        {

        var query = input.OrderBy(x => 0);

        var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("GroupName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.GroupName) : query.ThenBy(x => x.GroupName);
                }

                if (item.Contains("SmartTitle", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SmartTitle) : query.ThenBy(x => x.SmartTitle);
                }

                if (item.Contains("SqlViewName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SqlViewName) : query.ThenBy(x => x.SqlViewName);
                }

                if (item.Contains("isFunction", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.isFunction) : query.ThenBy(x => x.isFunction);
                }

            }
            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.GroupName);
            }

            return query;
        }
    }
}
