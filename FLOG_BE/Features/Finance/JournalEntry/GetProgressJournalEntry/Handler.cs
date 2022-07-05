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

namespace FLOG_BE.Features.Finance.JournalEntry.GetProgressJournalEntry
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

            var query = GetTransactions(request.Initiator.UserId, request.Filter);
            query = GetTransactionSorted(query, request.Sort);

            var list = await PaginatedList<Entities.JournalEntryHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
                      
            return ApiResult<Response>.Ok(new Response()
            {
                JournalEntries = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.JournalEntryHeader> GetTransactions(string personId, RequestFilter filter)
        {
            List<Person> ListUser = _contextCentral.Persons.ToList();
                        
            var query = (from x in _context.JournalEntryHeaders
                         join curr in _context.Currencies on x.CurrencyCode equals curr.CurrencyCode
                         where x.Status == DOCSTATUS.NEW || x.Status == DOCSTATUS.PROCESS || x.Status == DOCSTATUS.REVISE || x.Status == DOCSTATUS.APPROVE
                         select new Entities.JournalEntryHeader
                         {
                             JournalEntryHeaderId = x.JournalEntryHeaderId,
                             DocumentNo = x.DocumentNo,
                             BranchCode = x.BranchCode ?? string.Empty,
                             TransactionDate = x.TransactionDate,
                             CurrencyCode = x.CurrencyCode,
                             DecimalPlaces = curr.DecimalPlaces,
                             ExchangeRate = x.ExchangeRate,
                             IsMultiply = x.IsMultiply,
                             SourceDocument = x.SourceDocument,
                             Description = x.Description,
                             OriginatingTotal = x.OriginatingTotal,
                             FunctionalTotal = x.FunctionalTotal,
                             CreatedBy = x.CreatedBy,
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             ModifiedDate = x.ModifiedDate,
                             Status = x.Status,
                             StatusComment = x.StatusComment

                         }).AsEnumerable().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.JournalEntryHeader>(true);
            
            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterBranch = filter.BranchCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterBranch.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterBranch)
                {
                    predicate = predicate.Or(x => x.BranchCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCurCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterCurCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterExcRateMin = filter.ExchangeRateMin?.Where(x => x > 0).ToList();
            if (filterExcRateMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterExcRateMin)
                {
                    predicate = predicate.Or(x => x.ExchangeRate >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterExcRateMax = filter.ExchangeRateMax?.Where(x => x > 0).ToList();
            if (filterExcRateMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterExcRateMax)
                {
                    predicate = predicate.Or(x => x.ExchangeRate <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterSourceDocument = filter.SourceDocument?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSourceDocument.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterSourceDocument)
                {
                    predicate = predicate.Or(x => x.SourceDocument.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }


            var filterOriginatingTotalMin = filter.OriginatingTotalMin?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterOriginatingTotalMin)
                {
                    predicate = predicate.Or(x => x.OriginatingTotal >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterOriginatingTotalMax = filter.OriginatingTotalMax?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterOriginatingTotalMax)
                {
                    predicate = predicate.Or(x => x.OriginatingTotal <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterFunctionalTotalMin = filter.FunctionalTotalMin?.Where(x => x > 0).ToList();
            if (filterFunctionalTotalMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterFunctionalTotalMin)
                {
                    predicate = predicate.Or(x => x.FunctionalTotal >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterFunctionalTotalMax = filter.FunctionalTotalMax?.Where(x => x > 0).ToList();
            if (filterFunctionalTotalMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterFunctionalTotalMax)
                {
                    predicate = predicate.Or(x => x.FunctionalTotal <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedByName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedByName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.JournalEntryHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }
        
            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.JournalEntryHeader> GetTransactionSorted(IQueryable<Entities.JournalEntryHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.DocumentNo);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
                }

                if (item.Contains("TransactionDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionDate) : query.ThenBy(x => x.TransactionDate);
                }

                if (item.Contains("BranchCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BranchCode) : query.ThenBy(x => x.BranchCode);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }
                if (item.Contains("ExchangeRate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRate) : query.ThenBy(x => x.ExchangeRate);
                }
                if (item.Contains("SourceDocument", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SourceDocument) : query.ThenBy(x => x.SourceDocument);
                }

                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("OriginatingTotal", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingTotal) : query.ThenBy(x => x.OriginatingTotal);
                }
                if (item.Contains("FunctionalTotal", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.FunctionalTotal) : query.ThenBy(x => x.FunctionalTotal);
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
                
                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
                }

            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.DocumentNo).ThenBy(x => x.TransactionDate);
            }

            return query;
        }
    }
}
