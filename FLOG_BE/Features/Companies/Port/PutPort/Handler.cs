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

namespace FLOG_BE.Features.Companies.Port.PutPort
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
                PortCode = request.Body.PortCode,
                PortName = request.Body.PortName
            };
            var port = await _context.Ports.FirstOrDefaultAsync(x => x.PortId == Guid.Parse(request.Body.PortId));
            if (port != null)
            {
                port.PortCode = request.Body.PortCode;
                port.PortName = request.Body.PortName;
                port.PortType = request.Body.PortType;
                port.CityCode = request.Body.CityCode;
                port.InActive = request.Body.InActive;
                port.ModifiedBy = request.Initiator.UserId;
                port.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.PortName = port.PortName;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
