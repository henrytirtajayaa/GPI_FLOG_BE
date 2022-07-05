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

namespace FLOG_BE.Features.Companies.City.DeleteCity
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
            var record = _context.Cities.FirstOrDefault(x => x.CityId == Guid.Parse(request.Body.CityId));
            if (record != null)
            {
                var city = _context.CompanyAddresses.FirstOrDefault(x => x.CityCode == record.CityCode);
                var container = _context.ContainerDepots.FirstOrDefault(x => x.CityCode == record.CityCode);
                var vendor = _context.VendorAddresses.FirstOrDefault(x => x.CityCode == record.CityCode);
                var customer = _context.CustomerAddresses.FirstOrDefault(x => x.CityCode == record.CityCode);

                if (city != null || container != null || vendor != null || customer != null)
                {
                    return ApiResult<Response>.ValidationError("CityCode Already In Use");
                }
                else
                {
                    _context.Attach(record);
                    _context.Remove(record);
                    await _context.SaveChangesAsync();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        CityId = request.Body.CityId
                    });
                }
            }
            else{
                return ApiResult<Response>.ValidationError("City not found.");
            }
            
        }

    }
}
