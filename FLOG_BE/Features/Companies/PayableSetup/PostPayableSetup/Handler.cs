using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Infrastructure.Mediator;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Companies;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.PayableSetup.PostPayableSetup
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
            var setup = new Entities.PayableSetup()
            {
                PayableSetupId = Guid.NewGuid(),
                DefaultRateType = request.Body.DefaultRateType,
                TaxRateType = request.Body.TaxRateType,
                AgingByDocdate = request.Body.AgingByDocdate,
                WriteoffLimit = request.Body.WriteoffLimit,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.PayableSetups.Add(setup);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            { 
                PayableSetupId = setup.PayableSetupId.ToString()
            });
        }
    }
}
