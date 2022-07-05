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

namespace FLOG_BE.Features.Companies.ApprovalSetup.GetApprovalSetupWithPerson
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

            var query = getApprovalSetup(request.Filter);
            query = getApprovalSetupSorted(query, request.Sort);

            var list = await PaginatedList<Entities.ApprovalSetupHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                ApprovalSetup = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.ApprovalSetupHeader> getApprovalSetup(RequestFilter filter)
        {
            var persons = _contextCentral.Persons.Where(x=>x.InActive == false);

            var query = _context.ApprovalSetupHeaders.AsQueryable();

            //FILTER ONLY CATEGORY WITH MEMBERS
            List<Entities.ApprovalSetupHeader> approvals = new List<Entities.ApprovalSetupHeader>();
            foreach(var header in query)
            {
                var existGroup = _context.ApprovalSetupDetails.Where(x => x.ApprovalSetupHeaderId == header.ApprovalSetupHeaderId && x.PersonCategoryId != null).AsQueryable();
                if (existGroup.Any())
                {
                    foreach(var grp in existGroup)
                    {
                        if(persons.Where(p=>p.PersonCategoryId.Equals(grp.PersonCategoryId.ToString(), StringComparison.OrdinalIgnoreCase)).Any()){
                            if(!approvals.Where(x=>x.ApprovalSetupHeaderId == grp.ApprovalSetupHeaderId).Any())
                            {
                                approvals.Add(header);
                            }
                        }
                    }
                }
                else
                {
                    approvals.Add(header);
                }
            }

            query = approvals.AsQueryable();

            var filterCode = filter.ApprovalCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.ApprovalCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                predicate = predicate.Or(x => x.Status == filter.Status);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupHeader>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.ApprovalSetupHeader> getApprovalSetupSorted(IQueryable<Entities.ApprovalSetupHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {

                if (item.Contains("ApprovalCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ApprovalCode) : query.ThenBy(x => x.ApprovalCode);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
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

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.ApprovalCode);
            }

            return query;
        }
    }
}
