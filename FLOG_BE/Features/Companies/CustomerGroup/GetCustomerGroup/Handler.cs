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

namespace FLOG_BE.Features.Companies.CustomerGroup.GetCustomerGroup
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

            var query = GetCustomerGroups(request.Filter);
            query = GetCustomerGroupsSorted(query, request.Sort);

            
            var list = await PaginatedList<Entities.CustomerGroup, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                CustomerGroups = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.CustomerGroup> GetCustomerGroups(RequestFilter filter)
        {
            var query = _context.CustomerGroups.AsQueryable();

            var filterCustGroupCode = filter.CustomerGroupCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustGroupCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterCustGroupCode)
                {
                    predicate = predicate.Or(x => x.CustomerGroupCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCustGroupName = filter.CustomerGroupName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustGroupName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterCustGroupName)
                {
                    predicate = predicate.Or(x => x.CustomerGroupName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPaymentTermCode = filter.PaymentTermCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaymentTermCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterPaymentTermCode)
                {
                    predicate = predicate.Or(x => x.PaymentTermCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterReceivableAccountNo = filter.ReceivableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterReceivableAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterReceivableAccountNo)
                {
                    predicate = predicate.Or(x => x.ReceivableAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterAccruedReceivableAccountNo = filter.AccruedReceivableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAccruedReceivableAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterAccruedReceivableAccountNo)
                {
                    predicate = predicate.Or(x => x.AccruedReceivableAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                predicate = predicate.Or(x => x.Inactive == filter.Inactive.Value);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CustomerGroup>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.CustomerGroup> GetCustomerGroupsSorted(IQueryable<Entities.CustomerGroup> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("CustomerGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerGroupCode) : query.ThenBy(x => x.CustomerGroupCode);
                }
                if (item.Contains("CustomerGroupName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerGroupName) : query.ThenBy(x => x.CustomerGroupName);
                }
                if (item.Contains("PaymentTermCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PaymentTermCode) : query.ThenBy(x => x.PaymentTermCode);
                }
                if (item.Contains("ReceivableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReceivableAccountNo) : query.ThenBy(x => x.ReceivableAccountNo);
                }
                if (item.Contains("AccruedReceivableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AccruedReceivableAccountNo) : query.ThenBy(x => x.AccruedReceivableAccountNo);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
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

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.CustomerGroupCode);
            }

            return query;
        }
    }
}
