using System;
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
using FLOG.Core;

namespace FLOG_BE.Features.Finance.Sales.Quotation.PutQuotation
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
            var response = new Response()
            {
                SalesQuotationId = request.Body.SalesQuotationId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {

                var editted = await _context.SalesQuotationHeaders.FirstOrDefaultAsync(x => x.SalesQuotationId == request.Body.SalesQuotationId);
                if (editted != null)
                {
                    editted.TransactionType = request.Body.TransactionType;
                    editted.TransactionDate = request.Body.TransactionDate;
                    editted.DocumentNo = request.Body.DocumentNo;
                    editted.CustomerId = request.Body.CustomerId;
                    editted.CustomerAddressCode = request.Body.CustomerAddressCode;
                    editted.SalesCode = request.Body.SalesCode;
                    editted.ShipperId = request.Body.ShipperId;
                    editted.ShipperAddressCode = request.Body.ShipperAddressCode;
                    editted.ConsigneeId = request.Body.ConsigneeId;
                    editted.ConsigneeAddressCode = request.Body.ConsigneeAddressCode;
                    editted.IsDifferentNotifyPartner = request.Body.IsDifferentNotifyPartner;
                    editted.NotifyPartnerId = request.Body.NotifyPartnerId;
                    editted.NotifyPartnerAddressCode = request.Body.NotifyPartnerAddressCode;
                    editted.ShippingLineId = request.Body.ShippingLineId;
                    editted.IsShippingLineMaster = request.Body.IsShippingLineMaster;
                    editted.ShippingLineCode = request.Body.ShippingLineCode;
                    editted.ShippingLineName = request.Body.ShippingLineName;
                    editted.ShippingLineVesselCode = request.Body.ShippingLineVesselCode;
                    editted.ShippingLineVesselName = request.Body.ShippingLineVesselName;
                    editted.ShippingLineShippingNo = request.Body.ShippingLineShippingNo;
                    editted.ShippingLineDelivery = request.Body.ShippingLineDelivery;
                    editted.ShippingLineArrival = request.Body.ShippingLineArrival;
                    editted.FeederLineId = request.Body.FeederLineId;
                    editted.IsFeederLineMaster = request.Body.IsFeederLineMaster;
                    editted.FeederLineCode = request.Body.FeederLineCode;
                    editted.FeederLineName = request.Body.FeederLineName;
                    editted.FeederLineVesselCode = request.Body.FeederLineVesselCode;
                    editted.FeederLineVesselName = request.Body.FeederLineVesselName;
                    editted.FeederLineShippingNo = request.Body.FeederLineShippingNo;
                    editted.FeederLineDelivery = request.Body.FeederLineDelivery;
                    editted.FeederLineArrival = request.Body.FeederLineArrival;
                    editted.TermOfShipment = request.Body.TermOfShipment;
                    editted.FinalDestination = request.Body.FinalDestination;
                    editted.PortOfLoading = request.Body.PortOfLoading;
                    editted.PortOfDischarge = request.Body.PortOfDischarge;
                    editted.Commodity = request.Body.Commodity;
                    editted.CargoGrossWeight = request.Body.CargoGrossWeight;
                    editted.CargoNetWeight = request.Body.CargoNetWeight;
                    editted.CargoDescription = request.Body.CargoDescription;
                    editted.Remark = request.Body.Remark;
                    editted.Status = request.Body.Status;
                    editted.StatusComment = request.Body.StatusComment;
                    //editted.Status = request.Body.Status;
                    editted.ModifiedBy = request.Initiator.UserId;
                    editted.ModifiedDate = DateTime.Now;
                    var details = await InsertDetails(request.Body,editted);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return ApiResult<Response>.Ok(new Response()
                    {
                        SalesQuotationId = editted.SalesQuotationId
                    });
                }
                else
                {
                    return ApiResult<Response>.ValidationError("Sales Quotation not found.");
                }



            }
        }

        private async Task<List<Entities.SalesQuotationDetail>> InsertDetails(RequestQuotationBody body, Entities.SalesQuotationHeader header)
        {
            List<Entities.SalesQuotationDetail> result = new List<Entities.SalesQuotationDetail>();

            if (body.SalesQuotationDetails != null)
            {
                // REMOVE EXISTING
                _context.QuotationDetails
               .Where(x => x.SalesQuotationId == body.SalesQuotationId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.SalesQuotationDetails)
                {
                    var Detail = new Entities.SalesQuotationDetail()
                    {
                        SalesQuotationDetailId = Guid.NewGuid(),
                        SalesQuotationId = header.SalesQuotationId,
                        ContainerId = item.ContainerId,
                        Qty = item.Qty,
                        UomDetailId = item.UomDetailId,
                        Remark = item.Remark,
                        Status = item.Status,
                    };

                    result.Add(Detail);
                }

                if (result.Count > 0)
                {
                    await _context.QuotationDetails.AddRangeAsync(result);
                }
            }

            return result;
        }


    }
}
