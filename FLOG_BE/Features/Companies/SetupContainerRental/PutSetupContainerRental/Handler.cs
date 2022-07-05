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

namespace FLOG_BE.Features.Companies.SetupContainerRental.PutSetupContainerRental
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
                SetupContainerRentalId = request.Body.SetupContainerRentalId
            };

            var setup = await _context.SetupContainerRentals.FirstOrDefaultAsync(x => x.SetupContainerRentalId == request.Body.SetupContainerRentalId);

            if (setup != null)
            {
                setup.TransactionType = request.Body.TransactionType;
                setup.RequestDocNo = request.Body.RequestDocNo;
                setup.DeliveryDocNo = request.Body.DeliveryDocNo;
                setup.ClosingDocNo = request.Body.ClosingDocNo;
                setup.UomScheduleCode = request.Body.UomScheduleCode;
                setup.CustomerFreeUsageDays = request.Body.CustomerFreeUsageDays;
                setup.ShippingLineFreeUsageDays = request.Body.ShippingLineFreeUsageDays;
                setup.CntOwnerFreeUsageDays = request.Body.CntOwnerFreeUsageDays;
                setup.ModifiedBy = request.Initiator.UserId;
                setup.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
