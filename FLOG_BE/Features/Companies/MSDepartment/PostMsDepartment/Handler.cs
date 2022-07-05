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

namespace FLOG_BE.Features.Companies.MSDepartment.PostMsDepartment
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
            if (await _context.MsDepartments.AnyAsync(x => x.DepartmentCode == request.Body.DepartmentCode && !x.Inactive))
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.DepartmentCode)} already exist.");
            try
            {
                var dept = new Entities.MsDepartment()
                {
                    DepartmentId = Guid.NewGuid(),
                    DepartmentCode = request.Body.DepartmentCode,
                    DepartmentName = request.Body.DepartmentName,
                    Inactive = false
                };

                _context.MsDepartments.Add(dept);

                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    DepartmentId = dept.DepartmentId
                });
            }
            catch (Exception ex)
            {
                return ApiResult<Response>.ValidationError(ex.Message);
            }
        }
    }
}
