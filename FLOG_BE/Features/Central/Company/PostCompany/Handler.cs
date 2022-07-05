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
using Entities = FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.Company.PostCompany
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

            var CompanyId = _context.Companies.FirstOrDefault(x => x.DatabaseId == request.Body.DatabaseId);
            if (CompanyId != null)
                return ApiResult<Response>.ValidationError("Database Id already exist"); 
            var CompanyName = _context.Companies.FirstOrDefault(x => x.CompanyName == request.Body.CompanyName);
            if (CompanyName != null)
                return ApiResult<Response>.ValidationError("Company Name already exist");
                       
            var company = new Entities.Company()
            {
                DatabaseId = request.Body.DatabaseId,
                CompanyName = request.Body.CompanyName,
                CompanyId = Guid.NewGuid().ToString(),
                DatabaseAddress = request.Body.DatabaseAddress,
                DatabasePassword = request.Body.DatabasePassword,
                InActive = request.Body.InActive,
                CoaSymbol = request.Body.CoaSymbol,
                CoaTotalLength = request.Body.CoaTotalLength,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now,
                SmtpServer = request.Body.SmtpServer,
                SmtpPort = request.Body.SmtpPort,
                SmtpUser = request.Body.SmtpUser,
                SmtpPassword = request.Body.SmtpPassword,
                CompanySecurities = new List<Entities.CompanySecurity>()
                {
                     new Entities.CompanySecurity()
                     {
                          PersonId = request.Initiator.UserId,
                          SecurityRoleId = request.Initiator.SecurityId
                     }
                }
            };

            _context.Companies.Add(company);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName
            });
        }
    }
}
