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

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementDetail
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

            var list = await PaginatedList<Entities.GLStatementDetail, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            return ApiResult<Response>.Ok(new Response()
            {
                Details = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.GLStatementDetail> getTransactions(string personId, RequestFilter filter)
        {
            var query = (from x in _context.GLStatementDetails
                         join sub in _context.GLStatementSubCategories on x.SubCategoryId equals sub.SubCategoryId
                         join c in _context.GLStatementCategories on sub.CategoryId equals c.CategoryId
                         where c.StatementType == filter.StatementType
                         select new Entities.GLStatementDetail
                         { 
                            DetailId = x.DetailId,
                            CategoryId = c.CategoryId,
                            SubCategoryId = x.SubCategoryId,
                            AccountName = x.AccountName,
                            IsCashflow = x.IsCashflow,
                            IsCashflowDynamic = x.IsCashflowDynamic,
                            ShowZeroValue = x.ShowZeroValue,
                            PosIndex = x.PosIndex,
                            SubCategoryKey = sub.SubCategoryKey,
                            SubCategoryCaption = sub.SubCategoryCaption,
                            CategoryKey = c.CategoryKey,
                            CategoryCaption = c.CategoryCaption,                            
                            DetailAccounts = (from sub in _context.GLStatementDetailSubs
                                              join coa in _context.Accounts on sub.AccountId equals coa.AccountId
                                              where sub.DetailId == x.DetailId
                                              select coa).ToList()
                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.GLStatementDetail>(true);

            var filterAccountName = filter.AccountName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAccountName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementDetail>(false);
                foreach (var filterItem in filterAccountName)
                {
                    predicate = predicate.Or(x => x.AccountName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterSubCategoryKey = filter.SubCategoryKey?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSubCategoryKey.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementDetail>(false);
                foreach (var filterItem in filterSubCategoryKey)
                {
                    predicate = predicate.Or(x => x.SubCategoryKey.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterSubCategoryCaption = filter.SubCategoryCaption?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSubCategoryCaption.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementDetail>(false);
                foreach (var filterItem in filterSubCategoryCaption)
                {
                    predicate = predicate.Or(x => x.SubCategoryCaption.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCategoryKey = filter.SubCategoryKey?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCategoryKey.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementDetail>(false);
                foreach (var filterItem in filterCategoryKey)
                {
                    predicate = predicate.Or(x => x.CategoryKey.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCategoryCaption = filter.SubCategoryCaption?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCategoryCaption.Any())
            {
                var predicate = PredicateBuilder.New<Entities.GLStatementDetail>(false);
                foreach (var filterItem in filterCategoryCaption)
                {
                    predicate = predicate.Or(x => x.CategoryCaption.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.GLStatementDetail> getSorted(IQueryable<Entities.GLStatementDetail> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.PosIndex);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("AccountName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AccountName) : query.ThenBy(x => x.AccountName);
                }

                if (item.Contains("SubCategoryKey", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SubCategoryKey) : query.ThenBy(x => x.SubCategoryKey);
                }

                if (item.Contains("SubCategoryCaption", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SubCategoryCaption) : query.ThenBy(x => x.SubCategoryCaption);
                }

                if (item.Contains("CategoryKey", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CategoryKey) : query.ThenBy(x => x.CategoryKey);
                }

                if (item.Contains("CategoryCaption", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CategoryCaption) : query.ThenBy(x => x.CategoryCaption);
                }

                if (item.Contains("PosIndex", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PosIndex) : query.ThenBy(x => x.PosIndex);
                }

                if (item.Contains("ShowZeroValue", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShowZeroValue) : query.ThenBy(x => x.ShowZeroValue);
                }

            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.PosIndex).ThenBy(x => x.AccountName);
            }

            return query;
        }
    }
}
