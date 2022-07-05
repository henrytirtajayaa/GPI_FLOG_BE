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
using FLOG.Core.Finance;

namespace FLOG_BE.Features.Finance.BankReconcile.GetProgressBankReconcile
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private FinanceCatalog _financeCatalog;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext contextCentral, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
            _financeCatalog = new FinanceCatalog(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getTransactions(request.Initiator.UserId, request.Filter);
            query = getSorted(query, request.Sort);

            var list = await PaginatedList<Entities.BankReconcileHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            return ApiResult<Response>.Ok(new Response()
            {
                Reconciles = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.BankReconcileHeader> getTransactions(string personId, RequestFilter filter)
        {
            List<Person> ListUser = _contextCentral.Persons.ToList();
            List<BankReconcileDetail> reconcileDetails = _context.BankReconcileDetails.ToList();
            List<BankReconcileHeader> reconcileHistory = _context.BankReconcileHeaders.Where(x => x.Status == DOCSTATUS.POST || x.Status == DOCSTATUS.VOID).ToList();
            
            var query = (from x in _context.BankReconcileHeaders
                          .Where(x => x.Status != DOCSTATUS.DELETE && x.Status != DOCSTATUS.VOID && x.Status != DOCSTATUS.CLOSE && x.Status != DOCSTATUS.POST)
                         select new Entities.BankReconcileHeader
                         {
                             BankReconcileId = x.BankReconcileId,
                             TransactionDate = x.TransactionDate,
                             DocumentNo = x.DocumentNo,
                             CurrencyCode = x.CurrencyCode,
                             CheckbookCode = x.CheckbookCode,
                             BankCutoffStart = x.BankCutoffStart,
                             BankCutoffEnd = x.BankCutoffEnd,
                             Description = x.Description,
                             BankEndingOrgBalance = x.BankEndingOrgBalance,
                             CheckbookEndingOrgBalance = x.CheckbookEndingOrgBalance,
                             BalanceDifference = (x.BankEndingOrgBalance - x.CheckbookEndingOrgBalance),
                             CreatedBy = x.CreatedBy,
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             VoidBy = x.VoidBy,
                             VoidByName = ListUser.Where(p => p.PersonId == x.VoidBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             VoidDate = x.VoidDate,
                             Status = x.Status,
                             StatusComment = x.StatusComment,
                             AllowVoid = false,
                             PrevBankReconcileId = x.PrevBankReconcileId,
                             PrevCheckbookBalance = (reconcileHistory.Where(r=>r.BankReconcileId==x.PrevBankReconcileId).Select(r=>r.BankEndingOrgBalance).FirstOrDefault()),
                             PrevReconcileDocNo = reconcileHistory.Where(r => r.BankReconcileId == x.PrevBankReconcileId).Select(r => r.DocumentNo).FirstOrDefault() ?? string.Empty,
                             ReconcileDetails = new List<BankReconcileDetail>(),
                            ReconcileAdjustments = (from adj in _context.BankReconcileAdjustments 
                                                      join ch in _context.Charges on adj.ChargesId equals ch.ChargesId
                                                      where adj.BankReconcileId == x.BankReconcileId
                                                      select new Entities.BankReconcileAdjustment
                                                      {
                                                          BankReconcileAdjustmentId = adj.BankReconcileAdjustmentId,
                                                          BankReconcileId = adj.BankReconcileId,
                                                          CheckbookTransactionId = adj.CheckbookTransactionId,
                                                          TransactionDetailId = adj.TransactionDetailId,
                                                          ChargesId = adj.ChargesId,
                                                          DocumentType = adj.DocumentType,
                                                          TransactionDate = adj.TransactionDate,
                                                          ChargesCode = ch.ChargesCode,
                                                          ChargesDescription = ch.ChargesName,
                                                          CurrencyCode = adj.CurrencyCode,
                                                          ExchangeRate = adj.ExchangeRate,
                                                          IsMultiply = adj.IsMultiply,
                                                          Description = adj.Description,
                                                          PaidSubject = adj.PaidSubject,
                                                          TransactionType = adj.TransactionType,
                                                          OriginatingAmount = adj.OriginatingAmount,
                                                          FunctionalAmount = adj.FunctionalAmount,
                                                          Status = adj.Status
                                                      }).ToList()
                         }).AsEnumerable().ToList().AsQueryable();

            foreach(var x in query)
            {
                x.ReconcileDetails = (from ba in _financeCatalog.GetBankActivities(x.CheckbookCode, x.BankCutoffEnd.Date, (Guid)x.BankReconcileId)
                                      where ba.TransactionDate.Date >= x.BankCutoffStart.Date && ba.TransactionDate <= x.BankCutoffEnd.Date
                                      orderby ba.TransactionDate ascending
                                      select new Entities.BankReconcileDetail
                                      {
                                          TransactionId = ba.TransactionId,
                                          Modul = ba.Modul,
                                          DocumentNo = ba.DocumentNo,
                                          DocumentType = ba.DocumentType,
                                          TransactionType = ba.TransactionType,
                                          PaidSubject = ba.SubjectCode,
                                          TransactionDate = ba.TransactionDate,
                                          OriginatingAmount = ba.OriginatingTotalAmount,
                                          IsChecked = ba.IsChecked,
                                      }).ToList();
            }

            var wherePredicates = PredicateBuilder.New<Entities.BankReconcileHeader>(true);
            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterDocumentNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterDocumentNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCurrencyCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurrencyCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterCurrencyCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterBankCutoffEndStart = filter.BankCutoffEndStart?.Where(x => x.HasValue).ToList();
            if (filterBankCutoffEndStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterBankCutoffEndStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.BankCutoffEnd).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterBankCutoffEndEnd = filter.BankCutoffEndEnd?.Where(x => x.HasValue).ToList();
            if (filterBankCutoffEndEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterBankCutoffEndEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.BankCutoffEnd).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCheckbookCode = filter.CheckbookCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterCheckbookCode)
                {
                    predicate = predicate.Or(x => x.CheckbookCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterBankEndingOrgBalanceMin = filter.BankEndingOrgBalanceMin?.Where(x => x > 0).ToList();
            if (filterBankEndingOrgBalanceMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterBankEndingOrgBalanceMin)
                {
                    predicate = predicate.Or(x => x.BankEndingOrgBalance >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterBankEndingOrgBalanceMax = filter.BankEndingOrgBalanceMax?.Where(x => x > 0).ToList();
            if (filterBankEndingOrgBalanceMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterBankEndingOrgBalanceMax)
                {
                    predicate = predicate.Or(x => x.BankEndingOrgBalance <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCheckbookEndingOrgBalanceMin = filter.CheckbookEndingOrgBalanceMin?.Where(x => x > 0).ToList();
            if (filterCheckbookEndingOrgBalanceMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterCheckbookEndingOrgBalanceMin)
                {
                    predicate = predicate.Or(x => x.CheckbookEndingOrgBalance >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterCheckbookEndingOrgBalanceMax = filter.CheckbookEndingOrgBalanceMax?.Where(x => x > 0).ToList();
            if (filterCheckbookEndingOrgBalanceMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterCheckbookEndingOrgBalanceMax)
                {
                    predicate = predicate.Or(x => x.CheckbookEndingOrgBalance <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.BankReconcileHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.BankReconcileHeader> getSorted(IQueryable<Entities.BankReconcileHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.DocumentNo);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("TransactionDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionDate) : query.ThenBy(x => x.TransactionDate);
                }

                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
                }

                if (item.Contains("CheckbookCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookCode) : query.ThenBy(x => x.CheckbookCode);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }

                if (item.Contains("BankCutoffStart", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BankCutoffStart) : query.ThenBy(x => x.BankCutoffStart);
                }

                if (item.Contains("BankCutoffEnd", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BankCutoffEnd) : query.ThenBy(x => x.BankCutoffEnd);
                }

                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }

                if (item.Contains("BankEndingOrgBalance", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BankEndingOrgBalance) : query.ThenBy(x => x.BankEndingOrgBalance);
                }

                if (item.Contains("CheckbookEndingOrgBalance", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookEndingOrgBalance) : query.ThenBy(x => x.CheckbookEndingOrgBalance);
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
