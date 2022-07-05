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

namespace FLOG_BE.Features.Companies.Customer.PostCustomer
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
            try
            {

                var CustomerCode = _context.Customers
                  .Where(x => x.CustomerCode == request.Body.CustomerCode)
                  .Select(x => x.CustomerCode).FirstOrDefault();

                if (CustomerCode != null)
                    return ApiResult<Response>.ValidationError("Customer Code already exist");

                var customer = new Entities.Customer()
                {

                    CustomerId = Guid.NewGuid(),
                    CustomerCode = request.Body.CustomerCode,
                    CustomerName = request.Body.CustomerName,
                    AddressCode = request.Body.AddressCode,
                    CustomerGroupCode = request.Body.CustomerGroupCode,
                    CustomerTaxName = request.Body.CustomerTaxName,
                    PaymentTermCode = request.Body.PaymentTermCode,
                    HasCreditLimit = request.Body.HasCreditLimit,
                    CreditLimit = request.Body.CreditLimit,
                    ShipToAddressCode = request.Body.ShipToAddressCode,
                    BillToAddressCode = request.Body.BillToAddressCode,
                    TaxAddressCode = request.Body.TaxAddressCode,
                    ReceivableAccountNo = request.Body.ReceivableAccountNo,
                    AccruedReceivableAccountNo = request.Body.AccruedReceivableAccountNo,
                    Inactive = request.Body.Inactive,
                    Status = request.Body.Status,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    CustomerId = customer.CustomerId,
                    CustomerCode = request.Body.CustomerCode,
                    CustomerName = request.Body.CustomerName
                });
            }
            catch (Exception e) {
                return ApiResult<Response>.ValidationError("error" + e.ToString());
            }
        }
    }
}
