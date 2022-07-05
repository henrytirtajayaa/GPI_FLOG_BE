using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.Country.PostCountry
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly CompanyContext _context;
        public readonly ILogin _login;
        public readonly HATEOASLinkCollection _linkCollection;


        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (await _context.Countries.AnyAsync(x => x.CountryCode == request.Body.CountryCode))
            {
                return ApiResult<Response>.ValidationError("Country Code already exist");
            }

            var country = new Entities.Country()
            {
                CountryId = Guid.NewGuid(),
                CountryCode = request.Body.CountryCode,
                CountryName = request.Body.CountryName,
                InActive = request.Body.InActive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Countries.Add(country);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            { 
                CountryId = country.CountryId.ToString(),
                CountryCode = country.CountryCode,
                CountryName = country.CountryName
            });
        }
    }
}
