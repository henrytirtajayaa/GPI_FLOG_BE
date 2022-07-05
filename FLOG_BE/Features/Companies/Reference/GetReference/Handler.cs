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

namespace FLOG_BE.Features.Companies.Reference.GetReference
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

            var query = getReference(request.Filter);
            query = getReferenceSorted(query, request.Sort);

            var list = await PaginatedList<Entities.Reference, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                References = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Reference> getReference(RequestFilter filter)
        {
            var query = _context.References.AsQueryable();

            var filterCode = filter.ReferenceCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Reference>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.ReferenceCode.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterName = filter.ReferenceName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Reference>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.ReferenceName.Contains(filterItem));
                }
                query = query.Where(predicate);
            }


            return query;
        }

        private IQueryable<Entities.Reference> getReferenceSorted(IQueryable<Entities.Reference> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("ReferenceCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReferenceCode) : query.ThenBy(x => x.ReferenceCode);
                }
            }

            if (!sortingList.Any(x => x.Contains("ReferenceCode", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.ReferenceCode);
            }

            return query;
        }
    }
}
