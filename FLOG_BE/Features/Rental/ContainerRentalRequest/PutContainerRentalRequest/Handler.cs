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
using FLOG_BE.Model.Companies.Entities;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutContainerRentalRequest
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            { 
                ContainerRentalRequestHeaderId = request.Body.ContainerRentalRequestHeaderId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var RentalRequestHeader = await _context.ContainerRentalRequestHeaders.FirstOrDefaultAsync(x => x.ContainerRentalRequestHeaderId == request.Body.ContainerRentalRequestHeaderId);

                    if (RentalRequestHeader != null)
                    {
                        RentalRequestHeader.TransactionType = request.Body.TransactionType;
                        RentalRequestHeader.DocumentDate = request.Body.DocumentDate;
                        RentalRequestHeader.CustomerId = request.Body.CustomerId;
                        RentalRequestHeader.CustomerName = request.Body.CustomerName;
                        RentalRequestHeader.AddressCode = request.Body.AddressCode;
                        RentalRequestHeader.SalesCode = request.Body.SalesCode;
                        RentalRequestHeader.VendorId = request.Body.VendorId;
                        RentalRequestHeader.VendorName = request.Body.VendorName;
                        RentalRequestHeader.BillToAddressCode = request.Body.BillToAddressCode;
                        RentalRequestHeader.ShipToAddressCode = request.Body.ShipToAddressCode;
                        RentalRequestHeader.ModifiedBy = request.Initiator.UserId;
                        RentalRequestHeader.ModifiedDate = DateTime.Now;

                        var RentalRequestDetail = await insertRentalRequestDetail(_context, request.Body);

                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            ContainerRentalRequestHeaderId = RentalRequestHeader.ContainerRentalRequestHeaderId
                        });
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError("Container Rental Update NOT Successful!");
                    }
                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private async Task<List<Entities.ContainerRentalRequestDetail>> insertRentalRequestDetail(CompanyContext ctx, RequestContainerRentalRequest body)
        {
            List<Entities.ContainerRentalRequestDetail> result = new List<ContainerRentalRequestDetail>();

            if (body.ContainerRentalRequestDetails != null)
            {
                //REMOVE EXISTING
                ctx.ContainerRentalRequestDetails
               .Where(x => x.ContainerRentalRequestHeaderId == body.ContainerRentalRequestHeaderId).ToList().ForEach(p => ctx.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ContainerRentalRequestDetails)
                {
                    var RentalRequestDetail = new Entities.ContainerRentalRequestDetail()
                    {
                        ContainerRentalRequestDetailId = Guid.NewGuid(),
                        ContainerRentalRequestHeaderId = body.ContainerRentalRequestHeaderId,
                        ContainerCode = item.ContainerCode,
                        ContainerName = item.ContainerName,
                        UomCode = item.UomCode,
                        Remarks = item.Remarks,
                        Quantity = item.Quantity,
                    };

                    result.Add(RentalRequestDetail);
                }

                if (result.Any())
                {
                    await _context.ContainerRentalRequestDetails.AddRangeAsync(result);
                }
            }

            return result;
        }
    }
}
