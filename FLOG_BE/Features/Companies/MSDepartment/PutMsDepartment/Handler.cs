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

namespace FLOG_BE.Features.Companies.MSDepartment.PutMsDepartment
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
                DepartmentId = request.Body.DepartmentId,
            };

            var dept = await _context.MsDepartments.FirstOrDefaultAsync(x => x.DepartmentId == request.Body.DepartmentId);
            if (dept != null)
            {
                dept.DepartmentCode = request.Body.DepartmentCode;
                dept.DepartmentName = request.Body.DepartmentName;
                dept.Inactive = request.Body.Inactive;
                dept.ModifiedBy = request.Initiator.UserId;
                dept.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            return ApiResult<Response>.Ok(response);

        }
    }
}
