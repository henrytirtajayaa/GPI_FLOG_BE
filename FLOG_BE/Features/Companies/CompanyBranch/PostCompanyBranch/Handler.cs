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

namespace FLOG_BE.Features.Companies.CompanyBranch.PostCompanyBranch
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

            if (await _context.CompanyBranchs.AnyAsync(x => x.BranchCode.Equals(request.Body.BranchCode, StringComparison.OrdinalIgnoreCase)))
                  return ApiResult<Response>.ValidationError("Company Branch Code already exist.");

            if (request.Body.Default)
            {
                var existing = _context.CompanyBranchs.ToList();
                foreach(var cb in existing)
                {
                    cb.Default = false;
                    _context.CompanyBranchs.Update(cb);
                }
            }

            var branch = new Entities.CompanyBranch()
            {
                CompanyBranchId = Guid.NewGuid(),
                BranchCode = request.Body.BranchCode,
                BranchName = request.Body.BranchName,
                Default = request.Body.Default,
                Inactive = request.Body.Inactive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.CompanyBranchs.Add(branch);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                CompanyBranchId = branch.CompanyBranchId
            });
        }
    }
}
