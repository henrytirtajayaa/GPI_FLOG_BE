using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Checkbook.PutCheckbook
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
            if (string.IsNullOrEmpty(request.Body.CheckbookCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookCode)} cannot be null");

            if ((request.Body.CheckbookCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookCode)} cannot more than 50 characters");

            if ((request.Body.CheckbookName).Length > 200)
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookName)} cannot more than 200 characters");

            if ((request.Body.CurrencyCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyCode)} cannot more than 50 characters");

            if ((request.Body.BankAccountCode).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankAccountCode)} cannot more than 100 characters");

            if ((request.Body.BankCode).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankCode)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
