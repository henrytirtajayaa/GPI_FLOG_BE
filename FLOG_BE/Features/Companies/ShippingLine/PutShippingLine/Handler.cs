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

namespace FLOG_BE.Features.Companies.ShippingLine.PutShippingLine
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
                ShippingLineId = request.Body.ShippingLineId,
                ShippingLineCode = request.Body.ShippingLineCode,
                ShippingLineName = request.Body.ShippingLineName,
                ShippingLineType = request.Body.ShippingLineType
            };

            var ShippingLine = await _context.ShippingLines.FirstOrDefaultAsync(x => x.ShippingLineId == request.Body.ShippingLineId);
            if (ShippingLine != null)
            {
                ShippingLine.ShippingLineCode = request.Body.ShippingLineCode;
                ShippingLine.ShippingLineName = request.Body.ShippingLineName;
                ShippingLine.ShippingLineType = request.Body.ShippingLineType;
                ShippingLine.VendorId = request.Body.VendorId;
                ShippingLine.IsFeeder = request.Body.IsFeeder;
                ShippingLine.Inactive = request.Body.Inactive;
                ShippingLine.Status = request.Body.Status;
                ShippingLine.ModifiedBy = request.Initiator.UserId;
                ShippingLine.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.ShippingLineId = ShippingLine.ShippingLineId;

            }
            else
            {
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.ShippingLineId)} not found.");
            }

            return ApiResult<Response>.Ok(response);


        }
    }
}
