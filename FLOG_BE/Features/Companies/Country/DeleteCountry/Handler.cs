using System;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;

namespace FLOG_BE.Features.Companies.Country.DeleteCountry
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
            var record = _context.Countries.FirstOrDefault(x => x.CountryId == Guid.Parse(request.Body.CountryId));

            if (record == null)
            {
                return ApiResult<Response>.ValidationError("Country Not Found");
            } else {
                var city = _context.Cities.FirstOrDefault(x => x.CountryID == record.CountryId);
                if (city == null)
                {
                    _context.Attach(record);
                    _context.Remove(record);
                    await _context.SaveChangesAsync();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        CountryId = request.Body.CountryId
                    });
                }
                else
                {
                    return ApiResult<Response>.ValidationError("Country Already In Use");
                }
            };
        }
    }
}
