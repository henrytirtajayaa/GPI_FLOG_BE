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

namespace FLOG_BE.Features.Companies.Uom.PostUomBase
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

            if (await _context.UOMBases.AnyAsync(x => x.UomCode.Trim().ToUpper() == request.Body.UomCode.Trim().ToUpper()))
                  return ApiResult<Response>.ValidationError("Uom Base Code already exist.");
           
            var uomBase = new Entities.UOMBase()
            {
                UomBaseId = Guid.NewGuid(),
                UomCode = request.Body.UomCode,
                UomName = request.Body.UomName,
            };

            _context.UOMBases.Add(uomBase);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                UomBaseId = uomBase.UomBaseId,
                UomCode = uomBase.UomCode,
                UomName = uomBase.UomName
            });
        }
    }
}
