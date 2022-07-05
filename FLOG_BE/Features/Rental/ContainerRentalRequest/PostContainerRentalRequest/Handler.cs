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
using FLOG_BE.Model.Companies.Entities;
using FLOG.Core.Finance.Util;
using Infrastructure;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PostContainerRentalRequest
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IDocumentGenerator _docGenerator;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(_context);
            _financeManager = new FinanceManager(context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.DocumentDate, TRX_MODULE.TRX_CONTAINER_RENT, DOCNO_FEATURE.TRXTYPE_RENTAL_REQUEST, request.Body.TransactionType, transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var RentalRequestHeader = new Entities.ContainerRentalRequestHeader()
                    {
                        ContainerRentalRequestHeaderId = Guid.NewGuid(),
                        DocumentNo = documentUniqueNo,
                        TransactionType = request.Body.TransactionType,
                        DocumentDate = request.Body.DocumentDate,
                        CustomerId = request.Body.CustomerId,
                        CustomerName = request.Body.CustomerName,
                        AddressCode = request.Body.AddressCode,
                        Status = DOCSTATUS.OPEN,
                        SalesCode = request.Body.SalesCode,
                        VendorId = request.Body.VendorId,
                        VendorName = request.Body.VendorName,
                        BillToAddressCode = request.Body.BillToAddressCode,
                        ShipToAddressCode = request.Body.ShipToAddressCode,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now
                    };

                    _context.ContainerRentalRequestHeaders.Add(RentalRequestHeader);

                    await _context.SaveChangesAsync();

                    if (RentalRequestHeader.ContainerRentalRequestHeaderId != null && RentalRequestHeader.ContainerRentalRequestHeaderId != Guid.Empty)
                    {
                        if (request.Body.ContainerRentalRequestDetails.Count > 0)
                        {
                            foreach (var item in request.Body.ContainerRentalRequestDetails)
                            {
                                var RentalRequestDetail = new Entities.ContainerRentalRequestDetail()
                                {
                                    ContainerRentalRequestDetailId = Guid.NewGuid(),
                                    ContainerRentalRequestHeaderId = RentalRequestHeader.ContainerRentalRequestHeaderId,
                                    ContainerCode = item.ContainerCode,
                                    ContainerName = item.ContainerName,
                                    UomCode = item.UomCode,
                                    Remarks = item.Remarks,
                                    Quantity = item.Quantity
                                };
                                _context.ContainerRentalRequestDetails.Add(RentalRequestDetail);
                            }
                            await _context.SaveChangesAsync();

                            transaction.Commit();
                            return ApiResult<Response>.Ok(new Response()
                            {
                                ContainerRentalRequestHeaderId = RentalRequestHeader.ContainerRentalRequestHeaderId,
                                DocumentNo = RentalRequestHeader.DocumentNo
                            });
                        }
                        else
                        {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError("Container Rental not exist !");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Container Rental can not be stored !");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Document Not can not be created !");
                }                
            }
        }
    }
}
