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

namespace FLOG_BE.Features.Companies.CustomerAddress.DeleteCustomerAddress
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
            var record = _context.CustomerAddresses.FirstOrDefault(x => x.CustomerAddressId == Guid.Parse(request.Body.CustomerAddressId));

            if(record != null)
            {
                bool inUsed = _context.Customers.Where(x => x.Inactive == false && x.CustomerId == record.CustomerId
                    && (x.AddressCode.Equals(record.AddressCode, StringComparison.OrdinalIgnoreCase) 
                     || x.ShipToAddressCode.Equals(record.AddressCode, StringComparison.OrdinalIgnoreCase)
                     || x.BillToAddressCode.Equals(record.AddressCode, StringComparison.OrdinalIgnoreCase))
                    ).Any();

                if(inUsed)
                {
                    return ApiResult<Response>.ValidationError(string.Format("{0} Address Code already used in Customer !", record.AddressCode));
                }

                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    CustomerAddressId = request.Body.CustomerAddressId
                });
            }
            else
            {
                return ApiResult<Response>.ValidationError("Customer not available for deletion !");
            }            
        }
    }
}
