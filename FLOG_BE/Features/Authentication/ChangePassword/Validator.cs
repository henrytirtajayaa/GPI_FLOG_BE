using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Authentication.ChangePassword
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
            if (string.IsNullOrEmpty(request.Body.OldPassword))
                return ValidationResult.NotFound($"{nameof(request.Body.OldPassword)} cannot be null!");

            if (string.IsNullOrEmpty(request.Body.NewPassword))
                return ValidationResult.NotFound($"{nameof(request.Body.NewPassword)} cannot be null!");

            if (string.IsNullOrEmpty(request.Body.ConfirmNewPassword))
                return ValidationResult.NotFound($"{nameof(request.Body.ConfirmNewPassword)} cannot be null!");

            if (request.Body.NewPassword != request.Body.ConfirmNewPassword)
                return ValidationResult.ValidationError($"{nameof(request.Body.NewPassword)} and {nameof(request.Body.ConfirmNewPassword)} does not match");

            return ValidationResult.Ok();
        }
    }
}
