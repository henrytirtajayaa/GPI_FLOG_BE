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

namespace FLOG_BE.Features.Companies.Customer.PutCustomer
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
              CustomerId = request.Body.CustomerId
            };

            var editted = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == request.Body.CustomerId);
            if (editted != null)
            {
                editted.CustomerCode = request.Body.CustomerCode;
                editted.CustomerName = request.Body.CustomerName;
                editted.AddressCode = request.Body.AddressCode;
                editted.CustomerGroupCode = request.Body.CustomerGroupCode;
                editted.CustomerTaxName = request.Body.CustomerTaxName;
                editted.PaymentTermCode = request.Body.PaymentTermCode;
                editted.TaxRegistrationNo = request.Body.TaxRegistrationNo;
                editted.HasCreditLimit = request.Body.HasCreditLimit;
                editted.CreditLimit = request.Body.CreditLimit;
                editted.ShipToAddressCode = request.Body.ShipToAddressCode;
                editted.BillToAddressCode = request.Body.BillToAddressCode;
                editted.TaxAddressCode = request.Body.TaxAddressCode;
                editted.ReceivableAccountNo = request.Body.ReceivableAccountNo;
                editted.AccruedReceivableAccountNo = request.Body.AccruedReceivableAccountNo;
                editted.Inactive = request.Body.Inactive;
                editted.Status = request.Body.Status;
                editted.ModifiedBy = request.Initiator.UserId;
                editted.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.CustomerId = editted.CustomerId;
            }
            else {
                return ApiResult<Response>.ValidationError("Customer not found.");
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
