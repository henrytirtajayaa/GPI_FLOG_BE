using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.Company.PostCompany
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
            if (string.IsNullOrEmpty(request.Body.CompanyName))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyName)} cannot be null");

            if ((request.Body.CompanyName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyName)} cannot more than 100 characters");

            if (string.IsNullOrEmpty(request.Body.DatabaseId))
                return ValidationResult.ValidationError($"{nameof(request.Body.DatabaseId)} cannot be null");

           // @"^[a-zA-Z_ ][a-zA-Z0-9_ ]*$")
            if (!Regex.Match(request.Body.DatabaseId, @"^[a-zA-Z_][a-zA-Z0-9_]*$").Success)
                return ValidationResult.ValidationError("DatabaseId cannot contains special charaters");

            return ValidationResult.Ok();
        }
    }
}
