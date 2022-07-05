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

namespace FLOG_BE.Features.Companies.ShippingLine.GetShippingLine
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

            var query = getShippingLine(request.Filter);
            query = getShippingLineSorted(query, request.Sort);
            List<Person> ListUser = await GetUser();


            var list = await PaginatedList<Entities.ShippingLine, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            List<ResponseItem> responseShiping;


            responseShiping = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
      
                ShippingLineId = x.ShippingLineId,
                ShippingLineCode = x.ShippingLineCode,
                ShippingLineName = x.ShippingLineName,
                ShippingLineType = x.ShippingLineType,
                VendorId = x.VendorId,
                VendorName = x.VendorName,
                IsFeeder = x.IsFeeder,
                Inactive = x.Inactive,
                Status = x.Status,
                CreatedBy = x.CreatedBy,
                CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
            }));

            return ApiResult<Response>.Ok(new Response()
            {
                ShippingLines = responseShiping,
                ListInfo = list.ListInfo
            });
        }
        private IQueryable<Entities.ShippingLine> getShippingLine(RequestFilter filter)
        {
            var query = _context.ShippingLines
                .Include(x => x.Vendor)
                .AsQueryable();

            var filterCode = filter.ShippingLineCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.ShippingLineCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.ShippingLineName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                foreach (var filterCoa in filterName)
                {
                    predicate = predicate.Or(x => x.ShippingLineName.Contains(filterCoa, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterType = filter.ShippingLineType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                foreach (var filterCoa in filterType)
                {
                    predicate = predicate.Or(x => x.ShippingLineType.Contains(filterCoa, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterVendorId = filter.VendorId?.Where(x => x != null).ToList();
            if (filterVendorId.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                foreach (var filterItem in filterVendorId)
                {
                    predicate = predicate.Or(x => x.VendorId == filterItem);
                }
                query = query.Where(predicate);
            }

            if (filter.IsFeeder.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                 predicate = predicate.Or(x => x.IsFeeder == filter.IsFeeder);
                query = query.Where(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
              
                    predicate = predicate.Or(x => x.Inactive == filter.Inactive);
                query = query.Where(predicate);
            }

            var filterStatus = filter.Status?.Where(x => x > 0).ToList();
            if (filterStatus.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                foreach (var filterItem in filterStatus)
                {
                    predicate = predicate.Or(x => x.Status == filterItem);
                }
                query = query.Where(predicate);
            }
            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ShippingLine>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }


            return query;
        }

        private IQueryable<Entities.ShippingLine> getShippingLineSorted(IQueryable<Entities.ShippingLine> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("ShippingLineCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShippingLineCode) : query.ThenBy(x => x.ShippingLineCode);
                }
                if (item.Contains("ShippingLineName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShippingLineName) : query.ThenBy(x => x.ShippingLineName);
                }
                if (item.Contains("ShippingLineType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShippingLineType) : query.ThenBy(x => x.ShippingLineType);
                }
                if (item.Contains("VendorId", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorId) : query.ThenBy(x => x.VendorId);
                }
                if (item.Contains("IsFeeder", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsFeeder) : query.ThenBy(x => x.IsFeeder);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
                }
                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
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

            if (!sortingList.Any(x => x.Contains("ShippingLineCode", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.ShippingLineCode);
            }

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
    }
}
