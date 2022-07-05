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
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Companies;
using FLOG.Core;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementCategory
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext contextCentral, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getTransactions(request.Initiator.UserId, request.Filter);
            query = getSorted(query, request.Sort);

            var list = await PaginatedList<Entities.GLStatementCategory, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            return ApiResult<Response>.Ok(new Response()
            {
                Categories = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.GLStatementCategory> getTransactions(string personId, RequestFilter filter)
        {
            var subCtgs = _context.GLStatementSubCategories.OrderBy(o=>o.CategoryId).ToList();

            var query = (from x in _context.GLStatementCategories
                         select new Entities.GLStatementCategory { 
                            StatementType = x.StatementType,
                            CategoryId = x.CategoryId,
                            CategoryKey = x.CategoryKey,
                            CategoryCaption = x.CategoryCaption,
                            SubCategories = subCtgs.Where(c=>c.CategoryId==x.CategoryId).ToList()
                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.GLStatementCategory>(true);
            
            var filterCategoryKey = filter.CategoryKey?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCategoryKey.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementCategory>(false);
                foreach (var filterItem in filterCategoryKey)
                {
                    predicate = predicate.Or(x => x.CategoryKey.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCategoryCaption = filter.CategoryCaption?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCategoryCaption.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementCategory>(false);
                foreach (var filterItem in filterCategoryCaption)
                {
                    predicate = predicate.Or(x => x.CategoryCaption.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterStatementType = filter.StatementType?.Where(x => x > 0).ToList();
            if (filterStatementType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementCategory>(false);
                foreach (var filterItem in filterStatementType)
                {
                    predicate = predicate.Or(x => x.StatementType == filterItem);
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.GLStatementCategory> getSorted(IQueryable<Entities.GLStatementCategory> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.StatementType);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("StatementType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.StatementType) : query.ThenBy(x => x.StatementType);
                }

                if (item.Contains("CategoryKey", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CategoryKey) : query.ThenBy(x => x.CategoryKey);
                }

                if (item.Contains("CategoryCaption", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CategoryCaption) : query.ThenBy(x => x.CategoryCaption);
                }
                                
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.StatementType).ThenBy(x => x.CategoryKey);
            }

            return query;
        }
    }
}
