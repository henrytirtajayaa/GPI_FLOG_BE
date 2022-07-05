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

namespace FLOG_BE.Features.Companies.CompanyBranch.PutCompanyBranch
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
            var branch = await _context.CompanyBranchs.FirstOrDefaultAsync(x => x.CompanyBranchId== request.Body.CompanyBranchId);
            if (branch != null)
            {
                if (request.Body.Default)
                {
                    var existing = _context.CompanyBranchs.ToList();
                    foreach (var cb in existing)
                    {
                        cb.Default = false;
                        _context.CompanyBranchs.Update(cb);
                    }
                }

                branch.BranchCode = request.Body.BranchCode;
                branch.BranchName = request.Body.BranchName;
                branch.Default = request.Body.Default;
                branch.Inactive = request.Body.Inactive;
                branch.ModifiedBy = request.Initiator.UserId;
                branch.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    CompanyBranchId = branch.CompanyBranchId
                });
            }
            else {
                return ApiResult<Response>.ValidationError("Company Branch not exist !");
            }          
        }
    }
}
