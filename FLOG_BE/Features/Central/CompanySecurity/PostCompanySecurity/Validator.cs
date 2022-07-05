using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.CompanySecurity.PostCompanySecurity
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
            if (string.IsNullOrEmpty(request.Body.UserId))
                return ValidationResult.ValidationError($"{nameof(request.Body.UserId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CompanyId))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.RoleId))
                return ValidationResult.ValidationError($"{nameof(request.Body.RoleId)} cannot be null");

            if (!Guid.TryParse(request.Body.UserId, out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.UserId)} Incorrect Guid Format");
            if (!Guid.TryParse(request.Body.CompanyId, out var newGuidComapny))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyId)} Incorrect Guid Format");
            if (!Guid.TryParse(request.Body.RoleId, out var newGuidRole))
                return ValidationResult.ValidationError($"{nameof(request.Body.RoleId)} Incorrect Guid Format");

            return ValidationResult.Ok();
        }
    }
}
