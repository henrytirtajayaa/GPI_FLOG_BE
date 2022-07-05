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

namespace FLOG_BE.Features.Companies.SalesPerson.GetSalesPerson
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

            var query = getSalesPerson(request.Filter);
            query = getSalesPersonSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();
            var list = await PaginatedList<Entities.SalesPerson, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> responseSalesPerson;
           
            responseSalesPerson = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                SalesPersonId = x.SalesPersonId,
                SalesCode = x.SalesCode,
                SalesName = x.SalesName,
                PersonId = x.PersonId,
                PersonFullName = ListUser.Where(p => Guid.Parse(p.PersonId) == x.PersonId).Select(p => p.PersonFullName).FirstOrDefault(),

            }));

            return ApiResult<Response>.Ok(new Response()
            {
                SalesPersons = responseSalesPerson,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.SalesPerson> getSalesPerson(RequestFilter filter)
        {
            var query = _context.SalesPersons.AsQueryable();

            var filterCode = filter.SalesCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesPerson>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.SalesCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            
            var filterName = filter.SalesName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesPerson>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.SalesName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.SalesPerson> getSalesPersonSorted(IQueryable<Entities.SalesPerson> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("SalesCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SalesCode) : query.ThenBy(x => x.SalesCode);
                }
                if (item.Contains("SalesName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SalesName) : query.ThenBy(x => x.SalesName);
                }
            }

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
    }
}
