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
using AutoMapper;

namespace FLOG_BE.Features.Companies.Customer.GetCustomer
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IMapper _mapper;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _mapper = mapper;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = GetCustomers(request.Filter);
            query = GetCustomerSorted(query, request.Sort);
            
            var list = await PaginatedList<Entities.Customer, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                Customers = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Customer> GetCustomers(RequestFilter filter)
        {
            var query = (from ct in _context.Customers
                         select new Entities.Customer
                         {
                             CustomerId = ct.CustomerId,
                             CustomerCode = ct.CustomerCode,
                             CustomerName = ct.CustomerName,
                             AddressCode = ct.AddressCode,
                             TaxRegistrationNo = ct.TaxRegistrationNo,
                             CustomerTaxName = ct.CustomerTaxName,
                             CustomerGroupCode = ct.CustomerGroupCode,
                             PaymentTermCode = ct.PaymentTermCode,
                             HasCreditLimit = ct.HasCreditLimit,
                             CreditLimit = ct.CreditLimit,
                             ShipToAddressCode = ct.ShipToAddressCode,
                             BillToAddressCode = ct.BillToAddressCode,
                             TaxAddressCode = ct.TaxAddressCode,
                             ReceivableAccountNo = ct.ReceivableAccountNo,
                             AccruedReceivableAccountNo = ct.AccruedReceivableAccountNo,
                             Inactive = ct.Inactive,
                             CreatedBy = ct.CreatedBy,
                             CreatedDate = ct.CreatedDate,
                             ModifiedBy = ct.ModifiedBy,
                             ModifiedDate = ct.ModifiedDate,
                             Status = ct.Status,
                             CustomerAddress = (from myaddr in _context.CustomerAddresses
                                     where myaddr.AddressCode == ct.AddressCode && myaddr.CustomerId == ct.CustomerId
                                     select myaddr).FirstOrDefault()
                         }).AsEnumerable().ToList().AsQueryable();

            if (filter.CustomerId != null && filter.CustomerId != Guid.Empty)
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                    predicate = predicate.Or(x => x.CustomerId == filter.CustomerId);
                query = query.Where(predicate);
            }
             var filterCustCode = filter.CustomerCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterCustCode)
                {
                    predicate = predicate.Or(x => x.CustomerCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCustName = filter.CustomerName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterCustName)
                {
                    predicate = predicate.Or(x => x.CustomerName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterAddressCode = filter.AddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterAddressCode)
                {
                    predicate = predicate.Or(x => x.AddressCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
              var filterTaxRegistrationNo = filter.TaxRegistrationNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxRegistrationNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterTaxRegistrationNo)
                {
                    predicate = predicate.Or(x => x.TaxRegistrationNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCustomerTaxName = filter.CustomerTaxName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustomerTaxName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterCustomerTaxName)
                {
                    predicate = predicate.Or(x => x.CustomerTaxName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCustomerGroupCode = filter.CustomerGroupCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustomerTaxName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterCustomerGroupCode)
                {
                    predicate = predicate.Or(x => x.CustomerGroupCode.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterPaymentTermCode = filter.PaymentTermCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaymentTermCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterPaymentTermCode)
                {
                    predicate = predicate.Or(x => x.PaymentTermCode.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            if (filter.HasCreditLimit.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                predicate = predicate.Or(x => x.HasCreditLimit == filter.HasCreditLimit);
                query = query.Where(predicate);
            }

            var FilterCreditLimit = filter.CreditLimit?.Where(x => x.HasValue).ToList();
            if (FilterCreditLimit.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in FilterCreditLimit)
                {
                    predicate = predicate.Or(x => x.CreditLimit == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterShipToAddressCode = filter.ShipToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShipToAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterShipToAddressCode)
                {
                    predicate = predicate.Or(x => x.ShipToAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }
            
            var filterBillToAddressCode = filter.BillToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterBillToAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterBillToAddressCode)
                {
                    predicate = predicate.Or(x => x.BillToAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterTaxAddressCode = filter.TaxAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterTaxAddressCode)
                {
                    predicate = predicate.Or(x => x.TaxAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterReceivableAccountNo = filter.ReceivableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterReceivableAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterReceivableAccountNo)
                {
                    predicate = predicate.Or(x => x.ReceivableAccountNo.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterAccruedReceivableAccountNo = filter.AccruedReceivableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAccruedReceivableAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterAccruedReceivableAccountNo)
                {
                    predicate = predicate.Or(x => x.AccruedReceivableAccountNo.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                predicate = predicate.Or(x => x.Inactive == filter.Inactive);
                query = query.Where(predicate);
            }

            var FilterStatus = filter.Status?.Where(x => x.HasValue).ToList();
            if (FilterStatus.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in FilterStatus)
                {
                    predicate = predicate.Or(x => x.Status == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Customer>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.Customer> GetCustomerSorted(IQueryable<Entities.Customer> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {

                if (item.Contains("CustomerCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerCode) : query.ThenBy(x => x.CustomerCode);
                }
                if (item.Contains("CustomerName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerName) : query.ThenBy(x => x.CustomerName);
                }
                if (item.Contains("AddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AddressCode) : query.ThenBy(x => x.AddressCode);
                }
                if (item.Contains("TaxRegistrationNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxRegistrationNo) : query.ThenBy(x => x.TaxRegistrationNo);
                }
                if (item.Contains("CustomerTaxName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerTaxName) : query.ThenBy(x => x.CustomerTaxName);
                }
                if (item.Contains("CustomerGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerGroupCode) : query.ThenBy(x => x.CustomerGroupCode);
                }
                if (item.Contains("PaymentTermCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PaymentTermCode) : query.ThenBy(x => x.PaymentTermCode);
                }
                if (item.Contains("HasCreditLimit", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.HasCreditLimit) : query.ThenBy(x => x.HasCreditLimit);
                }  
                if (item.Contains("CreditLimit", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreditLimit) : query.ThenBy(x => x.CreditLimit);
                }
                if (item.Contains("ShipToAddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShipToAddressCode) : query.ThenBy(x => x.ShipToAddressCode);
                }
                if (item.Contains("BillToAddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BillToAddressCode) : query.ThenBy(x => x.BillToAddressCode);
                }
                if (item.Contains("TaxAddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxAddressCode) : query.ThenBy(x => x.TaxAddressCode);
                }
                if (item.Contains("ReceivableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReceivableAccountNo) : query.ThenBy(x => x.ReceivableAccountNo);
                }
                if (item.Contains("ReceivableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReceivableAccountNo) : query.ThenBy(x => x.ReceivableAccountNo);
                }
                if (item.Contains("AccruedReceivableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AccruedReceivableAccountNo) : query.ThenBy(x => x.AccruedReceivableAccountNo);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
                }
                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
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
                query = query.ThenBy(x => x.CustomerCode);
            }

            return query;
        }

        private async Task<string> getContactPerson(string AddressCode)
        {
            return await _context.CustomerAddresses.Where(x => x.AddressCode == AddressCode).Select( x => x.ContactPerson).FirstOrDefaultAsync();
        }
        private async Task<string> getAddress(string AddressCode)
        {
            return await _context.CustomerAddresses.Where(x => x.AddressCode == AddressCode).Select(x => x.Address).FirstOrDefaultAsync();
        }
    }
}
