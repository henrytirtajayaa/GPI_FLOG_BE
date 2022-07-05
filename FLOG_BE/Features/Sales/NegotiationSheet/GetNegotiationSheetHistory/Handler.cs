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

namespace FLOG_BE.Features.Sales.NegotiationSheet.GetNegotiationSheetHistory
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

            var list = await PaginatedList<Entities.NegotiationSheetHeader, Entities.NegotiationSheetHeader>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                NegotiationSheetHeaders = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.NegotiationSheetHeader> getSalesData(string personId, RequestFilter filter)
        {
            List<Customer> listCustomer = _context.Customers.ToList();
            List<Vendor> listVendor = _context.Vendors.ToList();
            List<ShippingLine> listShippingLine = _context.ShippingLines.ToList();

            List<Person> listUser = _contextCentral.Persons.ToList();
            List<TaxSchedule> listTaxSchedule = _context.TaxSchedules.ToList();

            var query = (from ng in _context.NegotiationSheetHeaders
                         join cust in _context.Customers on ng.CustomerId equals cust.CustomerId
                         where ng.Status != DOCSTATUS.NEW && ng.Status != DOCSTATUS.PROCESS && ng.Status != DOCSTATUS.REVISE
                         select new Entities.NegotiationSheetHeader
                         {
                             NegotiationSheetId = ng.NegotiationSheetId,
                             TransactionDate = ng.TransactionDate,
                             TransactionType = ng.TransactionType,
                             BranchCode = ng.BranchCode ?? string.Empty,
                             DocumentNo = ng.DocumentNo,
                             SalesOrderId = ng.SalesOrderId,
                             SoDocumentNo = _context.SalesOrderHeaders.Where(so => so.SalesOrderId.Equals(ng.SalesOrderId)).Select(so => so.DocumentNo).FirstOrDefault() ?? "",
                             Remark = ng.Remark,
                             Status = ng.Status,
                             TotalFuncBuying = ng.TotalFuncBuying,
                             TotalFuncSelling = ng.TotalFuncSelling,
                             TotalFuncProfit = ng.TotalFuncSelling - ng.TotalFuncBuying,
                             CreatedBy = listUser.Where(x => x.PersonId.Equals(ng.CreatedBy.ToString(), StringComparison.OrdinalIgnoreCase)).Select(p => p.PersonFullName).FirstOrDefault() ?? "",
                             CreatedDate = ng.CreatedDate,
                             CustomerId = ng.CustomerId,
                             CustomerCode = cust.CustomerCode,
                             CustomerName = cust.CustomerName,
                             CustomerAddressCode = ng.CustomerAddressCode,
                             CustomerBillToAddressCode = ng.CustomerBillToAddressCode,
                             CustomerShipToAddressCode = ng.CustomerShipToAddressCode,
                             QuotDocumentNo = ng.QuotDocumentNo,
                             SalesCode = ng.SalesCode,
                             SalesName = _context.SalesPersons.Where(sp => sp.SalesCode == ng.SalesCode).Select(sp => sp.SalesName).FirstOrDefault() ?? string.Empty,
                             ShipmentStatus = ng.ShipmentStatus,
                             MasterNo = ng.MasterNo,
                             AgreementNo = ng.AgreementNo,
                             BookingNo = ng.BookingNo,
                             HouseNo = ng.HouseNo,
                             ShipperId = ng.ShipperId,
                             ShipperCode = _context.Customers.Where(shipper => shipper.CustomerId.Equals(ng.ShipperId)).Select(sp => sp.CustomerCode).FirstOrDefault() ?? string.Empty,
                             ShipperName = _context.Customers.Where(shipper => shipper.CustomerId.Equals(ng.ShipperId)).Select(sp => sp.CustomerName).FirstOrDefault() ?? string.Empty,
                             ShipperAddressCode = ng.ShipperAddressCode,
                             ShipperBillToAddressCode = ng.ShipperBillToAddressCode,
                             ShipperShipToAddressCode = ng.ShipperShipToAddressCode,
                             ConsigneeId = ng.ConsigneeId,
                             ConsigneeCode = _context.Customers.Where(consignee => consignee.CustomerId.Equals(ng.ConsigneeId)).Select(sp => sp.CustomerCode).FirstOrDefault() ?? string.Empty,
                             ConsigneeName = _context.Customers.Where(consignee => consignee.CustomerId.Equals(ng.ConsigneeId)).Select(sp => sp.CustomerName).FirstOrDefault() ?? string.Empty,
                             ConsigneeAddressCode = ng.ConsigneeAddressCode,
                             ConsigneeBillToAddressCode = ng.ConsigneeBillToAddressCode,
                             ConsigneeShipToAddressCode = ng.ConsigneeShipToAddressCode,
                             IsDifferentNotifyPartner = ng.IsDifferentNotifyPartner,
                             NotifyPartnerId = ng.NotifyPartnerId,
                             NotifyPartnerCode = _context.Customers.Where(np => np.CustomerId.Equals(ng.NotifyPartnerId)).Select(sp => sp.CustomerCode).FirstOrDefault() ?? string.Empty,
                             NotifyPartnerName = _context.Customers.Where(np => np.CustomerId.Equals(ng.NotifyPartnerId)).Select(sp => sp.CustomerName).FirstOrDefault() ?? string.Empty,
                             NotifyAddressCode = ng.NotifyAddressCode,
                             NotifyBillToAddressCode = ng.NotifyBillToAddressCode,
                             NotifyShipToAddressCode = ng.NotifyShipToAddressCode,
                             ShippingLineId = ng.ShippingLineId,
                             IsShippingLineMaster = ng.IsShippingLineMaster,
                             ShippingLineCode = listShippingLine.Where(sl => sl.ShippingLineId == ng.ShippingLineId).Select(v => v.ShippingLineCode).FirstOrDefault() ?? "",
                             ShippingLineName = listShippingLine.Where(sl => sl.ShippingLineId == ng.ShippingLineId).Select(v => v.ShippingLineName).FirstOrDefault() ?? "",
                             ShippingLineOwner = listVendor.Where(vd => vd.VendorId == listShippingLine.Where(sl => sl.ShippingLineId == ng.ShippingLineId).Select(v => v.VendorId).FirstOrDefault()).Select(vd => vd.VendorCode).FirstOrDefault() ?? string.Empty,
                             ShippingLineType = listShippingLine.Where(sl => sl.ShippingLineId == ng.ShippingLineId).Select(v => v.ShippingLineType).FirstOrDefault() ?? "",
                             ShippingLineShippingNo = ng.ShippingLineShippingNo,
                             ShippingLineETD = ng.ShippingLineETD,
                             ShippingLineETA = ng.ShippingLineETA,
                             ShippingLineVesselCode = ng.ShippingLineVesselCode,
                             ShippingLineVesselName = ng.ShippingLineVesselName,
                             TermOfShipment = ng.TermOfShipment,
                             FinalDestination = ng.FinalDestination,
                             PortOfLoading = ng.PortOfLoading,
                             PortOfDischarge = ng.PortOfDischarge,
                             Commodity = ng.Commodity,
                             CargoGrossWeight = ng.CargoGrossWeight,
                             CargoNetWeight = ng.CargoNetWeight,
                             CargoDescription = ng.CargoDescription,
                             NsContainers = (from cont in _context.NegotiationSheetContainers
                                             join mscontainer in _context.Containers on cont.ContainerId equals mscontainer.ContainerId
                                             where cont.NegotiationSheetId == ng.NegotiationSheetId
                                             orderby cont.RowIndex ascending
                                             select new Entities.NegotiationSheetContainer
                                             {
                                                 NSContainerId = cont.NSContainerId,
                                                 NegotiationSheetId = cont.NegotiationSheetId,
                                                 ContainerId = cont.ContainerId,
                                                 ContainerCode = mscontainer.ContainerCode ?? "",
                                                 ContainerName = mscontainer.ContainerName ?? "",
                                                 Qty = cont.Qty,
                                                 UomDetailId = cont.UomDetailId,
                                                 Remark = cont.Remark,
                                                 RowId = cont.RowId,
                                                 Status = cont.Status
                                             }).ToList(),
                             NsTruckings = (from truck in _context.NegotiationSheetTruckings
                                            join vhc in _context.VehicleTypes on truck.VehicleTypeId equals vhc.VehicleTypeId
                                            where truck.NegotiationSheetId == ng.NegotiationSheetId
                                            orderby truck.RowIndex ascending
                                            select new NegotiationSheetTrucking
                                            {
                                                NsTruckingId = truck.NsTruckingId,
                                                NegotiationSheetId = truck.NegotiationSheetId,
                                                RowId = truck.RowId,
                                                Qty = truck.Qty,
                                                TruckloadTerm = truck.TruckloadTerm,
                                                VehicleTypeId = truck.VehicleTypeId,
                                                UomDetailId = truck.UomDetailId,
                                                Remark = truck.Remark,
                                                VendorId = truck.VendorId,
                                                Status = truck.Status,
                                                VehicleTypeCode = vhc.VehicleTypeCode ?? "",
                                                VehicleTypeName = vhc.VehicleTypeName ?? "",
                                            }).ToList(),
                             NsSellings = (from sell in _context.NegotiationSheetSellings
                                           join ch in _context.Charges on sell.ChargeId equals ch.ChargesId
                                           join curr in _context.Currencies on sell.CurrencyCode equals curr.CurrencyCode
                                           where sell.NegotiationSheetId == ng.NegotiationSheetId
                                           orderby sell.RowIndex ascending
                                           select new NegotiationSheetSelling
                                           {
                                               NsSellingId = sell.NsSellingId,
                                               NegotiationSheetId = sell.NegotiationSheetId,
                                               ChargeId = sell.ChargeId,
                                               ChargeCode = ch.ChargesCode ?? "",
                                               ChargeName = ch.ChargesName,
                                               RowId = sell.RowId,
                                               TaxScheduleId = sell.TaxScheduleId,
                                               ScheduleCode = listTaxSchedule.Where(p => p.TaxScheduleId == sell.TaxScheduleId).Select(p => p.TaxScheduleCode).FirstOrDefault(),
                                               PercentDiscount = listTaxSchedule.Where(p => p.TaxScheduleId == sell.TaxScheduleId).Select(p => p.PercentOfSalesPurchase).FirstOrDefault(),
                                               TaxablePercentTax = listTaxSchedule.Where(p => p.TaxScheduleId == sell.TaxScheduleId).Select(p => p.TaxablePercent).FirstOrDefault(),
                                               CurrencyCode = sell.CurrencyCode,
                                               IsMultiply = sell.IsMultiply,
                                               ExchangeRate = sell.ExchangeRate,
                                               OriginatingAmount = sell.OriginatingAmount,
                                               OriginatingTax = sell.OriginatingTax,
                                               OriginatingDiscount = sell.OriginatingDiscount,
                                               OriginatingExtendedAmount = sell.OriginatingExtendedAmount,
                                               FunctionalTax = sell.FunctionalTax,
                                               FunctionalDiscount = sell.FunctionalDiscount,
                                               FunctionalExtendedAmount = sell.FunctionalExtendedAmount,
                                               IsTaxAfterDiscount = sell.IsTaxAfterDiscount,
                                               DecimalPlaces = curr.DecimalPlaces,
                                               PaymentCondition = sell.PaymentCondition,
                                               CustomerId = sell.CustomerId,
                                               ChargeTo = listCustomer.Where(p => p.CustomerId == sell.CustomerId).Select(p => p.CustomerCode).FirstOrDefault() ?? "",
                                               Remark = sell.Remark,
                                               Status = sell.Status,
                                               UnitAmount = sell.UnitAmount,
                                               Quantity = sell.Quantity,
                                               NsBuyings = (from buy in _context.NegotiationSheetBuyings
                                                            join buyCharge in _context.Charges on buy.ChargeId equals buyCharge.ChargesId
                                                            join curr2 in _context.Currencies on buy.CurrencyCode equals curr2.CurrencyCode
                                                            where buy.NsSellingId == sell.NsSellingId && buy.NegotiationSheetId == sell.NegotiationSheetId
                                                            orderby buy.RowIndex ascending
                                                            select new Entities.NegotiationSheetBuying
                                                            {
                                                                NsBuyingId = buy.NsBuyingId,
                                                                RowId = buy.RowId,
                                                                NegotiationSheetId = buy.NegotiationSheetId,
                                                                NsSellingId = buy.NsSellingId,
                                                                ChargeId = buy.ChargeId,
                                                                ChargeCode = buyCharge.ChargesCode,
                                                                ChargeName = buyCharge.ChargesName,
                                                                ChargeTo = listVendor.Where(p => p.VendorId == buy.VendorId).Select(p => p.VendorCode).FirstOrDefault() ?? "",
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
                                                                ScheduleCode = listTaxSchedule.Where(n => n.TaxScheduleId == buy.TaxScheduleId).Select(n => n.TaxScheduleCode).FirstOrDefault(),
                                                                TaxablePercentTax = listTaxSchedule.Where(n => n.TaxScheduleId == buy.TaxScheduleId).Select(n => n.TaxablePercent).FirstOrDefault(),
                                                                IsTaxAfterDiscount = buy.IsTaxAfterDiscount,
                                                                DecimalPlaces = curr2.DecimalPlaces,
                                                                PercentDiscount = buy.PercentDiscount,
                                                                PaymentCondition = buy.PaymentCondition,
                                                                VendorId = buy.VendorId,
                                                                Remark = buy.Remark,
                                                                Status = buy.Status,
                                                                UnitAmount = buy.UnitAmount,
                                                                Quantity = buy.Quantity
                                                            }).ToList(),
                                           }).ToList(),
                             CurrentApprovers = new List<FLOG.Core.DocumentNo.TrxPersonApprover>()
                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.NegotiationSheetHeader>(true);

            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTrxType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTrxType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterTrxType)
                {
                    predicate = predicate.Or(x => x.TransactionType.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterSoDocumentNo = filter.SoDocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSoDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterSoDocumentNo)
                {
                    predicate = predicate.Or(x => x.SoDocumentNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }
            
            var filterMasterNo = filter.MasterNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterMasterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterMasterNo)
                {
                    predicate = predicate.Or(x => x.MasterNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterAgreementNo = filter.AgreementNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAgreementNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterAgreementNo)
                {
                    predicate = predicate.Or(x => x.AgreementNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterName = filter.CustomerName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.CustomerName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterShippingLineName = filter.ShippingLineName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShippingLineName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterShippingLineName)
                {
                    predicate = predicate.Or(x => x.ShippingLineName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterTermShipment = filter.TermOfShipment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTermShipment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterTermShipment)
                {
                    predicate = predicate.Or(x => x.TermOfShipment.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterFinalDestination = filter.FinalDestination?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterFinalDestination.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterFinalDestination)
                {
                    predicate = predicate.Or(x => x.FinalDestination.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterPortOfLoading = filter.PortOfLoading?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPortOfLoading.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterPortOfLoading)
                {
                    predicate = predicate.Or(x => x.PortOfLoading.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterPortOfDischarge = filter.PortOfDischarge?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPortOfDischarge.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterPortOfDischarge)
                {
                    predicate = predicate.Or(x => x.PortOfDischarge.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterCommodity = filter.Commodity?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCommodity.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterCommodity)
                {
                    predicate = predicate.Or(x => x.Commodity.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.NegotiationSheetHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.NegotiationSheetHeader> getSorted(IQueryable<Entities.NegotiationSheetHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.DocumentNo);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
                }
                
                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }

                if (item.Contains("TransactionDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionDate) : query.ThenBy(x => x.TransactionDate);
                }

                if (item.Contains("SoDocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SoDocumentNo) : query.ThenBy(x => x.SoDocumentNo);
                }

                if (item.Contains("MasterNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.MasterNo) : query.ThenBy(x => x.MasterNo);
                }

                if (item.Contains("AgreementNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AgreementNo) : query.ThenBy(x => x.AgreementNo);
                }

                if (item.Contains("CustomerCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerCode) : query.ThenBy(x => x.CustomerCode);
                }

                if (item.Contains("CustomerName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerName) : query.ThenBy(x => x.CustomerName);
                }

                if (item.Contains("ShippingLineName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShippingLineName) : query.ThenBy(x => x.ShippingLineName);
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
