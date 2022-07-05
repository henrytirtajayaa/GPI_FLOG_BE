using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.PutExchangeRateHeader
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
            if (string.IsNullOrEmpty(request.Body.ExchangeRateCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.ExchangeRateCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.Description))
                return ValidationResult.ValidationError($"{nameof(request.Body.Description)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CurrencyCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.ExpiredPeriod))
                return ValidationResult.ValidationError($"{nameof(request.Body.ExpiredPeriod)} cannot be null");

            if (request.Body.ExpiredPeriod == "0")
                return ValidationResult.ValidationError($"{nameof(request.Body.ExpiredPeriod)} must not 0");

            if (request.Body.CalculationType == 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.CalculationType)} must not 0");

            if (request.Body.RateType == 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.RateType)} must not 0");

            return ValidationResult.Ok();
        }
    }
}
