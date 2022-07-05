using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.ArReceipt.PostCustomerReceipt
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
            if (request.Body.TransactionDate == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.TransactionDate)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CurrencyCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyCode)} cannot be null");

            if (request.Body.ExchangeRate < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.ExchangeRate)} must greater than 0");

            return ValidationResult.Ok();
        }
    }
}
