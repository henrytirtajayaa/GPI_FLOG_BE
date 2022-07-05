using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Country.PutCountry
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
            if (string.IsNullOrEmpty(request.Body.CountryId))
                return ValidationResult.ValidationError($"{nameof(request.Body.CountryId)} cannot be null");

            if ((request.Body.CountryId).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.CountryId)} cannot more than 50 characters");

            if (string.IsNullOrEmpty(request.Body.CountryName))
                return ValidationResult.ValidationError($"{nameof(request.Body.CountryName)} cannot be null");

            if ((request.Body.CountryName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.CountryName)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
