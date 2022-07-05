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

namespace FLOG_BE.Features.Companies.CustomerVendorRelation.PutCustomerVendorRelation
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
              RelationId = request.Body.RelationId
            };

            var editted = await _context.CustomerVendorRelations.FirstOrDefaultAsync(x => x.RelationId == request.Body.RelationId);
            if (editted != null)
            {
                editted.VendorId = request.Body.VendorId;
                editted.CustomerId = request.Body.CustomerId;
                editted.ModifiedBy = request.Initiator.UserId;
                editted.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.RelationId = editted.RelationId;
            }
            else {
                return ApiResult<Response>.ValidationError("Customer Vendor Relation not found.");
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
