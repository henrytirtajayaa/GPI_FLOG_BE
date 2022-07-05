using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.UserGroup.PostUserGroup
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
            if (string.IsNullOrEmpty(request.Body.UserGroupCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.UserGroupCode)} cannot be null");

            if ((request.Body.UserGroupCode).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.UserGroupCode)} cannot more than 50 characters");

            if (string.IsNullOrEmpty(request.Body.UserGroupName))
                return ValidationResult.ValidationError($"{nameof(request.Body.UserGroupName)} cannot be null");

            if ((request.Body.UserGroupName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.UserGroupName)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
