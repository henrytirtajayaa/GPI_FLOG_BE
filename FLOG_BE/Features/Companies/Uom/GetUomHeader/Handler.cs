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

namespace FLOG_BE.Features.Companies.Uom.GetUomHeader
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

            var list = await PaginatedList<Entities.UOMHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                UomHeaders = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.UOMHeader> getData(string personId, RequestFilter filter)
        {
            var query = (from h in _context.UOMHeaders
                         join uom in _context.UOMBases on h.UomBaseId equals uom.UomBaseId
                         where h.Inactive == false
                         select new Entities.UOMHeader
                         {
                             UomHeaderId = h.UomHeaderId,
                             UomScheduleCode = h.UomScheduleCode,
                             UomScheduleName = h.UomScheduleName,
                             UomBaseId = h.UomBaseId,
                             UomBaseCode = uom.UomCode,
                             UomBaseName = uom.UomName,
                             ModifiedBy = h.ModifiedBy,
                             ModifiedDate = h.ModifiedDate,
                             UomDetails = (from d in _context.UOMDetails where d.UomHeaderId == h.UomHeaderId
                                           orderby d.EquivalentQty ascending
                                           select d
                                           ).ToList()
                         }
                         ).AsQueryable();

            var filterCode = filter.UomScheduleCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.UOMHeader>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.UomScheduleCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.UomScheduleName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.UOMHeader>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.UomScheduleName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterUomCode = filter.UomBaseCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterUomCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.UOMHeader>(true);
                foreach (var filterItem in filterUomCode)
                {
                    predicate = predicate.Or(x => x.UomBaseCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterUomName = filter.UomBaseName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.UOMHeader>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.UomBaseName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            return query;

        }

        private IQueryable<Entities.UOMHeader> getSorted(IQueryable<Entities.UOMHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.UomScheduleCode);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("UomScheduleCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UomScheduleCode) : query.ThenBy(x => x.UomScheduleCode);
                }

                if (item.Contains("UomScheduleName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UomScheduleName) : query.ThenBy(x => x.UomScheduleName);
                }

                if (item.Contains("UomBaseCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UomBaseCode) : query.ThenBy(x => x.UomBaseCode);
                }
                if (item.Contains("UomBaseName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UomBaseName) : query.ThenBy(x => x.UomBaseName);
                }
                
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.UomScheduleCode);
            }

            return query;
        }
    }
}
