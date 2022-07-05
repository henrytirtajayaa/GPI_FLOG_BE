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

namespace FLOG_BE.Features.Companies.ContainerDepot.PutContainerDepot
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
                DepotCode = request.Body.DepotCode,
                DepotName = request.Body.DepotName
            };
            var containerdepot = await _context.ContainerDepots.FirstOrDefaultAsync(x => x.ContainerDepotId == request.Body.ContainerDepotId);
            if(containerdepot != null)
            {
                containerdepot.DepotCode = request.Body.DepotCode;
                containerdepot.DepotName = request.Body.DepotName;
                containerdepot.OwnerVendorId = request.Body.OwnerVendorId;
                containerdepot.CityCode = request.Body.CityCode;
                containerdepot.InActive = request.Body.Inactive;
                containerdepot.ModifiedBy = request.Initiator.UserId;
                containerdepot.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.DepotCode = containerdepot.DepotCode;
            }
            
            return ApiResult<Response>.Ok(response);
        }
    }
}
