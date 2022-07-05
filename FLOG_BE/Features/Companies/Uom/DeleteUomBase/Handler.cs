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

namespace FLOG_BE.Features.Companies.Uom.DeleteUomBase
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
            bool any = await _context.UOMHeaders.Where(x => x.UomBaseId == request.Body.UomBaseId).AnyAsync();

            if(any)
                return ApiResult<Response>.ValidationError("Base Uom already applied. Deletion is not allowed !");

            var record = _context.UOMBases.FirstOrDefault(x => x.UomBaseId == request.Body.UomBaseId);
            if (record != null)
            {
                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    UomBaseId = request.Body.UomBaseId
                });
            }
            else{
                return ApiResult<Response>.ValidationError("Uom Base not found.");
            }
            
        }

    }
}
