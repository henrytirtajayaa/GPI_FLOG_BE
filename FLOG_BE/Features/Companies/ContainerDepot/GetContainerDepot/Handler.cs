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

namespace FLOG_BE.Features.Companies.ContainerDepot.GetContainerDepot
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

            var query = getContainerDepot(request.Initiator.UserId, request.Filter);
            query = getContainerDepotSorted(query, request.Sort);

            var list = await PaginatedList<Entities.ContainerDepot, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                ContainerDepots = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.ContainerDepot> getContainerDepot(string personId, RequestFilter filter)
        {
            var query = (from depo in _context.ContainerDepots
                     .Include(x => x.Cities)
                     select new Entities.ContainerDepot
                     {
                         ContainerDepotId = depo.ContainerDepotId,
                         DepotCode = depo.DepotCode,
                         DepotName = depo.DepotName,
                         OwnerVendorId = depo.OwnerVendorId,
                         CityCode = depo.CityCode,
                         Cities = depo.Cities,
                         VendorCode = (from vd in _context.Vendors where vd.VendorId == depo.OwnerVendorId
                                       select vd.VendorCode).FirstOrDefault(),
                         VendorName = (from vd in _context.Vendors
                                       where vd.VendorId == depo.OwnerVendorId
                                       select vd.VendorName).FirstOrDefault(),
                         InActive = depo.InActive,
                         ModifiedBy = depo.ModifiedBy,
                         ModifiedDate = depo.ModifiedDate,
                     })
                     .AsQueryable();

            var filterCode = filter.DepotCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.DepotCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterDb = filter.DepotName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDb.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                foreach (var filterItem in filterDb)
                {
                    predicate = predicate.Or(x => x.DepotName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterVendor = filter.VendorName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendor.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                foreach (var filterItem in filterVendor)
                {
                    predicate = predicate.Or(x => x.VendorName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCity = filter.CityName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCity.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                foreach (var filterItem in filterCity)
                {
                    predicate = predicate.Or(x => x.Cities.CityName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                predicate = predicate.Or(x => x.InActive == filter.InActive);
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerDepot>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.ContainerDepot> getContainerDepotSorted(IQueryable<Entities.ContainerDepot> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("DepotCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DepotCode) : query.ThenBy(x => x.DepotCode);
                }

                if (item.Contains("DepotName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DepotName) : query.ThenBy(x => x.DepotName);
                }

                if (item.Contains("VendorName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorName) : query.ThenBy(x => x.VendorName);
                }

                if (item.Contains("CityName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Cities.CityName) : query.ThenBy(x => x.Cities.CityName);
                }

                if (item.Contains("InActive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.InActive) : query.ThenBy(x => x.InActive);
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
                query = query.ThenBy(x => x.DepotCode);
            }

            return query;
        }
    }
}
