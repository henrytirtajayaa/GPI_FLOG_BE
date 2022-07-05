using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace FLOG_BE.Features.Companies.Country.PutCountry
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
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            {
                CountryId = request.Body.CountryId,
                CountryName = request.Body.CountryName
            };

            var country = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == Guid.Parse(request.Body.CountryId));
            if (country != null)
            {
                country.CountryName = request.Body.CountryName;
                country.InActive = request.Body.InActive;
                country.ModifiedBy = request.Initiator.UserId;
                country.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.CountryName = country.CountryName;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
