using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Port.PutPort
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

            if (!Guid.TryParse(request.Body.PortId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.PortId)} Incorrect Guid Format");

            if (string.IsNullOrEmpty(request.Body.PortCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.PortCode)} cannot be null");

            if ((request.Body.PortName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.PortName)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
