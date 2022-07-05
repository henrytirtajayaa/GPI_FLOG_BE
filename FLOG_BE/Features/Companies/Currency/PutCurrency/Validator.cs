using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Currency.PutCurrency
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

            if (string.IsNullOrEmpty(request.Body.Description))
                return ValidationResult.ValidationError($"{nameof(request.Body.Description)} cannot be null");

            if (request.Body.DecimalPlaces < 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.Description)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.Symbol))
                return ValidationResult.ValidationError($"{nameof(request.Body.Symbol)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CurrencyUnit))
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyUnit)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
