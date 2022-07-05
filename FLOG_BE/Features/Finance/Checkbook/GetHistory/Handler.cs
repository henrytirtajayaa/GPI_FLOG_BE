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

namespace FLOG_BE.Features.Finance.Checkbook.GetHistory
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

            var query = getTransaction(request.Initiator.UserId, request.Filter);
            query = getTransactionSorted(query, request.Sort);

            var list = await PaginatedList<Entities.CheckbookTransactionHeader, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<Person> ListUser = await GetUser();
            List<ReponseItem> response;

            List<Entities.Checkbook> ListCheckbook = await _context.Checkbooks.ToListAsync();

            response = new List<ReponseItem>(list.Select(x => new ReponseItem
            {
                CheckbookTransactionId = x.CheckbookTransactionId,
                DocumentType = x.DocumentType,
                DocumentNo = x.DocumentNo,
                BranchCode = x.BranchCode ?? string.Empty,
                TransactionDate = x.TransactionDate,
                TransactionType = x.TransactionType,
                CurrencyCode = x.CurrencyCode,
                ExchangeRate = x.ExchangeRate,
                CheckbookCode = x.CheckbookCode,
                IsVoid = x.IsVoid,
                VoidDocumentNo = x.VoidDocumentNo,
                PaidSubject = x.PaidSubject,
                SubjectCode = x.SubjectCode,
                Description = x.Description,
                OriginatingTotalAmount = x.OriginatingTotalAmount,
                FunctionalTotalAmount = x.FunctionalTotalAmount,
                Status = x.Status,
                StatusComment = x.StatusComment,
                VoidBy = x.VoidBy,
                VoidDate = x.VoidDate,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate,
                CreatedName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                VoidByName = ListUser.Where(p => p.PersonId == x.VoidBy).Select(p => p.PersonFullName).FirstOrDefault(),
                BankAccountCode = ListCheckbook.Where(c => c.CheckbookCode == x.CheckbookCode).Select(c => c.BankAccountCode).FirstOrDefault(),
                ApprovalCode = ListCheckbook.Where(c => c.CheckbookCode == x.CheckbookCode).Select(c => c.ApprovalCode).FirstOrDefault()
            }));
            return ApiResult<Response>.Ok(new Response()
            {
                Checkbooks = response,
                ListInfo = list.ListInfo
            });
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }

        private IQueryable<Entities.CheckbookTransactionHeader> getTransaction(string personId, RequestFilter filter)
        {

            List<Person> ListUser = _contextCentral.Persons.ToList();

            var query = _context.CheckbookTransactionHeaders
                 .Where(x => x.Status != DOCSTATUS.NEW && x.Status != DOCSTATUS.PROCESS && x.Status != DOCSTATUS.APPROVE && x.Status != DOCSTATUS.DISAPPROVE && x.Status != DOCSTATUS.INACTIVE)
                 .AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(true);

            var filterDocType = filter.DocumentType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDocType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterDocType)
                {
                    predicate = predicate.Or(x => x.DocumentType.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterTransType)
                {
                    predicate = predicate.Or(x => x.TransactionType.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterCurCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterCurCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterExcRateMin = filter.ExchangeRateMin?.Where(x => x > 0).ToList();
            if (filterExcRateMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterExcRateMin)
                {
                    predicate = predicate.Or(x => x.ExchangeRate >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterExcRateMax = filter.ExchangeRateMax?.Where(x => x > 0).ToList();
            if (filterExcRateMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterExcRateMax)
                {
                    predicate = predicate.Or(x => x.ExchangeRate <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCheckbookCode = filter.CheckbookCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterCheckbookCode)
                {
                    predicate = predicate.Or(x => x.CheckbookCode.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            if (filter.IsVoid.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                predicate = predicate.Or(x => x.IsVoid == filter.IsVoid);
                wherePredicates.And(predicate);
            }

            var filterVoidDocumentNo = filter.VoidDocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVoidDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterVoidDocumentNo)
                {
                    predicate = predicate.Or(x => x.VoidDocumentNo.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterPaidSubject = filter.PaidSubject?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaidSubject.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterPaidSubject)
                {
                    predicate = predicate.Or(x => x.PaidSubject.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterSubjectCode = filter.SubjectCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSubjectCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterSubjectCode)
                {
                    predicate = predicate.Or(x => x.SubjectCode.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }


            var filterOriginatingTotalAmountMax = filter.OriginatingTotalAmountMax?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterOriginatingTotalAmountMax)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalAmount <= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterOriginatingTotalAmountMin = filter.OriginatingTotalAmountMin?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterOriginatingTotalAmountMin)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalAmount >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterFunctionalTotalAmountMax = filter.FunctionalTotalAmountMax?.Where(x => x > 0).ToList();
            if (filterFunctionalTotalAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterFunctionalTotalAmountMax)
                {
                    predicate = predicate.Or(x => x.FunctionalTotalAmount <= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterFunctionalTotalAmountMin = filter.FunctionalTotalAmountMin?.Where(x => x > 0).ToList();
            if (filterFunctionalTotalAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterFunctionalTotalAmountMin)
                {
                    predicate = predicate.Or(x => x.FunctionalTotalAmount >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterVoidBy = filter.VoidBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVoidBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterVoidBy)
                {
                    predicate = predicate.Or(x => x.VoidBy.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterVoidStart = filter.VoidDateStart?.Where(x => x.HasValue).ToList();
            if (filterVoidStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterVoidStart)
                {
                    predicate = predicate.Or(x => x.VoidDate.HasValue && ((DateTime)x.VoidDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterVoidEnd = filter.VoidDateEnd?.Where(x => x.HasValue).ToList();
            if (filterVoidEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterVoidEnd)
                {
                    predicate = predicate.Or(x => x.VoidDate.HasValue && ((DateTime)x.VoidDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CheckbookTransactionHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.CheckbookTransactionHeader> getTransactionSorted(IQueryable<Entities.CheckbookTransactionHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {

                if (item.Contains("DocumentType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookCode) : query.ThenBy(x => x.DocumentType);
                }

                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
                }

                if (item.Contains("TransactionDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionDate) : query.ThenBy(x => x.TransactionDate);
                }

                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }
                if (item.Contains("ExchangeRate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRate) : query.ThenBy(x => x.ExchangeRate);
                }
                if (item.Contains("CheckbookCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookCode) : query.ThenBy(x => x.CheckbookCode);
                }

                if (item.Contains("IsVoid", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsVoid) : query.ThenBy(x => x.IsVoid);
                }

                if (item.Contains("VoidDocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VoidDocumentNo) : query.ThenBy(x => x.VoidDocumentNo);
                }

                if (item.Contains("PaidSubject", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PaidSubject) : query.ThenBy(x => x.PaidSubject);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("SubjectCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SubjectCode) : query.ThenBy(x => x.SubjectCode);
                }
                if (item.Contains("OriginatingTotalAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingTotalAmount) : query.ThenBy(x => x.OriginatingTotalAmount);
                }

                if (item.Contains("FunctionalTotalAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.FunctionalTotalAmount) : query.ThenBy(x => x.FunctionalTotalAmount);
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
                if (item.Contains("VoidBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VoidBy) : query.ThenBy(x => x.VoidBy);
                }
                if (item.Contains("VoidDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VoidDate) : query.ThenBy(x => x.VoidDate);
                }

                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
                }

                if (item.Contains("StatusComment", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.StatusComment) : query.ThenBy(x => x.StatusComment);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.DocumentNo);
            }

            return query;
        }
    }
}
