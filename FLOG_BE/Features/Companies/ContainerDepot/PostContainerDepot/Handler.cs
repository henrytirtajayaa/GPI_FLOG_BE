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

namespace FLOG_BE.Features.Companies.ContainerDepot.PostContainerDepot
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

            if (await _context.ContainerDepots.AnyAsync(x => x.DepotCode == request.Body.DepotCode))
                return ApiResult<Response>.ValidationError("Depot Code already exist.");
            
            var ContainerDepot = new Entities.ContainerDepot()
            {
                ContainerDepotId = Guid.NewGuid(),
                DepotCode = request.Body.DepotCode,
                DepotName = request.Body.DepotName,
                OwnerVendorId = request.Body.OwnerVendorId,
                CityCode = request.Body.CityCode,
                InActive = request.Body.Inactive
            };

            _context.ContainerDepots.Add(ContainerDepot);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                ContainerDepotId = ContainerDepot.ContainerDepotId,
                DepotCode = ContainerDepot.DepotCode,
                DepotName = ContainerDepot.DepotName,
            });
        }
    }
}
