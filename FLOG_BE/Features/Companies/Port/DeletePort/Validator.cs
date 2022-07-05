using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Companies.Port.DeletePort
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


            if (string.IsNullOrEmpty(request.Body.PortId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.PortId)} cannot be null");

            if (!Guid.TryParse(request.Body.PortId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.PortId)} Incorrect Guid Format");


            return ValidationResult.Ok();
        }
    }
}
