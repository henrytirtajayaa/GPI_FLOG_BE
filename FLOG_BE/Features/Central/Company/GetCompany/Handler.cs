using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Central.Entities;
using LinqKit;

namespace FLOG_BE.Features.Central.Company.GetCompany
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

            var query = getUserCompany(request.Initiator.UserId, request.Filter);
            query = getUserCompanySorted(query, request.Sort);

            var list = await PaginatedList<Entities.Company, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                Companies = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Company> getUserCompany(string personId, RequestFilter filter)
        {
            //var query = _context.Companies.AsQueryable();
            var query = (from comp in _context.Companies
                         select new Entities.Company
                         {
                             CompanyId = comp.CompanyId,
                             CompanyName = comp.CompanyName,
                             DatabaseId = comp.DatabaseId,
                             DatabaseAddress = comp.DatabaseAddress,
                             DatabasePassword = comp.DatabasePassword,
                             CoaSymbol = comp.CoaSymbol,
                             CoaTotalLength = comp.CoaTotalLength,
                             InActive = comp.InActive,
                             CreatedBy = comp.CreatedBy,
                             CreatedDate = comp.CreatedDate,
                             ModifiedBy = comp.ModifiedBy,
                             ModifiedDate = comp.ModifiedDate,
                             SmtpServer = comp.SmtpServer,
                             SmtpPort = comp.SmtpPort,
                             SmtpPassword = comp.SmtpPassword,
                             SmtpUser = comp.SmtpUser,
                             CreatedByName = (from usr in _context.Persons
                                     where usr.PersonId == comp.CreatedBy
                                     select usr.PersonFullName).FirstOrDefault(),
                             ModifiedByName = (from usr in _context.Persons
                                               where usr.PersonId == comp.ModifiedBy
                                         select usr.PersonFullName).FirstOrDefault()
                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.Company>(true);

            var filterName = filter.CompanyName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.CompanyName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterDb = filter.DatabaseId?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDb.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterDb)
                {
                    predicate = predicate.Or(x => x.DatabaseId.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterDbAddress = filter.DatabaseAddress?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDbAddress.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterDbAddress)
                {
                    predicate = predicate.Or(x => x.DatabaseAddress.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterCoaSymbol = filter.CoaSymbol?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCoaSymbol.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterCoaSymbol)
                {
                    predicate = predicate.Or(x => x.CoaSymbol.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterCoaTotalLengthMin= filter.CoaTotalLengthMin?.Where(x => x > 0).ToList();
            if (filterCoaTotalLengthMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterCoaTotalLengthMin)
                {
                    predicate = predicate.Or(x => x.CoaTotalLength >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCoaTotalLengthMax = filter.CoaTotalLengthMax?.Where(x => x > 0).ToList();
            if (filterCoaTotalLengthMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterCoaTotalLengthMax)
                {
                    predicate = predicate.Or(x => x.CoaTotalLength <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedByName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime) x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedByName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Company>(false);
                predicate = predicate.Or(x => x.InActive == filter.InActive);
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.Company> getUserCompanySorted(IQueryable<Entities.Company> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("CompanyName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CompanyName) : query.ThenBy(x => x.CompanyName);
                }

                if (item.Contains("DataBaseId", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DatabaseId) : query.ThenBy(x => x.DatabaseId);
                }

                if (item.Contains("DatabaseAddress", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DatabaseAddress) : query.ThenBy(x => x.DatabaseAddress);
                }

                if (item.Contains("CoaSymbol", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CoaSymbol) : query.ThenBy(x => x.CoaSymbol);
                }

                if (item.Contains("CoaTotalLength", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CoaTotalLength) : query.ThenBy(x => x.CoaTotalLength);
                }

                if (item.Contains("CreatedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedByName) : query.ThenBy(x => x.CreatedByName);
                }

                if (item.Contains("CreatedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedDate) : query.ThenBy(x => x.CreatedDate);
                }

                if (item.Contains("ModifiedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedByName) : query.ThenBy(x => x.ModifiedByName);
                }

                if (item.Contains("ModifiedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedDate) : query.ThenBy(x => x.ModifiedDate);
                }

                if (item.Contains("InActive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.InActive) : query.ThenBy(x => x.InActive);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.CompanyName);
            }

            return query;
        }
    }
}
