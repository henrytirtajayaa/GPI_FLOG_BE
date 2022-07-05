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

namespace FLOG_BE.Features.Companies.Port.PostPort
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

            if (await _context.Ports.AnyAsync(x => x.PortCode == request.Body.PortCode))
                return ApiResult<Response>.ValidationError("Port already exist.");


            var port = new Entities.Port()
            {
                PortId = Guid.NewGuid(),
                PortCode = request.Body.PortCode,
                PortName = request.Body.PortName,
                PortType = request.Body.PortType,
                CityCode = request.Body.CityCode,
                InActive = request.Body.InActive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Ports.Add(port);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                PortId = port.PortId.ToString(),
                PortCode = port.PortCode,
                PortName = port.PortName,
                PortType = port.PortType
            });
        }
    }
}
