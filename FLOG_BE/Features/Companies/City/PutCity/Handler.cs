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

namespace FLOG_BE.Features.Companies.City.PutCity
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
                CityCode = request.Body.CityCode,
                CityName = request.Body.CityName
            };
            var City = await _context.Cities.FirstOrDefaultAsync(x => x.CityId == Guid.Parse(request.Body.CityId));
            if (City != null)
            {
                if (!await _context.Countries.AnyAsync(x => x.CountryId == Guid.Parse(request.Body.CountryId)))
                {
                    return ApiResult<Response>.ValidationError("Country is not exist.");
                }
                else
                {
                    try
                    {
                        City.CityCode = request.Body.CityCode;
                        City.CityName = request.Body.CityName;
                        City.Province = request.Body.Province;
                        City.CountryID = Guid.Parse(request.Body.CountryId);
                        City.Inactive = request.Body.inActive;
                        City.ModifiedBy = request.Initiator.UserId;
                        City.ModifiedDate = DateTime.Now;

                        await _context.SaveChangesAsync();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            CityId = City.CityId.ToString(),
                            CityCode = City.CityCode,
                            CityName = City.CityName,
                            Province = City.Province
                        });
                    }
                    catch(Exception ex)
                    {
                        return ApiResult<Response>.ValidationError(ex.Message);
                    }     
                }   
            }
            else {
                return ApiResult<Response>.ValidationError("City not found.");
            }          
        }
    }
}
