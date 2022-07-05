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

namespace FLOG_BE.Features.Companies.Vendor.PostVendor
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

            if (await _context.Vendors.AnyAsync(x => x.VendorCode == request.Body.VendorCode))
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.VendorCode)} already exist.");
            

            var Vendor = new Entities.Vendor()
            {

                VendorId = Guid.NewGuid(),
                VendorName = request.Body.VendorName,
                VendorCode = request.Body.VendorCode,
                AddressCode = request.Body.AddressCode,
                TaxRegistrationNo = request.Body.TaxRegistrationNo,
                VendorTaxName = request.Body.VendorTaxName,
                VendorGroupCode = request.Body.VendorGroupCode,
                PaymentTermCode = request.Body.PaymentTermCode,
                HasCreditLimit = request.Body.HasCreditLimit,
                CreditLimit = request.Body.CreditLimit,
                ShipToAddressCode = request.Body.ShipToAddressCode,
                BillToAddressCode = request.Body.BillToAddressCode,
                TaxAddressCode = request.Body.TaxAddressCode,
                PayableAccountNo = request.Body.PayableAccountNo,
                AccruedPayableAccountNo = request.Body.AccruedPayableAccountNo,
                Inactive = request.Body.Inactive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Vendors.Add(Vendor);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                VendorId = Vendor.VendorId,
                VendorCode = Vendor.VendorCode,
                VendorName = Vendor.VendorName
            });
        }
    }
}
