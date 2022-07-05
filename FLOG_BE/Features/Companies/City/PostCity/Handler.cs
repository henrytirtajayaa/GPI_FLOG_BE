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

namespace FLOG_BE.Features.Companies.City.PostCity
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

            if (await _context.Cities.AnyAsync(x => x.CityCode == request.Body.CityCode))
                  return ApiResult<Response>.ValidationError("City Code already exist.");
           
            var City = new Entities.City()
            {
                CityId = Guid.NewGuid(),
                CityCode = request.Body.CityCode,
                CityName = request.Body.CityName,
                Province = request.Body.Province,
                CountryID = Guid.Parse(request.Body.CountryId),
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Cities.Add(City);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                CityId = City.CityId.ToString(),
                CityCode = City.CityCode,
                CityName = City.CityName,
                Province = City.Province

            });
        }
    }
}
