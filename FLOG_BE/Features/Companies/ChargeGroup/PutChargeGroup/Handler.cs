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

namespace FLOG_BE.Features.Companies.ChargeGroup.PutChargeGroup
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
                ChargeGroupId = request.Body.ChargeGroupId,
                ChargeGroupCode = request.Body.ChargeGroupCode,
                ChargeGroupName = request.Body.ChargeGroupName
            };

            var Charges = await _context.ChargeGroups.FirstOrDefaultAsync(x => x.ChargeGroupId== Guid.Parse(request.Body.ChargeGroupId));
            if (Charges != null)
            {

           
                Charges.ChargeGroupId = Guid.Parse(request.Body.ChargeGroupId);
                Charges.ChargeGroupCode = request.Body.ChargeGroupCode;
                Charges.ChargeGroupName = request.Body.ChargeGroupName;
                
                _context.ChargeGroups.Update(Charges);

                await _context.SaveChangesAsync();
                response.ChargeGroupId = Charges.ChargeGroupId.ToString();
                response.ChargeGroupName = Charges.ChargeGroupName;
            }
            else {
                return ApiResult<Response>.ValidationError("Charge Group Id not found.");
            }

            return ApiResult<Response>.Ok(response);
            
        }
    }
}
