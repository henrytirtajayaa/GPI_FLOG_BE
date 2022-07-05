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
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Companies.ChargeGroup.GetChargeGroup
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

            var query = getChargeGroup(request.Filter);
            query = getChargeSorted(query, request.Sort);

            var list = await PaginatedList<Entities.ChargeGroup, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

             

            return ApiResult<Response>.Ok(new Response()
            {
                ChargeGroup = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.ChargeGroup> getChargeGroup(RequestFilter filter)
        {
            var query = _context.ChargeGroups.AsQueryable();

            var filterCode = filter.ChargeGroupCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ChargeGroup>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.ChargeGroupCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.ChargeGroupName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ChargeGroup>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.ChargeGroupName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }


            return query;
        }

        private IQueryable<Entities.ChargeGroup> getChargeSorted(IQueryable<Entities.ChargeGroup> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("ChargeGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ChargeGroupCode) : query.ThenBy(x => x.ChargeGroupCode);
                }
                if (item.Contains("ChargesName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ChargeGroupName) : query.ThenBy(x => x.ChargeGroupName);
                }
               

            }

            return query;
        }
     
    }
}
