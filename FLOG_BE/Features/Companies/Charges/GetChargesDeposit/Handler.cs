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
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Companies.Charges.GetChargesDeposit
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

            var query = getCharges(request.Filter);
            query = getChargesSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();
            List<Entities.ChargeGroup> ListGroup = await GetGroup();
            var list = await PaginatedList<Entities.Charges, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> responseCharges;

            responseCharges = new List<ResponseItem>(list.Select(x => new ResponseItem
            {

                ChargesId = x.ChargesId,
                ChargesCode = x.ChargesCode,
                ChargeGroupCode = x.ChargeGroupCode,
                ChargeGroupName = ListGroup.Where(p => p.ChargeGroupCode == x.ChargeGroupCode).Select(p => p.ChargeGroupName).FirstOrDefault(),
                ChargesName = x.ChargesName,
                TransactionType = x.TransactionType,
                IsPurchasing = x.IsPurchasing,
                IsSales = x.IsPurchasing,
                IsInventory = x.IsInventory,
                IsFinancial = x.IsFinancial,
                IsAsset = x.IsAsset,
                IsDeposit = x.IsDeposit,
                RevenueAccountNo = x.RevenueAccountNo,
                TempRevenueAccountNo = x.TempRevenueAccountNo,
                CostAccountNo = x.CostAccountNo,
                TaxScheduleCode = x.TaxScheduleCode,
                ShippingLineType = x.ShippingLineType,
                HasCosting = x.HasCosting,
                InActive = x.InActive,
                CreatedBy = x.CreatedBy,
                CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
                ChargesDetails = (from d in _context.ChargesDetails
                                  join sl in _context.ShippingLines on d.ShippingLineId equals sl.ShippingLineId
                                  where d.ChargesId.ToString().Equals(x.ChargesId, StringComparison.OrdinalIgnoreCase) 
                                  orderby sl.ShippingLineName ascending
                                  select new ResponseChargesDetail
                                  {
                                      ChargesDetailId = d.ChargesDetailId,
                                      ShippingLineId = d.ShippingLineId,
                                      RevenueAccountNo = d.RevenueAccountNo,
                                      TempRevenueAccountNo = d.TempRevenueAccountNo,
                                      CostAccountNo = d.CostAccountNo,
                                      ShippingLineCode = sl.ShippingLineCode,
                                      ShippingLineName = sl.ShippingLineName
                                  }).ToList()
            }));

            return ApiResult<Response>.Ok(new Response()
            {
                Charges = responseCharges,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Charges> getCharges(RequestFilter filter)
        {
            var query = _context.Charges.AsQueryable().Where(x => x.IsDeposit == true);

            var filterCode = filter.ChargesCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.ChargesCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.ChargesName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.ChargesName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTransactionType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterTransactionType)
                {
                    predicate = predicate.Or(x => x.TransactionType.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.IsPurchasing.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                predicate = predicate.Or(x => x.IsPurchasing == filter.IsPurchasing);
                query = query.Where(predicate);
            }
            if (filter.IsSales.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                predicate = predicate.Or(x => x.IsSales == filter.IsSales);
                query = query.Where(predicate);
            }
            if (filter.IsInventory.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                predicate = predicate.Or(x => x.IsInventory == filter.IsInventory);
                query = query.Where(predicate);
            }
            if (filter.IsFinancial.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                predicate = predicate.Or(x => x.IsFinancial == filter.IsFinancial);
                query = query.Where(predicate);
            }
            if (filter.IsAsset.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                predicate = predicate.Or(x => x.IsAsset == filter.IsAsset);
                query = query.Where(predicate);
            }

            var filterRevenueAccountNo = filter.RevenueAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterRevenueAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterRevenueAccountNo)
                {
                    predicate = predicate.Or(x => x.RevenueAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTempRevenueAccountNo = filter.TempRevenueAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTempRevenueAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterTempRevenueAccountNo)
                {
                    predicate = predicate.Or(x => x.TempRevenueAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCostAccountNo = filter.CostAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCostAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterCostAccountNo)
                {
                    predicate = predicate.Or(x => x.CostAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTaxScheduleCode = filter.TaxScheduleCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxScheduleCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterTaxScheduleCode)
                {
                    predicate = predicate.Or(x => x.TaxScheduleCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterShippingLineType = filter.ShippingLineType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShippingLineType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterShippingLineType)
                {
                    predicate = predicate.Or(x => x.ShippingLineType.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.HasCosting.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                predicate = predicate.Or(x => x.HasCosting == filter.InActive);
                query = query.Where(predicate);
            }

            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                predicate = predicate.Or(x => x.InActive == filter.InActive);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Charges>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
            }

            return query;
        }

        private IQueryable<Entities.Charges> getChargesSorted(IQueryable<Entities.Charges> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("ChargesCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ChargesCode) : query.ThenBy(x => x.ChargesCode);
                }
                if (item.Contains("ChargesName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ChargesName) : query.ThenBy(x => x.ChargesName);
                }
                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }
                if (item.Contains("ChargeGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ChargeGroupCode) : query.ThenBy(x => x.ChargeGroupCode);
                }
                if (item.Contains("IsPurchasing", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsPurchasing) : query.ThenBy(x => x.IsPurchasing);
                }
                if (item.Contains("IsSales", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsSales) : query.ThenBy(x => x.IsSales);
                }
                if (item.Contains("IsSales", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsInventory) : query.ThenBy(x => x.IsInventory);
                }
                if (item.Contains("IsFinancial", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsFinancial) : query.ThenBy(x => x.IsFinancial);
                }
                if (item.Contains("IsAsset", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsAsset) : query.ThenBy(x => x.IsAsset);
                }
                if (item.Contains("RevenueAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RevenueAccountNo) : query.ThenBy(x => x.RevenueAccountNo);
                }
                if (item.Contains("TempRevenueAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TempRevenueAccountNo) : query.ThenBy(x => x.TempRevenueAccountNo);
                }
                if (item.Contains("CostAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CostAccountNo) : query.ThenBy(x => x.CostAccountNo);
                }
                if (item.Contains("TaxScheduleCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxScheduleCode) : query.ThenBy(x => x.TaxScheduleCode);
                }
                if (item.Contains("ShippingLineType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShippingLineType) : query.ThenBy(x => x.ShippingLineType);
                }
                if (item.Contains("HasCosting", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.HasCosting) : query.ThenBy(x => x.HasCosting);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
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

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
        private async Task<List<Entities.ChargeGroup>> GetGroup()
        {
            return await _context.ChargeGroups.ToListAsync();
        }
    }
}
