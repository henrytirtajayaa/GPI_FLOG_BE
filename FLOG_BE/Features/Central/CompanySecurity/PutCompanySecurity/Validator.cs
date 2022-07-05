using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.CompanySecurity.PutCompanySecurity
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
            if (string.IsNullOrEmpty(request.Body.CompanySecurityId))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanySecurityId)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CompanyId))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyId)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.UserId))
                return ValidationResult.ValidationError($"{nameof(request.Body.UserId)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.RoleId))
                return ValidationResult.ValidationError($"{nameof(request.Body.RoleId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
