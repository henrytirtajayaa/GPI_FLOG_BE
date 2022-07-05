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

namespace FLOG_BE.Features.Companies.NumberFormatHeader.GetNumberFormatHeader
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _contextCentral;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context,FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getNumberFormatHeader(request.Initiator.UserId, request.Filter);
            query = getNumberFormatHeaderSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();

            var list = await PaginatedList<Entities.NumberFormatHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            List<ResponseItem> responseFormatNumber;

            responseFormatNumber = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
              
                FormatHeaderId = x.FormatHeaderId,
                DocumentId = x.DocumentId,
                Description = x.Description,
                LastGeneratedNo = x.LastGeneratedNo,
                NumberFormat = x.NumberFormat,
                InActive = x.InActive,
                IsMonthlyReset = x.IsMonthlyReset,
                IsYearlyReset = x.IsYearlyReset,
                CreatedBy = x.CreatedBy,
                CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
            }));



            return ApiResult<Response>.Ok(new Response()
            {
                NumberFormatHeaders = responseFormatNumber,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.NumberFormatHeader> getNumberFormatHeader(string personId, RequestFilter filter)
        {
            var query = _context.NumberFormatHeaders
                     .AsQueryable();

            var filterDocumentId = filter.DocumentId?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDocumentId.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                foreach (var filterItem in filterDocumentId)
                {
                    predicate = predicate.Or(x => x.DocumentId.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterNumberFormat = filter.NumberFormat?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNumberFormat.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                foreach (var filterItem in filterNumberFormat)
                {
                    predicate = predicate.Or(x => x.NumberFormat.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                predicate = predicate.Or(x => x.InActive == filter.InActive);
                query = query.Where(predicate);
            }
            if (filter.IsMonthlyReset.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                predicate = predicate.Or(x => x.IsMonthlyReset == filter.IsMonthlyReset);
                query = query.Where(predicate);
            }
            if (filter.IsYearlyReset.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                predicate = predicate.Or(x => x.IsYearlyReset == filter.IsYearlyReset);
                query = query.Where(predicate);
            }


            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatHeader>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.NumberFormatHeader> getNumberFormatHeaderSorted(IQueryable<Entities.NumberFormatHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("DocumentId", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentId) : query.ThenBy(x => x.DocumentId);
                }

                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }

                if (item.Contains("NumberFormat", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.NumberFormat) : query.ThenBy(x => x.NumberFormat);
                }
                if (item.Contains("InActive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.InActive) : query.ThenBy(x => x.InActive);
                }
                if (item.Contains("IsMonthlyReset", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsMonthlyReset) : query.ThenBy(x => x.IsMonthlyReset);
                } 
                if (item.Contains("IsYearlyReset", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsYearlyReset) : query.ThenBy(x => x.IsYearlyReset);
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

            if (!sortingList.Any(x => x.Contains("NumberFormat", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.NumberFormat);
            }

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }

    }
}
