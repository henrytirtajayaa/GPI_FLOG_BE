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

namespace FLOG_BE.Features.Companies.Vendor.PutVendor
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
                VendorId = request.Body.VendorId,
                VendorCode = request.Body.VendorCode,
                VendorName = request.Body.VendorName
            };

            var Vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.VendorId == request.Body.VendorId);
            if (Vendor != null)
            {
                Vendor.VendorCode = request.Body.VendorCode;
                Vendor.VendorName = request.Body.VendorName;
                Vendor.AddressCode = request.Body.AddressCode;
                Vendor.TaxRegistrationNo = request.Body.TaxRegistrationNo;
                Vendor.VendorTaxName = request.Body.VendorTaxName;
                Vendor.VendorGroupCode = request.Body.VendorGroupCode;
                Vendor.PaymentTermCode = request.Body.PaymentTermCode;
                Vendor.HasCreditLimit = request.Body.HasCreditLimit;
                Vendor.CreditLimit = request.Body.CreditLimit;
                Vendor.ShipToAddressCode = request.Body.ShipToAddressCode;
                Vendor.BillToAddressCode = request.Body.BillToAddressCode;
                Vendor.TaxAddressCode = request.Body.TaxAddressCode;
                Vendor.PayableAccountNo = request.Body.PayableAccountNo;
                Vendor.AccruedPayableAccountNo = request.Body.AccruedPayableAccountNo;
                Vendor.Inactive = request.Body.Inactive;
                Vendor.ModifiedBy = request.Initiator.UserId;
                Vendor.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.VendorId = Vendor.VendorId;

            }
            else
            {
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.VendorId)} not found.");
            }

            return ApiResult<Response>.Ok(response);


        }
    }
}
