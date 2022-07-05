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

namespace FLOG_BE.Features.Companies.VehicleType.PutVehicleType
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
                VehicleTypeId = request.Body.VehicleTypeId,
                VehicleTypeName = request.Body.VehicleTypeName
            };

            var vehicle = await _context.VehicleTypes.FirstOrDefaultAsync(x => x.VehicleTypeId == request.Body.VehicleTypeId);
            if(vehicle != null)
            {
                vehicle.VehicleTypeCode = request.Body.VehicleTypeCode;
                vehicle.VehicleTypeName = request.Body.VehicleTypeName;
                vehicle.VehicleCategory = request.Body.VehicleCategory;
                vehicle.Inactive = request.Body.Inactive;

                await _context.SaveChangesAsync();

                response.VehicleTypeName = vehicle.VehicleTypeName;
            }
            
            return ApiResult<Response>.Ok(response);
        }
    }
}
