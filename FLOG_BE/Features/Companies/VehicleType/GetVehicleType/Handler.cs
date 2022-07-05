using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Companies;

namespace FLOG_BE.Features.Companies.VehicleType.GetVehicleType
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

            var query = getVehicleType(request.Initiator.UserId, request.Filter);
            query = getUserVehicleTypeSorted(query, request.Sort);

            var list = await PaginatedList<Entities.VehicleType, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                VehicleTypes = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.VehicleType> getVehicleType(string personId, RequestFilter filter)
        {
            var query = _context.VehicleTypes.AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.VehicleType>(true);

            var filterCode = filter.VehicleTypeCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VehicleType>(false);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.VehicleTypeCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterName = filter.VehicleTypeName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VehicleType>(false);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.VehicleTypeName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }
            var filterCat = filter.VehicleCategory?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCat.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VehicleType>(false);
                foreach (var filterItem in filterCat)
                {
                    predicate = predicate.Or(x => x.VehicleCategory.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.VehicleType>(false);
                predicate = predicate.Or(x => x.Inactive == filter.Inactive);
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.VehicleType> getUserVehicleTypeSorted(IQueryable<Entities.VehicleType> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("VehicleTypeCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VehicleTypeCode) : query.ThenBy(x => x.VehicleTypeCode);
                }
                if (item.Contains("VehicleTypeName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VehicleTypeName) : query.ThenBy(x => x.VehicleTypeName);
                }
                if (item.Contains("VehicleCategory", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VehicleCategory) : query.ThenBy(x => x.VehicleCategory);
                }

                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.VehicleTypeId);
            }

            return query;
        }
    }
}
