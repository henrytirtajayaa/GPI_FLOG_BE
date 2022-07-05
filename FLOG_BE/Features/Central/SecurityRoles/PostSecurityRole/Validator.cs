using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.SecurityRoles.PostSecurityRole
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
            if (string.IsNullOrEmpty(request.Body.RoleCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.RoleCode)} cannot be null");
            if ((request.Body.RoleCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.RoleCode)} cannot more than 50 characters");


            if ((request.Body.RoleName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.RoleName)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
