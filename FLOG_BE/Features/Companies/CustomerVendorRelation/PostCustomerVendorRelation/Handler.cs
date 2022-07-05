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

namespace FLOG_BE.Features.Companies.CustomerVendorRelation.PostCustomerVendorRelation
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

            if (await _context.CustomerVendorRelations.AnyAsync(x => x.CustomerId == request.Body.CustomerId && x.VendorId == request.Body.VendorId))
                return ApiResult<Response>.ValidationError($" Combination CustomerId and Vendor Id already exist.");
            

            var CustomerVendor = new Entities.CustomerVendorRelation()
            {

                RelationId = Guid.NewGuid(),
                CustomerId = request.Body.CustomerId,
                VendorId = request.Body.VendorId,
                 CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.CustomerVendorRelations.Add(CustomerVendor);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                RelationId = CustomerVendor.RelationId,
                CustomerId = CustomerVendor.CustomerId,
                VendorId = CustomerVendor.VendorId
                
               
            });
        }
    }
}
