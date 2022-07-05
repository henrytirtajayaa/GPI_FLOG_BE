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

namespace FLOG_BE.Features.Companies.VehicleType.PostVehicleType
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
            var VehicleCode = _context.VehicleTypes.FirstOrDefault(x => x.VehicleTypeCode == request.Body.VehicleTypeCode);
            if (VehicleCode != null)
                return ApiResult<Response>.ValidationError("Vehicle Code already exist");
                       
            var vehicle = new Entities.VehicleType()
            {
                VehicleTypeId = Guid.NewGuid(),
                VehicleTypeCode = request.Body.VehicleTypeCode,
                VehicleTypeName = request.Body.VehicleTypeName,
                VehicleCategory = request.Body.VehicleCategory,
                Inactive = request.Body.Inactive,
                
            };

            _context.VehicleTypes.Add(vehicle);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                VehicleTypeId = vehicle.VehicleTypeId,
                VehicleTypeName = vehicle.VehicleTypeName
            });
        }
    }
}
