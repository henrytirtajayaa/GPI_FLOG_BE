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
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Companies.VendorGroup.GetVendorGroup
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _contextCentral;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getVendorGroup(request.Initiator.UserId, request.Filter);
            query = getVendorGroupSorted(query, request.Sort);
            List<Person> ListUser = await GetUser();

            var list = await PaginatedList<Entities.VendorGroup, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> response;

            response = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
               
                VendorGroupId = x.VendorGroupId.ToString(),
                VendorGroupCode = x.VendorGroupCode,
                VendorGroupName = x.VendorGroupName,
                PaymentTermCode = x.PaymentTermCode,
                PayableAccountNo = x.PayableAccountNo,
                AccruedPayableAccountNo = x.AccruedPayableAccountNo,
                InActive = x.InActive,
                CreatedBy = x.CreatedBy,
                CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
            }));


            return ApiResult<Response>.Ok(new Response()
            {
                VendorGroups = response,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.VendorGroup> getVendorGroup(string personId, RequestFilter filter)
        {
            var query = _context.VendorGroups
                     .AsQueryable();

            var filterVendorGroupCode = filter.VendorGroupCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendorGroupCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                foreach (var filterItem in filterVendorGroupCode)
                {
                    predicate = predicate.Or(x => x.VendorGroupCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterVendorGroupName = filter.VendorGroupName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendorGroupName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                foreach (var filterItem in filterVendorGroupName)
                {
                    predicate = predicate.Or(x => x.VendorGroupName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            } 
            
            var filterPaymentTermCode = filter.PaymentTermCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaymentTermCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                foreach (var filterItem in filterPaymentTermCode)
                {
                    predicate = predicate.Or(x => x.PaymentTermCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            
            var filterPayableAccountNo = filter.PayableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaymentTermCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                foreach (var filterItem in filterPayableAccountNo)
                {
                    predicate = predicate.Or(x => x.PayableAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            
            var filterAccruedPayableAccountNo = filter.AccruedPayableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAccruedPayableAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                foreach (var filterItem in filterAccruedPayableAccountNo)
                {
                    predicate = predicate.Or(x => x.AccruedPayableAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            
            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                    predicate = predicate.Or(x => x.InActive == filter.InActive);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.VendorGroup>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.VendorGroup> getVendorGroupSorted(IQueryable<Entities.VendorGroup> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("VendorGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorGroupCode) : query.ThenBy(x => x.VendorGroupCode);
                }

                if (item.Contains("VendorGroupName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorGroupName) : query.ThenBy(x => x.VendorGroupName);
                }

                if (item.Contains("PaymentTermCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PaymentTermCode) : query.ThenBy(x => x.PaymentTermCode);
                }
                if (item.Contains("PayableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PayableAccountNo) : query.ThenBy(x => x.PayableAccountNo);
                }
                if (item.Contains("AccruedPayableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AccruedPayableAccountNo) : query.ThenBy(x => x.AccruedPayableAccountNo);
                }
                if (item.Contains("InActive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.InActive) : query.ThenBy(x => x.InActive);
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

 

            if (!sortingList.Any(x => x.Contains("VendorGroupCode", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.VendorGroupCode);
            }

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
    }
}
