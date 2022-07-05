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

namespace FLOG_BE.Features.Finance.Sales.Quotation.GetQuotationAll
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

            var query = getQuotationHistory(request.Initiator.UserId, request.Filter);
            query = getSorted(query, request.Sort);

            var list = await PaginatedList<Entities.SalesQuotationHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                SalesQuotationHeader = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.SalesQuotationHeader> getQuotationHistory(string personId, RequestFilter filter)
        {
            List<Customer> ListCustomer = _context.Customers.ToList();
            List<ShippingLine> ListShippingLine = _context.ShippingLines.ToList();

            List<SalesPerson> ListPerson = _context.SalesPersons.ToList();
            List<Container> ListContainer = _context.Containers.ToList();
            List<Person> ListUser = _contextCentral.Persons.ToList();

            var query = (from x in _context.SalesQuotationHeaders
                         where x.Status != DOCSTATUS.CLOSE
                         select new Entities.SalesQuotationHeader
                         {
                             SalesQuotationId = x.SalesQuotationId,
                             TransactionType = x.TransactionType,
                             TransactionDate = x.TransactionDate,
                             RowId = x.RowId,
                             DocumentNo = x.DocumentNo,
                             CustomerId = x.CustomerId,
                             CustomerCode = ListCustomer.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerCode).FirstOrDefault(),
                             CustomerName = ListCustomer.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerName).FirstOrDefault(),
                             CustomerAddressCode = x.CustomerAddressCode,
                             SalesCode = x.SalesCode,
                             SalesPerson = ListPerson.Where(p => p.SalesCode == x.SalesCode).Select(p => p.SalesName).FirstOrDefault(),
                             ShipperId = x.ShipperId,
                             ShipperName = ListCustomer.Where(p => p.CustomerId == x.ShipperId).Select(p => p.CustomerName).FirstOrDefault() ?? String.Empty,
                             ShipperCode = ListCustomer.Where(p => p.CustomerId == x.ShipperId).Select(p => p.CustomerCode).FirstOrDefault() ?? String.Empty,
                             ShipperAddressCode = x.ShipperAddressCode,
                             ConsigneeId = x.ConsigneeId,
                             ConsigneeCode = ListCustomer.Where(p => p.CustomerId == x.ConsigneeId).Select(p => p.CustomerCode).FirstOrDefault() ?? String.Empty,
                             ConsigneeName = ListCustomer.Where(p => p.CustomerId == x.ConsigneeId).Select(p => p.CustomerName).FirstOrDefault() ?? String.Empty,
                             ConsigneeAddressCode = x.ConsigneeAddressCode,
                             IsDifferentNotifyPartner = x.IsDifferentNotifyPartner,
                             NotifyPartnerId = x.NotifyPartnerId,
                             NotifyPartnerAddressCode = x.NotifyPartnerAddressCode,
                             NotifyPartnerCode = ListCustomer.Where(p => p.CustomerId == x.NotifyPartnerId).Select(p => p.CustomerCode).FirstOrDefault() ?? String.Empty,
                             NotifyPartnerName = ListCustomer.Where(p => p.CustomerId == x.NotifyPartnerId).Select(p => p.CustomerName).FirstOrDefault() ?? String.Empty,
                             ShippingLineId = x.ShippingLineId,
                             IsShippingLineMaster = x.IsShippingLineMaster,
                             ShippingLineCode = x.ShippingLineCode,
                             ShippingLineVesselCode = x.ShippingLineVesselCode,
                             ShippingLineVesselName = x.ShippingLineVesselName,
                             ShippingLineName = x.IsShippingLineMaster ? ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.ShippingLineName).FirstOrDefault() : x.ShippingLineName,
                             ShippingLineOwner = x.IsShippingLineMaster ? ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.Vendor.VendorName).FirstOrDefault() ?? "" : "",
                             ShippingLineType = x.IsShippingLineMaster ? ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.ShippingLineType).FirstOrDefault() : "",
                             ShippingLineShippingNo = x.ShippingLineShippingNo,
                             ShippingLineDelivery = x.ShippingLineDelivery,
                             ShippingLineArrival = x.ShippingLineArrival,
                             FeederLineId = x.FeederLineId,
                             IsFeederLineMaster = x.IsFeederLineMaster,
                             FeederLineCode = x.FeederLineCode,
                             FeederLineVesselCode = x.FeederLineVesselCode,
                             FeederLineVesselName = x.FeederLineVesselName,
                             FeederLineName = x.IsFeederLineMaster ? ListShippingLine.Where(p => p.ShippingLineId == x.FeederLineId).Select(p => p.ShippingLineName).FirstOrDefault() ?? String.Empty : x.FeederLineName,
                             FeederLineOwner = x.IsFeederLineMaster ? ListShippingLine.Where(p => p.ShippingLineId == x.FeederLineId).Select(p => p.Vendor.VendorName).FirstOrDefault() ?? String.Empty : String.Empty,
                             FeederLineType = x.IsFeederLineMaster ? ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.ShippingLineType).FirstOrDefault() ?? String.Empty : String.Empty,
                             FeederLineShippingNo = x.FeederLineShippingNo,
                             FeederLineDelivery = x.FeederLineDelivery,
                             FeederLineArrival = x.FeederLineArrival,
                             TermOfShipment = x.TermOfShipment,
                             FinalDestination = x.FinalDestination,
                             PortOfLoading = x.PortOfLoading,
                             PortOfDischarge = x.PortOfDischarge,
                             Commodity = x.Commodity,
                             CargoGrossWeight = x.CargoGrossWeight,
                             CargoNetWeight = x.CargoNetWeight,
                             CargoDescription = x.CargoDescription,
                             Remark = x.Remark,
                             Status = x.Status,
                             StatusComment = x.StatusComment,
                             CreatedBy = x.CreatedBy,
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault() ?? String.Empty,
                             ModifiedDate = x.ModifiedDate,
                             TotalContainer = _context.QuotationDetails.Where(s => s.SalesQuotationId == x.SalesQuotationId).Select(s => s.Qty).Sum(),
                             SalesQuotationDetails = (from s in _context.QuotationDetails
                                                      where s.SalesQuotationId == x.SalesQuotationId
                                                      select new Entities.SalesQuotationDetail
                                                      {
                                                          SalesQuotationDetailId = s.SalesQuotationDetailId,
                                                          RowId = s.RowId,
                                                          SalesQuotationId = s.SalesQuotationId,
                                                          ContainerId = s.ContainerId,
                                                          ContainerCode = ListContainer.Where(p => p.ContainerId == s.ContainerId).Select(p => p.ContainerCode).FirstOrDefault(),
                                                          ContainerName = ListContainer.Where(p => p.ContainerId == s.ContainerId).Select(p => p.ContainerName).FirstOrDefault(),
                                                          Qty = s.Qty,
                                                          UomDetailId = s.UomDetailId,
                                                          Remark = s.Remark,
                                                          Status = s.Status,
                                                      }).OrderBy(p => p.RowId).ToList(),

                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.SalesQuotationHeader>(true);

            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterPerson = filter.SalesPerson?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPerson.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterPerson)
                {
                    predicate = predicate.Or(x => x.SalesPerson.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterName = filter.CustomerName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.CustomerName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterShipperName = filter.ShipperName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShipperName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterShipperName)
                {
                    predicate = predicate.Or(x => x.ShipperName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterShipperLineOwner = filter.ShippingLineOwner?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShipperLineOwner.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterShipperLineOwner)
                {
                    predicate = predicate.Or(x => x.ShippingLineOwner.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }


            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesQuotationHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.SalesQuotationHeader> getSorted(IQueryable<Entities.SalesQuotationHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.DocumentNo);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
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
