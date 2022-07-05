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
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.Sales.Quotation.PostQuotation
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(context); 
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_SALES, DOCNO_FEATURE.TRXTYPE_SALES_QUOT, request.Body.TransactionType, transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var header = new Entities.SalesQuotationHeader()
                    {
                        SalesQuotationId = Guid.NewGuid(),
                        TransactionType = request.Body.TransactionType,
                        TransactionDate = request.Body.TransactionDate,
                        DocumentNo = documentUniqueNo,
                        CustomerId = request.Body.CustomerId,
                        CustomerAddressCode = request.Body.CustomerAddressCode,
                        SalesCode = request.Body.SalesCode,
                        ShipperId = request.Body.ShipperId,
                        ShipperAddressCode = request.Body.ShipperAddressCode,
                        ConsigneeId = request.Body.ConsigneeId,
                        ConsigneeAddressCode = request.Body.ConsigneeAddressCode,
                        IsDifferentNotifyPartner = request.Body.IsDifferentNotifyPartner,
                        NotifyPartnerId = request.Body.NotifyPartnerId,
                        NotifyPartnerAddressCode = request.Body.NotifyPartnerAddressCode,
                        ShippingLineId = request.Body.ShippingLineId,
                        IsShippingLineMaster = request.Body.IsShippingLineMaster,
                        ShippingLineCode = request.Body.ShippingLineCode,
                        ShippingLineName = request.Body.ShippingLineName,
                        ShippingLineVesselCode = request.Body.ShippingLineVesselCode,
                        ShippingLineVesselName = request.Body.ShippingLineVesselName,
                        ShippingLineShippingNo = request.Body.ShippingLineShippingNo,
                        ShippingLineDelivery = request.Body.ShippingLineDelivery,
                        ShippingLineArrival = request.Body.ShippingLineArrival,
                        FeederLineId = request.Body.FeederLineId,
                        IsFeederLineMaster = request.Body.IsFeederLineMaster,
                        FeederLineCode = request.Body.FeederLineCode,
                        FeederLineName = request.Body.FeederLineName,
                        FeederLineVesselCode = request.Body.FeederLineVesselCode,
                        FeederLineVesselName = request.Body.FeederLineVesselName,
                        FeederLineShippingNo = request.Body.FeederLineShippingNo,
                        FeederLineDelivery = request.Body.FeederLineDelivery,
                        FeederLineArrival = request.Body.FeederLineArrival,
                        TermOfShipment = request.Body.TermOfShipment,
                        FinalDestination = request.Body.FinalDestination,
                        PortOfLoading = request.Body.PortOfLoading,
                        PortOfDischarge = request.Body.PortOfDischarge,
                        Commodity = request.Body.Commodity,
                        CargoGrossWeight = request.Body.CargoGrossWeight,
                        CargoNetWeight = request.Body.CargoNetWeight,
                        CargoDescription = request.Body.CargoDescription,
                        Remark = request.Body.Remark,
                        Status = DOCSTATUS.NEW,
                        StatusComment = request.Body.StatusComment,
                        CreatedDate = DateTime.Now,
                        CreatedBy = request.Initiator.UserId,

                    };

                    bool validDetail = false;
                    _context.SalesQuotationHeaders.Add(header);

                    if (header.SalesQuotationId != null && header.SalesQuotationId != Guid.Empty)
                    {

                        var details = await InsertDetails(request.Body, header);

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return ApiResult<Response>.Ok(new Response()
                        {
                            SalesQuotationId = header.SalesQuotationId,
                            DocumentNo = header.DocumentNo
                        });

                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Payable Transaction can not be stored.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Document No can not be created. Please check Document No Setup!");
                }
            }
        }

        private async Task<List<Entities.SalesQuotationDetail>> InsertDetails(RequestQuotationBody body, Entities.SalesQuotationHeader header)
        {
            List<Entities.SalesQuotationDetail> result = new List<Entities.SalesQuotationDetail>();

            if (body.SalesQuotationDetails != null)
            {
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
