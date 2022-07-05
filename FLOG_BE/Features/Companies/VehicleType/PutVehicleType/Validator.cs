using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.VehicleType.PutVehicleType
{
    public class Validator : AsyncRequestValidator<Request>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Validator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<ValidationResult> Validate(Request request)
        {
            if (string.IsNullOrEmpty(request.Body.VehicleTypeCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.VehicleTypeCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.VehicleTypeName))
                return ValidationResult.ValidationError($"{nameof(request.Body.VehicleTypeName)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
