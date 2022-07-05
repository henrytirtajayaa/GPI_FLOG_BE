using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.VendorGroup.PostVendorGroup
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly CompanyContext _context;
        public readonly ILogin _login;
        public readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (await _context.VendorGroups.AnyAsync(x => x.VendorGroupCode == request.Body.VendorGroupCode))
            {
                return ApiResult<Response>.ValidationError("VendorGroup Code already exist");
            }

            var vendorgroup = new Entities.VendorGroup()
            {
                VendorGroupId = Guid.NewGuid(),
                VendorGroupCode = request.Body.VendorGroupCode,
                VendorGroupName = request.Body.VendorGroupName,
                PaymentTermCode = request.Body.PaymentTermCode,
                PayableAccountNo = request.Body.PayableAccountNo,
                AccruedPayableAccountNo = request.Body.AccruedPayableAccountNo,
                InActive = request.Body.InActive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.VendorGroups.Add(vendorgroup);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                VendorGroupId = vendorgroup.VendorGroupId.ToString(),
                VendorGroupCode = vendorgroup.VendorGroupCode,
                VendorGroupName = vendorgroup.VendorGroupName,
                PaymentTermCode = vendorgroup.PaymentTermCode,
                PayableAccountNo = vendorgroup.PayableAccountNo,
                AccruedPayableAccountNo = vendorgroup.AccruedPayableAccountNo,
                InActive = vendorgroup.InActive,
            });
        }
    }
}
