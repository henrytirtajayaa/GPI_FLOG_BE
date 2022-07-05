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

namespace FLOG_BE.Features.Companies.ChargeGroup.PostChargeGroup
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

            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();
            if (await _context.ChargeGroups.AnyAsync(x => x.ChargeGroupCode == request.Body.ChargeGroupCode))
                return ApiResult<Response>.ValidationError("Charge Group Code already exist.");

            var Charges = new Entities.ChargeGroup()
            {
                ChargeGroupId = Guid.NewGuid(),
                ChargeGroupCode = request.Body.ChargeGroupCode,
                ChargeGroupName = request.Body.ChargeGroupName
               
            };

            _context.ChargeGroups.Add(Charges);
            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                ChargeGroupId = Charges.ChargeGroupId.ToString(),
                ChargeGroupCode = Charges.ChargeGroupCode,
                ChargeGroupName = Charges.ChargeGroupName
            });
        }
    }
}
