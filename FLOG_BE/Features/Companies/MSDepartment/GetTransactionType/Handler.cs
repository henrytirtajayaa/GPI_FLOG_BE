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

namespace FLOG_BE.Features.Companies.MSDepartment.GetMsDepartment
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
            _context = context;
            _linkCollection = linkCollection;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getMsDepartment(request.Initiator.UserId, request.Filter);
            query = getMsDepartmentSorted(query, request.Sort);

            var list = await PaginatedList<Entities.MsDepartment, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                Departments = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.MsDepartment> getMsDepartment(string personId, RequestFilter filter)
        {
            var query = _context.MsDepartments.Where(x=>x.Inactive == false).AsQueryable();

            var filterDepartmentCode = filter.DepartmentCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDepartmentCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.MsDepartment>(true);
                foreach (var filterItem in filterDepartmentCode)
                {
                    predicate = predicate.Or(x => x.DepartmentCode.ToUpper().Contains(filterItem.ToUpper()));
                }
                query = query.Where(predicate);
            }

            var filterTransactionName = filter.DepartmentName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.MsDepartment>(true);
                foreach (var filterItem in filterTransactionName)
                {
                    predicate = predicate.Or(x => x.DepartmentName.ToUpper().Contains(filterItem.ToUpper()));
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.MsDepartment> getMsDepartmentSorted(IQueryable<Entities.MsDepartment> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("DepartmentCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DepartmentCode) : query.ThenBy(x => x.DepartmentCode);
                }
                if (item.Contains("DepartmentName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DepartmentName) : query.ThenBy(x => x.DepartmentName);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.DepartmentCode);
            }

            return query;
        }
    }
}
