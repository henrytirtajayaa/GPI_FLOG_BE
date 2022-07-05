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
using LinqKit;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Companies.Uom.GetUomBase
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

            var query = getData(request.Initiator.UserId, request.Filter);
            query = getSorted(query, request.Sort);

            var list = await PaginatedList<Entities.UOMBase, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                UomBases = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.UOMBase> getData(string personId, RequestFilter filter)
        {
            var query = _context.UOMBases
                    .AsQueryable();

            var filterCode = filter.UomCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.UOMBase>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.UomCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.UomName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.UOMBase>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.UomName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            return query;

        }

        private IQueryable<Entities.UOMBase> getSorted(IQueryable<Entities.UOMBase> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.UomCode);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("UomCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UomCode) : query.ThenBy(x => x.UomCode);
                }

                if (item.Contains("CityName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UomName) : query.ThenBy(x => x.UomName);
                }

            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.UomCode);
            }

            return query;
        }
    }
}
