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

namespace FLOG_BE.Features.Companies.ReceivableSetup.PostReceivableSetup
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
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var setup = new Entities.ReceivableSetup()
            {
                ReceivableSetupId = Guid.NewGuid(),
                DefaultRateType = request.Body.DefaultRateType,
                TaxRateType = request.Body.TaxRateType,
                AgingByDocdate = request.Body.AgingByDocdate,
                WriteoffLimit = request.Body.WriteoffLimit,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.ReceivableSetups.Add(setup);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                ReceivableSetupId = setup.ReceivableSetupId.ToString(),
                DefaultRateType = setup.DefaultRateType,
                TaxRateType = setup.TaxRateType,
                AgingByDocdate = setup.AgingByDocdate,
                WriteoffLimit = setup.WriteoffLimit
            });
        }
    }
}
