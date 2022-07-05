﻿using System;
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
using FLOG_BE.Model.Companies.View;

namespace FLOG_BE.Features.Finance.Payable.GetAdvancePayment
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
            query = getPayableSorted(query, request.Sort);

            var list = await PaginatedList<APAdvancePayment, APAdvancePayment>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            return ApiResult<Response>.Ok(new Response()
            {
                Advances = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<APAdvancePayment> getTransaction(string personId, RequestFilter filter)
        {
            var query = _context.APPendingAdvancePayments.Where(x => x.VendorId == filter.VendorId)
                .AsNoTracking().AsEnumerable().ToList().AsQueryable();
            
            var wherePredicates = PredicateBuilder.New<APAdvancePayment>(true);
                        
            if (!string.IsNullOrEmpty(filter.DocumentNo))
            {
                var predicate = PredicateBuilder.New<APAdvancePayment>(false);
                predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filter.DocumentNo.ToLower()));
                wherePredicates.And(predicate);
            }

            if (!string.IsNullOrEmpty(filter.CurrencyCode))
            {
                var predicate = PredicateBuilder.New<APAdvancePayment>(false);
                predicate = predicate.Or(x => x.CurrencyCode.ToLower().Contains(filter.CurrencyCode.ToLower()));
                wherePredicates.And(predicate);
            }

            if (!string.IsNullOrEmpty(filter.CheckbookCode))
            {
                var predicate = PredicateBuilder.New<APAdvancePayment>(false);
                predicate = predicate.Or(x => x.CheckbookCode.ToLower().Contains(filter.CheckbookCode.ToLower()));
                wherePredicates.And(predicate);
            }

            if (filter.TransactionDateStart.HasValue)
            {
                var predicate = PredicateBuilder.New<APAdvancePayment>(false);
                predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filter.TransactionDateStart);
                wherePredicates.And(predicate);
            }

            if (filter.TransactionDateEnd.HasValue)
            {
                var predicate = PredicateBuilder.New<APAdvancePayment>(false);
                predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filter.TransactionDateEnd);
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);
            
            return query;
        }

        private IQueryable<APAdvancePayment> getPayableSorted(IQueryable<APAdvancePayment> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.TransactionDate);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {

                if (item.Contains("VendorCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorCode) : query.ThenBy(x => x.VendorCode);
                }

                if (item.Contains("VendorName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorName) : query.ThenBy(x => x.VendorName);
                }

                if (item.Contains("TransactionDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionDate) : query.ThenBy(x => x.TransactionDate);
                }

                if (item.Contains("DocumentType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentType) : query.ThenBy(x => x.DocumentType);
                }

                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
                }

                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }
                
                if (item.Contains("CheckbookCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookCode) : query.ThenBy(x => x.CheckbookCode);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }
                if (item.Contains("ExchangeRate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRate) : query.ThenBy(x => x.ExchangeRate);
                }
                
                if (item.Contains("OriginatingAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingAmount) : query.ThenBy(x => x.OriginatingAmount);
                }
                if (item.Contains("OriginatingPaid", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingPaid) : query.ThenBy(x => x.OriginatingPaid);
                }
                if (item.Contains("OriginatingBalance", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingBalance) : query.ThenBy(x => x.OriginatingBalance);
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
