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
using AutoMapper;

namespace FLOG_BE.Features.Companies.Vendor.GetVendor
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

            var query = getVendor(request.Filter);
            query = getVendorSorted(query, request.Sort);


            var list = await PaginatedList<Entities.Vendor, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                Vendors = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Vendor> getVendor(RequestFilter filter)
        {
            var query = (from ct in _context.Vendors
                         select new Entities.Vendor
                         {
                             VendorId = ct.VendorId,
                             VendorCode = ct.VendorCode,
                             VendorName = ct.VendorName,
                             AddressCode = ct.AddressCode,
                             TaxRegistrationNo = ct.TaxRegistrationNo,
                             VendorTaxName = ct.VendorTaxName,
                             VendorGroupCode = ct.VendorGroupCode,
                             PaymentTermCode = ct.PaymentTermCode,
                             HasCreditLimit = ct.HasCreditLimit,
                             CreditLimit = ct.CreditLimit,
                             ShipToAddressCode = ct.ShipToAddressCode,
                             BillToAddressCode = ct.BillToAddressCode,
                             TaxAddressCode = ct.TaxAddressCode,
                             PayableAccountNo = ct.PayableAccountNo,
                             AccruedPayableAccountNo = ct.AccruedPayableAccountNo,
                             Inactive = ct.Inactive,
                             CreatedBy = ct.CreatedBy,
                             CreatedDate = ct.CreatedDate, 
                             ModifiedBy = ct.ModifiedBy,
                             ModifiedDate = ct.ModifiedDate, 
                             VendorAddress = (from myaddr in _context.VendorAddresses
                                              where myaddr.AddressCode == ct.AddressCode && myaddr.VendorId == ct.VendorId
                                              select myaddr).FirstOrDefault()
                         }).AsEnumerable().ToList().AsQueryable();

            var filterVendorId = filter.VendorId?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendorId.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterVendorId)
                {
                    predicate = predicate.Or(x => x.VendorId.ToString().Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCode = filter.VendorCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.VendorCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.VendorName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.VendorName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterAddressCode = filter.AddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterAddressCode)
                {
                    predicate = predicate.Or(x => x.AddressCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTax = filter.TaxRegistrationNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterTax)
                {
                    predicate = predicate.Or(x => x.TaxRegistrationNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTaxName = filter.VendorTaxName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterTaxName)
                {
                    predicate = predicate.Or(x => x.VendorTaxName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterGroupCode = filter.VendorGroupCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterGroupCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterGroupCode)
                {
                    predicate = predicate.Or(x => x.VendorGroupCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPaymentTerm = filter.PaymentTermCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaymentTerm.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterPaymentTerm)
                {
                    predicate = predicate.Or(x => x.PaymentTermCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.HasCreditLimit.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                    predicate = predicate.Or(x => x.HasCreditLimit == filter.HasCreditLimit);
                query = query.Where(predicate);
            }

      
            var filterCreditLimit = filter.CreditLimitMin?.Where(x => x.HasValue).ToList();
            if (filterCreditLimit.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterCreditLimit)
                {
                    predicate = predicate.Or(x => x.CreditLimit >= filterItem);
                }
                query = query.Where(predicate);
            }  
            var filterCreditLimitMax = filter.CreditLimitMax?.Where(x => x.HasValue).ToList();
            if (filterCreditLimit.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterCreditLimitMax)
                {
                    predicate = predicate.Or(x => x.CreditLimit <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterShip = filter.ShipToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShip.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterShip)
                {
                    predicate = predicate.Or(x => x.ShipToAddressCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterBill = filter.BillToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterBill.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterBill)
                {
                    predicate = predicate.Or(x => x.BillToAddressCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTaxAddressCode = filter.TaxAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterTaxAddressCode)
                {
                    predicate = predicate.Or(x => x.TaxAddressCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPayable = filter.PayableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPayable.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterPayable)
                {
                    predicate = predicate.Or(x => x.PayableAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterAccrued = filter.AccruedPayableAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAccrued.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
                foreach (var filterItem in filterAccrued)
                {
                    predicate = predicate.Or(x => x.AccruedPayableAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Vendor>(true);
               
                predicate = predicate.Or(x => x.Inactive == filter.Inactive);
               
                query = query.Where(predicate);
            }



            return query;
        }

        private IQueryable<Entities.Vendor> getVendorSorted(IQueryable<Entities.Vendor> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

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
                if (item.Contains("AddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AddressCode) : query.ThenBy(x => x.AddressCode);
                } 
                if (item.Contains("TaxRegistrationNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxRegistrationNo) : query.ThenBy(x => x.TaxRegistrationNo);
                }
                if (item.Contains("VendorTaxName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorTaxName) : query.ThenBy(x => x.VendorTaxName);
                }
                if (item.Contains("VendorGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorGroupCode) : query.ThenBy(x => x.VendorGroupCode);
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
                if (item.Contains("PayableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PayableAccountNo) : query.ThenBy(x => x.PayableAccountNo);
                }
                if (item.Contains("AccruedPayableAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AccruedPayableAccountNo) : query.ThenBy(x => x.AccruedPayableAccountNo);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
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

        private async Task<string> getContactPerson(string AddressCode)
        {
            return await _context.VendorAddresses.Where(x => x.AddressCode == AddressCode).Select(x => x.ContactPerson).FirstOrDefaultAsync();
        }

        private async Task<string> getAddress(string AddressCode)
        {
            return await _context.VendorAddresses.Where(x => x.AddressCode == AddressCode).Select(x => x.Address).FirstOrDefaultAsync();
        }
    }
}
