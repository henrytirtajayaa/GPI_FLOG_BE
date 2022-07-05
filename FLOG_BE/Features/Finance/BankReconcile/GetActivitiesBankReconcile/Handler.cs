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
using ViewEntities = FLOG_BE.Model.Companies.View;

namespace FLOG_BE.Features.Finance.BankReconcile.GetActivitiesBankReconcile
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

            var list = await PaginatedList<ResponseItem, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            return ApiResult<Response>.Ok(new Response()
            {
                CheckbookActivities = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<ResponseItem> getTransactions(string personId, RequestFilter filter)
        {
            var activities = _financeCatalog.GetBankActivities(filter.CheckbookCode, (DateTime) filter.BankCutoffEnd, (Guid)filter.BankReconcileId);

            var query = (from x in activities
                         where x.Status == DOCSTATUS.POST orderby x.TransactionDate ascending
                         select new ResponseItem
                         {
                             TransactionId = x.TransactionId,
                             Modul = x.Modul,
                             DocumentType = x.DocumentType,
                             DocumentNo = x.DocumentNo,
                             TransactionDate = x.TransactionDate,
                             TransactionType = x.TransactionType,
                             CurrencyCode = x.CurrencyCode,
                             ExchangeRate = x.ExchangeRate,
                             CheckbookCode = x.CheckbookCode,
                             PaidSubject = x.SubjectCode,
                             SubjectCode = x.SubjectCode,
                             Description = x.Description,
                             OriginatingTotalAmount = x.OriginatingTotalAmount,
                             BankAccountCode = x.BankAccountCode,
                             OriginatingDebit = x.OriginatingDebit,
                             OriginatingCredit = x.OriginatingCredit,
                             IsChecked = x.IsChecked,                             
                         }).AsEnumerable().AsQueryable();

            return query;
        }

        private IQueryable<ResponseItem> getSorted(IQueryable<ResponseItem> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.TransactionDate);

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

                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }

                if (item.Contains("ExchangeRate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRate) : query.ThenBy(x => x.ExchangeRate);
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
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.TransactionDate).ThenBy(x => x.DocumentNo);
            }

            return query;
        }
    }
}
