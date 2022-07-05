﻿using System;
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

namespace FLOG_BE.Features.Companies.Bank.GetBank
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getBank(request.Initiator.UserId, request.Filter);
            query = getBankSorted(query, request.Sort);

            var list = await PaginatedList<Entities.Bank, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                Banks = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Bank> getBank(string personId, RequestFilter filter)
        {
            var query = _context.Banks
                     .Include(x => x.Cities)
                     .AsQueryable();

            var filterBankCode = filter.BankCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterBankCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterBankCode)
                {
                    predicate = predicate.Or(x => x.BankCode.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterDb = filter.BankName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDb.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterDb)
                {
                    predicate = predicate.Or(x => x.BankName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterAddress = filter.Address?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAddress.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterAddress)
                {
                    predicate = predicate.Or(x => x.Address.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCity = filter.CityName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCity.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterCity)
                {
                    predicate = predicate.Or(x => x.Cities.CityName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                predicate = predicate.Or(x => x.InActive == filter.InActive);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Bank>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.Bank> getBankSorted(IQueryable<Entities.Bank> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("BankCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BankCode) : query.ThenBy(x => x.BankCode);
                }

                if (item.Contains("BankName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BankName) : query.ThenBy(x => x.BankName);
                }

                if (item.Contains("Address", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Address) : query.ThenBy(x => x.Address);
                }

                if (item.Contains("CityName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CityCode) : query.ThenBy(x => x.CityCode);
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

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.BankCode);
            }

            return query;
        }
    }
}
