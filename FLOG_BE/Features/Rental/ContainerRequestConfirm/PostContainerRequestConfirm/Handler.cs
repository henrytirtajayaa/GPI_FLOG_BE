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

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PostContainerRequestConfirm
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
            //string documentUniqueNo = _docGenerator.UniqueDocumentNo(request.Body.DocumentDate, FLOG.Core.TRX_MODULE.TRX_CONTAINER_RENTAL_REQUEST, FLOG.Core.DocumentNo.DOCNO_SETUP.CONTAINER_RENTAL_REQUEST, request.Body.TransactionType);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var DoConfirmHeader = new Entities.ContainerRequestConfirmHeader()
                {
                    ContainerRequestConfirmHeaderId = Guid.NewGuid(),
                    DocumentDate = request.Body.DocumentDate,
                    ContainerRentalRequestHeaderId = request.Body.ContainerRentalRequestHeaderId,
                    Status = DOCSTATUS.NEW,
                    DeliveryOrderNo = request.Body.DeliveryOrderNo.ToUpper(),
                    IssueDate = request.Body.IssueDate,
                    ExpiredDate = request.Body.ExpiredDate,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now
                };

                _context.ContainerRequestConfirmHeaders.Add(DoConfirmHeader);

                await _context.SaveChangesAsync();

                if (DoConfirmHeader.ContainerRequestConfirmHeaderId != null && DoConfirmHeader.ContainerRequestConfirmHeaderId != Guid.Empty)
                {
                    if (request.Body.ContainerRequestConfirmDetails.Count > 0)
                    {
                        //REMOVE EXISTING
                        request.Body.ContainerRequestConfirmDetails
                            .Where(x => x.ContainerRequestConfirmHeaderId == DoConfirmHeader.ContainerRequestConfirmHeaderId).ToList().ForEach(p => _context.Remove(p));

                        foreach (var item in request.Body.ContainerRequestConfirmDetails)
                        {
                            var RentalRequestDetail = new Entities.ContainerRequestConfirmDetail()
                            {
                                ContainerRequestConfirmDetailId = Guid.NewGuid(),
                                ContainerRequestConfirmHeaderId = DoConfirmHeader.ContainerRequestConfirmHeaderId,
                                ContainerRentalRequestDetailId = item.ContainerRentalRequestDetailId,
                                Remarks = item.Remarks,
                                Grade = item.Grade,
                                QuantityToConfirm = item.QuantityToConfirm,
                                QuantityBalance = item.QuantityBalance
                            };
                            _context.ContainerRequestConfirmDetails.Add(RentalRequestDetail);
                        }
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return ApiResult<Response>.Ok(new Response()
                        {
                            ContainerRequestConfirmHeaderId = DoConfirmHeader.ContainerRequestConfirmHeaderId,
                            DeliveryOrderNo = DoConfirmHeader.DeliveryOrderNo
                        });
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Container Rental not exist.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Container Rental can not be stored.");
                }
            }
        }
    }
}
