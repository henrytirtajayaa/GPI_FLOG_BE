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

namespace FLOG_BE.Features.Companies.Vendor.DeleteVendor
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
            var record = _context.Vendors.FirstOrDefault(x => x.VendorId == request.Body.VendorId);
            if (record != null)
            {
                var payable = _context.PayableTransactionHeaders.FirstOrDefault(x => x.VendorCode == record.VendorCode);
                var payment = _context.ApPaymentHeaders.FirstOrDefault(x => x.VendorCode == record.VendorCode);
                var checkbook = _context.CheckbookTransactionHeaders.FirstOrDefault(x => x.SubjectCode == record.VendorCode);

                if (payable != null || payment != null || checkbook != null)
                {
                    return ApiResult<Response>.ValidationError("Vendor Already In Use");
                }else
                {
                    _context.Attach(record);
                    _context.Remove(record);
                    await _context.SaveChangesAsync();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        VendorId = request.Body.VendorId
                    });
                }
            }
            else
            {
                return ApiResult<Response>.ValidationError("Vendor not found.");
            }
        }
    }
}
