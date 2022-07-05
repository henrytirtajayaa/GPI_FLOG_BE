using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.User.PutUser
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
            if (string.IsNullOrEmpty(request.Body.UserFullName))
                return ValidationResult.ValidationError($"{nameof(request.Body.UserFullName)} cannot be null");
         
            if (string.IsNullOrEmpty(request.Body.EmailAddress))
                return ValidationResult.ValidationError($"{nameof(request.Body.EmailAddress)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
