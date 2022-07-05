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

namespace FLOG_BE.Features.Companies.VendorGroup.PutVendorGroup
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
                VendorGroupCode = request.Body.VendorGroupCode,
                VendorGroupName = request.Body.VendorGroupName,
                PaymentTermCode = request.Body.PaymentTermCode,
                PayableAccountNo = request.Body.PayableAccountNo,
                AccruedPayableAccountNo = request.Body.AccruedPayableAccountNo,
                InActive = request.Body.InActive
            };

            var vendorgroup = await _context.VendorGroups.FirstOrDefaultAsync(x => x.VendorGroupId == Guid.Parse(request.Body.VendorGroupId));
            if (vendorgroup != null)
            {     
                vendorgroup.VendorGroupCode = request.Body.VendorGroupCode;
                vendorgroup.VendorGroupName = request.Body.VendorGroupName;
                vendorgroup.PaymentTermCode = request.Body.PaymentTermCode;
                vendorgroup.PayableAccountNo = request.Body.PayableAccountNo;
                vendorgroup.AccruedPayableAccountNo = request.Body.AccruedPayableAccountNo;
                vendorgroup.InActive = request.Body.InActive;
                vendorgroup.ModifiedBy = request.Initiator.UserId;
                vendorgroup.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.VendorGroupCode = vendorgroup.VendorGroupCode;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
