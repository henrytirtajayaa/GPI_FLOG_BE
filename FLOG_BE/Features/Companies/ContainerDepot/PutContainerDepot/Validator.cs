using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.ContainerDepot.PutContainerDepot
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

            if (!Guid.TryParse(request.Body.ContainerDepotId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.ContainerDepotId)} Incorrect Guid Format");

            if (string.IsNullOrEmpty(request.Body.DepotCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.DepotCode)} cannot be null");

            if ((request.Body.DepotName).Length > 200)
                return ValidationResult.ValidationError($"{nameof(request.Body.DepotName)} cannot more than 200 characters");

            return ValidationResult.Ok();
        }
    }
}
