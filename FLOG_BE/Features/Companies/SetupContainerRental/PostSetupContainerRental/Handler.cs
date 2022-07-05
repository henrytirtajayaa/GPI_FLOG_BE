using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Infrastructure.Mediator;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Companies;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.SetupContainerRental.PostSetupContainerRental
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly CompanyContext _context;
        public readonly ILogin _login;
        public readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var setup = new Entities.SetupContainerRental()
            {
                SetupContainerRentalId = Guid.NewGuid(),
                TransactionType = request.Body.TransactionType,
                RequestDocNo = request.Body.RequestDocNo,
                DeliveryDocNo = request.Body.DeliveryDocNo,
                ClosingDocNo = request.Body.ClosingDocNo,
                UomScheduleCode = request.Body.UomScheduleCode,
                CustomerFreeUsageDays = request.Body.CustomerFreeUsageDays,
                ShippingLineFreeUsageDays = request.Body.ShippingLineFreeUsageDays,
                CntOwnerFreeUsageDays = request.Body.CntOwnerFreeUsageDays
            };

            _context.SetupContainerRentals.Add(setup);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            { 
                SetupContainerRentalId = setup.SetupContainerRentalId
            });
        }
    }
}
