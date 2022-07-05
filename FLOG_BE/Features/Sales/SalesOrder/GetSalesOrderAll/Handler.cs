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

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.GetSalesOrderAll
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

            var query = getSalesData(request.Initiator.UserId, request.Filter);
            query = getSorted(query, request.Sort);

            var list = await PaginatedList<Entities.SalesOrderHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                SalesOrderHeader = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.SalesOrderHeader> getSalesData(string personId, RequestFilter filter)
        {
            List<Customer> ListCustomer = _context.Customers.ToList();
            List<Vendor> ListVendor = _context.Vendors.ToList();
            List<ShippingLine> ListShippingLine = _context.ShippingLines.ToList();

            List<SalesPerson> ListPerson = _context.SalesPersons.ToList();
            List<Container> ListContainer = _context.Containers.ToList();
            List<Person> ListUser = _contextCentral.Persons.ToList();
            List<Charges> ListCharges = _context.Charges.ToList();
            List<TaxSchedule> ListSchedule = _context.TaxSchedules.ToList();
            List<Currency> ListCurrency = _context.Currencies.ToList();

            var query = (from x in _context.SalesOrderHeaders
                          .Where(x => x.Status != DOCSTATUS.CANCEL)
                         select new Entities.SalesOrderHeader
                         {
                             SalesOrderId = x.SalesOrderId,
                             TransactionType = x.TransactionType,
                             TransactionDate = x.TransactionDate,
                             RowId = x.RowId,
                             DocumentNo = x.DocumentNo,
                             BranchCode = x.BranchCode ?? string.Empty,
                             QuotDocumentNo = x.QuotDocumentNo,
                             CustomerId = x.CustomerId,
                             CustomerCode = ListCustomer.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerCode).FirstOrDefault() ?? "",
                             CustomerName = ListCustomer.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerName).FirstOrDefault() ?? "",
                             CustomerAddressCode = x.CustomerAddressCode,
                             CustomerBillToAddressCode = x.CustomerBillToAddressCode,
                             CustomerShipToAddressCode = x.CustomerShipToAddressCode,
                             SalesCode = x.SalesCode,
                             MasterNo = x.MasterNo,
                             AgreementNo = x.AgreementNo,
                             BookingNo = x.BookingNo,
                             HouseNo = x.HouseNo,
                             NsNo = x.NsNo,
                             ShipmentStatus = x.ShipmentStatus,
                             SalesPerson = ListPerson.Where(p => p.SalesCode == x.SalesCode).Select(p => p.SalesName).FirstOrDefault(),
                             ShipperId = x.ShipperId,
                             ShipperName = ListCustomer.Where(p => p.CustomerId == x.ShipperId).Select(p => p.CustomerName).FirstOrDefault() ?? "",
                             ShipperCode = ListCustomer.Where(p => p.CustomerId == x.ShipperId).Select(p => p.CustomerCode).FirstOrDefault() ?? "",
                             ShipperAddressCode = x.ShipperAddressCode,
                             ShipperBillToAddressCode = x.ShipperBillToAddressCode,
                             ShipperShipToAddressCode = x.ShipperShipToAddressCode,
                             ConsigneeId = x.ConsigneeId,
                             ConsigneeCode = ListCustomer.Where(p => p.CustomerId == x.ConsigneeId).Select(p => p.CustomerCode).FirstOrDefault() ?? "",
                             ConsigneeName = ListCustomer.Where(p => p.CustomerId == x.ConsigneeId).Select(p => p.CustomerName).FirstOrDefault() ?? "",
                             ConsigneeAddressCode = x.ConsigneeAddressCode,
                             ConsigneeBillToAddressCode = x.ConsigneeBillToAddressCode,
                             ConsigneeShipToAddressCode = x.ConsigneeShipToAddressCode,
                             IsDifferentNotifyPartner = x.IsDifferentNotifyPartner,
                             NotifyPartnerId = x.NotifyPartnerId,
                             NotifyPartnerCode = ListCustomer.Where(p => p.CustomerId == x.NotifyPartnerId).Select(p => p.CustomerCode).FirstOrDefault() ?? "",
                             NotifyPartnerName = ListCustomer.Where(p => p.CustomerId == x.NotifyPartnerId).Select(p => p.CustomerName).FirstOrDefault() ?? "",
                             NotifyPartnerAddressCode = x.NotifyPartnerAddressCode,
                             NotifyPartnerBilltoAddressCode = x.NotifyPartnerBilltoAddressCode,
                             NotifyPartnerShipToAddressCode = x.NotifyPartnerShipToAddressCode,
                             ShippingLineId = x.ShippingLineId,
                             ShippingLineVesselCode = x.ShippingLineVesselCode,
                             ShippingLineVesselName = x.ShippingLineVesselName,
                             ShippingLineName = ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.ShippingLineName).FirstOrDefault() ?? "",
                             ShippingLineOwner = ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.Vendor.VendorName).FirstOrDefault() ?? "",
                             ShippingLineType = ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.ShippingLineType).FirstOrDefault() ?? "",
                             ShippingLineShippingNo = x.ShippingLineShippingNo,
                             ShippingLineDelivery = x.ShippingLineDelivery,
                             ShippingLineArrival = x.ShippingLineArrival,
                             FeederLineId = x.FeederLineId,
                             FeederLineVesselCode = x.FeederLineVesselCode,
                             FeederLineVesselName = x.FeederLineVesselName,
                             FeederLineName = ListShippingLine.Where(p => p.ShippingLineId == x.FeederLineId).Select(p => p.ShippingLineName).FirstOrDefault(),
                             FeederLineOwner = ListShippingLine.Where(p => p.ShippingLineId == x.FeederLineId).Select(p => p.Vendor.VendorName).FirstOrDefault(),
                             FeederLineType = ListShippingLine.Where(p => p.ShippingLineId == x.ShippingLineId).Select(p => p.ShippingLineType).FirstOrDefault(),
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
                             TotalFuncSelling = x.TotalFuncSelling,
                             TotalFuncBuying = x.TotalFuncBuying,
                             Remark = x.Remark,
                             Status = x.Status,
                             StatusComment = x.StatusComment,
                             CreatedBy = ListUser.Where(p => p.PersonId.Equals(x.CreatedBy.ToString(), StringComparison.OrdinalIgnoreCase)).Select(p => p.PersonFullName).FirstOrDefault() ?? "",
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             ModifiedDate = x.ModifiedDate,
                             TotalContainer = _context.SalesOrderContainers.Where(s => s.SalesOrderId == x.SalesOrderId).Select(s => s.Qty).Sum(),
                             FunctionalSellingAmount = _context.SalesOrderSellings.Where(s => s.SalesOrderId == x.SalesOrderId).Select(s => s.FunctionalExtendedAmount).Sum(),
                             FunctionalBuyingAmount = _context.SalesOrderBuyings.Where(s => s.SalesOrderId == x.SalesOrderId).Select(s => s.FunctionalExtendedAmount).Sum(),
                             EstimatedFunctionalAmount = (_context.SalesOrderSellings.Where(s => s.SalesOrderId == x.SalesOrderId).Select(s => s.FunctionalExtendedAmount).Sum()) - (_context.SalesOrderBuyings.Where(s => s.SalesOrderId == x.SalesOrderId).Select(s => s.FunctionalExtendedAmount).Sum()),
                             SalesOrderContainers = (from s in _context.SalesOrderContainers
                                                     where s.SalesOrderId == x.SalesOrderId
                                                     orderby s.RowId ascending
                                                     select new Entities.SalesOrderContainer
                                                     {
                                                         SalesOrderContainerId = s.SalesOrderContainerId,
                                                         RowId = s.RowId,
                                                         SalesOrderId = s.SalesOrderId,
                                                         ContainerId = s.ContainerId,
                                                         ContainerCode = ListContainer.Where(p => p.ContainerId == s.ContainerId).Select(p => p.ContainerCode).FirstOrDefault(),
                                                         ContainerName = ListContainer.Where(p => p.ContainerId == s.ContainerId).Select(p => p.ContainerName).FirstOrDefault(),
                                                         Qty = s.Qty,
                                                         UomDetailId = s.UomDetailId,
                                                         Remark = s.Remark,
                                                         Status = s.Status,
                                                     }).ToList(),
                             SalesOrderTruckings = (from s in _context.SalesOrderTruckings
                                                    join vhc in _context.VehicleTypes on s.VehicleTypeId equals vhc.VehicleTypeId
                                                    where s.SalesOrderId == x.SalesOrderId
                                                    select new Entities.SalesOrderTrucking
                                                    {
                                                        SalesOrderTruckingId = s.SalesOrderTruckingId,
                                                        RowId = s.RowId,
                                                        SalesOrderId = s.SalesOrderId,
                                                        VehicleTypeId = s.VehicleTypeId,
                                                        TruckloadTerm = s.TruckloadTerm,
                                                        VendorId = s.VendorId,
                                                        Qty = s.Qty,
                                                        UomDetailId = s.UomDetailId,
                                                        Remark = s.Remark,
                                                        VendorCode = ListVendor.Where(v => v.VendorId == s.VendorId).Select(v => v.VendorCode).FirstOrDefault() ?? "",
                                                        VendorName = ListVendor.Where(v => v.VendorId == s.VendorId).Select(v => v.VendorName).FirstOrDefault() ?? "",
                                                        VehicleTypeCode = vhc.VehicleTypeCode ?? "",
                                                        VehicleTypeName = vhc.VehicleTypeName ?? "",
                                                        Status = s.Status,
                                                    }).ToList(),
                             SalesOrderSellings = (from s in _context.SalesOrderSellings
                                                   where s.SalesOrderId == x.SalesOrderId
                                                   orderby s.RowId descending
                                                   select new Entities.SalesOrderSelling
                                                   {

                                                       SalesOrderSellingId = s.SalesOrderSellingId,
                                                       RowId = s.RowId,
                                                       SalesOrderId = s.SalesOrderId,
                                                       ChargeId = s.ChargeId,
                                                       ChargeCode = ListCharges.Where(p => p.ChargesId == s.ChargeId).Select(p => p.ChargesCode).FirstOrDefault(),
                                                       ChargeName = ListCharges.Where(p => p.ChargesId == s.ChargeId).Select(p => p.ChargesName).FirstOrDefault(),
                                                       ScheduleId = s.TaxScheduleId,
                                                       TaxScheduleId = s.TaxScheduleId,
                                                       ScheduleCode = ListSchedule.Where(p => p.TaxScheduleId == s.TaxScheduleId).Select(p => p.TaxScheduleCode).FirstOrDefault(),
                                                       PercentDiscount = ListSchedule.Where(p => p.TaxScheduleId == s.TaxScheduleId).Select(p => p.PercentOfSalesPurchase).FirstOrDefault(),
                                                       TaxablePercentTax = ListSchedule.Where(p => p.TaxScheduleId == s.TaxScheduleId).Select(p => p.TaxablePercent).FirstOrDefault(),
                                                       CurrencyCode = s.CurrencyCode,
                                                       IsMultiply = s.IsMultiply,
                                                       ExchangeRate = s.ExchangeRate,
                                                       OriginatingAmount = s.OriginatingAmount,
                                                       OriginatingTax = s.OriginatingTax,
                                                       OriginatingDiscount = s.OriginatingDiscount,
                                                       OriginatingExtendedAmount = s.OriginatingExtendedAmount,
                                                       FunctionalTax = s.FunctionalTax,
                                                       FunctionalDiscount = s.FunctionalDiscount,
                                                       FunctionalExtendedAmount = s.FunctionalExtendedAmount,
                                                       IsTaxAfterDiscount = s.IsTaxAfterDiscount,
                                                       DecimalPlaces = ListCurrency.Where(p => p.CurrencyCode == s.CurrencyCode).Select(p => p.DecimalPlaces).FirstOrDefault(),
                                                       PaymentCondition = s.PaymentCondition,
                                                       CustomerId = s.CustomerId,
                                                       ChargeTo = ListCustomer.Where(p => p.CustomerId == s.CustomerId).Select(p => p.CustomerCode).FirstOrDefault(),
                                                       Remark = s.Remark,
                                                       Status = s.Status,
                                                       UnitAmount = s.UnitAmount,
                                                       Quantity = s.Quantity,
                                                       SoBuyings = (from buy in _context.SalesOrderBuyings
                                                                        where buy.SalesOrderSellingId == s.SalesOrderSellingId
                                                                        orderby buy.RowId descending
                                                                        select new Entities.SalesOrderBuying
                                                                        {
                                                                            SalesOrderBuyingId = buy.SalesOrderBuyingId,
                                                                            RowId = buy.RowId,
                                                                            SalesOrderId = buy.SalesOrderId,
                                                                            SalesOrderSellingId = buy.SalesOrderSellingId,
                                                                            ChargeId = buy.ChargeId,
                                                                            ChargeCode = ListCharges.Where(p => p.ChargesId == buy.ChargeId).Select(p => p.ChargesCode).FirstOrDefault(),
                                                                            ChargeName = ListCharges.Where(p => p.ChargesId == buy.ChargeId).Select(p => p.ChargesName).FirstOrDefault(),
                                                                            ChargeTo = ListVendor.Where(p => p.VendorId == buy.VendorId).Select(p => p.VendorCode).FirstOrDefault(),
                                                                            ExchangeRate = buy.ExchangeRate,
                                                                            CurrencyCode = buy.CurrencyCode,
                                                                            IsMultiply = buy.IsMultiply,
                                                                            OriginatingAmount = buy.OriginatingAmount,
                                                                            OriginatingTax = buy.OriginatingTax,
                                                                            OriginatingDiscount = buy.OriginatingDiscount,
                                                                            OriginatingExtendedAmount = buy.OriginatingExtendedAmount,
                                                                            FunctionalTax = buy.FunctionalTax,
                                                                            FunctionalDiscount = buy.FunctionalDiscount,
                                                                            FunctionalExtendedAmount = buy.FunctionalExtendedAmount,
                                                                            TaxScheduleId = buy.TaxScheduleId,
                                                                            TaxablePercentTax = ListSchedule.Where(n => n.TaxScheduleId == buy.TaxScheduleId).Select(n => n.TaxablePercent).FirstOrDefault(),
                                                                            ScheduleId = buy.TaxScheduleId,
                                                                            IsTaxAfterDiscount = buy.IsTaxAfterDiscount,
                                                                            DecimalPlaces = ListCurrency.Where(p => p.CurrencyCode == buy.CurrencyCode).Select(p => p.DecimalPlaces).FirstOrDefault(),
                                                                            PercentDiscount = buy.PercentDiscount,
                                                                            PaymentCondition = buy.PaymentCondition,
                                                                            VendorId = buy.VendorId,
                                                                            Remark = buy.Remark,
                                                                            Status = buy.Status,
                                                                            UnitAmount = buy.UnitAmount,
                                                                            Quantity = buy.Quantity,
                                                                        }).ToList(),
                                                   }).ToList(),

                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.SalesOrderHeader>(true);

            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterTrxType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTrxType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterTrxType)
                {
                    predicate = predicate.Or(x => x.TransactionType.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterPerson = filter.SalesPerson?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPerson.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterPerson)
                {
                    predicate = predicate.Or(x => x.SalesPerson.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterName = filter.CustomerName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.CustomerName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterShipperName = filter.ShipperName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShipperName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterShipperName)
                {
                    predicate = predicate.Or(x => x.ShipperName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterShipperLineOwner = filter.ShippingLineOwner?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShipperLineOwner.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterShipperLineOwner)
                {
                    predicate = predicate.Or(x => x.ShippingLineOwner.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }


            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterMasterNo = filter.MasterNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterMasterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterMasterNo)
                {
                    predicate = predicate.Or(x => x.MasterNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterAgreementNo = filter.AgreementNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAgreementNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SalesOrderHeader>(false);
                foreach (var filterItem in filterAgreementNo)
                {
                    predicate = predicate.Or(x => x.AgreementNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.SalesOrderHeader> getSorted(IQueryable<Entities.SalesOrderHeader> input, List<string> sort)
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
