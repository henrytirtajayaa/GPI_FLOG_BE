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

namespace FLOG_BE.Features.Companies.PayableSetup.PutPayableSetup
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
                PayableSetupId = request.Body.PayableSetupId
            };

            var setup = await _context.PayableSetups.FirstOrDefaultAsync(x => x.PayableSetupId == Guid.Parse(request.Body.PayableSetupId));
            if (setup != null)
            {
                setup.DefaultRateType = request.Body.DefaultRateType;
                setup.TaxRateType = request.Body.TaxRateType;
                setup.AgingByDocdate = request.Body.AgingByDocdate;
                setup.WriteoffLimit = request.Body.WriteoffLimit;
                setup.ModifiedBy = request.Initiator.UserId;
                setup.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
