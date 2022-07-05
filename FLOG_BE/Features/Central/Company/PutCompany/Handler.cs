using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.Company.PutCompany
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, HATEOASLinkCollection linkCollection)
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
                CompanyId = request.Body.CompanyId,
                CompanyName = request.Body.CompanyName
            };

            var company = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == request.Body.CompanyId);
            if(company != null)
            {
                company.CompanyName = request.Body.CompanyName;
                company.DatabaseAddress = request.Body.DatabaseAddress;
                if(!string.IsNullOrEmpty(request.Body.DatabasePassword))
                    company.DatabasePassword = request.Body.DatabasePassword;
                company.ModifiedBy = request.Initiator.UserId;
                company.ModifiedDate = DateTime.Now;
                company.InActive = request.Body.InActive;
                company.SmtpServer = request.Body.SmtpServer;
                company.SmtpPort = request.Body.SmtpPort;
                company.SmtpUser = request.Body.SmtpUser;
                company.SmtpPassword = request.Body.SmtpPassword;

                await _context.SaveChangesAsync();

                response.CompanyName = company.CompanyName;
            }
            
            return ApiResult<Response>.Ok(response);
        }
    }
}
