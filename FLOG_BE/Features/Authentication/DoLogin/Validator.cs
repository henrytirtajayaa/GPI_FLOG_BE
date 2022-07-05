using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Authentication.DoLogin
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
            if (string.IsNullOrEmpty(request.Email))
                return ValidationResult.NotFound("Username cannot be null !");
            if (string.IsNullOrEmpty(request.Password))
                return ValidationResult.NotFound("Password cannot be null !");

            return ValidationResult.Ok();
        }
    }
}
