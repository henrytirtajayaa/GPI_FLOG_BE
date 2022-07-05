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

namespace FLOG_BE.Features.Companies.ShippingLine.PostShippingLine
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

            if (await _context.ShippingLines.AnyAsync(x => x.ShippingLineCode == request.Body.ShippingLineCode))
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.ShippingLineCode)} already exist.");
            

            var shippingLine = new Entities.ShippingLine()
            {

                ShippingLineId = Guid.NewGuid(),
                ShippingLineCode = request.Body.ShippingLineCode,
                ShippingLineName = request.Body.ShippingLineName,
                ShippingLineType = request.Body.ShippingLineType,
                VendorId = request.Body.VendorId,
                IsFeeder = request.Body.IsFeeder,
                Status = request.Body.Status,
                Inactive = request.Body.Inactive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.ShippingLines.Add(shippingLine);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                ShippingLineId = shippingLine.ShippingLineId,
                ShippingLineName = shippingLine.ShippingLineName,
                ShippingLineType = shippingLine.ShippingLineType,
                ShippingLineCode = shippingLine.ShippingLineCode
            });
        }
    }
}
