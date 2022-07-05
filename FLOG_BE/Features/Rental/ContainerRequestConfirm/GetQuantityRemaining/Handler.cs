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

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.GetQuantityRemaining
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

            var query = getContainerRentalRequest(request.Initiator.UserId, request.Filter);
            query = getContainerRentalRequestSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();
            List<Customer> ListCustomer = await GetCustomer();

            var list = await PaginatedList<Entities.ContainerRentalRequestHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                ContainerRentalRequest = list,
                ListInfo = list.ListInfo
            });
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
        private async Task<List<Customer>> GetCustomer()
        {
            return await _context.Customers.ToListAsync();
        }

        private IQueryable<Entities.ContainerRentalRequestHeader> getContainerRentalRequest(string personId, RequestFilter filter)
        {
            List<Person> ListUser = _contextCentral.Persons.ToList();
            List<Customer> litdata = _context.Customers.ToList();
            List<Vendor> litvendor = _context.Vendors.ToList();

            var query = (from x in _context.ContainerRentalRequestHeaders
                         where x.Status != DOCSTATUS.CLOSE && x.Status != DOCSTATUS.CANCEL && x.Status != DOCSTATUS.NEW && x.Status != DOCSTATUS.PROCESS && x.Status != DOCSTATUS.OPEN && x.Status != DOCSTATUS.APPROVE && x.Status != DOCSTATUS.DISAPPROVE
                         select new Entities.ContainerRentalRequestHeader
                         {
                             ContainerRentalRequestHeaderId = x.ContainerRentalRequestHeaderId,
                             TransactionType = x.TransactionType,
                             DocumentDate = x.DocumentDate,
                             DocumentNo = x.DocumentNo,
                             CustomerId = x.CustomerId,
                             CustomerCode = litdata.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerCode).FirstOrDefault(),
                             CustomerName = litdata.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerName).FirstOrDefault(),
                             AddressCode = x.AddressCode,
                             Status = x.Status,
                             SalesCode = x.SalesCode,
                             VendorId = x.VendorId,
                             VendorCode = litvendor.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorCode).FirstOrDefault(),
                             VendorName = litvendor.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorName).FirstOrDefault(),
                             BillToAddressCode = x.BillToAddressCode,
                             ShipToAddressCode = x.ShipToAddressCode,
                             CreatedBy = x.CreatedBy,
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             ModifiedDate = x.ModifiedDate,
                             CanceledBy = x.CanceledBy,
                             CanceledByName = ListUser.Where(p => p.PersonId == x.CanceledBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CanceledDate = x.CanceledDate,
                             ContainerRentalRequestDetails = (from s in _context.ContainerConfirmQuantities
                                                              where s.ContainerRentalRequestHeaderId == x.ContainerRentalRequestHeaderId
                                                              && s.QuantityRemaining > 0
                                                              select new Entities.ContainerRentalRequestDetail
                                                              {
                                                                  ContainerRentalRequestDetailId = s.ContainerRentalRequestDetailId,
                                                                  ContainerRentalRequestHeaderId = s.ContainerRentalRequestHeaderId,
                                                                  ContainerCode = s.ContainerCode,
                                                                  ContainerName = s.ContainerName,
                                                                  UomCode = s.UomCode,
                                                                  //Quantity = (from cf in _context.ContainerRequestConfirmDetails
                                                                  //            where cf.ContainerRequestConfirmDetailId == s.ContainerRequestConfirmDetailId && 
                                                                  //            cf.ContainerRequestConfirmHeaderId != s.ContainerRequestConfirmHeaderId
                                                                  //            select new { Balance = cf.QuantityBalance }).Sum(c=>c.Balance)
                                                                  Quantity = s.QuantityRemaining
                                                              }).ToList(),
                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(true);

            var filterTransactionType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterTransactionType)
                {
                    predicate = predicate.Or(x => x.TransactionType.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDocumentNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterTransactionType)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDocDateStart = filter.DocumentDateStart?.Where(x => x.HasValue).ToList();
            if (filterDocDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterDocDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.DocumentDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterDocDateEnd = filter.DocumentDateEnd?.Where(x => x.HasValue).ToList();
            if (filterDocDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterDocDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.DocumentDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCustomerCode = filter.CustomerCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustomerCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCustomerCode)
                {
                    predicate = predicate.Or(x => x.CustomerCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCustomerName = filter.CustomerName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustomerName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCustomerName)
                {
                    predicate = predicate.Or(x => x.CustomerName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterAddressCode = filter.AddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterAddressCode)
                {
                    predicate = predicate.Or(x => x.AddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterSalesCode = filter.SalesCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSalesCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterSalesCode)
                {
                    predicate = predicate.Or(x => x.SalesCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterContainerOwnerName = filter.VendorName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterContainerOwnerName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterContainerOwnerName)
                {
                    predicate = predicate.Or(x => x.VendorName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterBillToAddressCode = filter.BillToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterBillToAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterBillToAddressCode)
                {
                    predicate = predicate.Or(x => x.BillToAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterShipToAddressCode = filter.ShipToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShipToAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterShipToAddressCode)
                {
                    predicate = predicate.Or(x => x.ShipToAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCanceledBy = filter.CanceledBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCanceledBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCanceledBy)
                {
                    predicate = predicate.Or(x => x.CanceledBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCanceledDateStart = filter.CanceledDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCanceledDateStart)
                {
                    predicate = predicate.Or(x => x.CanceledDate.HasValue && ((DateTime)x.CanceledDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCanceledDateEnd = filter.CanceledDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCanceledDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ContainerRentalRequestHeader>(false);
                foreach (var filterItem in filterCanceledDateEnd)
                {
                    predicate = predicate.Or(x => x.CanceledDate.HasValue && ((DateTime)x.CanceledDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.ContainerRentalRequestHeader> getContainerRentalRequestSorted(IQueryable<Entities.ContainerRentalRequestHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.DocumentDate);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }

                if (item.Contains("DocumentDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentDate) : query.ThenBy(x => x.DocumentDate);
                }

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

                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
                }

                if (item.Contains("SalesCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SalesCode) : query.ThenBy(x => x.SalesCode);
                }

                if (item.Contains("ContainerOwnerName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorName) : query.ThenBy(x => x.VendorName);
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

                if (item.Contains("CanceledBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CanceledBy) : query.ThenBy(x => x.CanceledBy);
                }

                if (item.Contains("CanceledDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CanceledDate) : query.ThenBy(x => x.CanceledDate);
                }
            }
            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.DocumentDate).ThenBy(x => x.CustomerCode);
            }

            return query;
        }
    }
}
